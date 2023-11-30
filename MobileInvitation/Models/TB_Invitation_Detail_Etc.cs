using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Detail_Etc
    {
        public int Invitation_ID { get; set; }
        public int Sort { get; set; }
        public string Etc_Title { get; set; }
        public string Information_Content { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
    }
}
