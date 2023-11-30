using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Coupon_Product
    {
        public TB_Coupon_Product()
        {
            TB_Coupon_Orders = new HashSet<TB_Coupon_Order>();
            TB_Coupon_Product_Options = new HashSet<TB_Coupon_Product_Option>();
        }

        public int Coupon_Product_ID { get; set; }
        public string Product_ID { get; set; }
        public string Product_Type { get; set; }
        public string Product_Category { get; set; }
        public string Affiliate { get; set; }
        public string Affiliate_Category { get; set; }
        public string Delegate_Affiliate_Code { get; set; }
        public string Product_Description { get; set; }
        public string Product_Name { get; set; }
        public string Image_Path { get; set; }
        public int? Retail_Price { get; set; }
        public int? Retail_Price_Tax { get; set; }
        public int? Sale_Price { get; set; }
        public int? Sale_Price_Tax { get; set; }
        public int? Total_Price { get; set; }
        public string Sale_End_Date { get; set; }
        public string Valid_Period { get; set; }
        public string Destination_URL { get; set; }

        public virtual ICollection<TB_Coupon_Order> TB_Coupon_Orders { get; set; }
        public virtual ICollection<TB_Coupon_Product_Option> TB_Coupon_Product_Options { get; set; }
    }
}
