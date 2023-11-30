using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Order_Coupon_Use
    {
        public int? Order_ID { get; set; }
        public int? Coupon_Publish_ID { get; set; }
        public int? Discount_Price { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Coupon_Publish Coupon_Publish { get; set; }
        public virtual TB_Order Order { get; set; }
    }
}
