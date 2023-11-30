using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Tax
    {
        public TB_Invitation_Tax()
        {
            TB_Remits = new HashSet<TB_Remit>();
        }

        public int Invitation_ID { get; set; }
        public int? Tax_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }

        public virtual TB_Tax Tax { get; set; }
        public virtual ICollection<TB_Remit> TB_Remits { get; set; }
    }
}
