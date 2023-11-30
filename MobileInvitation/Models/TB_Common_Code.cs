using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Common_Code
    {
        public string Code_Group { get; set; }
        public string Code { get; set; }
        public string Code_Name { get; set; }
        public int? Sort { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Extra_Code { get; set; }

        public virtual TB_Common_Code_Group Code_GroupNavigation { get; set; }
    }
}
