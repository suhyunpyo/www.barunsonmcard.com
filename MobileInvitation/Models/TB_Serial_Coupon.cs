using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Serial_Coupon
    {
        public TB_Serial_Coupon()
        {
            TB_Serial_Coupon_Publishes = new HashSet<TB_Serial_Coupon_Publish>();
        }

        public int Coupon_ID { get; set; }
        public string Coupon_Name { get; set; }
        public string Use_Available_Standard_Code { get; set; }
        public int? Standard_Purchase_Price { get; set; }
        public string Discount_Method_Code { get; set; }
        public double? Discount_Rate { get; set; }
        public int? Discount_Price { get; set; }
        public string Period_Method_Code { get; set; }
        public string Publish_Start_Date { get; set; }
        public string Publish_End_Date { get; set; }
        public string Publish_Period_Code { get; set; }
        public string Coupon_Type_Code { get; set; }
        public string Description { get; set; }
        public string Coupon_Image_URL { get; set; }
        public string Use_YN { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Coupon_Apply_Code { get; set; }
        public int? Coupon_Apply_Product_ID { get; set; }
        public string Serial_Coupon_Number { get; set; }

        public virtual ICollection<TB_Serial_Coupon_Publish> TB_Serial_Coupon_Publishes { get; set; }
    }
}
