using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class VW_User_QNA
    {
        public int QNA_ID { get; set; }
		public string Q_KIND { get; set; }
		public int COMPANY_SEQ { get; set; }
        public string SALES_GUBUN { get; set; }
        public Int64? ORDER_ID { get; set; }
        public string NAME { get; set; }
        public string USERID { get; set; }
        public string EMAIL { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string UPFILE_1 { get; set; }
		public string UPFILE_2 { get; set; }
		public DateTime REGIST_DATETIME { get; set; }
        public string ANSWER_CONTENT { get; set; }
        public DateTime? ANSWER_DATETIME { get; set; }
        public string ADMIN_NAME { get; set; }
        public string ADMIN_UPFILE1 { get; set; }        
        public string STAT { get; set; }
    }
}
