using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Depositor_Hit
    {
        public int Depositor_Hits_ID { get; set; }
        public string User_ID { get; set; }
        public int? Unique_Number { get; set; }
        public string Bank_Code { get; set; }
        public string Account_Number { get; set; }
        public string Depositor { get; set; }
        public string Hits_Depositor { get; set; }
        public string Trading_Number { get; set; }
        public string Status_Code { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public string Request_Date { get; set; }
        public DateTime? Request_DateTime { get; set; }
        public string Request_Result_DateTime { get; set; }
    }
}
