using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.Data.Invitation;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Order;
using MobileInvitation.Data.Product;
using MobileInvitation.Data.Template;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using MobileInvitation.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileInvitation.Areas.User.Controllers.Order
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class OrderController : Controller
    {
        public readonly PathController _PathController;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IOperationRepository _operationRepository;

        public IHttpContextAccessor _accessor;
        
        private readonly barunsonContext _barunson;
        private readonly ITossPaymentService _tossPay;
        private readonly BarunnConfig _barunnConfig;


        public OrderController(PathController PathController, IOrderRepository orderRepository, IProductRepository productRepository, ITemplateRepository templateRepository, 
            IInvitationRepository invitationRepository, IHttpContextAccessor accessor, IOperationRepository operationRepository,
            barunsonContext barunsonContext, BarunnConfig barunnConfig, ITossPaymentService tossPaymentService)
        {
            _PathController = PathController;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _templateRepository = templateRepository;
            _invitationRepository = invitationRepository;
            _operationRepository = operationRepository;
            _accessor = accessor;

            _barunson = barunsonContext;
            _barunnConfig = barunnConfig;
            _tossPay = tossPaymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Private Function, 
        /// <summary>
        /// 주문 정보와 로그인(비회원 포함) 사용자 일치 여부 검사
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oName"></param>
        /// <param name="oEMail"></param>
        /// <returns></returns>
        private bool CheckUser(string userId, string oName, string oEMail)
        {
            var result = false;
            if (User.IsInRole("Guest")) //비회원
            {
                var name = User.FindFirst("Name").Value;
                var email = User.FindFirst("Email").Value;
                result = (oName.Equals(name, StringComparison.InvariantCultureIgnoreCase) &&
                            oEMail.Replace("%", "@").Equals(email.Replace("%","@"), StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                result = (User.FindFirst("Id").Value == userId);
            }
            return result;
        }
        /// <summary>
        /// 비회원으로 주문시 주문자이름과 주문자이메일 입력값이 키값이 되기때문에 주문완료할때마다 비회원고객의 로그인 정보(이름&이메일)를 갱신해줘야됨
        /// EX) 비회원으로 로그인(이름/아이디 입력)하지 않고  비회원 주문 버튼을 클릭해서 들어온 후에 주문한뒤 바로 마이페이지로 접속하게되면 주문한 데이터가 보이지가 않음    
        /// </summary>
        private void Temporary_Logut_Login(TB_Order order, TB_Invitation_Detail detail)
        {
            #region 기존 정보 로그아웃 
            HttpContext.SignOutAsync("userAuth");

            Response.Cookies.Delete("u_auth_nx");
            Response.Cookies.Delete("u_auth_ix");
            Response.Cookies.Delete("u_auth_ex");
            Response.Cookies.Delete("u_auth_ax");
            Response.Cookies.Delete("u_auth_dx");
            #endregion

            //주문시 입력한 주문자명과 이메일주소로 다시 강제 로그인 처리 

            string AUTH = "Guest";

            string Wedding_Date = detail.WeddingDate;
            string Wedding_MM = detail.WeddingMM;

            var claims = new List<Claim>
            {
                new Claim("Id", ""),
                new Claim("Name", order.Name),
                new Claim("Email", string.IsNullOrEmpty(order.Email) ? "" : order.Email),
                new Claim("Hp",  string.IsNullOrEmpty(order.CellPhone_Number) ? "" : order.CellPhone_Number),
                new Claim("Wedding_Date", string.IsNullOrEmpty(Wedding_Date) ? "" : Wedding_Date),
                new Claim(ClaimTypes.Role, AUTH),

            };

            var ci = new ClaimsIdentity(claims, "userAuth");
            var authenticationProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                IssuedUtc = DateTimeOffset.UtcNow,
                IsPersistent = true
            };
            HttpContext.SignInAsync("userAuth"
            , new ClaimsPrincipal(ci), authenticationProperties); // 옵션

            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Domain = ".barunsonmcard.com",
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };

        }

        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (dir.Exists)
            {

                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    string tempPath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(tempPath, true);
                }

            }
        }

        private string getDayOfWeek(DateTime dt)
        {
            string result = "";

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = "월";
                    break;
                case DayOfWeek.Tuesday:
                    result = "화";
                    break;
                case DayOfWeek.Wednesday:
                    result = "수";
                    break;
                case DayOfWeek.Thursday:
                    result = "목";
                    break;
                case DayOfWeek.Friday:
                    result = "금";
                    break;
                case DayOfWeek.Saturday:
                    result = "토";
                    break;
                case DayOfWeek.Sunday:
                    result = "일";
                    break;
            }

            return result;
        }


        #endregion

        #region 공통 주문 단계별 Action method
        /// <summary>
        /// 주문 시작, 기본 주문 정보 생성
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/Regist/{Product_Id?}/{Device_Type?}")]
        public IActionResult Regist(int Product_Id, string Device_Type)
        {
            string Step_URL = string.Empty;
            try
            {
                var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();;
                #region 주문 저장

                var productQuery = from product in _barunson.TB_Products
                                   where product.Product_ID == Product_Id
                                   && product.Display_YN == "Y"
                                   select product;
                                   
                var prodItem = productQuery.FirstOrDefault();

                //상품이 없거나, 비공계 상태 인경우
                if (prodItem == null)
                    return RedirectToAction("index", "Main");

                var Product_Code = prodItem.Original_Product_Code;
                var Product_Category_Code = prodItem.Product_Category_Code;
          
                var order = new TB_Order
                {
                    Order_Price = prodItem.Price,
                    User_ID = User.FindFirst("Id").Value,
                    Name = User.FindFirst("Name").Value,
                    CellPhone_Number = User.FindFirst("Hp") != null ? User.FindFirst("Hp").Value.Replace("-", "") : "",
                    Email = User.FindFirst("Email").Value,
                    Order_Status_Code = "OSC03", //주문대기
                    Payment_Status_Code = "PSC01", //결재전
                    Regist_User_ID = User.FindFirst("Id").Value,
                    Regist_DateTime = DateTime.Now,
                    Regist_IP = ipAddr,
                    Update_User_ID = User.FindFirst("Id").Value,
                    Update_DateTime = DateTime.Now,
                    Update_IP = ipAddr
                };

                var Order_Id = _orderRepository.TB_Order_Insert_Sql(order);

                #endregion

                #region 주문상품 저장

                var order_product = new TB_Order_Product
                {
                    Order_ID = Order_Id,
                    Product_ID = Product_Id,
                    Product_Type_Code = "MC0001",
                    Item_Price = prodItem.Price,
                    Item_Count = 1,
                    Total_Price = prodItem.Price,
                    Regist_User_ID = User.FindFirst("id").Value,
                    Regist_DateTime = DateTime.Now,
                    Regist_IP = ipAddr,
                    Update_User_ID = User.FindFirst("id").Value,
                    Update_DateTime = DateTime.Now,
                    Update_IP = ipAddr
                };

                _orderRepository.TB_Order_Product_Insert_Sql(order_product);

                #endregion

                #region 초대장 저장

                TB_Invitation invitation = new TB_Invitation
                {
                    Order_ID = Order_Id,
                    Template_ID = prodItem.Template_ID.Value,
                    User_ID = User.FindFirst("id").Value,
                    Regist_User_ID = User.FindFirst("id").Value,
                    Regist_DateTime = DateTime.Now,
                    Regist_IP = ipAddr,
                    Update_User_ID = User.FindFirst("id").Value,
                    Update_DateTime = DateTime.Now,
                    Update_IP = ipAddr
                };

                int Invitation_Id = _invitationRepository.TB_Invitation_Insert_Sql(invitation);

                #endregion

                #region TAX 저장
                _invitationRepository.SP_T_TAX_Save_Sql(Invitation_Id);
                #endregion

                #region 초대장 상세 저장

                var template_details = _templateRepository.TB_Template_Detail_Entity(invitation.Template_ID);

                TB_Invitation_Detail invitation_detail = new TB_Invitation_Detail();

                if (template_details.Count > 0)
                {
                    invitation_detail.Invitation_ID = Invitation_Id;

                    //Uniqueue URL 생성
                    Random generator = new Random();
                    const string chars = "abcdefghijklmnopqrstuvwxyz";
                    for (int cnt = 0; cnt < 6; cnt++)
                    {
                        var iurl = new string(Enumerable.Repeat(chars, 6).Select(s => s[generator.Next(s.Length)]).ToArray());
                        iurl += generator.Next(0, 1000000).ToString("D6");

                        var isOk = _invitationRepository.CheckDuplicateURL(iurl);
                        if (isOk)
                        {
                            invitation_detail.Invitation_URL = iurl;
                            break;
                        }
                    }

                    invitation_detail.Invitation_Title = " ";

                    invitation_detail.Outline_Type_Code = "OTC01"; //default : 지도
                    invitation_detail.Gallery_Type_Code = "GTC02"; //default : 바둑판
                    invitation_detail.Invitation_Video_Type_Code = "VTC01"; //default : 유튜브

                    invitation_detail.Groom_Parents1_Title = "아버지";
                    invitation_detail.Groom_Parents2_Title = "어머니";
                    invitation_detail.Bride_Parents1_Title = "아버지";
                    invitation_detail.Bride_Parents2_Title = "어머니";


                    invitation_detail.Groom_Global_Phone_Number = "+82";
                    invitation_detail.Bride_Global_Phone_Number = "+82";
                    invitation_detail.Groom_Parents1_Global_Phone_Number = "+82";
                    invitation_detail.Groom_Parents2_Global_Phone_Number = "+82";
                    invitation_detail.Bride_Parents1_Global_Phone_Number = "+82";
                    invitation_detail.Bride_Parents2_Global_Phone_Number = "+82";

                    invitation_detail.Greetings = template_details[0].Greetings;

                    invitation_detail.Groom_Name = " ";
                    invitation_detail.Bride_Name = " ";

                    string Wedding_Date = DateTime.Now.ToString("yyyy-MM-dd");
                    string Time_Type_Code = "오전";
                    string WeddingHHmm = "0000";
                    if (User.FindFirst("Wedding_Date") != null && User.FindFirst("Wedding_Date").Value != "" && Product_Category_Code != "PCC03")
                    {
                        Wedding_Date = User.FindFirst("Wedding_Date").Value;

                    }
                    string[] WDArray = Wedding_Date.Split('-');
                    string WeddingYY = WDArray[0];
                    string WeddingMM = WDArray[1];
                    string WeddingDD = WDArray[2];
                    string WeddingWeek = getDayOfWeek(Convert.ToDateTime(Wedding_Date));

                    string WeddingHour = User.FindFirst("Wedding_HH")?.Value;
                    string WeddingMin = User.FindFirst("Wedding_MM")?.Value;
                    if (string.IsNullOrEmpty(WeddingHour))
                        WeddingHour = "00";
                    if (string.IsNullOrEmpty(WeddingMin))
                        WeddingMin = "00";

                    if (User.FindFirst("Wedding_PM_AM")?.Value == "PM" && Int32.Parse(WeddingHour) > 12)
                    {
                        Time_Type_Code = "오후";
                        WeddingHour = (Int32.Parse(WeddingHour) - 12).ToString();
                    }

                    WeddingHHmm = WeddingHour + WeddingMin;
                    invitation_detail.WeddingYY = WeddingYY;
                    invitation_detail.WeddingMM = WeddingMM;
                    invitation_detail.WeddingDD = WeddingDD;
                    invitation_detail.WeddingWeek = WeddingWeek;
                    invitation_detail.WeddingHour = WeddingHour;
                    invitation_detail.WeddingMin = WeddingMin;

                    invitation_detail.WeddingHHmm = WeddingHHmm;
                    invitation_detail.WeddingDate = Wedding_Date;
                    invitation_detail.Time_Type_Code = Time_Type_Code;
                    invitation_detail.Weddinghall_Name = " ";

                    invitation_detail.WeddingWeek_Eng_YN = template_details[0].WeddingWeek_Eng_YN;
                    invitation_detail.Time_Type_Eng_YN = template_details[0].Time_Type_Eng_YN;

                    invitation_detail.Regist_User_ID = User.FindFirst("id").Value;
                    invitation_detail.Regist_DateTime = DateTime.Now;
                    invitation_detail.Regist_IP = ipAddr;
                    invitation_detail.Update_User_ID = User.FindFirst("id").Value;
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = ipAddr;

                    //청첩장은 화환 기본 설정
                    if (Product_Category_Code == "PCC01")
                    {
                        invitation_detail.Flower_gift_YN = "Y";
                    }
                }

                _invitationRepository.TB_Invitation_Detail_Insert_Sql(invitation_detail);

                #endregion

                #region 초대장 영역 저장

                var template_areas = _templateRepository.TB_Template_Area_Entity(invitation.Template_ID);

                foreach (var item in template_areas)
                {
                    TB_Invitation_Area invitation_area = new TB_Invitation_Area
                    {
                        Invitation_ID = Invitation_Id,
                        Area_ID = item.Area_ID,
                        Size_Height = item.Size_Height,
                        Size_Width = item.Size_Width,
                        Color = item.Color,
                        Sort = item.Sort,
                        Regist_User_ID = User.FindFirst("id").Value,
                        Regist_DateTime = DateTime.Now,
                        Regist_IP = ipAddr,
                        Update_User_ID = User.FindFirst("id").Value,
                        Update_DateTime = DateTime.Now,
                        Update_IP = ipAddr
                    };

                    _invitationRepository.TB_Invitation_Area_Insert_Sql(invitation_area);
                }
                #endregion

                #region 초대장 경로에 리소스 복사 
                string source_path = _PathController.Upload_Path() + "\\template\\" + Product_Code;
                string dest_path = _PathController.Upload_Path() + "\\invitation\\" + DateTime.Now.ToString("yyMMdd") + "\\" + Invitation_Id;

                DirectoryCopy(source_path, dest_path);

                #endregion

                #region 초대장 아이템 리소스 저장

                var template_items = _templateRepository.TB_Template_Item_LIst(invitation.Template_ID);

                foreach (var item in template_items)
                {
                    var item_resources = _templateRepository.TB_Item_Resource_LIst(item.Resource_ID);

                    int Resource_ID = 0;
                    foreach (var resource in item_resources)
                    {
                        TB_Item_Resource item_Resource = new TB_Item_Resource();

                        item_Resource.CharacterSet = resource.CharacterSet;
                        item_Resource.Character_Size = resource.Character_Size;
                        item_Resource.Color = resource.Color;
                        item_Resource.Background_Color = resource.Background_Color;
                        item_Resource.Bold_YN = resource.Bold_YN;
                        item_Resource.Italic_YN = resource.Italic_YN;
                        item_Resource.Underline_YN = resource.Underline_YN;
                        item_Resource.BetweenText = resource.BetweenText;
                        item_Resource.BetweenLine = resource.BetweenLine;
                        item_Resource.Vertical_Alignment = resource.Vertical_Alignment;
                        item_Resource.Horizontal_Alignment = resource.Horizontal_Alignment;
                        item_Resource.Sort = resource.Sort;
                        item_Resource.Font = resource.Font;

                        string Resource_URL = string.Empty;
                        if (!string.IsNullOrEmpty(resource.Resource_URL))
                        {
                            if (resource.Resource_URL.Contains("/template/"))
                            {
                                string filename = resource.Resource_URL.Substring(resource.Resource_URL.LastIndexOf("/") + 1, resource.Resource_URL.Length - resource.Resource_URL.LastIndexOf("/") - 1);
                                Resource_URL = "/upload/invitation/" + DateTime.Now.ToString("yyMMdd") + "/" + Invitation_Id + "/" + filename;
                            }
                            else
                            {
                                Resource_URL = resource.Resource_URL;
                            }
                        }


                        item_Resource.Resource_URL = Resource_URL;
                        item_Resource.Resource_Height = resource.Resource_Height;
                        item_Resource.Resource_Width = resource.Resource_Width;
                        item_Resource.Resource_Type_Code = resource.Resource_Type_Code;

                        item_Resource.Regist_User_ID = User.FindFirst("id").Value;
                        item_Resource.Regist_DateTime = DateTime.Now;
                        item_Resource.Regist_IP = ipAddr;
                        item_Resource.Update_User_ID = User.FindFirst("id").Value;
                        item_Resource.Update_DateTime = DateTime.Now;
                        item_Resource.Update_IP = ipAddr;

                        Resource_ID = _invitationRepository.TB_Item_Resource_Insert_Sql(item_Resource);
                    }

                    TB_Invitation_Item invitation_Item = new TB_Invitation_Item
                    {
                        Invitation_ID = Invitation_Id,
                        Resource_ID = Resource_ID,
                        Area_ID = item.Area_ID,
                        Item_Type_Code = item.Item_Type_Code,
                        Location_Top = item.Location_Top,
                        Location_Left = item.Location_Left,
                        Size_Height = item.Size_Height,
                        Size_Width = item.Size_Width,
                        Regist_User_ID = User.FindFirst("id").Value,
                        Regist_DateTime = DateTime.Now,
                        Regist_IP = ipAddr,
                        Update_User_ID = User.FindFirst("id").Value,
                        Update_DateTime = DateTime.Now,
                        Update_IP = ipAddr
                    };

                    _invitationRepository.TB_Invitation_Item_Insert_Sql(invitation_Item);
                }

                #endregion

                IActionResult result = null;

                switch (Product_Category_Code)
                {
                    case "PCC01":   //청첩장
                        result = RedirectToAction("McardStep1", "Order", new { Order_Id = Order_Id });
                        break;
                    case "PCC02":   //감사장
                        result = RedirectToAction("MthanksStep1", "Order", new { Order_Id = Order_Id });
                        break;
                    case "PCC03":   //돌잔치
                        result = RedirectToAction("MDollStep1", "Order", new { Order_Id = Order_Id });
                        break;
                    default:
                        result = RedirectToAction("index", "Main");
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
                string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

                _operationRepository.Error_Save_Sql(ex.ToString(), "Regist", User_ID, User_Name);
                return RedirectToAction("index", "Main");
            }
        }

        /// <summary>
        /// 주문 1번째 스텝
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <param name="Product_Category_Code"></param>
        /// <returns></returns>
        private IActionResult Step1(int Order_Id, string Product_Category_Code)
        {
            ViewData["Order_Id"] = Order_Id;

            ViewData["Outline_Type_Code"] = "OTC01";

            var order = _invitationRepository.StepOrder_Sql(Order_Id, Product_Category_Code);

            if (order != null && CheckUser(order.User_ID, order.Name, order.Email))
            {
                ViewBag.Invitation = _invitationRepository.TB_Invitation_Entity(Order_Id);

                string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";

                ViewData["User_ID"] = User_ID;
                ViewData["Name"] = order.Name;
                ViewData["CellPhone_Number"] = order.CellPhone_Number;

                ViewData["Email_Account"] = string.Empty;
                ViewData["Email_Address"] = string.Empty;

                if (!string.IsNullOrWhiteSpace(order.Email))
                {
                    string str = order.Email.Replace("%", "@");
                    string[] result = str.Split(new char[] { '@' });
                    ViewData["Email_Account"] = result[0];
                    ViewData["Email_Address"] = result[1];
                }

                ViewData["KakaoBankingUrl"] = Url.ActionLink("Account", "KakaoRemit", new { OrderId = order.Order_ID });

                if (ViewBag.Invitation.Count > 0)
                {
                    ViewData["Invitation_Id"] = ViewBag.Invitation[0].Invitation_ID;

                    int Invitation_Id = Int32.Parse(ViewData["Invitation_Id"].ToString());


                    ViewBag.invitation_detail = _invitationRepository.TB_Invitation_Detail_Entity(Invitation_Id);

                    ViewData["Objects"] = JsonConvert.SerializeObject(_invitationRepository.TB_Invitation_Item_Resource_LIst(Invitation_Id));

                    ViewData["ETCs"] = JsonConvert.SerializeObject(_invitationRepository.TB_Invitation_Detail_Etc_List(Invitation_Id));

                    ViewData["Banks"] = JsonConvert.SerializeObject(_invitationRepository.TB_Bank_List());

                    ViewData["Sender"] = JsonConvert.SerializeObject(_invitationRepository.TB_Account_List(Product_Category_Code));
                    ViewData["GroomSender"] = JsonConvert.SerializeObject(_invitationRepository.TB_Account_List_By_Extra_Code("1"));
                    ViewData["BrideSender"] = JsonConvert.SerializeObject(_invitationRepository.TB_Account_List_By_Extra_Code("2"));

                    ViewData["Accounts"] = JsonConvert.SerializeObject(_invitationRepository.TB_Account_Extra_List(Invitation_Id));
                    ViewData["GroomAccounts"] = JsonConvert.SerializeObject(_invitationRepository.TB_Invitation_Account_List(Invitation_Id, 1));
                    ViewData["BrideAccounts"] = JsonConvert.SerializeObject(_invitationRepository.TB_Invitation_Account_List(Invitation_Id, 2));


                    ViewBag.template = _templateRepository.TB_Template_Entity(Int32.Parse(ViewBag.Invitation[0].Template_ID.ToString()));


                    if (ViewBag.template.Count > 0)
                    {
                        ViewData["Photo_YN"] = ViewBag.template[0].Photo_YN;

                        if (ViewData["Photo_YN"].ToString() == "Y")
                        {
                            //포토청첩장
                            ViewBag.PhotoImage = _invitationRepository.PhotoImage_Entity(Invitation_Id);

                            ViewData["Photo_Max_Width"] = ViewBag.PhotoImage.width;
                            ViewData["Photo_Max_Height"] = ViewBag.PhotoImage.height;
                        }
                    }

                    if (ViewBag.invitation_detail.Count > 0)
                    {
                        ViewData["Invitation_Title"] = ViewBag.invitation_detail[0].Invitation_Title.ToString().Trim();
                        string Invitation_URL = string.Empty;

                        Invitation_URL = ViewBag.invitation_detail[0].Invitation_URL;

                        if (Invitation_URL.Length > 12)
                        {
                            ViewData["Invitation_URL"] = string.Empty;
                        }
                        else
                        {
                            ViewData["Invitation_URL"] = Invitation_URL;
                        }

                        ViewData["SNS_Image_URL"] = ViewBag.invitation_detail[0].SNS_Image_URL;
                        ViewData["SNS_Image_Height"] = ViewBag.invitation_detail[0].SNS_Image_Height;
                        ViewData["SNS_Image_Width"] = ViewBag.invitation_detail[0].SNS_Image_Width;

                        ViewData["Delegate_Image_URL"] = ViewBag.invitation_detail[0].Delegate_Image_URL;
                        ViewData["Delegate_Image_Height"] = ViewBag.invitation_detail[0].Delegate_Image_Height;
                        ViewData["Delegate_Image_Width"] = ViewBag.invitation_detail[0].Delegate_Image_Width;



                        ViewData["Greetings"] = ViewBag.invitation_detail[0].Greetings;
                        ViewData["Groom_Name"] = ViewBag.invitation_detail[0].Groom_Name != null ? ViewBag.invitation_detail[0].Groom_Name.Trim() : "";
                        ViewData["Groom_Global_Phone_YN"] = ViewBag.invitation_detail[0].Groom_Global_Phone_YN;
                        ViewData["Groom_Global_Phone_Number"] = ViewBag.invitation_detail[0].Groom_Global_Phone_Number;
                        ViewData["Groom_Phone"] = ViewBag.invitation_detail[0].Groom_Phone;
                        ViewData["Bride_Name"] = ViewBag.invitation_detail[0].Bride_Name != null ? ViewBag.invitation_detail[0].Bride_Name.Trim() : "";
                        ViewData["Bride_Global_Phone_YN"] = ViewBag.invitation_detail[0].Bride_Global_Phone_YN;
                        ViewData["Bride_Global_Phone_Number"] = ViewBag.invitation_detail[0].Bride_Global_Phone_Number;
                        ViewData["Bride_Phone"] = ViewBag.invitation_detail[0].Bride_Phone;
                        ViewData["Groom_Parents1_Name"] = ViewBag.invitation_detail[0].Groom_Parents1_Name;
                        ViewData["Groom_Parents1_Global_Phone_Number_YN"] = ViewBag.invitation_detail[0].Groom_Parents1_Global_Phone_Number_YN;
                        ViewData["Groom_Parents1_Global_Phone_Number"] = ViewBag.invitation_detail[0].Groom_Parents1_Global_Phone_Number;
                        ViewData["Groom_Parents1_Phone"] = ViewBag.invitation_detail[0].Groom_Parents1_Phone;
                        ViewData["Groom_Parents2_Name"] = ViewBag.invitation_detail[0].Groom_Parents2_Name;
                        ViewData["Groom_Parents2_Global_Phone_Number_YN"] = ViewBag.invitation_detail[0].Groom_Parents2_Global_Phone_Number_YN;
                        ViewData["Groom_Parents2_Global_Phone_Number"] = ViewBag.invitation_detail[0].Groom_Parents2_Global_Phone_Number;
                        ViewData["Groom_Parents2_Phone"] = ViewBag.invitation_detail[0].Groom_Parents2_Phone;
                        ViewData["Bride_Parents1_Name"] = ViewBag.invitation_detail[0].Bride_Parents1_Name;
                        ViewData["Bride_Parents1_Global_Phone_Number_YN"] = ViewBag.invitation_detail[0].Bride_Parents1_Global_Phone_Number_YN;
                        ViewData["Bride_Parents1_Global_Phone_Number"] = ViewBag.invitation_detail[0].Bride_Parents1_Global_Phone_Number;
                        ViewData["Bride_Parents1_Phone"] = ViewBag.invitation_detail[0].Bride_Parents1_Phone;
                        ViewData["Bride_Parents2_Name"] = ViewBag.invitation_detail[0].Bride_Parents2_Name;
                        ViewData["Bride_Parents2_Global_Phone_Number_YN"] = ViewBag.invitation_detail[0].Bride_Parents2_Global_Phone_Number_YN;
                        ViewData["Bride_Parents2_Global_Phone_Number"] = ViewBag.invitation_detail[0].Bride_Parents2_Global_Phone_Number;
                        ViewData["Bride_Parents2_Phone"] = ViewBag.invitation_detail[0].Bride_Parents2_Phone;

                        ViewData["Groom_Parents1_Title"] = ViewBag.invitation_detail[0].Groom_Parents1_Title;
                        ViewData["Groom_Parents2_Title"] = ViewBag.invitation_detail[0].Groom_Parents2_Title;
                        ViewData["Bride_Parents1_Title"] = ViewBag.invitation_detail[0].Bride_Parents1_Title;
                        ViewData["Bride_Parents2_Title"] = ViewBag.invitation_detail[0].Bride_Parents2_Title;
                        if (ViewBag.invitation_detail[0].WeddingDate == "1900-01-01")
                        {
                            ViewData["WeddingDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                            string[] WDArray = ViewData["WeddingDate"].ToString().Split('-');
                            string WeddingYY = WDArray[0];
                            string WeddingMM = WDArray[1];
                            string WeddingDD = WDArray[2];
                            ViewData["WeddingYY"] = WeddingYY;
                            ViewData["WeddingMM"] = WeddingMM;
                            ViewData["WeddingDD"] = WeddingDD;
                            ViewData["WeddingWeek"] = getDayOfWeek(DateTime.Now);
                        }
                        else
                        {
                            ViewData["WeddingDate"] = ViewBag.invitation_detail[0].WeddingDate;
                            ViewData["WeddingYY"] = ViewBag.invitation_detail[0].WeddingYY;
                            ViewData["WeddingMM"] = ViewBag.invitation_detail[0].WeddingMM;
                            ViewData["WeddingDD"] = ViewBag.invitation_detail[0].WeddingDD;
                            ViewData["WeddingWeek"] = ViewBag.invitation_detail[0].WeddingWeek;

                        }
                        ViewData["Time_Type_Code"] = ViewBag.invitation_detail[0].Time_Type_Code;
                        ViewData["WeddingHHmm"] = ViewBag.invitation_detail[0].WeddingHHmm != null ? ViewBag.invitation_detail[0].WeddingHHmm.Trim() : "";
                        ViewData["WeddingHour"] = ViewBag.invitation_detail[0].WeddingHour != null ? ViewBag.invitation_detail[0].WeddingHour.Trim() : "";
                        ViewData["WeddingMin"] = ViewBag.invitation_detail[0].WeddingMin != null ? ViewBag.invitation_detail[0].WeddingMin.Trim() : "";


                        ViewData["Weddinghall_Name"] = ViewBag.invitation_detail[0].Weddinghall_Name != null ? ViewBag.invitation_detail[0].Weddinghall_Name.Trim() : "";
                        ViewData["WeddingHallDetail"] = ViewBag.invitation_detail[0].WeddingHallDetail;
                        ViewData["Weddinghall_Address"] = ViewBag.invitation_detail[0].Weddinghall_Address;
                        ViewData["Weddinghall_PhoneNumber"] = ViewBag.invitation_detail[0].Weddinghall_PhoneNumber;

                        ViewData["Location_LAT"] = ViewBag.invitation_detail[0].Location_LAT;
                        ViewData["Location_LOT"] = ViewBag.invitation_detail[0].Location_LOT;

                        ViewData["Outline_Type_Code"] = ViewBag.invitation_detail[0].Outline_Type_Code;
                        ViewData["Outline_Image_URL"] = ViewBag.invitation_detail[0].Outline_Image_URL;

                        ViewData["GuestBook_Use_YN"] = ViewBag.invitation_detail[0].GuestBook_Use_YN;
                        ViewData["Etc_Information_Use_YN"] = ViewBag.invitation_detail[0].Etc_Information_Use_YN;
                        ViewData["Parents_Information_Use_YN"] = ViewBag.invitation_detail[0].Parents_Information_Use_YN;
                        ViewData["MoneyGift_Remit_Use_YN"] = ViewBag.invitation_detail[0].MoneyGift_Remit_Use_YN;
                        ViewData["MoneyAccount_Remit_Use_YN"] = ViewBag.invitation_detail[0].MoneyAccount_Remit_Use_YN;
                        ViewData["MoneyAccount_Div_Use_YN"] = ViewBag.invitation_detail[0].MoneyAccount_Div_Use_YN;
                        ViewData["Invitation_Video_Use_YN"] = ViewBag.invitation_detail[0].Invitation_Video_Use_YN;
                        ViewData["Gallery_Use_YN"] = ViewBag.invitation_detail[0].Gallery_Use_YN;
                        ViewData["Flower_gift_YN"] = (ViewBag.invitation_detail[0].Flower_gift_YN == "C") ? "N" : ViewBag.invitation_detail[0].Flower_gift_YN;

                        ViewData["Gallery_Type_Code"] = ViewBag.invitation_detail[0].Gallery_Type_Code;
                        ViewData["Invitation_Video_Type_Code"] = ViewBag.invitation_detail[0].Invitation_Video_Type_Code;

                        ViewData["Time_Type_Eng_YN"] = ViewBag.invitation_detail[0].Time_Type_Eng_YN;
                        ViewData["WeddingWeek_Eng_YN"] = ViewBag.invitation_detail[0].WeddingWeek_Eng_YN;

                        ViewData["DetailNewLineYN"] = ViewBag.invitation_detail[0].DetailNewLineYN;

                    }
                }

                ViewData["upload_path"] = "/upload/invitation/" + order.Order_Code.Substring(1, 6) + "/" + ViewData["Invitation_Id"];

                // 돌잔치
                if (Product_Category_Code == "PCC03")
                {
                    if (!string.IsNullOrEmpty(ViewBag.invitation_detail[0].ExtendData))
                    {
                        ViewData["BabyInfos"] = ViewBag.invitation_detail[0].ExtendData;
                    }
                    else
                    {
                        var babyinfos = new List<BabyFirstBirthViewModel>();
                        babyinfos.Add(new BabyFirstBirthViewModel
                        {
                            Name = "",
                            Birthday = DateTime.Now.Date,
                            idx = 1,
                            ExtraInfos = new List<BabyFirstBirthExtraInfo>()
                        });
                        ViewData["BabyInfos"] = JsonConvert.SerializeObject(babyinfos);
                    }
                }

                return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));

            }
            else
                return RedirectToAction("index", "Main");
        }

        /// <summary>
        /// 주문 1번째 스텝 저장
        /// </summary>
        /// <param name="order"></param>
        /// <param name="invitation_detail"></param>
        /// <param name="invitation_detail_etcs"></param>
        /// <param name="account_extras"></param>
        /// <param name="babyInfos"></param>
        /// <returns></returns>
        private IActionResult Step1Save(TB_Order order
            , TB_Invitation_Detail invitation_detail
            , List<TB_Invitation_Detail_Etc> invitation_detail_etcs
            , List<TB_Account_Extra> account_extras
            , List<TB_Invitation_Account> invitation_accounts = null
            , string ExtendData = null)
        {
            var result = new JsonOrderSaveResultModel
            {
                success = "N",
                result = "",
                message = "",
                auth = User.Identity.IsAuthenticated
            };
            
            try
            {
                if (result.auth)
                {
                    var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();;
                    var userId = User.FindFirst("Id").Value;
                    var updateId = userId;
                    //  비회원 주문 버튼을 클릭해서 들어온 후에 주문하는 경우에는 로그인 쿠키정보가 빈값으로 설정되어 
                    //  마이페이지 접속시 상품 노출이 불가
                    if (User.IsInRole("Guest") && !string.Equals(User.FindFirst("name").Value, order.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Temporary_Logut_Login(order, invitation_detail);
                        updateId = order.Name;
                    }
                    var userName = User.FindFirst("name").Value;

                    var invitation_detailes = _invitationRepository.TB_Invitation_Detail_Entity(invitation_detail.Invitation_ID);

                    string ORG_Invitation_URL = invitation_detailes[0].Invitation_URL;

                    //URL 변경시 중복 확인
                    if (ORG_Invitation_URL != invitation_detail.Invitation_URL)
                    {
                        var inv_detail = _invitationRepository.CheckDuplicateURL_Entity(invitation_detail.Invitation_URL);

                        if (inv_detail != null)
                        {
                            result.message = "duplicate";
                            return Json(result);
                        }
                    }

                    order.User_ID = userId;
                    order.Email = order.Email.Replace("%", "@");

                    //주문 수정
                    order.Update_User_ID = updateId;
                    order.Update_DateTime = DateTime.Now;
                    order.Update_IP = ipAddr;

                    _orderRepository.TB_Order_Update_Sql(order);

                    //초대장 상세 수정
                    invitation_detail.Update_User_ID = updateId;
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = ipAddr;

                    //기타 확장 정보 추가 (예:아기정보)
                    invitation_detail.ExtendData = ExtendData;

                    _invitationRepository.TB_Invitation_Detail_Update_Sql(invitation_detail);

                    //기타정보삭제
                    _invitationRepository.TB_Invitation_Detail_Etc_Delete_Sql(invitation_detail.Invitation_ID);

                    if (invitation_detail.Etc_Information_Use_YN == "Y" && invitation_detail_etcs != null)
                    {
                        //기타정보등록
                        foreach (var item in invitation_detail_etcs)
                        {
                            TB_Invitation_Detail_Etc invitation_detail_etc = new TB_Invitation_Detail_Etc();

                            invitation_detail_etc.Invitation_ID = item.Invitation_ID;
                            invitation_detail_etc.Sort = item.Sort;
                            invitation_detail_etc.Etc_Title = item.Etc_Title;
                            invitation_detail_etc.Information_Content = item.Etc_Title != null && item.Information_Content == null ? "" : item.Information_Content;

                            _invitationRepository.TB_Invitation_Detail_Etc_Sql(invitation_detail_etc);
                        }
                    }

                    //축의금(통합)계좌정보 삭제
                    _invitationRepository.TB_Account_Extra_Delete_Sql(invitation_detail.Invitation_ID);

                    if (invitation_detail.MoneyAccount_Remit_Use_YN == "Y" && account_extras != null)
                    {
                        //축의금(통합)계좌정보 등록
                        foreach (var item in account_extras)
                        {
                            TB_Account_Extra account_extra = new TB_Account_Extra();

                            account_extra.Invitation_ID = item.Invitation_ID;
                            account_extra.Sort = item.Sort;
                            account_extra.Send_Target_Code = item.Send_Target_Code;
                            account_extra.Send_Name = item.Send_Name;
                            account_extra.Bank_Code = item.Bank_Code;
                            account_extra.Account_Number = item.Account_Number;
                            account_extra.Account_Holder = item.Account_Holder;

                            _invitationRepository.TB_Account_Extra_Sql(account_extra);
                        }
                    }
                    //축의금(분리)계좌정보 삭제
                    _invitationRepository.TB_Invitation_Account_Delete_Sql(invitation_detail.Invitation_ID);
                    if (invitation_detail.MoneyAccount_Div_Use_YN == "Y" && invitation_accounts != null)
                    {
                        //축의금(분리)계좌정보 등록
                        foreach (var item in invitation_accounts)
                        {
                            TB_Invitation_Account invitation_account = new TB_Invitation_Account();

                            invitation_account.Invitation_ID = item.Invitation_ID;
                            invitation_account.Sort = item.Sort;
                            invitation_account.Category = item.Category;
                            invitation_account.Send_Target_Code = item.Send_Target_Code;
                            invitation_account.Send_Name = item.Send_Target_Code != "ATC005" ? _invitationRepository.Get_Send_Name(item.Send_Target_Code) : item.Send_Name;
                            invitation_account.Bank_Code = item.Bank_Code;
                            invitation_account.Account_Number = item.Account_Number;
                            invitation_account.Account_Holder = item.Account_Holder;

                            _invitationRepository.TB_Invitation_Account_Sql(invitation_account);
                        }
                    }
                    result.success = "Y";
                    result.result = result.message = "";
                }
            }
            catch (Exception ex)
            {
                string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
                string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

                _operationRepository.Error_Save_Sql(ex.ToString(), "Step1Save", User_ID, User_Name);
                result.success = "N";
                result.result = result.message = "오류가 발생하였습니다. 관리자에게 문의해주세요.";

            }
            return Json(result);
        }

        /// <summary>
        /// 주문 2번째 스텝
        /// </summary>
        /// <param name="Order_Id"></param>
        private IActionResult Step2(int Order_Id, string Product_Category_Code)
        {
            var order = _invitationRepository.StepOrder_Sql(Order_Id, Product_Category_Code);
            ViewBag.Invitation = _invitationRepository.TB_Invitation_Entity(Order_Id);

            if (order != null && ViewBag.Invitation.Count > 0 && CheckUser(order.User_ID, order.Name, order.Email))
            {
                ViewData["Order_Id"] = Order_Id;

                ViewData["Invitation_Id"] = ViewBag.Invitation[0].Invitation_ID;

                ViewBag.invitation_detail = _invitationRepository.TB_Invitation_Detail_Entity(Int32.Parse(ViewData["Invitation_Id"].ToString()));

                if (ViewBag.invitation_detail.Count > 0)
                {
                    ViewData["Gallery_Use_YN"] = ViewBag.invitation_detail[0].Gallery_Use_YN;
                    ViewData["Gallery_Type_Code"] = ViewBag.invitation_detail[0].Gallery_Type_Code;
                    ViewData["Invitation_Video_Use_YN"] = ViewBag.invitation_detail[0].Invitation_Video_Use_YN;
                    ViewData["Invitation_Video_Type_Code"] = ViewBag.invitation_detail[0].Invitation_Video_Type_Code;
                    ViewData["Invitation_Video_URL"] = ViewBag.invitation_detail[0].Invitation_Video_URL;
                    ViewData["GalleryPreventPhoto_YN"] = ViewBag.invitation_detail[0].GalleryPreventPhoto_YN;
                }

                ViewBag.Gallery = _invitationRepository.TB_Gallery_List(Int32.Parse(ViewData["Invitation_Id"].ToString()));

                ViewData["upload_path"] = "/upload/invitation/" + order.Order_Code.Substring(1, 6) + "/" + ViewData["Invitation_Id"] + "/gallery";

                return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));

            }
            else
                return RedirectToAction("index", "Main");

        }

        /// <summary>
        /// 주문 2번째 스텝 저장
        /// </summary>
        /// <param name="invitation_detail"></param>
        /// <returns></returns>
        private JsonResult Step2Save(TB_Invitation_Detail invitation_detail)
        {
            var result = new JsonOrderSaveResultModel
            {
                success = "N",
                result = "",
                message = "",
                auth = User.Identity.IsAuthenticated
            };
            try
            {
                if (result.auth)
                {
                    var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();;
                    //로그인 사용자 검사, 관리자 로그인시 문제
                    var order = _orderRepository.TB_Order_EntityFromInvitation_ID(invitation_detail.Invitation_ID);
                    if (order == null || !CheckUser(order.User_ID, order.Name, order.Email))
                    {
                        result.message = "인증 정보 오류가 발생했습니다.";
                    }
                    else
                    {
                        var userId = User.FindFirst("Id").Value;
                        if (User.IsInRole("Guest"))
                            userId = order.Name;

                        //초대장 상세 수정
                        invitation_detail.Update_User_ID = userId;
                        invitation_detail.Update_DateTime = DateTime.Now;
                        invitation_detail.Update_IP = ipAddr;

                        _invitationRepository.Video_Gallery_Update_Sql(invitation_detail);

                        result.success = "Y";
                    }

                }

            }
            catch { }
            return Json(result);
        }
        
        /// <summary>
        /// 주문 마지막 단계, 상품정보 및 결제 정보 모델 생성
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("Order/StepLast/{orderId}")]
        public async Task<IActionResult> StepLastAsync(int orderId)
        {
            var model = new OrderLastStepModel();
            var query = from o in _barunson.TB_Orders
                        join op in _barunson.TB_Order_Products on o.Order_ID equals op.Order_ID
                        join p in _barunson.TB_Products on op.Product_ID equals p.Product_ID
                        join i in _barunson.TB_Invitations on o.Order_ID equals i.Order_ID
                        where o.Order_ID == orderId
                        select new 
                        {
                            OrderID = i.Order_ID,
                            OrderCode = o.Order_Code,
                            InvitationId = i.Invitation_ID,
                            OrderDateTime = o.Regist_DateTime.Value,
                            ProductCode = p.Product_Code,
                            ProductName = p.Product_Name,
                            ProductPrice = p.Price,
                            TotalPrice = op.Total_Price,
                            o.PG_ID,
                            o.Email,
                            o.Name,
                            o.User_ID,
                            p.Product_Category_Code,
                            o.Payment_Status_Code
                        };

            var item = await query.FirstOrDefaultAsync();

            if (item != null && CheckUser(item.User_ID, item.Name, item.Email))
            {
                if (item.Payment_Status_Code == "PSC02") //결재완료건이면 상세페이지로 이동
                {
                    return RedirectToAction("Order_Detail", "Member", new { Order_Id = orderId });
                }    
                else if (item.Payment_Status_Code == "PSC04") //입금대기건이면 마이페이지로 이동
                {
                    return RedirectToAction("Mypage", "Member");
                }   
                model = new OrderLastStepModel
                {
                    UserID = item.User_ID,
                    OrderID = item.OrderID,
                    OrderCode = item.OrderCode,
                    InvitationId = item.InvitationId,
                    OrderDateTime = item.OrderDateTime,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    TotalPrice = item.TotalPrice,

                    tossClientKey = _tossPay.ClientKey,
                    ComplateUrl = Url.Action("Complete", new { Order_Code = item.OrderCode })
                };


                model.UseCouponList = GetUserCouponList(orderId);

                if (item.Product_Category_Code == "PCC01") //초대장
                {
                    model.BackToUrl = Url.Action("McardStep2", "Order", new { Order_Id = item.OrderID });
                }
                else if (item.Product_Category_Code == "PCC02") //감사장
                {
                    model.BackToUrl = Url.Action("MthanksStep1", "Order", new { Order_Id = item.OrderID });
                }
                else if (item.Product_Category_Code == "PCC03") //돌잔치
                {
                    model.BackToUrl = Url.Action("MDollStep2", "Order", new { Order_Id = item.OrderID });
                }
                return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
            }
            else
            {
                return RedirectToAction("index", "Main");
            }
        }

        /// <summary>
        /// 주문완료, 주문(결제) 완료시 SMS 발송 / 결제완료시 자동쿠폰 발급
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/Complete/{Order_Code?}")]
        public IActionResult Complete(string Order_Code)
        {
            var model = new OrderPayFinalModel
            {
                OrderCode = Order_Code,
                Message = "감사합니다 <br />주문이 성공적으로 완료 되었습니다.",
                UserName = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "",
                BackToUrl = null
            };
            try
            {
                string Order_Path = "";

                if (UrlHelper.IsMobile(Request))
                {
                    Order_Path = "M";
                }
                else
                {
                    Order_Path = "PC";
                }
                _orderRepository.User_Order_Sms_Send_Sql(Order_Code, Order_Path);
            }
            catch  { }

            ViewData["Order_Code"] = Order_Code;
            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }
        #endregion

        #region 결제
        /// <summary>
        /// 결제 정보 확인.
        /// 전액 쿠폰 사용시 주문 완료 처리
        /// 쿠폰, 결제 금액 검증 후 결제 진행하도록 Json 응답
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="CouponPublishID"></param>
        /// <returns></returns>
        [Route("Order/Payment")]
        [HttpPost]
        public IActionResult Payment(int OrderId, int CouponPublishID)
        {
            var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
            var toDay = DateTime.Today;
            var now = DateTime.Now;
            var UserId = User.FindFirst("Id").Value;
            var model = new OrderPaymentModel
            {
                Status = false,
                Message = "결제 실패 하였습니다.",
            };
            try
            {
                var prodQry = from o in _barunson.TB_Orders
                              join op in _barunson.TB_Order_Products on o.Order_ID equals op.Order_ID
                              join p in _barunson.TB_Products on op.Product_ID equals p.Product_ID
                              where o.Order_ID == OrderId
                              select new
                              {
                                  p.Product_Code,
                                  p.Product_ID,
                                  ProductPrice = p.Price,
                                  TotalPrice = op.Total_Price,
                                  o.Order_Code,
                                  o.Email,
                                  o.Name,
                                  o.User_ID,
                              };

                var prodItem = prodQry.FirstOrDefault();

                model = new OrderPaymentModel
                {
                    Status = false,
                    Message = "",
                    OrderID = OrderId,
                    CouponPublishID = 0,        //쿠폰 적용 확인 후 할당
                    TotalPrice = prodItem.TotalPrice.Value,
                    PaymentPrice = prodItem.TotalPrice.Value,
                    CouponPrice = 0,
                    tossRequestPayment = null
                };
                UseCouponInfo coupon = null;
                if (!string.IsNullOrEmpty(UserId) && CouponPublishID > 0) //회원이고 쿠폰 적용시
                {
                    #region 쿠폰 정보 쿼리

                    var couponQry1 = from c in _barunson.TB_Coupons
                                     join cp in _barunson.TB_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                                     where cp.User_ID == UserId
                                     && cp.Coupon_Publish_ID == CouponPublishID
                                     && cp.Retrieve_DateTime == null
                                     && cp.Use_YN == "N"
                                     select new CouponDataInfo(
                                         c.Coupon_ID,
                                         c.Coupon_Name,
                                         cp.Coupon_Publish_ID,
                                         cp.Expiration_Date,
                                         cp.Regist_DateTime,
                                         c.Standard_Purchase_Price,
                                         c.Coupon_Apply_Code,
                                         c.Coupon_Apply_Product_ID,
                                         c.Discount_Method_Code,
                                         c.Discount_Rate,
                                         c.Discount_Price,
                                         c.Period_Method_Code,
                                         c.Publish_Start_Date,
                                         c.Publish_End_Date,
                                        "General"
                                     );
                    var couponQry2 = from c in _barunson.TB_Serial_Coupons
                                     join cp in _barunson.TB_Serial_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                                     where cp.User_ID == UserId
                                     && cp.Coupon_Publish_ID == CouponPublishID
                                     && cp.Retrieve_DateTime == null
                                     && cp.Use_YN == "N"
                                     select new CouponDataInfo(
                                         c.Coupon_ID,
                                         c.Coupon_Name,
                                         cp.Coupon_Publish_ID,
                                         cp.Expiration_Date,
                                         cp.Regist_DateTime,
                                         c.Standard_Purchase_Price,
                                         c.Coupon_Apply_Code,
                                         c.Coupon_Apply_Product_ID,
                                         c.Discount_Method_Code,
                                         c.Discount_Rate,
                                         c.Discount_Price,
                                         c.Period_Method_Code,
                                         c.Publish_Start_Date,
                                         c.Publish_End_Date,
                                         "Serial"
                                     );
                    #endregion

                    var couponItem = couponQry1.FirstOrDefault();
                    if (couponItem == null)
                        couponItem = couponQry2.FirstOrDefault();

                    if (couponItem == null)
                    {
                        model.Status = false;
                        model.Message = "쿠폰 적용에 실패 하였습니다. 선택 하신 쿠폰 정보를 확인 해주세요.";
                        return Json(model);
                    }

                    coupon = GetUseCouponInfo(couponItem, toDay, prodItem.Product_Code, prodItem.TotalPrice.Value);
                    if (!coupon.IsCopuponUsing)
                    {
                        model.Status = false;
                        model.Message = "쿠폰 적용에 실패 하였습니다. 선택 하신 쿠폰 정보를 확인 해주세요.";
                        return Json(model);
                    }
                    model.CouponPublishID = CouponPublishID;
                    model.TotalPrice = coupon.TotalPrice;
                    model.CouponPrice = coupon.CouponPrice;
                    model.PaymentPrice = coupon.PaymentPrice;
                }

                //쿠폰 전액 결제일 경우 바로 완료 처리
                //브라우져에서 Status==true && PaymentPrice == 0 는 완료 페이지로 이동
                if (model.PaymentPrice == 0)
                {
                    using (var tran = _barunson.Database.BeginTransaction())
                    {
                        //주문 업데이트
                        var order = (from o in _barunson.TB_Orders where o.Order_ID == OrderId select o).First();
                        order.Payment_Method_Code = "PMC04";
                        order.Payment_Status_Code = "PSC02";
                        order.Order_Status_Code = "OSC01";
                        order.Order_DateTime = now;
                        order.Update_DateTime = now;
                        order.Update_IP = ipAddr;
                        order.Update_User_ID = UserId;
                        order.Payment_DateTime = now;
                        order.Coupon_Price = model.CouponPrice;
                        order.Order_Path = UrlHelper.IsMobile(Request) ? "M" : "PC";
                        order.Payment_Path = UrlHelper.IsMobile(Request) ? "M" : "PC";

                        //쿠폰 업데이트
                        if (coupon != null)
                        {
                            if (coupon.CouponType == "General")
                            {
                                var couponpublish = (from o in _barunson.TB_Coupon_Publishes where o.Coupon_Publish_ID == CouponPublishID select o).First();
                                couponpublish.Use_YN = "Y";
                                couponpublish.Use_DateTime = now;

                                var ordCoupon = new TB_Order_Coupon_Use
                                {
                                    Order_ID = OrderId,
                                    Coupon_Publish_ID = CouponPublishID,
                                    Discount_Price = model.CouponPrice,
                                    Regist_User_ID = UserId,
                                    Regist_DateTime = now,
                                    Regist_IP = ipAddr,
                                    Update_User_ID = UserId,
                                    Update_DateTime = now,
                                    Update_IP = ipAddr
                                };
                                _barunson.TB_Order_Coupon_Uses.Add(ordCoupon);
                            }
                            else if (coupon.CouponType == "Serial")
                            {
                                var couponpublish = (from o in _barunson.TB_Serial_Coupon_Publishes where o.Coupon_Publish_ID == CouponPublishID select o).First();
                                couponpublish.Use_YN = "Y";
                                couponpublish.Use_DateTime = now;

                                var ordCoupon = new TB_Order_Serial_Coupon_Use
                                {
                                    Order_ID = OrderId,
                                    Coupon_Publish_ID = CouponPublishID,
                                    Discount_Price = model.CouponPrice,
                                    Regist_User_ID = UserId,
                                    Regist_DateTime = now,
                                    Regist_IP = ipAddr,
                                    Update_User_ID = UserId,
                                    Update_DateTime = now,
                                    Update_IP = ipAddr
                                };
                                _barunson.TB_Order_Serial_Coupon_Uses.Add(ordCoupon);
                            }
                        }
                        _barunson.SaveChanges();
                        tran.Commit();
                    }

                    model.Status = true;
                    model.Message = "";
                }
                else
                {
                    model.Status = true;
                    model.Message = "";
                    //결제 정보
                    model.tossRequestPayment = new TossRequestPayment
                    {
                        amount = model.PaymentPrice,
                        orderId = prodItem.Order_Code,
                        orderName = prodItem.Product_Code,
                        successUrl = Url.ActionLink("PaySuccess", "Order"),
                        failUrl = Url.ActionLink("PayFail", "Order"),
                        customerEmail = prodItem.Email,
                        customerName = prodItem.Name
                    };
                    model.IdempotencyKey = Guid.NewGuid().ToString();
					//결제 진행 검증을 위하여 모델을 데이터 베이스에 저장. Key = Order_Code 
					var tempData = (from t in _barunson.TB_Temp_Orders where t.Order_Code == prodItem.Order_Code select t).FirstOrDefault();
                    if (tempData == null) 
                    {
                        tempData = new TB_Temp_Order
                        {
                            Order_Code = prodItem.Order_Code,
                            Coupon_Price = model.CouponPrice,
                            Coupon_Publish_ID = model.CouponPublishID == 0 ? null : model.CouponPublishID,
                        };
                        _barunson.TB_Temp_Orders.Add(tempData);
                    }
                    tempData.PaymentData = JsonConvert.SerializeObject(model);
                    _barunson.SaveChanges();
                }

            }
            catch (Exception ex) 
            {
                model.Status = false;
                model.Message = "결제중 오류가 발생하였습니다. 관리자에게 문의해주세요.";

                string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
                string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

                _operationRepository.Error_Save_Sql(ex.ToString(), "Order/Payment", User_ID, User_Name);
            }

            return Json( model );
        }

        /// <summary>
        /// 결제 성공 콜백
        /// </summary>
        /// <param name="paymentKey">결제키</param>
        /// <param name="orderId">TB_Order의 Order_Code</param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Route("Order/PaySuccess")]
        public async Task<IActionResult> PaySuccess(string paymentKey, string orderId, int amount)
        {
            //기본값 실패
            IActionResult result = RedirectToAction("index", "Main");
            string userID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
            string userName = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

            //기본 파라메터 검사, 누락식 부정 요청으로 home으로 이동
            if (!string.IsNullOrEmpty(paymentKey) && !string.IsNullOrEmpty(orderId) && amount > 0)
            {
                try
                {
                    var tempData = await (from t in _barunson.TB_Temp_Orders where t.Order_Code == orderId select t).FirstOrDefaultAsync();
                    if (tempData != null && !string.IsNullOrEmpty(tempData.PaymentData))
                    {
                        var paymentData = JsonConvert.DeserializeObject<OrderPaymentModel>(tempData.PaymentData);

                        //결제 금액 일치하면 결제승인 프로세스 시작
                        if (paymentData.PaymentPrice == amount)
                        {
                           
                            //승인요청
                            var toss = await _tossPay.ConfirmAsync(new TossPostPaymentConfirm
                            {
                                paymentKey = paymentKey,
                                orderId = orderId,
                                amount = amount
                            }, paymentData.IdempotencyKey);

							//결제 승인 응답, 결재수단이 신용카드/ 계좌이체면 결제완료,  그 외 입금대기
							if (toss.status == "DONE" || toss.status == "WAITING_FOR_DEPOSIT")
                            {
                                #region DB Update
                                var isDbUpdate = await PaySuccessOrderUpdate(toss, userID, userName);

								//db update 실패시 결제 취소 실행
                                if (isDbUpdate)
                                {
									result = RedirectToAction("Complete", "Order", new { Order_Code = orderId});
								}
                                else
                                {
                                    await _tossPay.CancelAsync(toss.paymentKey, new TossPostPaymentCancel
                                    {
                                        cancelReason = "DB처리 실패로 인하여 Rollback 처리",
									}, paymentData.IdempotencyKey);

									result = RedirectToAction("PayFail", "Order", new { orderId = orderId, message = "결제요청이 실패 하였습니다. 자동 결제 취소 되었습니다.", code = "10" });

								}
								#endregion
							}
							else 
                            {
                                result = RedirectToAction("PayFail", "Order", new { orderId = orderId, message = "결제요청이 실패 하였습니다. (결제상태오류)", code = "20" });
                                _operationRepository.Error_Save_Sql($"결제상태오류: paymentKey:{paymentKey}, orderId:{orderId}, amount:{amount}, status:{toss.status}", "PaySuccess", userID, userName);
                            }

                        }
                        else
                        {
                            result = RedirectToAction("PayFail", "Order", new { orderId = orderId, message = "결제요청이 실패 하였습니다. (결제금액불일치)", code = "30" });
                            _operationRepository.Error_Save_Sql($"결제금액불일치: paymentKey:{paymentKey}, orderId:{orderId}, amount:{amount}", "PaySuccess", userID, userName);
                        }
                        //완료시 temp data 삭제
                    }
                    else
                    {
                        result = RedirectToAction("PayFail", "Order", new { orderId = orderId, message = "결제요청이 실패 하였습니다. (결제정보누락)", code = "40" });
                        _operationRepository.Error_Save_Sql($"결제정보누락: paymentKey:{paymentKey}, orderId:{orderId}, amount:{amount}", "PaySuccess", userID, userName);
                    }
                }
                catch (Exception ex)
                {
                    result = RedirectToAction("PayFail", "Order", new { orderId = orderId, message = "결제요청이 실패 하였습니다. (기타오류)", code = "50" });
                    _operationRepository.Error_Save_Sql(ex.ToString(), "PaySuccess", userID, userName);
                }
            }
           
            return result;
        }

        /// <summary>
        /// 결제 성공에 대한 주문 데이터 업데이트
        /// </summary>
        /// <param name="tossData"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task<bool> PaySuccessOrderUpdate(TossPayment tossData, string userID, string userName)
        {
            bool result = false;
			var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();

			try
			{
                var finaceNameList = await (from a in _barunson.TB_Common_Codes where a.Code_Group == "toss_bank" || a.Code_Group == "toss_card" select new { a.Code_Group, a.Code, a.Code_Name }).ToListAsync();

				string paymentMethodCode = "", financeName = "", financeAuthNumber = "", cardInstallment = "", cashReceiptPublishYN = "", nointYN = "" ;
                string payerName = "", escrowYN = "", accountNumber = "";

                if (tossData.card != null) //카드, 간편결제(애플페이,삼성페이,토스 등)
                {
                    paymentMethodCode = "PMC01";
                    financeName = finaceNameList.FirstOrDefault(m => m.Code_Group == "toss_card" && m.Code == tossData.card.acquirerCode)?.Code_Name;    //카드 매입사 코드. 기존은 상호였으나, toss에서 는 금융사 코드로 응답.
                    financeAuthNumber = tossData.card.approveNo; //승인번호
                    cardInstallment = tossData.card.installmentPlanMonths.ToString(); //할부개월수
                    nointYN = tossData.card.isInterestFree.HasValue ? (tossData.card.isInterestFree.Value ? "Y" : "N") : "N"; //무이자여부
                }
                else if (tossData.transfer != null) //계좌이체, 간편결제(토스 등)
                {
                    paymentMethodCode = "PMC03";
                    financeName = finaceNameList.FirstOrDefault(m => m.Code_Group == "toss_bank" && m.Code == tossData.transfer.bankCode)?.Code_Name; ; //은행 코드. 기존은 상호였으나, toss에서 는 금융사 코드로 응답.
                }
                else if (tossData.virtualAccount != null) //가상계좌
                {
                    paymentMethodCode = "PMC02";
                    financeName = finaceNameList.FirstOrDefault(m => m.Code_Group == "toss_bank" && m.Code == tossData.virtualAccount.bankCode)?.Code_Name; //은행 코드. 기존은 상호였으나, toss에서 는 금융사 코드로 응답.
                    payerName = tossData.virtualAccount.customerName;
                    accountNumber = tossData.virtualAccount.accountNumber;
                    financeAuthNumber = tossData.secret;    //가상계좌 웹훅시 데이터 검증용 값.
                }
                else if (tossData.easyPay != null) //간편결제, 다른 결제 정보 없음. 카드로 사용
                {
                    paymentMethodCode = "PMC01";
                    financeName = tossData.easyPay.provider;
                }
               
                cashReceiptPublishYN = tossData.cashReceipt != null ? "Y" : "N";    //현금영수증발급여부
				escrowYN = tossData.useEscrow.HasValue ? (tossData.useEscrow.Value ? "Y" : "N") : "N"; //에스크로 적용 여부 Y : 에스크로 적용, N : 에스크로 미적용

				using (var tran = await _barunson.Database.BeginTransactionAsync())
                {
                    var order = await (from o in _barunson.TB_Orders where o.Order_Code == tossData.orderId select o).FirstAsync();
                    order.Trading_Number = tossData.paymentKey;
                    order.Payment_Method_Code = paymentMethodCode;
                    order.Payment_Status_Code = tossData.status == "DONE" ? "PSC02" : "PSC04";
                    order.Finance_Name = financeName;
                    order.Finance_Auth_Number = financeAuthNumber;
                    order.Account_Number = accountNumber;
                    order.Payer_Name = payerName;
                    order.Card_Installment = cardInstallment;
                    order.Noint_YN = nointYN;
                    order.CashReceipt_Publish_YN = cashReceiptPublishYN;
                    order.Escrow_YN = escrowYN;
                    order.Payment_Price = Convert.ToInt32(tossData.totalAmount);

                    order.Order_Status_Code = "OSC01";  //주문완료

                    order.Order_DateTime = DateTime.ParseExact(tossData.requestedAt, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);
                    order.Deposit_DeadLine_DateTime = order.Order_DateTime.Value.AddHours(72); //무통장입금마감일 & 신용카드,계좌이체 취소마감일 -> 실제 토스쪽으로 넘기는 마감일과 약간의 차이는 존재할수 있음

					if (tossData.status == "DONE")
                        order.Payment_DateTime = DateTime.ParseExact(tossData.approvedAt, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);
                    
					order.Update_DateTime = DateTime.Now;
					order.Update_IP = ipAddr;

					result = await _barunson.SaveChangesAsync() > 0;
                    if (result)
					    await tran.CommitAsync();
                    else
                        await tran.RollbackAsync();

                }
            }
            catch (Exception ex)
            {
                result = false;
                var message = $"결제업데이트실패: paymentKey:{tossData.paymentKey}, orderId:{tossData.orderId}, error:{ex}";
				_operationRepository.Error_Save_Sql(message, "PaySuccessOrderUpdate", userID, userName);
			}

			return result;
        }

        /// <summary>
        /// 결제 실패
        /// </summary>
        /// <param name="code"></param>
        /// <param name="orderId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Route("Order/PayFail")]
        public async Task<IActionResult> PayFail(string code, string orderId, string message)
        {
            var model = new OrderPayFinalModel
            { 
                OrderCode = orderId,
                Message = message,
                UserName = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "",
                BackToUrl = null
            };
            try
            {
                var order = await (from o in _barunson.TB_Orders where o.Order_Code == orderId select new { o.Order_ID }).FirstOrDefaultAsync();
                if (order != null)
                    model.BackToUrl = Url.Action("StepLast", "Order", new { orderId = order.Order_ID });
            }
            catch
            { }
            
            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
        }

        /// <summary>
        /// 가상계좌 상태 변경 웹훅
        /// https://www.barunsonmcard.com/Order/PayDepositCallback
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
		[Route("Order/PayDepositCallback")]
		public async Task<IActionResult> PayDepositCallback()
        {
            bool isSuccess = false;
            try
            {
                TossWebHookDeposit tossData = null;
                using (var reader = new StreamReader(Request.Body))
                {
                    var inputstring = await reader.ReadToEndAsync();
                    tossData = JsonConvert.DeserializeObject<TossWebHookDeposit>(inputstring);
                }
                if (tossData != null)
                {
                    var order = await (from o in _barunson.TB_Orders where o.Order_Code == tossData.orderId select o).FirstOrDefaultAsync();
                    var now = DateTime.Now;
                    //등록된 웹훅 URL에 상점과 토스페이먼츠가 아닌 제 3자에 의한 잘못된 요청이 들어올 수 있습니다.
                    //토스페이먼츠 서버에서 돌아온 올바른 요청이라면 결제 승인 결과로 돌아온 Payment 객체의 secret 값과 가상계좌 웹훅 이벤트 본문으로 돌아온 secret 값이 같습니다.
                    if (order != null && order.Finance_Auth_Number == tossData.secret)
                    {
                        if (tossData.status == "DONE") //입금완료
                        {
                            order.Payment_Status_Code = "PSC02";
                            order.Payment_DateTime = now;
                        }
                        else if (tossData.status == "WAITING_FOR_DEPOSIT") //입금대기
                        {
                            order.Payment_Status_Code = "PSC04";
                            order.Payment_DateTime = null;
                        }
                        else if (tossData.status == "CANCELED") //취소
                        {
                            order.Payment_Status_Code = "PSC05";
                            order.Cancel_DateTime = now;
                            order.Cancel_Time = now.ToString("HH");
                        }
                        order.Update_DateTime = now;
                        await _barunson.SaveChangesAsync();
                        isSuccess = true;
                    }
                }
            }
            catch { }
            if (isSuccess)
			    return Ok();
            else
                return BadRequest();
        }
        #endregion

        #region Coupon
        /// <summary>
        /// 사용자 쿠폰정보, 사용여부, 금액 등 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDay"></param>
        /// <param name="productCode"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        private UseCouponInfo GetUseCouponInfo(CouponDataInfo item, DateTime toDay, string productCode, int totalPrice)
        {
            var m = new UseCouponInfo
            {
                CouponType = item.CouponType,
                CouponID = item.Coupon_ID,
                CouponName = item.Coupon_Name,
                CouponPublishID = item.Coupon_Publish_ID,
                ExpirationDate = !string.IsNullOrEmpty(item.Expiration_Date) ? DateTime.Parse(item.Expiration_Date) : null,
                RegistDateTime = item.Regist_DateTime,
                IsCopuponUsing = false,
                DiscountMethodCode = item.Discount_Method_Code,
                DiscountRate = item.Discount_Rate,
                DiscountPrice = item.Discount_Price,
                PeriodMethodCode = item.Period_Method_Code,
                PublishStartDate = !string.IsNullOrEmpty(item.Publish_Start_Date) ? DateTime.Parse(item.Publish_Start_Date) : null,
                PublishEndDate = !string.IsNullOrEmpty(item.Publish_End_Date) ? DateTime.Parse(item.Publish_End_Date) : null,
            };
            //구매 금액 검사: 제약 없음, 또는 구매금액이 제약금액 보가 크면 사용 가능
            var chek1 = (item.Standard_Purchase_Price == null || item.Standard_Purchase_Price.Value <= totalPrice);

            //만료일 검사: 무제한, 또는 만료일 이내 사용가능
            var chek2 = false;
            if (item.Period_Method_Code == "PMC03") //무제한
                chek2 = true;
            else if (item.Period_Method_Code == "PMC01") //기간입력
                chek2 = (m.PublishStartDate <= toDay && m.PublishEndDate >= toDay);
            else if (item.Period_Method_Code == "PMC02") //발행일로부터 X일
                chek2 = (m.ExpirationDate >= toDay);

            //상품제약 검사 Coupon_Apply_Code : CET01 상품전체 CET02 지정 상품 적용 CET03 지정 상품 제외
            var chek3 = false;
            if (item.Coupon_Apply_Code == "CET01")
                chek3 = true;
            else
            {
                var q1 = from a in _barunson.TB_Apply_Products
                         where a.Product_Apply_ID == item.Coupon_Apply_Product_ID
                         && a.Product_Code == productCode
                         && item.CouponType == "General"
                         select new { a.Product_Apply_ID, a.Product_Code };
                var q2 = from a in _barunson.TB_Serial_Apply_Products
                         where a.Product_Apply_ID == item.Coupon_Apply_Product_ID
                         && a.Product_Code == productCode
                         && item.CouponType == "Serial"
                         select new { a.Product_Apply_ID, a.Product_Code };
                var applyprodItems = q1.Concat(q2).ToList();

                if (item.Coupon_Apply_Code == "CET02")
                    chek3 = applyprodItems.Count > 0;
                else if (item.Coupon_Apply_Code == "CET03")
                    chek3 = applyprodItems.Count == 0;
            }


            //모든 조건이 만족되야 사용가능.
            m.IsCopuponUsing = chek1 & chek2 & chek3;
            m.TotalPrice = totalPrice;
            if (!m.IsCopuponUsing)
            {
                m.CouponPrice = 0;
                m.PaymentPrice = totalPrice;
            }
            else
            {
                if (m.DiscountMethodCode == "DMC01") //금액
                {
                    m.CouponPrice = m.DiscountPrice.Value;
                    m.PaymentPrice = totalPrice - m.DiscountPrice.Value;
                }
                else if (m.DiscountMethodCode == "DMC02") //%
                {
                    m.CouponPrice = Convert.ToInt32(totalPrice * m.DiscountRate / 100);
                    m.PaymentPrice = totalPrice - m.CouponPrice;
                }
                else if (m.DiscountMethodCode == "DMC03") //전액
                {
                    m.CouponPrice = totalPrice;
                    m.PaymentPrice = 0;
                }

                if (m.PaymentPrice < 0)
                {
                    m.CouponPrice = totalPrice;
                    m.PaymentPrice = 0;
                }
            }
            return m;
        }

        /// <summary>
        /// 주문에 대한 사용자 쿠폰 목록
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private List<UseCouponInfo> GetUserCouponList(int orderId)
        {
            var model = new List<UseCouponInfo>();
            var UserId = User.FindFirst("Id").Value;
            var toDay = DateTime.Today;

            if (!string.IsNullOrEmpty(UserId))
            {
                //Product 정보
                var prodQry = from o in _barunson.TB_Orders
                            join op in _barunson.TB_Order_Products on o.Order_ID equals op.Order_ID
                            join p in _barunson.TB_Products on op.Product_ID equals p.Product_ID
                            where o.Order_ID == orderId
                            select new
                            {
                                p.Product_Code,
                                p.Product_ID,
                                ProductPrice = p.Price,
                                TotalPrice = op.Total_Price,
                            };

                var prodItem = prodQry.FirstOrDefault();
                if (prodItem == null) 
                    return model;

                var query1 = from c in _barunson.TB_Coupons
                             join cp in _barunson.TB_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                             where cp.User_ID == UserId
                             && cp.Retrieve_DateTime == null
                             && cp.Use_YN == "N"
                             select new CouponDataInfo(
                                 c.Coupon_ID,
                                 c.Coupon_Name,
                                 cp.Coupon_Publish_ID,
                                 cp.Expiration_Date,
                                 cp.Regist_DateTime,
                                 c.Standard_Purchase_Price,
                                 c.Coupon_Apply_Code,
                                 c.Coupon_Apply_Product_ID,
                                 c.Discount_Method_Code,
                                 c.Discount_Rate,
                                 c.Discount_Price,
                                 c.Period_Method_Code,
                                 c.Publish_Start_Date,
                                 c.Publish_End_Date,
                                "General"
                             );

                var query2 = from c in _barunson.TB_Serial_Coupons
                             join cp in _barunson.TB_Serial_Coupon_Publishes on c.Coupon_ID equals cp.Coupon_ID
                             where cp.User_ID == UserId
                             && cp.Retrieve_DateTime == null
                             && cp.Use_YN == "N"
                             select new CouponDataInfo(
                                 c.Coupon_ID,
                                 c.Coupon_Name,
                                 cp.Coupon_Publish_ID,
                                 cp.Expiration_Date,
                                 cp.Regist_DateTime,
                                 c.Standard_Purchase_Price,
                                 c.Coupon_Apply_Code,
                                 c.Coupon_Apply_Product_ID,
                                 c.Discount_Method_Code,
                                 c.Discount_Rate,
                                 c.Discount_Price,
                                 c.Period_Method_Code,
                                 c.Publish_Start_Date,
                                 c.Publish_End_Date,
                                 "Serial"
                             );

                var items = new List<CouponDataInfo>();
                items.AddRange(query1.ToList());
                items.AddRange(query2.ToList());
               
                foreach (var item in items)
                {
                    model.Add(GetUseCouponInfo(item, toDay, prodItem.Product_Code, prodItem.TotalPrice.Value));
                }
            }

            var orderdModel = model.OrderByDescending(o => o.DiscountMethodCode)
                .ThenByDescending(o => o.DiscountRate)
                .ThenByDescending(o => o.DiscountPrice)
                .ToList();

            return orderdModel;
        }

        #endregion

        #region 청접장 주문 단계별

        /// <summary>
        /// 청접장 주문 1단계
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/McardStep1/{Order_Id?}")]
        [Route("Order/m_McardStep1/{Order_Id?}")]
        public IActionResult McardStep1(int Order_Id)
        {
            return Step1(Order_Id, "PCC01");
        }

        /// <summary>
        /// 청접장 주문 1단계 저장
        /// </summary>
        /// <param name="order"></param>
        /// <param name="invitation_detail"></param>
        /// <param name="invitation_detail_etcs"></param>
        /// <param name="account_extras"></param>
        /// <param name="invitation_accounts"></param>
        /// <returns></returns>
        [Route("Order/McardStep1_Save")]
        [AllowAnonymous] 
        public IActionResult McardStep1_Save(TB_Order order, TB_Invitation_Detail invitation_detail, List<TB_Invitation_Detail_Etc> invitation_detail_etcs, List<TB_Account_Extra> account_extras, List<TB_Invitation_Account> invitation_accounts)
        {
            return Step1Save(order, invitation_detail, invitation_detail_etcs, account_extras, invitation_accounts);
        }

        /// <summary>
        /// 청접장 주문 2단계
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/McardStep2/{Order_Id?}")]
        [Route("Order/m_McardStep2/{Order_Id?}")]
        public IActionResult McardStep2(int Order_Id)
        {
            return Step2(Order_Id, "PCC01");
        }

        /// <summary>
        /// 청접장 주문 2단계 저장
        /// </summary>
        /// <param name="invitation_detail"></param>
        /// <returns></returns>
        [Route("Order/McardStep2_Save")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult McardStep2_Save(TB_Invitation_Detail invitation_detail)
        {
            return Step2Save(invitation_detail);
        }

        #endregion

        #region 감사장 주문 단계

        /// <summary>
        /// 감사장 주문 1 단계
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/MthanksStep1/{Order_Id?}")]
        [Route("Order/m_MthanksStep1/{Order_Id?}")]
        public IActionResult MthanksStep1(int Order_Id)
        {
            ViewData["Order_Id"] = Order_Id;

            ViewData["Outline_Type_Code"] = "OTC01";

            var order = _invitationRepository.StepOrder_Sql(Order_Id, "PCC02");

            if (order != null)
            {
                ViewBag.Invitation = _invitationRepository.TB_Invitation_Entity(Order_Id);

                ViewData["Name"] = order.Name;
                ViewData["CellPhone_Number"] = order.CellPhone_Number;

                ViewData["Email_Account"] = string.Empty;
                ViewData["Email_Address"] = string.Empty;

                if (!string.IsNullOrWhiteSpace(order.Email))
                {
                    string str = order.Email.Replace("%", "@"); ;
                    string[] result = str.Split(new char[] { '@' });
                    ViewData["Email_Account"] = result[0];
                    ViewData["Email_Address"] = result[1];
                }

                if (ViewBag.Invitation.Count > 0)
                {
                    ViewData["Invitation_Id"] = ViewBag.Invitation[0].Invitation_ID;

                    int Invitation_Id = Int32.Parse(ViewData["Invitation_Id"].ToString());


                    ViewBag.invitation_detail = _invitationRepository.TB_Invitation_Detail_Entity(Invitation_Id);

                    ViewBag.template = _templateRepository.TB_Template_Entity(Int32.Parse(ViewBag.Invitation[0].Template_ID.ToString()));

                    if (ViewBag.template.Count > 0)
                    {
                        ViewData["Photo_YN"] = ViewBag.template[0].Photo_YN;

                        if (ViewData["Photo_YN"].ToString() == "Y")
                        {
                            //포토청첩장
                            ViewBag.PhotoImage = _invitationRepository.PhotoImage_Entity(Invitation_Id);

                            ViewData["Photo_Max_Width"] = ViewBag.PhotoImage.width;
                            ViewData["Photo_Max_Height"] = ViewBag.PhotoImage.height;
                        }
                    }

                    if (ViewBag.invitation_detail.Count > 0)
                    {
                        ViewData["Invitation_Title"] = ViewBag.invitation_detail[0].Invitation_Title.ToString().Trim();
                        string Invitation_URL = string.Empty;

                        Invitation_URL = ViewBag.invitation_detail[0].Invitation_URL;

                        if (Invitation_URL.Length > 12)
                        {
                            ViewData["Invitation_URL"] = string.Empty;
                        }
                        else
                        {
                            ViewData["Invitation_URL"] = Invitation_URL;
                        }

                        ViewData["SNS_Image_URL"] = ViewBag.invitation_detail[0].SNS_Image_URL;
                        ViewData["SNS_Image_Height"] = ViewBag.invitation_detail[0].SNS_Image_Height;
                        ViewData["SNS_Image_Width"] = ViewBag.invitation_detail[0].SNS_Image_Width;

                        ViewData["Delegate_Image_URL"] = ViewBag.invitation_detail[0].Delegate_Image_URL;
                        ViewData["Delegate_Image_Height"] = ViewBag.invitation_detail[0].Delegate_Image_Height;
                        ViewData["Delegate_Image_Width"] = ViewBag.invitation_detail[0].Delegate_Image_Width;
                        ViewData["Greetings"] = ViewBag.invitation_detail[0].Greetings;

                        ViewData["Groom_Name"] = ViewBag.invitation_detail[0].Groom_Name != null ? ViewBag.invitation_detail[0].Groom_Name.Trim() : "";
                        ViewData["Groom_Global_Phone_YN"] = ViewBag.invitation_detail[0].Groom_Global_Phone_YN;
                        ViewData["Groom_Global_Phone_Number"] = ViewBag.invitation_detail[0].Groom_Global_Phone_Number;
                        ViewData["Groom_Phone"] = ViewBag.invitation_detail[0].Groom_Phone;
                        ViewData["Bride_Name"] = ViewBag.invitation_detail[0].Bride_Name != null ? ViewBag.invitation_detail[0].Bride_Name.Trim() : "";
                        ViewData["Bride_Global_Phone_YN"] = ViewBag.invitation_detail[0].Bride_Global_Phone_YN;
                        ViewData["Bride_Global_Phone_Number"] = ViewBag.invitation_detail[0].Bride_Global_Phone_Number;
                        ViewData["Bride_Phone"] = ViewBag.invitation_detail[0].Bride_Phone;

                    }
                }
                ViewData["upload_path"] = "/upload/invitation/" + order.Order_Code.Substring(1, 6) + "/" + ViewData["Invitation_Id"];
            }
            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
        }

        [Route("Order/MthanksStep1_Save")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult MthanksStep1_Save(TB_Order order, TB_Invitation_Detail invitation_detail)
        {
            var result = new JsonOrderSaveResultModel
            {
                success = "N",
                result = "",
                message = "",
                auth = User.Identity.IsAuthenticated
            };

            try
            {
                if (result.auth)
                {
                    var ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
                    var userId = User.FindFirst("Id").Value;
                    //  비회원 주문 버튼을 클릭해서 들어온 후에 주문하는 경우에는 로그인 쿠키정보가 빈값으로 설정되어 
                    //  마이페이지 접속시 상품 노출이 불가
                    if (User.IsInRole("Guest") && !string.Equals(User.FindFirst("name").Value, order.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Temporary_Logut_Login(order, invitation_detail);
                        userId = order.Name;
                    }
                    var userName = User.FindFirst("name").Value;

                    var invitation_detailes = _invitationRepository.TB_Invitation_Detail_Entity(invitation_detail.Invitation_ID);

                    string ORG_Invitation_URL = invitation_detailes[0].Invitation_URL;

                    //URL 변경시 중복 확인
                    if (ORG_Invitation_URL != invitation_detail.Invitation_URL)
                    {
                        var inv_detail = _invitationRepository.CheckDuplicateURL_Entity(invitation_detail.Invitation_URL);

                        if (inv_detail != null)
                        {
                            result.message = "duplicate";
                            return Json(result);
                        }
                    }

                    order.User_ID = userId;
                    order.Email = order.Email.Replace("%", "@");

                    //주문 수정
                    order.Update_User_ID = userId;
                    order.Update_DateTime = DateTime.Now;
                    order.Update_IP = ipAddr;

                    _orderRepository.TB_Order_Update_Sql(order);

                    //초대장 상세 수정
                    invitation_detail.Update_User_ID = userId;
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = ipAddr;

                    _invitationRepository.TB_Invitation_Detail_Update_For_Thanks_Sql(invitation_detail);


                    result.success = "Y";
                    result.result = result.message = "";
                }
            }
            catch (Exception ex)
            {
                string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
                string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";

                _operationRepository.Error_Save_Sql(ex.ToString(), "MthanksStep1_Save", User_ID, User_Name);
                result.success = "N";
                result.result = result.message = "오류가 발생하였습니다. 관리자에게 문의해주세요.";
            }
            return Json(result);
        }

        #endregion

        #region 돌잔치

        /// <summary>
        /// 돌잔치 제작하기 페이지 이동
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <returns></returns>
        [Route("Order/MDollStep1/{Order_Id?}")]
        [Route("Order/m_MDollStep1/{Order_Id?}")]
        public IActionResult MDollStep1(int Order_Id)
        {
            return Step1(Order_Id, "PCC03");
        }

        [Route("/Order/MDollStep1_Save")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult MDollStep1_Save(TB_Order order, TB_Invitation_Detail invitation_detail, List<TB_Invitation_Detail_Etc> invitation_detail_etcs, 
            List<TB_Account_Extra> account_extras, List<BabyFirstBirthViewModel> babyInfos)
        {
            var extendData = "";
            if (babyInfos != null)
                extendData = JsonConvert.SerializeObject(babyInfos);
            return Step1Save(order, invitation_detail, invitation_detail_etcs, account_extras, null, extendData);
        }

        [Route("/Order/MDollStep2/{Order_Id?}")]
        [Route("Order/m_MDollStep2/{Order_Id?}")]
        public IActionResult MDollStep2(int Order_Id)
        {
            return Step2(Order_Id, "PCC03");
        }

        [Route("Order/MDollStep2_Save")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult MDollStep2_Save(TB_Invitation_Detail invitation_detail)
        {
            return Step2Save(invitation_detail);
        }

        #endregion

        #region SMS/MMS
        [Route("Order/Mms_Send/{Order_Id?}/{Phone?}")]
        public string Invitation_Mms_Send(int Order_Id, string Phone)
        {
            return _orderRepository.User_Invitation_Mms_Send_Sql(Order_Id, Phone);
        }

        #endregion

        #region 주문 삭제
        [Route("Order/Order_Del/{Order_Id?}")]
        /// <summary>
        /// 마이페이지 - 주문 삭제 , 일단 삭제보다는 Order_Status_Code값을 주문 취소로 변경
        /// </summary>
        /// <param name="Order_Id"></param>
        public void Order_Del(int Order_Id)
        {
            _orderRepository.User_Order_Del(Order_Id);
        }

        #endregion
    }

}
