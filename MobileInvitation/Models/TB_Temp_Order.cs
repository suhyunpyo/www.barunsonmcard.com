using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Temp_Order
    {
        public string Order_Code { get; set; }
        public int? Coupon_Publish_ID { get; set; }
        public int? Coupon_Price { get; set; }

        public string PaymentData { get; set; }
    }
}
