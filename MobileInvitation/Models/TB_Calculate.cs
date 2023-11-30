using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Calculate
    {
        public int Calculate_ID { get; set; }
        public int? Remit_ID { get; set; }
        public string Calculate_Type_Code { get; set; }
        public int? Remit_Price { get; set; }
        public string Remit_Bank_Code { get; set; }
        public string Remit_Account_Number { get; set; }
        public string Remit_Content { get; set; }
        public string Trading_Number { get; set; }
        public int? Unique_Number { get; set; }
        public string Request_DateTime { get; set; }
        public string Request_Date { get; set; }
        public string Status_Code { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public DateTime? Calculate_DateTime { get; set; }

        public virtual TB_Remit Remit { get; set; }
    }
}
