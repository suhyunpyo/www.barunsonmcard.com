using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Tax
    {
        public TB_Tax()
        {
            TB_Invitation_Taxes = new HashSet<TB_Invitation_Tax>();
        }

        public int Tax_ID { get; set; }
        public int Tax { get; set; }
        public int? Previous_Tax { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }

        public virtual ICollection<TB_Invitation_Tax> TB_Invitation_Taxes { get; set; }
    }
}
