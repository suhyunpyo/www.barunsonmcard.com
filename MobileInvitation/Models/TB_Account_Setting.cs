using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Account_Setting
    {
        public int Account_Setting_ID { get; set; }
        public string Barunn_Bank_Code { get; set; }
        public string Barunn_Account_Number { get; set; }
        public string Kakao_Bank_Code { get; set; }
        public string Kakao_Account_Number { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
    }
}
