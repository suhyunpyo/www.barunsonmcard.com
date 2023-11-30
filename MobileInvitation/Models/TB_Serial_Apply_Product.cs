using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Serial_Apply_Product
    {
        public int? Product_Apply_ID { get; set; }
        public string Product_Code { get; set; }

        public virtual TB_Serial_Coupon_Apply_Product Product_Apply { get; set; }
    }
}
