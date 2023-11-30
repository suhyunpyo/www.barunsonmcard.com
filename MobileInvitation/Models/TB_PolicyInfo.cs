using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_PolicyInfo
    {
        public int Seq { get; set; }
        public string AdminName { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string PolicyDiv { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime RegDate { get; set; }
    }
}
