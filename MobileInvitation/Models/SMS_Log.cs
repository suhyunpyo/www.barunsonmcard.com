using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class SMS_Log
    {
        public string ORDER_CDOE { get; set; }
        public DateTime? regdate { get; set; }
        public string CONTENT { get; set; }
        public string ORDER_HPHONE { get; set; }
    }
}
