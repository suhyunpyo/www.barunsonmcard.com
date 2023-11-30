using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Popup_Item
    {
        public int Popup_Item_ID { get; set; }
        public int Popup_ID { get; set; }
        public string Popup_Type_Code { get; set; }
        public string Image_URL { get; set; }
        public string Link_URL { get; set; }
        public string Period_Type_Code { get; set; }
        public string Start_Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Date { get; set; }
        public string End_Time { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Popup Popup { get; set; }
    }
}
