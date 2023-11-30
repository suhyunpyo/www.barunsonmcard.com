using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Account
    {
        public int Invitation_ID { get; set; }
        public int Sort { get; set; }
        public int Category { get; set; }
        public string Send_Target_Code { get; set; }
        public string Send_Name { get; set; }
        public string Bank_Code { get; set; }
        public string Account_Number { get; set; }
        public string Account_Holder { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
    }
}
