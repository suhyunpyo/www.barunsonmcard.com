using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class Order
    {
        public int Order_ID { get; set; }

        public virtual TB_Coupon_Order TB_Coupon_Order { get; set; }
    }
}
