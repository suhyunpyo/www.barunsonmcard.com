using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Account
    {
        public int Account_ID { get; set; }
        public int Invitation_ID { get; set; }
        public string User_ID { get; set; }
        public string Account_Type_Code { get; set; }
        public string Bank_Code { get; set; }
        public string Account_Number { get; set; }
        public string Depositor_Name { get; set; }
        public int? Sort { get; set; }
        public DateTime? Regist_DateTime { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
    }
}
