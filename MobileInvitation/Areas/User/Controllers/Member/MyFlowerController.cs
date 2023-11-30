using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Areas.User.Controllers.Member
{
    /// <summary>
    /// 화환 선물 목록 보기
    /// </summary>
	[Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users")]
    public class MyFlowerController : PathController
    {
        private const string PID = "flasystem";

		public MyFlowerController(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
            :base(environment, httpContextAccessor, barunsonContext, barunnConfig)
        {
            
		}

        public async Task<IActionResult> Index(MyFlowerViewModel model)
        {
			model.RouteController = "MyFlower";
			model.RouteAction = "Index";
			model.DataModel = new List<MyFlowerOrderDataModel>();
			model.Title = "축하 화환 관리";

			if (UrlHelper.IsMobile(Request))
                model.PageSize = 10;
            else
                model.PageSize = 5;

            var orderQ = from o in _barunsonDb.TB_Orders
                         join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                         join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                         join op in _barunsonDb.TB_Order_Products on o.Order_ID equals op.Order_ID
                         join p in _barunsonDb.TB_Products on op.Product_ID equals p.Product_ID
                         where o.Order_ID == model.OrderId && o.User_ID == User.FindFirst("Id").Value
                         select new
                         {
                             id.WeddingDate,
                             id.WeddingHour,
                             id.WeddingMin,
                             id.Time_Type_Code,
                             p.Product_Category_Code
                         };
            var orderItem = await orderQ.FirstOrDefaultAsync();
            if (orderItem != null)
            {
                switch (orderItem.Product_Category_Code)
                {

                    case "PCC01":
						model.Title = "결혼 축하 화환 관리";
						break;
					case "PCC03":
						model.Title = "돌잔치 축하 화환 관리";
						break;
					default:
						break;
                }
                var eventDate = GetWeddingDate(orderItem.WeddingDate, orderItem.WeddingHour, orderItem.WeddingMin, orderItem.Time_Type_Code);

				var query = from f in _barunsonDb.TB_Order_PartnerShip 
                            where f.Order_ID == model.OrderId && f.P_Id == PID && f.P_OrderState == "배송완료"
                            orderby f.LastUpdateTime
                            select new
                            {
                                f.P_OrderCode,
                                f.P_ExtendData,
                                f.P_Order_Name,
                                f.P_ProductName,

                            };
                var seq = 1;

                //총 아이템 수
                model.Count = await query.CountAsync();
                //페이지 수 만큼 데이터 읽기
                query = query.Skip(model.PageFrom).Take(model.PageSize);
                seq += model.PageFrom;
                
                var items = await query.ToListAsync();

                foreach (var item in items)
                {
                    var jsonData = JObject.Parse(item.P_ExtendData);
                    model.DataModel.Add(new MyFlowerOrderDataModel
                    {
                        No = seq,
                        POrderCode = item.P_OrderCode,
                        OrderTitle = (string)jsonData["deli_title"],
                        OrderName = item.P_Order_Name,
                        ProductName = item.P_ProductName,
                        WeddingDate = eventDate,
                    });

                    seq++;

                }
                //배너
                #region 배너
                model.Banners = await GetBanners("화환 배너");

                #endregion
            }
			return View(Moblie_Redirect(HttpContext.Request.RouteValues, Request), model);

        }

        /// <summary>
        /// Excel 출력
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public async Task<IActionResult> Excel(int orderid)
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
			workSheet.Cells[rowIndex, colIndex++].Value = "구분";
			workSheet.Cells[rowIndex, colIndex++].Value = "보낸 사람";
			workSheet.Cells[rowIndex, colIndex++].Value = "상품명";
			workSheet.Cells[rowIndex, colIndex++].Value = "수령일";

			var orderQ = from o in _barunsonDb.TB_Orders
                        join i in _barunsonDb.TB_Invitations on o.Order_ID equals i.Order_ID
                        join id in _barunsonDb.TB_Invitation_Details on i.Invitation_ID equals id.Invitation_ID
                        where o.Order_ID == orderid && o.User_ID == User.FindFirst("Id").Value
						select new
                        {
							id.WeddingDate,
							id.WeddingHour,
							id.WeddingMin,
							id.Time_Type_Code
						};
			var orderItem = await orderQ.FirstOrDefaultAsync();
			if (orderItem != null)
            {
				var eventDate = GetWeddingDate(orderItem.WeddingDate, orderItem.WeddingHour, orderItem.WeddingMin, orderItem.Time_Type_Code);

				var query = from f in _barunsonDb.TB_Order_PartnerShip
							where f.Order_ID == orderid && f.P_Id == PID && f.P_OrderState == "배송완료"
							orderby f.LastUpdateTime
							select new
							{
								f.P_OrderCode,
								f.P_ExtendData,
								f.P_Order_Name,
								f.P_ProductName,
							};
				var items = await query.ToListAsync();

				var seq = 1;
				foreach (var item in items)
                {
					rowIndex++;
					colIndex = 1;
					var jsonData = JObject.Parse(item.P_ExtendData);

					workSheet.Cells[rowIndex, colIndex++].Value = seq;
                    workSheet.Cells[rowIndex, colIndex++].Value = (string)jsonData["deli_title"];
                    workSheet.Cells[rowIndex, colIndex++].Value = item.P_Order_Name;
                    workSheet.Cells[rowIndex, colIndex++].Value = item.P_ProductName;
                    workSheet.Cells[rowIndex, colIndex++].Value = eventDate?.ToString("yyyy.MM.dd");
					seq++;
				}
			}

			return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MyFlowerList.xlsx");
		}
    }
}
