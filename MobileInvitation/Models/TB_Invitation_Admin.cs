using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Admin
    {
        public int seq { get; set; }
        public string admin_id { get; set; }
        public string admin_pwd { get; set; }
        public string admin_name { get; set; }
        public string admin_mail { get; set; }
        public byte admin_level { get; set; }
        public int? company_seq { get; set; }
        public string is_reviewMail { get; set; }
        public string is_errorMail { get; set; }
        public DateTime reg_date { get; set; }
        public string is_reviewSMS { get; set; }
        public string admin_hphone { get; set; }
        public string admin_photo { get; set; }
        public int? access_flag { get; set; }
        public string JOB_NAME { get; set; }
    }
}
