using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Category
    {
        public int Category_ID { get; set; }
        public int? Parent_Category_ID { get; set; }
        public string Category_Name { get; set; }
        public string Category_Type_Code { get; set; }
        public string Category_Name_Type_Code { get; set; }
        public string Category_Name_PC { get; set; }
        public string Category_Name_PC_URL { get; set; }
        public string Category_Name_Mobile { get; set; }
        public string Category_Name_Mobile_URL { get; set; }
        public int? Category_Step { get; set; }
        public int? Sort { get; set; }
        public string Display_YN { get; set; }
        public int? Icon_ID { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
    }
}
