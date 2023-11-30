using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Coupon_Publish_TEST
    {
        public int Coupon_Publish_ID { get; set; }
        public int Coupon_ID { get; set; }
        public string User_ID { get; set; }
        public string Use_YN { get; set; }
        public DateTime? Use_DateTime { get; set; }
        public string Expiration_Date { get; set; }
        public DateTime? Retrieve_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Coupon Coupon { get; set; }
    }
}
