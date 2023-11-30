using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Banner_Item
    {
        public int Banner_Item_ID { get; set; }
        public int Banner_ID { get; set; }
        public string Banner_Type_Code { get; set; }
        public string Image_URL { get; set; }
        public string Deadline_Type_Code { get; set; }
        public string Start_Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Date { get; set; }
        public string End_Time { get; set; }
        public string Link_URL { get; set; }
        public string NewPage_YN { get; set; }
        public int? Click_Count { get; set; }
        public int? Sort { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Banner_Main_Description { get; set; }
        public string Banner_Add_Description { get; set; }
        public string Image_URL2 { get; set; }

        public virtual TB_Banner Banner { get; set; }
    }
}
