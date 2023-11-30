using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Product_Image
    {
        public int Image_ID { get; set; }
        public int Product_ID { get; set; }
        public string Image_URL { get; set; }
        public string Image_Type_Code { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Product Product { get; set; }
    }
}
