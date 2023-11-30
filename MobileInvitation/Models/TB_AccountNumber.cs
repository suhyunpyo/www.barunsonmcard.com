using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_AccountNumber
    {
        public int AccountNumber_ID { get; set; }
        public int? Invitation_ID { get; set; }
        public string Account_Type_Code { get; set; }
        public string Account_Name { get; set; }
        public int? External_AccountNumber_ID { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
    }
}
