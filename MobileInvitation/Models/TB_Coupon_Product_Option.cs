using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Coupon_Product_Option
    {
        public int Coupon_Product_Option_ID { get; set; }
        public int? Coupon_Product_ID { get; set; }
        public string Option_Name { get; set; }
        public string Option_Value { get; set; }

        public virtual TB_Coupon_Product Coupon_Product { get; set; }
    }
}
