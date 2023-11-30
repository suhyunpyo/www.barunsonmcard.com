using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class Origin_Invitation_Detail
    {
        public int? Order_ID { get; set; }
        public int? Invitation_ID { get; set; }
        public string Origin_Invitation_URL { get; set; }
        public DateTime? Reg_Date { get; set; }
    }
}
