using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class VW_User
    {
        public string USER_ID { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string NAME { get; set; }
        public string CELLPHONE_NUMBER { get; set; }
        public string WEDDING_DATE { get; set; }
        public string WEDDINGCARD_ORDER_YN { get; set; }
        public DateTime JOIN_DATETIME { get; set; }
        public string EMAIL { get; set; }
        public string REGIST_USER_ID { get; set; }
        public DateTime REGIST_DATETIME { get; set; }
        public string REGIST_IP { get; set; }
        public string UPDATE_USER_ID { get; set; }
        public int? UPDATE_DATETIME { get; set; }
        public int? UPDATE_IP { get; set; }
        public string DELETE_USER_ID { get; set; }
        public int? DELETE_DATETIME { get; set; }
        public string DELETE_IP { get; set; }
        public string DUPINFO { get; set; }
        public string PWD { get; set; }
        public string REFERER_SALES_GUBUN { get; set; }
        public string CARD_CODE { get; set; }
        public string INTEGRATION_MEMBER_YORN { get; set; }
    }
}
