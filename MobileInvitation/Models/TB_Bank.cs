using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Bank
    {
        public string Bank_Code { get; set; }
        public string Bank_Name { get; set; }
        public string Use_YN { get; set; }
        public int? Sort { get; set; }
    }
}
