using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Template
    {
        public TB_Template()
        {
            TB_Invitations = new HashSet<TB_Invitation>();
            TB_Products = new HashSet<TB_Product>();
            TB_Template_Areas = new HashSet<TB_Template_Area>();
            TB_Template_Items = new HashSet<TB_Template_Item>();
        }

        public int Template_ID { get; set; }
        public string Template_Name { get; set; }
        public string Preview_URL { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Photo_YN { get; set; }
        public string Background_Color { get; set; }
        public string Attached_File1_URL { get; set; }
        public string Attached_File2_URL { get; set; }

        public virtual TB_Template_Detail TB_Template_Detail { get; set; }
        public virtual ICollection<TB_Invitation> TB_Invitations { get; set; }
        public virtual ICollection<TB_Product> TB_Products { get; set; }
        public virtual ICollection<TB_Template_Area> TB_Template_Areas { get; set; }
        public virtual ICollection<TB_Template_Item> TB_Template_Items { get; set; }
    }
}
