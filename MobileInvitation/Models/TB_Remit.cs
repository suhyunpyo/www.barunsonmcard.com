using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Remit
    {
        public TB_Remit()
        {
            TB_Calculates = new HashSet<TB_Calculate>();
        }

        public int Remit_ID { get; set; }
        public int? Account_ID { get; set; }
        public int? Coupon_Order_ID { get; set; }
        public int? Invitation_ID { get; set; }
        public string Partner_Order_ID { get; set; }
        public string Transaction_ID { get; set; }
        public string Transaction_Detail_ID { get; set; }
        public string Item_Name { get; set; }
        public int? Total_Price { get; set; }
        public string Account_Number { get; set; }
        public string Bank_Code { get; set; }
        public string Remitter_Name { get; set; }
        public string Payment_Token { get; set; }
        public string Result_Code { get; set; }
        public string Send_Status { get; set; }
        public string Status_Code { get; set; }
        public string Error_Code { get; set; }
        public string Error_Message { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public DateTime? Ready_DateTime { get; set; }
        public DateTime? Request_DateTime { get; set; }
        public DateTime? Complete_DateTime { get; set; }
        public string Complete_Date { get; set; }

        public virtual TB_Coupon_Order Coupon_Order { get; set; }
        public virtual TB_Invitation_Tax Invitation { get; set; }
        public virtual ICollection<TB_Calculate> TB_Calculates { get; set; }
    }
}
