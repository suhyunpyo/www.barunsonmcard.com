using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Company_Tax
    {
        public int Company_Tax_ID { get; set; }
        public int? Remit_Tax { get; set; }
        public int? Calculate_Tax { get; set; }
        public int? Hits_Tax { get; set; }
        public string Apply_Start_Date { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
    }
}
