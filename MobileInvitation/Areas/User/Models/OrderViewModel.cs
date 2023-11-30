using MobileInvitation.Payment;
using System;
using System.Collections.Generic;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 주문 마지막 결제관련 모델
    /// </summary>
    public class OrderLastStepModel
    {
        public string UserID { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public int InvitationId { get; set; }
        public DateTime OrderDateTime { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public int ProductPrice { get; set; }
        public int? TotalPrice { get; set; }

        public string BackToUrl { get; set; }
        public string ComplateUrl { get; set; }

        public List<UseCouponInfo> UseCouponList { get; set; }

        public string tossClientKey { get; set; }

    }

    /// <summary>
    /// 주문 결제 모델
    /// </summary>
    public class OrderPaymentModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public int OrderID { get; set; }
        public int CouponPublishID { get; set; }
        /// <summary>
        /// 상품 금액
        /// </summary>
        public int TotalPrice { get; set; }
        /// <summary>
        /// 실제 결제 금액
        /// </summary>
        public int PaymentPrice { get; set; }
        /// <summary>
        /// 쿠폰 적용 금액
        /// </summary>
        public int CouponPrice { get; set; }

		/// <summary>
		/// Toss 상의 IdempotencyKey
		/// </summary>
		public string IdempotencyKey { get; set; }
		public TossRequestPayment tossRequestPayment { get; set; }

    }

    /// <summary>
    /// 주문 결과 모델, 성공 또는 실패
    /// </summary>
    public class OrderPayFinalModel
    {
        public string OrderCode { get; set; }
        public string UserName { get; set; }

        public string Message { get; set; }
        public string BackToUrl { get; set; }
        
    }
}
