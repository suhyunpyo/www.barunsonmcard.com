using MobileInvitation.Models;
using System;
using System.Collections.Generic;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 축하 화환 관리 뷰
    /// </summary>
    public class MyFlowerViewModel: PageViewModel
    {
        /// <summary>
        /// 주문 ID
        /// </summary>
        public int OrderId { get; set; }
		public string Title { get; set; }

        public List<MyFlowerOrderDataModel> DataModel { set; get; }

        public override int PageSize { get; set; } = 5;

        public override Dictionary<string, string> RouteData
        {
            get
            {
                var routeall = new Dictionary<string, string>
                {
                    { nameof(OrderId), OrderId.ToString() },
                    { nameof(PageSize), PageSize.ToString() },
                };
                return routeall;
            }
        }

        /// <summary>
        /// 노출 배너 목록
        /// </summary>
        public List<BannerViewModel> Banners { get; set; }
    }
    /// <summary>
    /// 화환 받은 내역
    /// </summary>
    public class MyFlowerOrderDataModel
    {
        /// <summary>
        /// 순번
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 주문 번호(화환)
        /// </summary>
        public string POrderCode { get; set; }
        /// <summary>
        /// 구분 
        /// </summary>
        public string OrderTitle { get; set; }
        /// <summary>
        /// 보낸사람
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// 상품명
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 예식일(수령일)
        /// </summary>
        public DateTime? WeddingDate { get; set; }
    }
}
