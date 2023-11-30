using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Common_Code_Group
    {
        public TB_Common_Code_Group()
        {
            TB_Common_Codes = new HashSet<TB_Common_Code>();
        }

        public string Code_Group { get; set; }
        public string Group_Name { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual ICollection<TB_Common_Code> TB_Common_Codes { get; set; }
    }
}
