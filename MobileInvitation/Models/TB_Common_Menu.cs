using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Common_Menu
    {
        public TB_Common_Menu()
        {
            InverseParent_Menu = new HashSet<TB_Common_Menu>();
        }

        public int Menu_ID { get; set; }
        public int? Parent_Menu_ID { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Type_Code { get; set; }
        public string Menu_URL { get; set; }
        public int? Menu_Step { get; set; }
        public int? Sort { get; set; }
        public string Display_YN { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Image_URL { get; set; }

        public virtual TB_Common_Menu Parent_Menu { get; set; }
        public virtual ICollection<TB_Common_Menu> InverseParent_Menu { get; set; }
    }
}
