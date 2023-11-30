using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileInvitation.Areas.User.Controllers.Member
{
    /// <summary>
    /// 카카오 송금 서비스 
    /// </summary>
    [Area("User")]
	[Authorize(AuthenticationSchemes = "userAuth", Roles = "Users")]
	public class KakaoRemitController : PathController
	{
		private readonly KakaoBankConfig kakaoBankConfig;
		private readonly IHttpClientFactory _httpClientFactory;
		public KakaoRemitController(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, barunsonContext barunsonContext, 
			BarunnConfig barunnConfig, KakaoBankConfig kakaoBankConfig, IHttpClientFactory httpClientFactory)
			: base(environment, httpContextAccessor, barunsonContext, barunnConfig)
		{
			this.kakaoBankConfig = kakaoBankConfig;
			_httpClientFactory = httpClientFactory;
		}
		#region Private Functions

		private async Task<SelectList> GetAccountTypeList(string prodCateCode, string allText = "전체")
		{
			var atcList = await GetCommonCodeAsync("Account_Type_Code");
			var acts = new List<SelectListItem>();
			if (prodCateCode == "PCC01")
			{
				acts = atcList.Where(x => x.Extra_Code == "1" || x.Extra_Code == "2")
					.Select(m => new SelectListItem { Text = m.Code_Name, Value = m.Code }).ToList();
			}
			else if (prodCateCode == "PCC03")
			{
				acts = atcList.Where(x => x.Extra_Code == "3")
					.Select(m => new SelectListItem { Text = m.Code_Name, Value = m.Code }).ToList();
			}
			acts.Insert(0, new SelectListItem { Text = allText, Value = "" });
			return new SelectList(acts, "Value", "Text");
		}

        /// <summary>
        /// 이름 마스킹 처리
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string NameMask(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 2)
                return value;
            else if (value.Length <= 3)
                return value.Substring(0, 1) +
                       new string('*', value.Length - 1) +
                       value.Substring(value.Length - 1);
            else
                return value.Substring(0, 2) +
                       new string('*', value.Length - 3) +
                       value.Substring(value.Length - 1);

        }

		private static readonly Dictionary<string,string> KPStatusCode = new Dictionary<string, string>
        {
            {"READY","RC001" },
            {"OPEN_PAYMENT","RC002" },
            {"SELECT_METHOD","RC002" },
            {"AUTH_PASSWORD","RC003" },
            {"PAYMENT_IN_PROGRESS","RC004" },
            {"FAIL_AUTH_PASSWORD","RC102" },
            {"SUCCESS_PAYMENT","RC005" },
            {"FAIL_PAYMENT","RC100" },
            {"QUIT_PAYMENT","RC101" }
        };
        #endregion

        #region 송금 정산

        /// <summary>
        /// 송금 내역 읽기
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<MyKakaoRemitViewModel> GetCalculate(MyKakaoRemitViewModel model, int pageSize)
		{
			var userID = User.FindFirst("Id").Value;

			model.RouteController = "KakaoRemit";
			model.RouteAction = "Calculate";
			model.DataModel = new List<MyKakaoRemitDataModel>();

			model.PageSize = pageSize;
			
			var orderQ = from o in _barunsonDb.TB_Orders
						 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
						 join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
						 join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
						 where o.Order_ID == model.OrderId && o.User_ID == userID
						 select new { i.Invitation_ID, p.Product_Category_Code };
			var orderItem = await orderQ.FirstOrDefaultAsync();

			if (orderItem != null)
			{
				model.AccountTypeList = await GetAccountTypeList(orderItem.Product_Category_Code, "전체");

				var query = from a in _barunsonDb.TB_Accounts
							join r in _barunsonDb.TB_Remits on a.Account_ID equals r.Account_ID
							join it in _barunsonDb.TB_Invitation_Taxes on r.Invitation_ID equals it.Invitation_ID
							join t in _barunsonDb.TB_Taxes on it.Tax_ID equals t.Tax_ID
							from c in _barunsonDb.TB_Calculates.Where(c => r.Remit_ID == c.Remit_ID && c.Calculate_Type_Code == "CTC02" && c.Status_Code == "200").DefaultIfEmpty()
							where a.User_ID == userID && a.Invitation_ID == orderItem.Invitation_ID
								&& r.Result_Code == "RC005"
							orderby r.Complete_DateTime descending
							select new
							{
								CompleteDate = r.Complete_DateTime,
								RemitterName = r.Remitter_Name,
								DepositorName = a.Depositor_Name,
								Price = r.Total_Price,
								Tax = t.Tax,
								RemitPrice = c.Remit_Price,
								CalculateDate = c.Calculate_DateTime,
								AccountTypeCode = a.Account_Type_Code
							};
				var totalitems = await query.ToListAsync();

				//총 아이템 수
				model.TotalCount = totalitems.Count;
				model.TotalPrice = totalitems.Sum(m => m.Price ?? 0);
				model.TotalTax = totalitems.Sum(m => m.Tax);
				model.TotalRemitPrice = totalitems.Sum(m => m.RemitPrice ?? 0);

				
				var selectItems = totalitems.Where(m => string.IsNullOrEmpty(model.AccountTypeCode) || m.AccountTypeCode == model.AccountTypeCode);
				model.Count = selectItems.Count();
                var seq = 1;
				seq += model.PageFrom;
				//페이지 수 만큼 데이터 읽기

				var items = selectItems.Skip(model.PageFrom).Take(model.PageSize);

				foreach (var item in items)
				{
					model.DataModel.Add(new MyKakaoRemitDataModel
					{
						No = seq,
						CompleteDate = item.CompleteDate,
						RemitterName = item.RemitterName,
						DepositorName = item.DepositorName,
						Price = item.Price ?? 0,
						Tax = item.Tax,
						RemitPrice = item.RemitPrice ?? 0,
						NonRemitPrice = item.RemitPrice.HasValue ? 0 : ((item.Price ?? 0) - item.Tax),
						CalculateDate = item.CalculateDate,
						AccountTypeCode = item.AccountTypeCode
					});

					seq++;
				}
			}
			return model;
		}

		/// <summary>
		/// 송금 정산 목록
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Calculate(MyKakaoRemitViewModel model)
		{
			var pageSize = 5;
			if (UrlHelper.IsMobile(Request))
				pageSize = 10;
			
			model = await GetCalculate(model, pageSize);

			#region 배너
			model.Banners = await GetBanners("축의금 배너");
			#endregion

			return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
		}

		/// <summary>
		/// 송금 정산 목록 엑셀 출력
		/// </summary>
		/// <param name="orderid"></param>
		/// <returns></returns>
		public async Task<IActionResult> CalculateExcel(int orderid, string accountTypeCode)
		{
			ExcelPackage.LicenseContext = LicenseContext.Commercial;
			ExcelPackage excel = new ExcelPackage();
			var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
			workSheet.Cells.Style.Font.SetFromFont("맑은 고딕", 10);

			int rowIndex = 1;
			int colIndex = 1;

			workSheet.Row(1).Height = 25;
			workSheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			workSheet.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
			workSheet.Row(1).Style.Font.SetFromFont("맑은 고딕", 10);

			workSheet.Cells[rowIndex, colIndex++].Value = "No";
			workSheet.Cells[rowIndex, colIndex++].Value = "입금일";
			workSheet.Cells[rowIndex, colIndex++].Value = "예금주";
			workSheet.Cells[rowIndex, colIndex++].Value = "입금자";
			workSheet.Cells[rowIndex, colIndex++].Value = "입금액";
			workSheet.Cells[rowIndex, colIndex++].Value = "서비스 이용료";
			workSheet.Cells[rowIndex, colIndex++].Value = "정산완료";
			workSheet.Cells[rowIndex, colIndex++].Value = "미정산";
			workSheet.Cells[rowIndex, colIndex++].Value = "정산일";

			var model = await GetCalculate(new MyKakaoRemitViewModel { OrderId = orderid, AccountTypeCode = accountTypeCode }, int.MaxValue);
			if (model != null && model.DataModel.Count > 0)
			{
				foreach (var item in model.DataModel)
				{
					rowIndex++;
					colIndex = 1;

					workSheet.Cells[rowIndex, colIndex++].Value = item.No;
					workSheet.Cells[rowIndex, colIndex++].Value = item.CompleteDate?.ToString("yyyy-MM-dd HH:mm:ss");
					workSheet.Cells[rowIndex, colIndex++].Value = item.RemitterName;
					workSheet.Cells[rowIndex, colIndex++].Value = item.DepositorName;
					workSheet.Cells[rowIndex, colIndex++].Value = item.Price;
					workSheet.Cells[rowIndex, colIndex++].Value = item.Tax;
					workSheet.Cells[rowIndex, colIndex++].Value = item.RemitPrice;
					workSheet.Cells[rowIndex, colIndex++].Value = item.NonRemitPrice;
					workSheet.Cells[rowIndex, colIndex++].Value = (item.NonRemitPrice > 0) ? "정산대기" : item.CalculateDate?.ToString("yyyy-MM-dd HH:mm:ss");

				}
			}
			return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BankingList.xlsx");
		}
		#endregion

		#region 축의금 설정
		[HttpGet]
		public async Task<IActionResult> Account(int OrderId)
		{
			var userID = User.FindFirst("Id").Value;
			var model = new MyKakaoRemitAccountModel { OrderId = OrderId };
			model.DataModel = new List<MyKakaoRemitAccountDataModel>();
			var orderQ = from o in _barunsonDb.TB_Orders
						 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
						 join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
						 join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
						 where o.Order_ID == model.OrderId && o.User_ID == userID
						 select new { i.Invitation_ID, p.Product_Category_Code };
			var orderItem = await orderQ.FirstOrDefaultAsync();
			if (orderItem == null)
			{
				return RedirectToAction("Index", "Main");
			}
			else
			{
				model.AccountTypeList = await GetAccountTypeList(orderItem.Product_Category_Code, "입금대상선택");
				model.BankList = new SelectList(await GetSelectListsCommonCodesAsync("Bank_Code", true, "", "은행선택"), "Value", "Text");

				var query = from Account in _barunsonDb.TB_Accounts
							from AccountTypeCode in _barunsonDb.TB_Common_Codes.Where(c => Account.Account_Type_Code == c.Code && c.Code_Group == "Account_Type_Code").DefaultIfEmpty()
							from BankCode in _barunsonDb.TB_Common_Codes.Where(c => Account.Bank_Code == c.Code && c.Code_Group == "Bank_Code").DefaultIfEmpty()
							where Account.User_ID == userID && Account.Invitation_ID == orderItem.Invitation_ID
							orderby Account.Sort
							select new MyKakaoRemitAccountDataModel
							{
								AccountId = Account.Account_ID,
								AccountTypeCode = Account.Account_Type_Code,
								AccountTypeName = AccountTypeCode.Code_Name,
								BankCode = Account.Bank_Code,
								BankName = BankCode.Code_Name,
								AccountNumber = Account.Account_Number,
								DepositorName = Account.Depositor_Name
							};

				model.DataModel = await query.ToListAsync();

			}
			return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);
		}
		/// <summary>
		/// 계좌정보 확인 및 저장
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> CheckDepositor(MyKakaoRemitAccountModel model)
		{
			var reuslt = new JsonReusltStatusModel { status = false, message = "계좌정보 확인중 오류가 발생하였습니다. (코드 10)" };

			try
			{
				var userID = User.FindFirst("Id").Value;

				#region 주문 정보 조회

				var orderQ = from o in _barunsonDb.TB_Orders
							 join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
							 join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
							 join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
							 where o.Order_ID == model.OrderId && o.User_ID == userID
							 select new { i.Invitation_ID, p.Product_Category_Code };
				var orderItem = await orderQ.FirstOrDefaultAsync();

				#endregion

				if (orderItem == null)
				{
					reuslt.message = "인증이 만료되었습니다. 다시 로그인 해주시기 바랍니다.";
					return Json(reuslt);
				}
				else
				{
					var Now = DateTime.Now;
					model.AccountTypeList = await GetAccountTypeList(orderItem.Product_Category_Code, "입금대상선택");
					model.BankList = new SelectList(await GetSelectListsCommonCodesAsync("Bank_Code", true, "", "은행선택"), "Value", "Text");

					#region 입력값 확인
					if (string.IsNullOrEmpty(model.AccountTypeCode) || !model.AccountTypeList.Any(m => m.Value == model.AccountTypeCode))
					{
						reuslt.message = "입금대상을 선택해주세요.";
						return Json(reuslt);
					}
					if (string.IsNullOrEmpty(model.BankCode) || !model.BankList.Any(m => m.Value == model.BankCode)
						|| string.IsNullOrEmpty(model.AccountNumber) || string.IsNullOrEmpty(model.DepositorName))
					{
						reuslt.message = "등록할 계좌 정보를 입력해주세요.";
						return Json(reuslt);
					}
					#endregion

					#region 등록된 계좌, 최대 한도 초과 여부 검사
					var existsAccounts = await (from m in _barunsonDb.TB_Accounts
												where m.User_ID == userID
												&& m.Invitation_ID == orderItem.Invitation_ID
												select m)
												.ToListAsync();
					if (existsAccounts != null)
					{
						if (existsAccounts.Count >= 4)
						{
							reuslt.message = "계좌는 최대 4개까지 설정할 수 있습니다.";
							return Json(reuslt);
						}
						if (existsAccounts.Any(m => m.Account_Number == model.AccountNumber))
						{
							reuslt.message = "이미 등록된 계좌번호입니다.";
							return Json(reuslt);
						}
					}
					#endregion

					#region 한 유저당 1분에 3건 까지만 조회 가능.. 
					var checktime = Now.AddMinutes(-1);
					var checkQuery = from m in _barunsonDb.TB_Depositor_Hits
									 where m.User_ID == userID && m.Request_DateTime > checktime
									 group m by m.User_ID into g
									 select new
									 {
										 LastDateTime = g.Max(x => x.Request_DateTime),
										 Count = g.Count(),
									 };
					var checItem = await checkQuery.FirstOrDefaultAsync();
					if (checItem != null && checItem.Count >= 3)
					{
						var sec = Now - checItem.LastDateTime;
						reuslt.message = $"잠시 후 다시 시도해주세요.({sec.Value.TotalSeconds}초 남음)";
						return Json(reuslt);
					}
					#endregion

					//최종 API 성공 여부
					var isSuccess = false;
					//고유 번호 생성
					var uniqueNum = await GetTelegramNoAsync(Now.ToString("yyyyMMdd"));

					#region 계좌 조회 기록 데이터 생성

					var InquireHit = new TB_Depositor_Hit
					{
						User_ID = userID,
						Request_DateTime = Now,
						Request_Date = Now.ToString("yyyyMMdd"),
						Bank_Code = model.BankCode,
						Account_Number = model.AccountNumber,
						Unique_Number = uniqueNum
					};
					_barunsonDb.TB_Depositor_Hits.Add(InquireHit);
					await _barunsonDb.SaveChangesAsync();

					#endregion

					#region 계좌번호 확인 API

					var InquireDepositor = new KP_FirmInquireDepositor
					{
						BankCode = model.BankCode,
						Account = model.AccountNumber,
						ApiKey = kakaoBankConfig.BankingApiKey,
						OrgCode = kakaoBankConfig.OrgCode,
						TelegramNo = uniqueNum
					};
					var apiUri = new Uri(kakaoBankConfig.BankingHost, kakaoBankConfig.InquireDepositorUri);
					var httpClient = _httpClientFactory.CreateClient();
					
					try
					{
						var bodystr = JsonSerializer.Serialize<KP_FirmInquireDepositor>(InquireDepositor);
						using (var request = new HttpRequestMessage())
						{
							request.Method = HttpMethod.Post;
							request.RequestUri = apiUri;
							request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

							var response = await httpClient.SendAsync(request);
							var responsestr =  await response.Content.ReadAsStringAsync();
							if (response.StatusCode == HttpStatusCode.OK)
							{
								var responseData = JsonSerializer.Deserialize<KP_FirmInquireDepositorResult>(responsestr);

								InquireHit.Status_Code = responseData.Status.ToString();
								if (responseData.Status == 200)
								{
									InquireHit.Trading_Number = responseData.NatvTrNo;
									InquireHit.Depositor = responseData.Depositor;
									InquireHit.Hits_Depositor = model.DepositorName;
									InquireHit.Request_Result_DateTime = responseData.RequestAt;

									if (model.DepositorName.StartsWith(responseData.Depositor, StringComparison.InvariantCultureIgnoreCase))
										isSuccess = true;
									else
										reuslt.message = "계좌정보가 맞지 않습니다.(예금주)";
								}
								else
								{
									InquireHit.Error_Code = responseData.ErrorCode;
									InquireHit.Error_Message = responseData.ErrorMessage;

									reuslt.message = $"계좌정보 확인중 오류가 발생하였습니다.{(responseData.ErrorMessage)}";
								}
							}
							else
							{
								var responseData = JsonSerializer.Deserialize<KP_Result>(responsestr);
								InquireHit.Status_Code = responseData.Status.ToString(); 
								InquireHit.Error_Code = responseData.ErrorCode;
								InquireHit.Error_Message = responseData.ErrorMessage;

								reuslt.message = $"계좌정보 확인중 오류가 발생하였습니다.{(responseData.ErrorMessage)}";
							}
							await _barunsonDb.SaveChangesAsync();
						}
					}
					catch (Exception ex)
					{
						var msg = ex.ToString();
						var len = msg.Length > 500 ? 500 : msg.Length;
						InquireHit.Status_Code = "500";
						InquireHit.Error_Message = $"API 호출 - {msg.Substring(0, len)}";
						reuslt.message = $"계좌정보 확인중 오류가 발생하였습니다. (코드 30)";
						await _barunsonDb.SaveChangesAsync();
					}
					#endregion

					#region 성공시 계좌 저장
					if (isSuccess)
					{
						var account = new TB_Account
						{
							Invitation_ID = orderItem.Invitation_ID,
							User_ID = userID,
							Account_Type_Code = model.AccountTypeCode,
							Bank_Code = model.BankCode,
							Account_Number = model.AccountNumber,
							Depositor_Name = InquireHit.Depositor,
							Sort = 0,
							Regist_DateTime = Now,
						};
						_barunsonDb.TB_Accounts.Add(account);
						await _barunsonDb.SaveChangesAsync();

						reuslt.status = isSuccess;
					}

					#endregion
				}
			}
			catch {}

			return Json(reuslt);
		}

		/// <summary>
		/// 계좌 삭제
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		public async Task<IActionResult> DeleteAccount(int OrderId, int accountID)
		{
			var reuslt = new JsonReusltStatusModel { status = false, message = "계좌정보 확인중 오류가 발생하였습니다. (코드 10)" };

			try
			{
				var userID = User.FindFirst("Id").Value;
				//등록된 계좌
				var existsAccounts = await (from m in _barunsonDb.TB_Accounts
											where m.User_ID == userID && m.Account_ID == accountID
											select m).FirstOrDefaultAsync();
				if (existsAccounts != null)
				{
					// 입금 내역 확인
					var inCount = await (from m in _barunsonDb.TB_Remits
										 where m.Account_ID == accountID && m.Result_Code == "RC005"
										 select m).CountAsync();
					if (inCount > 0)
					{
						reuslt.message = "입금 내역이 있어 삭제할 수 없습니다.";
					}
					else
					{
						_barunsonDb.TB_Accounts.Remove(existsAccounts);
						await _barunsonDb.SaveChangesAsync();
						reuslt.status = true;
					}
				}
				else
				{
					reuslt.message = "계좌 정보가 올바르지 않습니다.";
				}
			}
			catch { }

			return Json(reuslt);
		}
				
		#endregion

		#region Info

		/// <summary>
		/// 송금서비스 안내 페이지
		/// </summary>
		/// <param name="OrderId"></param>
		/// <returns></returns>
		public IActionResult Info(int OrderId)
		{
			return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), OrderId);
		}
        #endregion

        #region 송금하기

        /// <summary>
        /// 송금계좌 목록
        /// </summary>
        /// <param name="id">Invitation ID</param>
        /// <returns></returns>
        [AllowAnonymous]
		public async Task<IActionResult> RemitAccounts(int id)
		{
			var query = from a in _barunsonDb.TB_Accounts
						join i in _barunsonDb.TB_Invitations on a.Invitation_ID equals i.Invitation_ID
						from AccountTypeCode in _barunsonDb.TB_Common_Codes.Where(c => a.Account_Type_Code == c.Code && c.Code_Group == "Account_Type_Code").DefaultIfEmpty()
						where a.Invitation_ID == id
						orderby a.Sort
						select new MyKakaoRemitAccountDataModel
						{
							AccountId = a.Account_ID,
							AccountTypeCode = a.Account_Type_Code,
							AccountTypeName = AccountTypeCode.Code_Name
                        };

			var model = await query.ToListAsync();

            return View(model);
        }

		/// <summary>
		/// 계좌로 송금하기 화면 표시
		/// </summary>
		/// <param name="id">Account ID</param>
		/// <returns></returns>
        [AllowAnonymous]
		[HttpGet]
        public async Task<IActionResult> RemitAccountTo(int id)
        {

			var model = new RemitAccountToViewModel
			{
				AccountId = 0,
				RemitterName = "",
				Price = 0,
			};
            try
			{
                var query = from a in _barunsonDb.TB_Accounts
                            where a.Account_ID == id
                            select a;
                var accountItem = await query.FirstOrDefaultAsync();
                if (accountItem != null)
				{
                    var atcList = await GetCommonCodeAsync("Account_Type_Code");
                    model.AccountId = accountItem.Account_ID;
					model.AccountTypeName = atcList.First(m => m.Code == accountItem.Account_Type_Code).Code_Name;
                }

            }
            catch { }
            return View(model);
        }

        /// <summary>
        /// 계좌로 송금하기 저장
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RemitAccountTo(RemitAccountToViewModel model)
        {
			//기본값은 실패
            IActionResult reuslt = RedirectToAction("RemitAccountFail", "KakaoRemit");
			try
			{
				var query = from a in _barunsonDb.TB_Accounts
							where a.Account_ID == model.AccountId
							select a;
				var accountItem = await query.FirstOrDefaultAsync();
				if (accountItem != null)
				{
					var now = DateTime.Now;
					//수신 공동 입금 정보(더즌과 계약된 전용 계좌)
					var AccountSetting = await (from a in _barunsonDb.TB_Account_Settings orderby a.Account_Setting_ID descending select a).FirstAsync();
                    var atcList = await GetCommonCodeAsync("Account_Type_Code");

                    #region 송금 기초 데이터
                    // 송금 고유 주문번호 생성
                    var rnd = new Random();
					var partnerOrderId = $"MC{now:yyyyMMddHHmmss}{rnd.Next(1, 999):000}";
					var Remit = new TB_Remit
					{
						Account_ID = accountItem.Account_ID,
						Invitation_ID = accountItem.Invitation_ID,
						Account_Number = accountItem.Account_Number,
						Bank_Code = accountItem.Bank_Code,
						Item_Name = accountItem.Depositor_Name,
						Partner_Order_ID = partnerOrderId,
						Result_Code = "RC000",
						Total_Price = model.Price,
                        Remitter_Name = model.RemitterName,
                        Regist_DateTime = now
					};
					_barunsonDb.TB_Remits.Add(Remit);
					await _barunsonDb.SaveChangesAsync();
                    #endregion

                    #region 송금 더존 API 호출 및 이동 URL 수신

                    var acc = new KP_FirmAccount
                    {
                        Number = AccountSetting.Kakao_Account_Number,
                        BankCode = AccountSetting.Kakao_Bank_Code
                    };

                    var Obj = new KP_TransferReady
                    {
                        ApiKey = kakaoBankConfig.MainApiKey,
                        Cid = kakaoBankConfig.MainCid,
                        PartnerOrderId = partnerOrderId, // 송금 자체 번호
                        PartnerUserId = accountItem.User_ID, // 청첩장 생성자 ID 
                        ItemName = $"{atcList.First(m => m.Code == accountItem.Account_Type_Code).Code_Name} {NameMask(accountItem.Depositor_Name)}", // 예금주 명 
                        TotalAmount = model.Price, // 파라메터로 받는 입금액
                        Account = acc, // 더즌 계좌 정보
                        SenderName = model.RemitterName, // 파라메터로 받는 송금자 명
                        ApprovalUrl = Url.ActionLink("RemitAccountApproval", "KakaoRemit", new { id = Remit.Remit_ID }),
                        CancelUrl = Url.ActionLink("RemitAccountCancel", "KakaoRemit", new { id = Remit.Remit_ID }),
                        FailUrl = Url.ActionLink("RemitAccountFail", "KakaoRemit", new { id = Remit.Remit_ID }),
                        CallbackUrl = Url.ActionLink("RemitAccountCallback", "KakaoRemit")
                    };

                    var apiUri = new Uri(kakaoBankConfig.MainHost, kakaoBankConfig.ReadyUri);
                    var httpClient = _httpClientFactory.CreateClient();
                    var bodystr = JsonSerializer.Serialize<KP_TransferReady>(Obj);
                    using (var request = new HttpRequestMessage())
					{
                        request.Method = HttpMethod.Post;
                        request.RequestUri = apiUri;
                        request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

                        var response = await httpClient.SendAsync(request);
                        var responsestr = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == HttpStatusCode.OK)
						{
                            var responseData = JsonSerializer.Deserialize<KP_TransferReadyResult>(responsestr);

                            Remit.Transaction_ID = responseData.Tid;
                            Remit.Result_Code = "RC001";
                            Remit.Ready_DateTime = DateTime.Now;
                            Remit.Status_Code = responseData.Status.ToString();

							//성공
							if (UrlHelper.IsMobile(Request))
								reuslt = Redirect(responseData.NextRedirectMobileUrl);
							else
                                reuslt = Redirect(responseData.NextRedirectPcUrl);
                        }
						else
						{
                            var responseData = JsonSerializer.Deserialize<KP_Result>(responsestr);
                            Remit.Result_Code = "RC102";
                            Remit.Status_Code = responseData.Status.ToString();
                            Remit.Error_Code = responseData.ErrorCode;
                            Remit.Error_Message = responseData.ErrorMessage;
                        }
                        await _barunsonDb.SaveChangesAsync();
                    }
                    #endregion

                }
            }
			catch { }
            return reuslt;
        }

		
		/// <summary>
		/// 승인요청
		/// </summary>
		/// <param name="id"></param>
		/// <param name="pg_token"></param>
		/// <returns></returns>
        [AllowAnonymous]
		[Route("RemitAccountApproval/{id}")]
        public async Task<IActionResult> RemitAccountApproval(int id, string pg_token)
		{
            //기본값은 실패
            IActionResult reuslt = RedirectToAction("RemitAccountFail", "KakaoRemit", new { id });
            try
            {
                var query = from a in _barunsonDb.TB_Remits
                            where a.Remit_ID == id
                            select a;
                var Remit = await query.FirstOrDefaultAsync();
                if (Remit != null)
				{
                    var accountItem = await (from a in _barunsonDb.TB_Accounts where a.Account_ID == Remit.Account_ID select a).FirstAsync();
                    var AccountSetting = await (from a in _barunsonDb.TB_Account_Settings orderby a.Account_Setting_ID descending select a).FirstAsync();

                    Remit.Result_Code = "RC003";
                    Remit.Payment_Token = pg_token;
					Remit.Request_DateTime = DateTime.Now;

                    await _barunsonDb.SaveChangesAsync();

                    #region 송금 더존 승인 API 호출
                    var acc = new KP_FirmAccount();
                    acc.Number = AccountSetting.Kakao_Account_Number;
                    acc.BankCode = AccountSetting.Kakao_Bank_Code;

                    var Approve = new KP_TransferApprove();
                    Approve.ApiKey = kakaoBankConfig.MainApiKey;
                    Approve.Cid = kakaoBankConfig.MainCid;
                    Approve.Tid = Remit.Transaction_ID;
					Approve.PartnerOrderId = Remit.Partner_Order_ID;
                    Approve.PartnerUserId = accountItem.User_ID;
                    Approve.PgToken = Remit.Payment_Token;
                    Approve.Account = acc;

                    var apiUri = new Uri(kakaoBankConfig.MainHost, kakaoBankConfig.ApproveUri);
                    var httpClient = _httpClientFactory.CreateClient();
                    var bodystr = JsonSerializer.Serialize<KP_TransferApprove>(Approve);
                    using (var request = new HttpRequestMessage())
					{
                        request.Method = HttpMethod.Post;
                        request.RequestUri = apiUri;
                        request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

                        var response = await httpClient.SendAsync(request);
                        var responsestr = await response.Content.ReadAsStringAsync();

                        if (response.StatusCode == HttpStatusCode.OK)
						{
                            var responseData = JsonSerializer.Deserialize<KP_TransferApproveResult>(responsestr);
                            Remit.Result_Code = "RC005";
							Remit.Total_Price = responseData.TotalAmount;
							Remit.Remitter_Name = responseData.SenderName;
							Remit.Complete_DateTime = DateTime.Now;
                            Remit.Complete_Date = DateTime.Now.ToString("yyyyMMdd");

                            reuslt = RedirectToAction("RemitAccountComplete", "KakaoRemit", new { id });
                        }
                        else
                        {
                            var responseData = JsonSerializer.Deserialize<KP_Result>(responsestr);
                            Remit.Result_Code = "RC100";
                            Remit.Status_Code = responseData.Status.ToString();
                            Remit.Error_Code = responseData.ErrorCode;
                            Remit.Error_Message = responseData.ErrorMessage;
                        }
                        await _barunsonDb.SaveChangesAsync();
                    }
                    #endregion
                }
            }
            catch { }
            return reuslt;
        }

		/// <summary>
		/// 송금 승인 완료
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [AllowAnonymous]
		[Route("RemitAccountComplete")]
		[Route("RemitAccountComplete/{id?}")]
        public IActionResult RemitAccountComplete(int? id)
        {
            return View();
        }

		/// <summary>
		/// 송금 콜백
		/// </summary>
		/// <returns></returns>
        [AllowAnonymous]
        [Route("RemitAccountCallback")]
        public async Task<IActionResult> RemitAccountCallback()
        {
            var Result = new Dictionary<string, string>();
            try
			{
				KP_TransferCallback Callback = null;
                using (var reader = new StreamReader(Request.Body))
                {
                    var inputstring = await reader.ReadToEndAsync();
                    Callback = JsonSerializer.Deserialize<KP_TransferCallback>(inputstring);
                }
				if (Callback != null)
				{
					var query = from a in _barunsonDb.TB_Remits
								where a.Transaction_ID == Callback.tid
								select a;

					var Remit = await query.FirstOrDefaultAsync();
					if (Remit != null)
					{
						Remit.Send_Status = Callback.send_status;
						if (KPStatusCode.ContainsKey(Callback.send_status))
						{
							Remit.Complete_DateTime = DateTime.Now;
							Remit.Complete_Date = DateTime.Now.ToString("yyyyMMdd");
							Remit.Result_Code = KPStatusCode[Callback.send_status];
						}
						else
						{
							Remit.Result_Code = "RC100";
						}
						await _barunsonDb.SaveChangesAsync();

						return new OkObjectResult(new { code = "1", msg = "정상수신되었습니다." });
					}
				}
            }
			catch {
               
            }
            return new BadRequestObjectResult(new { code = "9", msg = "오류가 발생하였습니다." });
        }
        /// <summary>
        /// 실패
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("RemitAccountFail")]
        [Route("RemitAccountFail/{id?}")]
        public IActionResult RemitAccountFail(int? id)
		{
            return View();
        }

		/// <summary>
		/// 취소
		/// </summary>
		/// <returns></returns>
        [AllowAnonymous]
        [Route("RemitAccountCancel")]
        [Route("RemitAccountCancel/{id?}")]
        public async Task<IActionResult> RemitAccountCancel(int? id)
        {
			try
			{
				if (id.HasValue)
				{
                    var query = from a in _barunsonDb.TB_Remits
                                where a.Remit_ID == id
                                select a;
                    var Remit = await query.FirstOrDefaultAsync();
                    if (Remit != null)
					{
                        Remit.Result_Code = "RC101";
                        await _barunsonDb.SaveChangesAsync();
                    }
                }
			}
			catch { }

            return View();
        }
        #endregion
    }
}
