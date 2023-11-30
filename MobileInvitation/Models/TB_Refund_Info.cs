using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Refund_Info
    {
        public int Refund_ID { get; set; }
        public int Order_ID { get; set; }
        public string Refund_Type_Code { get; set; }
        public int? Refund_Price { get; set; }
        public string Bank_Type_Code { get; set; }
        public string AccountNumber { get; set; }
        public string Refund_Status_Code { get; set; }
        public string Depositor_Name { get; set; }
        public string Refund_Content { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public DateTime? Refund_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Order Order { get; set; }
    }
}
