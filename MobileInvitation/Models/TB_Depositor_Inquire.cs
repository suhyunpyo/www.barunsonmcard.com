using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Depositor_Inquire
    {
        public int Depositor_Inquire_ID { get; set; }
        public string User_ID { get; set; }
        public string Trading_Number { get; set; }
        public string Depositor { get; set; }
        public DateTime? Request_DateTime { get; set; }
        public string Request_Result_DateTime { get; set; }
    }
}
