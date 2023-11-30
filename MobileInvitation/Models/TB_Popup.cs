using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Popup
    {
        public TB_Popup()
        {
            TB_Popup_Items = new HashSet<TB_Popup_Item>();
        }

        public int Popup_ID { get; set; }
        public string Popup_Title { get; set; }
        public string Popup_PC_YN { get; set; }
        public string Popup_Mobile_YN { get; set; }
        public int Popup_Location_Top { get; set; }
        public int Popup_Location_Left { get; set; }
        public int Popup_Height { get; set; }
        public int Popup_Width { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual ICollection<TB_Popup_Item> TB_Popup_Items { get; set; }
    }
}
