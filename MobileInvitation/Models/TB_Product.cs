using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Product
    {
        public TB_Product()
        {
            TB_Order_Products = new HashSet<TB_Order_Product>();
            TB_Product_Categories = new HashSet<TB_Product_Category>();
            TB_Product_Icons = new HashSet<TB_Product_Icon>();
            TB_Product_Images = new HashSet<TB_Product_Image>();
        }

        public int Product_ID { get; set; }
        public int? Template_ID { get; set; }
        public string Product_Code { get; set; }
        public string Product_Category_Code { get; set; }
        public string Product_Brand_Code { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public int Price { get; set; }
        public string Display_YN { get; set; }
        public string Main_Image_URL { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Original_Product_Code { get; set; }
        public string Preview_Image_URL { get; set; }
        public string SetCard_URL { get; set; }
        public string SetCard_Mobile_URL { get; set; }
        public string SetCard_Display_YN { get; set; }
        public virtual TB_Template Template { get; set; }
        public virtual ICollection<TB_Order_Product> TB_Order_Products { get; set; }
        public virtual ICollection<TB_Product_Category> TB_Product_Categories { get; set; }
        public virtual ICollection<TB_Product_Icon> TB_Product_Icons { get; set; }
        public virtual ICollection<TB_Product_Image> TB_Product_Images { get; set; }
    }
}
