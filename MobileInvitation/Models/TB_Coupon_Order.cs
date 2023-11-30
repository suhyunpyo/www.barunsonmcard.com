using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Coupon_Order
    {
        public TB_Coupon_Order()
        {
            TB_Remits = new HashSet<TB_Remit>();
        }

        public int Coupon_Order_ID { get; set; }
        public int? Coupon_Product_ID { get; set; }
        public string Coupon_OrderNumber { get; set; }
        public string Callback_PhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Option_Code { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
        public int? Request_Count { get; set; }
        public DateTime? Result_DateTime { get; set; }
        public string Result_Code { get; set; }
        public string Result_Content { get; set; }
        public string PIN_Number { get; set; }
        public string Futures_Trading_Number { get; set; }
        public string Coupon_Expiration_DateTime { get; set; }
        public string StampOffice { get; set; }
        public string Stamp_Code { get; set; }
        public string Stamp_Type { get; set; }
        public string PIN_Option_Information { get; set; }

        public virtual TB_Coupon_Product Coupon_Product { get; set; }
        public virtual ICollection<TB_Remit> TB_Remits { get; set; }
    }
}
