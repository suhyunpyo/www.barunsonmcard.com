using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class VW_Admin
    {
        public string ADMIN_NAME { get; set; }
        public string ADMIN_ID { get; set; }
        public string ADMIN_PASSWORD { get; set; }
        public byte ADMIN_TYPE { get; set; }
        public DateTime REGIST_DATETIME { get; set; }
    }
}
