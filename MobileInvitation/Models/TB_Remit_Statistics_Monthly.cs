using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Remit_Statistics_Monthly
    {
        public string Date { get; set; }
        public int? Remit_Price { get; set; }
        public int? Tax { get; set; }
        public int? Remit_Tax { get; set; }
        public int? Calculate_Tax { get; set; }
        public int? Hits_Tax { get; set; }
        public int? User_Count { get; set; }
        public int? Account_Count { get; set; }
        public int? Remit_Count { get; set; }
    }
}
