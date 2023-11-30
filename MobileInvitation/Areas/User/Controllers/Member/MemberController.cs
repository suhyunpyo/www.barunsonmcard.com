using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.Data.Coupon;
using MobileInvitation.Data.Mcard;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Product;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using MobileInvitation.Payment;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using X.PagedList;

namespace MobileInvitation.Areas.User.Controllers.Member
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class MemberController : PathController
    {
        #region 내부 변수 선언
        private readonly BarShopContext _barShopDb;
		private IHttpContextAccessor _accessor;
        private readonly IOperationRepository _operationRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMcardRepository _mcardRepository;
        private readonly IProductRepository _ProductRepositoy;

        private readonly ITossPaymentService _tossPay;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly CookieOptions cookieOptions;

        #endregion

        #region 생성자
        public MemberController(IProductRepository ProductRepository, IOperationRepository operationRepository,
            ICouponRepository couponRepository,   IMcardRepository mcardRepository,
            IHttpContextAccessor accessor, IWebHostEnvironment environment, 
            barunsonContext barunsonContext, BarunnConfig barunnConfig, ITossPaymentService tossPaymentService, IHttpClientFactory httpClientFactory,
            BarShopContext barShopContext)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _barShopDb = barShopContext;

			_accessor = accessor;
            _operationRepository = operationRepository;
            _couponRepository = couponRepository;
            _mcardRepository = mcardRepository;
            _ProductRepositoy = ProductRepository;

            _tossPay = tossPaymentService;
            _httpClientFactory = httpClientFactory;

            cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Domain = ".barunsonmcard.com",
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };
        }
        #endregion
        [Route("Member")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Index()
        {
            //인증이 되고 , 권한이 사용자 
            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                return RedirectToAction("Index", "Main");
            }
            else
            {
                return RedirectToAction("Login", "Member");
            }
        }
        #region Private Functions
        /// <summary>
        /// 취소 가능 여부
        /// </summary>
        /// <returns></returns>
        private (bool, string) CheckIsCancel(string Payment_Method_Code, DateTime orderDate, DateTime? deadLineDate, int accountCount)
        {
            if (Payment_Method_Code == "PMC02") //가상계좌
                return (false, "무통장입금(가상계좌) 결제 고객은<br> 고객센터 전화(1644 - 0708)를 통해 진행해 주세요.");

            //제작 완료 이후 3일이 경과
            var checkDateTime = orderDate;
            if (deadLineDate.HasValue)
                checkDateTime = deadLineDate.Value;
            else
                checkDateTime = checkDateTime.AddHours(72);

            if (checkDateTime < DateTime.Now)
                return (false, "제작 완료 이후 3일이 경과하여 취소/환불 하실 수 없습니다.");

            //송금서비스 사용
            if (accountCount > 0)
                return (false, "송금 서비스를 사용 중이며,<br>송금 내역이 있는 경우, 취소 / 환불하실 수<br> 없습니다.<br>초대장을 비공개로 전환하는 '비공개' 기능을<Br> 이용해 주세요.");

            return (true, "");
        }

        #endregion

        #region Login & Logout

        [Route("Member/Login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                //비회원 주문시 이름이 없어 무한 리디렉션 문제 발생됨.
                if (User.IsInRole("Guest") && string.IsNullOrEmpty(User.FindFirst("Name").Value))
                { }
                else
                    return Redirect(returnUrl);
            }
                

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["IsOrder"] = returnUrl.StartsWith("/Order/Regist");

            return View();
        }

        /// <summary>
        /// 회원 로그인 처리
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("Member/LoginMember")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginMember(string UserId, string Password, string returnUrl = "/")
        {
            var result = new JsonReusltStatusModel { status = false, message = "아이디 또는 비밀번호를 확인해주세요.", errors = new List<string>() };
            try
            {
                var account = await _barunsonDb.VW_Users
                    .FromSqlInterpolated($"Select * From VW_USER Where USER_ID = {UserId} And PWDCOMPARE({Password}, TRY_CONVERT(VARBINARY(200), PWD, 1)) = 1 ")
                    .FirstOrDefaultAsync();

                if (account != null)
                {
                    DateTime? wedd = null;
                    if (!string.IsNullOrEmpty(account.WEDDING_DATE) && account.WEDDING_DATE != "1900-01-01")
                    {
                        try
                        {
                            var splitDate = account.WEDDING_DATE.Split('-');
                            if (splitDate.Length >= 3)
                            {
                                wedd = new DateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2]));

                                if (!string.IsNullOrEmpty(splitDate[3]))
                                    wedd.Value.AddHours(int.Parse(splitDate[3]));
                                if (!string.IsNullOrEmpty(splitDate[4]))
                                    wedd.Value.AddMinutes(int.Parse(splitDate[4]));
                            }
                        }
                        catch {
                            wedd = null;
                        }
                    }

                    await SignInAsync("Users", account.USER_ID, account.USER_ID, account.NAME, account.EMAIL, account.CELLPHONE_NUMBER, account.DUPINFO, wedd);
                    
                    result.status = true;
                    result.message = returnUrl;
                }
            }
            catch { }
            return Json(result);
        }

        /// <summary>
        /// 비회원 로그인 처리
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("Member/LoginNonUser")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginNonUser(string Name, string Email, string returnUrl = "/")
        {
            var result = new JsonReusltStatusModel { status = false, message = "입력하신 정보와 일치하는 주문정보가 없습니다.", errors = new List<string>() };
            try
            {
                var query = from o in _barunsonDb.TB_Orders
                            where o.Name == Name && o.Email == Email
                            orderby o.Regist_DateTime descending
                            select o;
                var order = await query.FirstOrDefaultAsync();
                if (order != null) 
                {

                    var sid = order.Name + "_" + order.Email;
                    await SignInAsync("Guest", sid, "", order.Name, order.Email, order.CellPhone_Number, "", null);
                    
                    result.status = true;
                    result.message = returnUrl;
                }
            }
            catch { }
            return Json(result);
        }

        /// <summary>
        /// 비회원 주문
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("Member/LoginNonUserOrder")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginNonUserOrder(string returnUrl = "/")
        {
            var result = new JsonReusltStatusModel { status = false, message = returnUrl, errors = new List<string>() };
            try
            {
                //비회원 주문시 빈값으로 인증처리하고 있음. 이름과 매일주소 받도록 개선 필요
                await SignInAsync("Guest", "", "", "", "", "", "", null);
                
                result.status = true;
                result.message = returnUrl;
            }
            catch { }
            return Json(result);
        }

        /// <summary>
        /// Admin page 에서 관리자가 사용자로 로그인
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [Route("Member/CSLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> CSLogin(string ticket)
        {
            await HttpContext.SignOutAsync("userAuth"); // 쿠키 인증 로그아웃

            var jsonstring = AesHelper.Decrypt128(ticket, AesHelper.AES_DEFAULT_KEY);
            var authTicket = JsonSerializer.Deserialize<AuthTicketModel>(jsonstring);

            string AUTH = "Users";
            if (string.IsNullOrEmpty(authTicket.UserID))
            {
                AUTH = "Guest";
            }
            await SignInAsync(AUTH, authTicket.UserID, authTicket.UserID, authTicket.Name, authTicket.Email, "", "", null);

            return RedirectToAction("Mypage", "Member");
        }
        public class AuthTicketModel
        {
            public string Name { get; set; }
            public string UserID { get; set; }
            public string Email { get; set; }
            public DateTime Issue { get; set; }
        }

        /// <summary>
        /// 공통 회원, 비회원 인증 처리
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="sid"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="dupinfo"></param>
        /// <param name="wddeingDate"></param>
        /// <returns></returns>
        private async Task SignInAsync(string authType, string sid, string userId, string name, string email, string phone, string dupinfo, DateTime? wddeingDate)
        {
            Response.Cookies.Append("u_auth_ax", !string.IsNullOrEmpty(authType) ? AesHelper.Encrypt128(authType, AesHelper.AES_DEFAULT_KEY) : "", cookieOptions);
            Response.Cookies.Append("u_auth_nx", !string.IsNullOrEmpty(name) ? AesHelper.Encrypt128(name, AesHelper.AES_DEFAULT_KEY) : "", cookieOptions);
            Response.Cookies.Append("u_auth_ix", !string.IsNullOrEmpty(userId) ? AesHelper.Encrypt128(userId, AesHelper.AES_DEFAULT_KEY) : "", cookieOptions);
            Response.Cookies.Append("u_auth_dx", !string.IsNullOrEmpty(dupinfo) ? AesHelper.Encrypt128(dupinfo, AesHelper.AES_DEFAULT_KEY) : "", cookieOptions);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, sid),
                new Claim("Id", userId),
                new Claim("Name", name),
                new Claim("Email", email),
                new Claim("Hp", phone),
                new Claim("DUPINFO", dupinfo),
                new Claim("Wedding_Date", wddeingDate.HasValue ? wddeingDate.Value.ToString("yyyy-MM-dd") : ""),
                new Claim("Wedding_HH", wddeingDate.HasValue ? wddeingDate.Value.ToString("HH") : ""),
                new Claim("Wedding_MM", wddeingDate.HasValue ? wddeingDate.Value.ToString("mm") : ""),
                new Claim("Wedding_PM_AM", wddeingDate.HasValue ? wddeingDate.Value.ToString("tt", CultureInfo.InvariantCulture) : ""),
                new Claim(ClaimTypes.Role, authType),
            };

            var ci = new ClaimsIdentity(claims, "userAuth");
            var authenticationProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IssuedUtc = DateTimeOffset.UtcNow,
                IsPersistent = true
            };
            await HttpContext.SignInAsync("userAuth", new ClaimsPrincipal(ci), authenticationProperties); 

        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        [Route("Member/Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("userAuth"); // 쿠키 인증 로그아웃

            Response.Cookies.Delete("u_auth_nx", cookieOptions);
            Response.Cookies.Delete("u_auth_ix", cookieOptions);
            Response.Cookies.Delete("u_auth_ex", cookieOptions);
            Response.Cookies.Delete("u_auth_ax", cookieOptions);
            Response.Cookies.Delete("u_auth_dx", cookieOptions);

            return RedirectToAction("Index", "Main");
        }
        #endregion

        #region 회원 정보 수정, 탈퇴
        /// <summary>
        /// 회원 정보 수정
        /// </summary>
        /// <returns></returns>
        [Route("Member/EditProfile")]
        public IActionResult EditProfile(string returnUrl)
        {
            var model = new JsonReusltStatusModel
            {
                status = false,
                message = string.Empty,
            };

            if (User.IsInRole("Users"))
            {
                string DupInfo = User.FindFirst("DUPINFO").Value; 
				var data = new User_Certification_Log()
				{
					CertID = Guid.NewGuid().ToString().ToUpper(),
					CertType = "NONE",
					DupInfo = DupInfo,
					RegDate = DateTime.Now
				};
                _barShopDb.User_Certification_Log.Add(data);
                _barShopDb.SaveChanges();

                var pathAndQuery = $"Profile-Modify?CertID={data.CertID}";
                if (!string.IsNullOrEmpty(returnUrl))
                    pathAndQuery += $"&returnUrl={returnUrl}";

				model.status = true;
                model.message = new Uri(_barunnConfig.Sites.BarunFamilyUrl, pathAndQuery).ToString();
            }
            else
            {
                model.message = "잘못된 접근으로 회원 정보 수정 할 수 없습니다..";
            }
            return Json(model);

        }

        /// <summary>
        /// 회원 탈퇴
        /// </summary>
        /// <returns></returns>
        [Route("Member/SecessionProfile")]
        public IActionResult SecessionProfile()
        {
            var reDirectUrl = Url.Action("Mypage", "Member");
            if (User.IsInRole("Users"))
            {
				string DupInfo = User.FindFirst("DUPINFO").Value;
				var data = new User_Certification_Log()
				{
					CertID = Guid.NewGuid().ToString().ToUpper(),
					CertType = "NONE",
					DupInfo = DupInfo,
					RegDate = DateTime.Now
				};
				_barShopDb.User_Certification_Log.Add(data);
				_barShopDb.SaveChanges();

                var pathAndQuery = $"Secession?CertID={data.CertID}&SiteDiv=BM&ReturnUrl={Url.ActionLink("Logout", "Member")}";
				reDirectUrl = new Uri(_barunnConfig.Sites.BarunFamilyUrl, pathAndQuery).ToString();
			}

            return Redirect(reDirectUrl);
        }

        #endregion

        #region 아이디/비번 찾기
        /// <summary>
        /// Nice 인증 암호화를 위한 내부 API 호출
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private async Task<FindIdViewModel> GetNiceEncData(string userID = "", string callbackurl = null)
        {
            FindIdViewModel result = null;
            var apiUri = new Uri(_barunnConfig.Sites.PrivateApiUrl, "/api/Nice/Encrypt");
            var httpClient = _httpClientFactory.CreateClient();

            if (callbackurl == null)
                callbackurl = Url.ActionLink("NiceCallback", "Member");

            var builder = new UriBuilder(apiUri);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["returnUrl"] = callbackurl;
            query["receiveData"] = userID;
            query["methodType"] = "post";
            query["popupYn"] = "Y";
            builder.Query = query.ToString();

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = builder.Uri;

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var restr = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<FindIdViewModel>(restr);
            }

            return result;
        }
        private async Task<NiceApiResponseData> GetNiceDecData(string tokenVersionId, string encData, string integrityValue)
        {
            NiceApiResponseData result = null;
            var apiUri = new Uri(_barunnConfig.Sites.PrivateApiUrl, "/api/Nice/Decrypt");
            var httpClient = _httpClientFactory.CreateClient();

            var builder = new UriBuilder(apiUri);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["tokenVersionId"] = tokenVersionId;
            query["encData"] = encData;
            query["integrityValue"] = integrityValue;
            builder.Query = query.ToString();

            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = builder.Uri;

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var restr = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<NiceApiResponseData>(restr);
            }
            return result;
        }
        /// <summary>
        /// 아이디/비번 찾기 화면
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("Member/FindIdPwd")]
        [AllowAnonymous]
        public async Task<IActionResult> FindIdPwd(string userID = "")
        {
            var callback = Url.ActionLink("NiceCallback", "Member");
            var model = await GetNiceEncData(userID, callback);

            return View(model);
        }

        /// <summary>
        /// NiceApi 콜백
        /// </summary>
        /// <param name="token_version_id"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("Member/NiceCallback")]
        [AllowAnonymous]
        public IActionResult NiceCallback(string token_version_id, string enc_data, string integrity_value)
        {
            var model = new FindIdViewModel
            {
                tokenVersionId = token_version_id,
                encData = enc_data,
                integrityValue = integrity_value
            };
            return View(model);
            
        }
        /// <summary>
        /// ID,Password 찾기 결과 화면
        /// </summary>
        /// <param name="token_version_id"></param>
        /// <param name="enc_data"></param>
        /// <param name="integrity_value"></param>
        /// <returns></returns>
        [Route("Member/FindIDResult")]
        [AllowAnonymous]
        public async Task<IActionResult> FindIDResult(FindIdViewModel input)
        {
            var model = new FindIdResponseViewModel
            {
                IsSuccess = false
            };
            try
            {
                var resData = await GetNiceDecData(input.tokenVersionId, input.encData, input.integrityValue);

                if (resData != null)
                {
                    var account = await (from a in _barunsonDb.VW_Users
                                         where a.DUPINFO == resData.di
                                         select new { a.USER_ID, a.DUPINFO }).FirstOrDefaultAsync();
                    if (account != null)
                    {
                        model.UserID = account.USER_ID;
                        model.AuthDupInfo = WebUtility.UrlEncode(account.DUPINFO);
                        if (!string.IsNullOrEmpty(resData.ReceiveData))
                        {
                            //비번찾기 - ReceiveData 에 ID가 있음.
                            model.SearchType = "pwd";
                            model.IsSuccess = account.USER_ID == resData.ReceiveData;
                        }
                        else
                        {
                            //ID 찾기
                            model.SearchType = "id";
                            model.IsSuccess = true;
                        }
                    }
                }
            }
            catch { }

            return View(model);
        }
        #endregion

        #region My Page

        /// <summary>
        /// My Page main, 
        /// PC - Left 메뉴 표시, 오른쪽에 Mypage_Order 주문내역 표시
        /// Mo - 메뉴 목록만 표시, 클릭시 메뉴 이동
        /// </summary>
        /// <returns></returns>
        [Route("Member/Mypage")]
        public async Task<IActionResult> Mypage()
        {
            var model = new MyPageViewModel
            {
                TabId = 0,
                Banners = await GetBanners("마이페이지 배너")
            };
            if (Request.Headers["Referer"].ToString().ToLower().IndexOf("order/complete") > 0)
                model.TabId = 1;

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }

        /// <summary>
        /// 주문내역 조회 
        /// tabid: 0=제작중, 1=제작완료, 2=취소/환불
        /// </summary>
        /// <returns></returns>
        [Route("Member/MyOrder")]
        public async Task<IActionResult> MyOrder(int tabid = 0)
        {
            var model = new MyPageOrderViewModel
            {
                TabId = tabid,
                DataModels = new List<MyOrderItemViewModel>()
            };
            var userID = User.FindFirst("Id").Value;
            var userEmail = User.FindFirst("Email").Value;
            var userName = User.FindFirst("Name").Value;
            if (!User.IsInRole("Users"))
                userID = "";
            var psc = new List<string> { "PSC01", "PSC02", "PSC04" };
            var rsc = new List<string> { "RSC01", "RSC02", "RSC04", "RSC05" };
            var now = DateTime.Now;

            #region 제작,완료,취소 주문 수량 계산

            var cntQ = _barunsonDb.TB_Orders.AsQueryable();
            if (User.IsInRole("Users")) //회원
                cntQ = cntQ.Where(m => m.User_ID == userID);
            else
                cntQ = cntQ.Where(m => m.Email == userEmail && m.Name == userName);

            //제작,완료
            
            var gcntItems = await cntQ.Where(m => m.Order_Status_Code == "OSC01" && psc.Contains(m.Payment_Status_Code))
                                    .GroupBy(m => m.Payment_Status_Code)
                                    .Select(g => new { code = g.Key, count = g.Count() })
                                    .ToListAsync();
            model.MakingCount = gcntItems.Where(m => m.code == "PSC01").Sum(m => m.count);
            model.CompleteCount = gcntItems.Where(m => m.code != "PSC01").Sum(m => m.count);

            //취소환불
            
            model.CancelCount = await cntQ.Join(_barunsonDb.TB_Refund_Infos, oid => oid.Order_ID, rid => rid.Order_ID, (oid, rid) => new { o = oid, r = rid  })
                                    .Where(m => rsc.Contains(m.r.Refund_Status_Code))
                                    .CountAsync();

            #endregion

            #region 주문 목록 쿼리 : 제작,완료
            if (model.TabId == 0 || model.TabId == 1)
            {
                if (model.TabId == 0)
                    psc = new List<string> { "PSC01" };
                else
                    psc = new List<string> { "PSC02", "PSC04" };

                var orderQuery = from o in _barunsonDb.TB_Orders
                                 join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
                                 join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
                                 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                                 join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                                 join d in _barunsonDb.TB_Common_Codes on new { Code = o.Payment_Method_Code, Code_Group = "PAYMENT_METHOD_CODE" } equals new { d.Code, d.Code_Group } into gd
                                 from pm in gd.DefaultIfEmpty()
                                 join e in _barunsonDb.TB_Common_Codes on new { Code = o.Payment_Status_Code, Code_Group = "PAYMENT_STATUS_CODE" } equals new { e.Code, e.Code_Group } into ge
                                 from ps in ge.DefaultIfEmpty()
                                 join f in _barunsonDb.TB_Common_Codes on new { Code = p.Product_Category_Code, Code_Group = "Product_Category_Code" } equals new { f.Code, f.Code_Group } into gf
                                 from pb in gf.DefaultIfEmpty()
                                 where ((o.User_ID == userID && userID != "") || (userID == "" && o.Email == userEmail && o.Name == userName))
                                    && o.Order_Status_Code == "OSC01" && psc.Contains(o.Payment_Status_Code)
                                 orderby o.Regist_DateTime descending
                                 select new MyOrderItemViewModel
                                 {
                                     OrderID = o.Order_ID,
                                     OrderCode = o.Order_Code,
                                     OrderPrice = o.Order_Price,
                                     InvitationId = i.Invitation_ID,
                                     OrderDateTime = o.Order_DateTime ?? o.Regist_DateTime.Value,
                                     PaymentDateTime = o.Payment_DateTime,
                                     ProductCategoryCode = p.Product_Category_Code,
                                     ProductCategoryName = pb.Code_Name,
                                     ProductCode = p.Product_Code,
                                     ProductName = p.Product_Name,
                                     MainImageUrl = p.Main_Image_URL,
                                     ProductPrice = p.Price,
                                     PaymentStatusCode = o.Payment_Status_Code,
                                     PaymentStatusName = ps.Code_Name,
                                     PaymentMethodCode = o.Payment_Method_Code,
                                     PaymentMethodName = pm.Code_Name,
                                     FinanceName = o.Finance_Name,
                                     AccountNumber = o.Account_Number,
                                     PaymentPrice = o.Payment_Price,
                                     IsDisplay = id.Invitation_Display_YN == "Y" && i.Invitation_Display_YN == "Y",
                                     WDate = id.WeddingDate,
                                     WHH = id.WeddingHour,
                                     WMM = id.WeddingMin,
                                     WTC = id.Time_Type_Code,
                                     IsMoneyGift = id.MoneyGift_Remit_Use_YN == "Y",
                                     isFlower = id.Flower_gift_YN == "Y",
                                 };
                model.DataModels = await orderQuery.ToListAsync();
                foreach(var item in model.DataModels)
                {
                    item.MainImageUrlFull = GetResourceAbsoluteUrl(item.MainImageUrl);
                    item.WeddingDate = GetWeddingDate(item.WDate, item.WHH, item.WMM, item.WTC);
                    
                    item.BadgeMent = "노출중지";
                    item.BadgeStatus = "type03";
                    
                    if (item.PaymentStatusCode == "PSC02") //결제 완료 
                    {
                        if (item.WeddingDate.HasValue && item.WeddingDate < now.AddMonths(-3))
                        {
                            item.BadgeMent = "기간만료";
                            item.BadgeStatus = "type02";
                        }
                        else
                        {
                            item.BadgeMent = "노출";
                            item.BadgeStatus = "type01";
                        }
                    }
                    else if (item.PaymentStatusCode == "PSC04")  //입금대기 
                    {
                        item.BadgeMent = "결제대기";
                        item.BadgeStatus = "type02";
                    }
                    
                    if (model.TabId == 1) //완료
                    {
                        //송금 서비스
                        if (item.IsMoneyGift)
                        {
                            item.MoneyGiftCount = await(from a in _barunsonDb.TB_Accounts where a.Invitation_ID == item.InvitationId select a).CountAsync();

                            if (item.MoneyGiftCount == 0) 
                                item.MoneyGiftUrl = new Uri(Url.ActionLink("Account", "KakaoRemit", new { OrderId = item.OrderID }));
                            else
                                item.MoneyGiftUrl = new Uri(Url.ActionLink("Calculate", "KakaoRemit", new { OrderId = item.OrderID }));
						}
         
                        //화환 선물
                        item.isFlower = await (from a in _barunsonDb.TB_Order_PartnerShip where a.Order_ID == item.OrderID && a.P_OrderState == "배송완료" && a.P_Id == "flasystem" select a).CountAsync() > 0;
                        if (item.isFlower)
                            item.FlowerUrl = new Uri(Url.ActionLink("Index", "MyFlower", new { OrderId = item.OrderID }));
                    }
                }

            }

            #endregion

            #region 주문 목록 쿼리 : 취소
            if (model.TabId == 2)
            {
                var orderQuery = from o in _barunsonDb.TB_Orders
                                 join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
                                 join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
                                 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                                 join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                                 join r in _barunsonDb.TB_Refund_Infos on o.Order_ID equals r.Order_ID
                                 join f in _barunsonDb.TB_Common_Codes on new { Code = p.Product_Category_Code, Code_Group = "Product_Category_Code" } equals new { f.Code, f.Code_Group } into gf
                                 from pb in gf.DefaultIfEmpty()
                                 where ((o.User_ID == userID && userID != "") || (userID == "" && o.Email == userEmail && o.Name == userName))
                                   && rsc.Contains(r.Refund_Status_Code)
                                 orderby o.Regist_DateTime descending
                                 select new MyOrderItemViewModel
                                 {
                                     OrderID = o.Order_ID,
                                     OrderCode = o.Order_Code,
                                     OrderPrice = o.Order_Price,
                                     InvitationId = i.Invitation_ID,
                                     OrderDateTime = o.Order_DateTime ?? o.Regist_DateTime.Value,
                                     PaymentDateTime = o.Payment_DateTime,
                                     PaymentPrice = o.Payment_Price,
                                     ProductCategoryCode = p.Product_Category_Code,
                                     ProductCategoryName = pb.Code_Name,
                                     ProductCode = p.Product_Code,
                                     ProductName = p.Product_Name,
                                     RefundDateTime = r.Refund_DateTime,
                                     RefundTypeCode = r.Refund_Type_Code,
                                     RefundStatusCode = r.Refund_Status_Code,
                                     RefundPrice = r.Refund_Price
                                 };
                model.DataModels = await orderQuery.ToListAsync();
                foreach (var item in model.DataModels)
                {
                    switch (item.RefundTypeCode)
                    {
                        case "RTC01":
                            item.PayName = " 신용카드";
                            item.RefundStatusName = "취소/환불 완료";
                            break;
                        case "RTC02":
                            item.PayName = "계좌이체";
                            item.RefundStatusName = "취소/환불 완료";
                            break;
                        case "RTC03":
                            item.PayName = "가상계좌";
                            item.RefundStatusName = (item.RefundStatusCode == "RSC01") ? "환불 진행 중" : "취소/환불 완료";
                            break;
                        case "RTC04":
                            item.PayName = "쿠폰";
                            item.RefundStatusName = "취소/환불 완료";
                            break;
                        case "RTC05":
                            item.PayName = "가상계좌";
                            item.RefundStatusName = "취소완료";
                            break;
                    }
                }
             }

            #endregion

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }

        /// <summary>
        /// 주문 상세 정보, 제작 완료시만 표시
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Member/Order_Detail/{Order_Id?}")]
        public async Task<IActionResult> Order_Detail(int Order_Id)
        {
            var userID = User.FindFirst("Id").Value;
            var userEmail = User.FindFirst("Email").Value;
            var userName = User.FindFirst("Name").Value;
            if (!User.IsInRole("Users"))
                userID = "";

            var orderQuery = from o in _barunsonDb.TB_Orders
                             join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
                             join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
                             join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                             join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                             join d in _barunsonDb.TB_Common_Codes on new { Code = o.Payment_Method_Code, Code_Group = "PAYMENT_METHOD_CODE" } equals new { d.Code, d.Code_Group } into gd
                             from pm in gd.DefaultIfEmpty()
                             join e in _barunsonDb.TB_Common_Codes on new { Code = o.Payment_Status_Code, Code_Group = "PAYMENT_STATUS_CODE" } equals new { e.Code, e.Code_Group } into ge
                             from ps in ge.DefaultIfEmpty()
                             join f in _barunsonDb.TB_Common_Codes on new { Code = p.Product_Category_Code, Code_Group = "Product_Category_Code" } equals new { f.Code, f.Code_Group } into gf
                             from pb in gf.DefaultIfEmpty()
                             where o.Order_ID == Order_Id 
                                && ((o.User_ID == userID && userID != "") || (userID == "" && o.Email == userEmail && o.Name == userName))
                             select new MyOrderDetailViewModel
                             {
                                 OrderID = o.Order_ID,
                                 OrderCode = o.Order_Code,
                                 OrderPrice = o.Order_Price,
                                 InvitationId = i.Invitation_ID,
                                 OrderDateTime = o.Order_DateTime ?? o.Regist_DateTime.Value,
                                 PaymentDateTime = o.Payment_DateTime,
                                 ProductCategoryCode = p.Product_Category_Code,
                                 ProductCategoryName = pb.Code_Name,
                                 ProductCode = p.Product_Code,
                                 ProductName = p.Product_Name,
                                 InvitationUrl = id.Invitation_URL,
                                 ProductPrice = p.Price,
                                 PaymentStatusCode = o.Payment_Status_Code,
                                 PaymentStatusName = ps.Code_Name,
                                 PaymentMethodCode = o.Payment_Method_Code,
                                 PaymentMethodName = pm.Code_Name,
                                 FinanceName = o.Finance_Name,
                                 AccountNumber = o.Account_Number,
                                 PaymentPrice = o.Payment_Price,
                                 DeadLineDate = o.Deposit_DeadLine_DateTime,
                                 CouponPrice = o.Coupon_Price,
                                 IsDisplay = id.Invitation_Display_YN == "Y" && i.Invitation_Display_YN == "Y",
                             };

            var model = await orderQuery.FirstOrDefaultAsync();
            if (model == null)
            {
                // 주문 정보가 없거나, 잘못된 접근
                return RedirectToAction("Mypage", "Member");
            }
            model.InvitationUrlFull = new Uri(SiteUri, "m/" + model.InvitationUrl); 

            var accountCount = await (from a in _barunsonDb.TB_Accounts
                                      join r in _barunsonDb.TB_Remits on a.Invitation_ID equals r.Invitation_ID
                                      where a.Invitation_ID == model.InvitationId && r.Total_Price > 10000
                                      select a).CountAsync();
            var isCancel = CheckIsCancel(model.PaymentMethodCode, model.OrderDateTime, model.DeadLineDate, accountCount);
            model.IsCancel = isCancel.Item1;
            model.NotCancelText = isCancel.Item2;

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }

        /// <summary>
        /// 노출 여부 수정
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <param name="display"></param>
        /// <returns></returns>
        [Route("Member/UpdateInvitationDisplay")]
        public async Task<IActionResult> UpdateInvitationDisplay(int Order_Id, bool notDisplay)
        {
            var model = new JsonReusltStatusModel { status = false, message = "공개/비공개 업데이트 오류가 발생 했습니다. 페이지를 새로고침 후 다시 시도 하세요." };

            try
            {
                var userID = User.FindFirst("Id").Value;
                var userEmail = User.FindFirst("Email").Value;
                var userName = User.FindFirst("Name").Value;
                if (!User.IsInRole("Users"))
                    userID = "";

                var orderQuery = from o in _barunsonDb.TB_Orders
                                 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                                 join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                                 where o.Order_ID == Order_Id
                                    && ((o.User_ID == userID && userID != "") || (userID == "" && o.Email == userEmail && o.Name == userName))
                                 select id;
                var item = await orderQuery.FirstOrDefaultAsync();
                if (item != null)
                {
                    item.Invitation_Display_YN = notDisplay ? "N" : "Y";
                    await _barunsonDb.SaveChangesAsync();
                    model.status = true;
                    model.message = "";
                }
            }
            catch
            {

            }
            return Json(model);
        }

        #endregion

        #region Wish 
        [Route("Member/WishList")]
        public async Task<IActionResult> WishList(MyWishViewModel model)
        {
            model.DataModel = new List<MyWishDataModel>();
            model.PageSize = 8;
            model.RouteController = "Member";
            model.RouteAction = "WishList";
            model.Count = 0;

            var userID = User.FindFirst("Id").Value;
            //회원만 접근
            if (!User.IsInRole("Users"))
                return RedirectToAction("Index", "Main");

            var query = from a in _barunsonDb.TB_Wish_Lists
                        join b in _barunsonDb.TB_Products on a.Product_ID equals b.Product_ID
                        where a.User_ID == userID
                        orderby a.Regist_DateTime descending
                        select new MyWishDataModel
                        {
                            WishID = a.Wish_ID,
                            ProductId = b.Product_ID,
                            ProductCode = b.Product_Code,
                            ProductName = b.Product_Name,
                            MainImageUrl = b.Main_Image_URL,
                            ProductPrice = b.Price
                        };
            model.Count = await query.CountAsync();
            model.DataModel = await query.Skip(model.PageFrom).Take(model.PageSize).ToListAsync();
            foreach (var item in model.DataModel)
            {
                item.MainImageUrlFull = GetResourceAbsoluteUrl(item.MainImageUrl);
            }

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }

        /// <summary>
        ///  위시리스트 - 삭제 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Member/WishDelete")]
        public async Task<IActionResult> WishDelete(List<int> wishId)
        {
            var userID = User.FindFirst("Id").Value;
            //회원만 접근
            if (!User.IsInRole("Users"))
                return RedirectToAction("Index", "Main");

            try
            {
                if (wishId != null && wishId.Count > 0)
                {
                    var query = from a in _barunsonDb.TB_Wish_Lists
                                where a.User_ID == userID && wishId.Contains(a.Wish_ID)
                                select a;
                    _barunsonDb.TB_Wish_Lists.RemoveRange(query);
                    await _barunsonDb.SaveChangesAsync();
                }
            }
            catch { }

            return RedirectToAction("WishList", "Member");
        }

        #endregion

        #region 쿠폰
        /// <summary>
        /// 프런트 - 마이페이지 - 회원 쿠폰 리스트 
        /// </summary>
        /// <returns></returns>
        [Route("Member/Coupon_List")]
        public async Task<IActionResult> Coupon_List(MyCouponViewModel model)
        {
            var userID = User.FindFirst("Id").Value;
            //회원만 접근
            if (!User.IsInRole("Users"))
                return RedirectToAction("Index", "Main");

            model.DataModel = new List<MyCouponDataModel>();
            model.PageSize = 21;
            model.RouteController = "Member";
            model.RouteAction = "Coupon_List";
            model.Count = 0;

            try
            {
                var query1 = from c in _barunsonDb.TB_Coupons
                            join cp in _barunsonDb.TB_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                            where cp.User_ID == userID && cp.Retrieve_DateTime == null && cp.Use_YN == "N"
                            orderby c.Discount_Method_Code descending, c.Discount_Rate descending, c.Discount_Price
                            select new MyCouponDataModel
                            {
                                CouponID = c.Coupon_ID,
                                CouponName = c.Coupon_Name,
                                DiscountMethodCode = c.Discount_Method_Code,
                                DiscountRate = c.Discount_Rate,
                                DiscountPrice = c.Discount_Price,
                                PeriodMethodCode = c.Period_Method_Code,
                                PublishStartDate = !string.IsNullOrEmpty(c.Publish_Start_Date) ? DateTime.Parse(c.Publish_Start_Date) : null,
                                PublishEndDate = !string.IsNullOrEmpty(c.Publish_End_Date) ? DateTime.Parse(c.Publish_End_Date) : null,
                                ExpirationDate = !string.IsNullOrEmpty(cp.Expiration_Date) ? DateTime.Parse(cp.Expiration_Date) : null,
                                RegistDateTime = cp.Regist_DateTime,
                                Description = c.Description
                            };
                var query2 = from c in _barunsonDb.TB_Serial_Coupons
                             join cp in _barunsonDb.TB_Serial_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                             where cp.User_ID == userID && cp.Retrieve_DateTime == null && cp.Use_YN == "N"
                             select new MyCouponDataModel
                             {
                                 CouponID = c.Coupon_ID,
                                 CouponName = c.Coupon_Name,
                                 DiscountMethodCode = c.Discount_Method_Code,
                                 DiscountRate = c.Discount_Rate,
                                 DiscountPrice = c.Discount_Price,
                                 PeriodMethodCode = c.Period_Method_Code,
                                 PublishStartDate = !string.IsNullOrEmpty(c.Publish_Start_Date) ? DateTime.Parse(c.Publish_Start_Date) : null,
                                 PublishEndDate = !string.IsNullOrEmpty(c.Publish_End_Date) ? DateTime.Parse(c.Publish_End_Date) : null,
                                 ExpirationDate = !string.IsNullOrEmpty(cp.Expiration_Date) ? DateTime.Parse(cp.Expiration_Date) : null,
                                 RegistDateTime = cp.Regist_DateTime,
                                 Description = c.Description
                             };
                var items = new List<MyCouponDataModel>();
                items.AddRange(await query1.ToListAsync());
                items.AddRange(await query2.ToListAsync());

                var toDay = DateTime.Today;
                foreach (var item in items)
                {
                    var chek2 = false;
                    if (item.PeriodMethodCode == "PMC03") //무제한
                        chek2 = true;
                    else if (item.PeriodMethodCode == "PMC01") //기간입력
                        chek2 = (item.PublishStartDate <= toDay && item.PublishEndDate >= toDay);
                    else if (item.PeriodMethodCode == "PMC02") //발행일로부터 X일
                        chek2 = (item.ExpirationDate >= toDay);

                    item.IsCopuponUsing = chek2;
                }
                model.Count = items.Count;
                model.UseCouponCount = items.Where(m => m.IsCopuponUsing).Count();

                if (UrlHelper.IsMobile(Request))
                {
                    model.DataModel = items.OrderByDescending(m => m.DiscountMethodCode)
                        .ThenByDescending(m => m.DiscountRate)
                        .ThenByDescending(m => m.DiscountPrice)
                        .ToList();
                }
                else
                {
                    model.DataModel = items.OrderByDescending(m => m.DiscountMethodCode)
                        .ThenByDescending(m => m.DiscountRate)
                        .ThenByDescending(m => m.DiscountPrice)
                        .Skip(model.PageFrom)
                        .Take(model.PageSize)
                        .ToList();
                }
            }
            catch { }

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);

        }
                
        /// 프런트 - 마이페이지 - 제작 완료 페이지 - 회원 쿠폰 리스트 
        /// </summary>
        /// <returns></returns>
        [Route("Member/Coupon_List_Popup_New/")]
        public IActionResult Coupon_List_Popup_New(string Product_Code)
        {
            string UserId = "";

            if (User.Identity.IsAuthenticated/* && (User.IsInRole("Users") || User.IsInRole("Guest"))*/)
            {
                UserId = User.FindFirst("Id").Value;
            }

            ViewBag.Popup_Coupon_List = _couponRepository.User_Coupon_List_Entity_New(UserId, Product_Code);

            return Json(new { Coupon_List = ViewBag.Popup_Coupon_List });

        }


        /// <summary>
        /// 쿠폰 등록
        /// </summary>
        /// <param name="coupon_number"></param>
        /// <returns></returns>
        [Route("Member/SetCouponProcess")]
        [HttpPost]
        public JsonResult SetCouponProcess(string coupon_number)
        {
            string result = String.Empty;
            string message = String.Empty;

            string UserID = User.FindFirst("Id").Value;
            string UserIP = UrlHelper.GetIP(_accessor.HttpContext.Connection.RemoteIpAddress);

            TB_Serial_Coupon_Publish model = new TB_Serial_Coupon_Publish();
            model.Use_YN = "N";
            model.Coupon_Number = coupon_number;
            model.Regist_User_ID = model.Update_User_ID = UserID;
            model.Regist_DateTime = model.Update_DateTime = DateTime.Now;
            model.Regist_IP = model.Update_IP = UserIP;

            if (_couponRepository.SetCouponProcess(model) > 0)
            {
                result = "Y";
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message });
        }

        #endregion

        #region GuestBook

        [Route("Member/Guestbook")]
        public IActionResult Guestbook(int InvitationId, int? Page, int? PageScale)
        {
            Page = Page ?? 1;
            PageScale = PageScale ?? 10;

            List<TB_GuestBook> list = _mcardRepository.UserGuestBookList(InvitationId, User.FindFirst("Id").Value);

            if (list == null)
            {
                list = new List<TB_GuestBook>();
            }

            ViewBag.InvitationId = InvitationId;
            ViewBag.TotalCount = list.Count();
            ViewBag.list = list.ToPagedList((int)Page, (int)PageScale);
            ViewBag.Page = Page;
            ViewBag.PageScale = PageScale;

            return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request));
        }

        [Route("Member/Guestbook/Display")]
        public IActionResult GuestbookDisplay(int InvitationId, int GuestbookId, string DisplayYn)
        {
            _mcardRepository.UserGuestbookDisplay(InvitationId, GuestbookId, DisplayYn, User.FindFirst("Id").Value);

            return Ok();
        }

        [Route("Member/Guestbook/Remove")]
        public IActionResult GuestbookRemove(int InvitationId, int GuestbookId)
        {
            Boolean result = _mcardRepository.RemoveUserGuestbook(InvitationId, GuestbookId, User.FindFirst("Id").Value);

            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            dicResult.Add("result", result);

            return Ok(dicResult);
        }
        #endregion

        #region 취소/환불

        /// <summary>
        /// 마이페이지 - 상세페이지에서 고객이 직접 결제취소/환불 신청(무통장 제외)
        /// 전달 받는 값중 Order_ID 만 사용함. 나머지 값은 보안위배 됨.
        /// 취소 불가 검증 코드 추가 
        /// </summary>
        /// <param name="Refund_Type_Code"></param>
        /// <param name="Trading_Number"></param>
        /// <param name="id"></param>
        /// <param name="Refund_Price"></param>
        /// <returns></returns>
        [Route("Member/User_Refund/{id}")]
        public async Task<IActionResult> User_Refund(int id)
        {
            var result = new JsonReusltRefundSaveModel { status = false, message1 = "결제 취소/환불 실패 하였습니다", message2 ="" };
            string userID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
            string userName = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

            try
            {
                var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
                var now = DateTime.Now;
                var Refund_Type_Code = "";

                //주문번호 오류 검사 안함..  예외 발생 시킴
                var order = await (from o in _barunsonDb.TB_Orders where o.Order_ID == id select o).SingleAsync();
                var invitationId = await (from i in _barunsonDb.TB_Invitations where i.Order_ID == id select i.Invitation_ID).SingleAsync();
                var accountCount = await (from a in _barunsonDb.TB_Accounts 
                                    join r in _barunsonDb.TB_Remits on a.Invitation_ID equals r.Invitation_ID
                                    where a.Invitation_ID == invitationId && r.Total_Price > 10000
                                    select a).CountAsync();

                if (order.Payment_Method_Code == "PMC01") //신용카드
                    Refund_Type_Code = "RTC01";
                else if (order.Payment_Method_Code == "PMC03") //계좌이체
                    Refund_Type_Code = "RTC02";
                else if (order.Payment_Method_Code == "PMC04") //쿠폰취소
                    Refund_Type_Code = "RTC04";
                else if (order.Payment_Method_Code == "PMC02") //가상계좌
                {
                    result.message1 = "무통장입금(가상계좌) 결제 고객은<br> 고객센터 전화(1644 - 0708)를 통해 진행해 주세요. ";
                    result.message2 = "";
                    return Json(result);
                }
                else
                {
                    result.message1 = "결제 취소 요청 오류 발생하였습니다. ";
                    result.message2 = "고객센터 전화(1644 - 0708)를 통해 진행해 주세요.";
                    return Json(result);
                }
                
                var Trading_Number = order.Trading_Number;
                int Refund_Price = order.Payment_Price ?? 0;

                #region 취소 불가 검토 
                var isCancel = CheckIsCancel(order.Payment_Method_Code, order.Order_DateTime ?? order.Regist_DateTime.Value, order.Deposit_DeadLine_DateTime, accountCount);
                if (!isCancel.Item1)
                {
                    result.message1 = isCancel.Item2;
                    result.message2 = "";
                    return Json(result);
                }
                
                #endregion

                if (Refund_Type_Code == "RTC01" || Refund_Type_Code == "RTC02") //신용카드, 계좌이체 취소 
                {
                    var response = await _tossPay.CancelAsync(Trading_Number, new TossPostPaymentCancel
                    {
                        cancelReason = "고객이 직접 결제취소/환불 신청",
                    });
                    if (response == null || response.cancels == null) 
                    {
                        result.message1 = "결제 취소 요청 오류 발생하였습니다.";
                        result.message2 = "고객센터 전화(1644 - 0708)를 통해 진행해 주세요.";
                        return Json(result);
                    }
                    
                }
                using(var tran = await _barunsonDb.Database.BeginTransactionAsync())
                {
                    var model = new TB_Refund_Info
                    {
                        Order_ID = id,
                        Refund_Type_Code = Refund_Type_Code,
                        Refund_Price = Refund_Price,
                        Refund_Status_Code = "RSC02",       // 환불완료, 
                        Refund_DateTime = now,
                        Regist_DateTime = now,
                        Regist_IP = ipAddr,
                        Regist_User_ID = userID,
                    };

                    _barunsonDb.TB_Refund_Infos.Add(model);

                    order.Payment_Status_Code = "PSC03";
                    order.Cancel_DateTime = now;
                    order.Cancel_Time = now.ToString("HH");

                    //발행 쿠폰이 있을경우 사용 쿠폰 삭제
                    var usedCoupons = await (from cu in _barunsonDb.TB_Order_Coupon_Uses where cu.Order_ID == id select cu).ToListAsync();
                    if (usedCoupons.Any())
                    {
                        foreach (var usedcoupon in usedCoupons)
                        {
                            var couponPublish = await (from cp in _barunsonDb.TB_Coupon_Publishes where cp.Coupon_Publish_ID == usedcoupon.Coupon_Publish_ID select cp).FirstOrDefaultAsync();
                            if (couponPublish != null)
                            {
                                couponPublish.Use_YN = "N";
                                couponPublish.Use_DateTime = null;
                            }
                        }
                        _barunsonDb.TB_Order_Coupon_Uses.RemoveRange(usedCoupons);
                    }

                    await _barunsonDb.SaveChangesAsync();

                    await tran.CommitAsync();

                    result.status = true;
                    result.message1 = Url.Action("Mypage", "Member");
                    result.message2 = "";
                }
            }
            catch (Exception ex)
            {
                _operationRepository.Error_Save_Sql(ex.ToString(), "User_Refund", userID, userName);

            }

            return Json(result);
        }

        #endregion

        /// <summary>
        /// 로그인한 사람과 주문건의 사용자 정보를 조회
        /// </summary>
        /// <param name="Order_ID"></param>
        /// <returns></returns>
        [Route("Member/User_Order_Chk/{gubun?}/{ID?}")]
        public int User_Order_Chk(string gubun , int ID)
        {
           // string ReturnValue = "T";

            string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
            string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";
            string User_Email = !string.IsNullOrEmpty(User.FindFirst("Email").Value) ? User.FindFirst("Email").Value : "";

            TB_Order model = new TB_Order();
            
            if (gubun.Equals("1")) model.Order_ID = ID;
            else model.Previous_Order_ID = ID;

            model.User_ID = User_ID;
            model.Name = User_Name;
            model.Email = User_Email;



            return _ProductRepositoy.User_Order_Chk_Entity(gubun, model);

        }


    }
}
