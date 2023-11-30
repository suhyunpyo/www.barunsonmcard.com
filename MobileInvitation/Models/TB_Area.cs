using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Area
    {
        public TB_Area()
        {
            TB_Invitation_Areas = new HashSet<TB_Invitation_Area>();
            TB_Template_Areas = new HashSet<TB_Template_Area>();
        }

        public int Area_ID { get; set; }
        public string Area_Name { get; set; }
        public string WeddingCard_YN { get; set; }
        public string ThanksCard_YN { get; set; }
        public string Edit_YN { get; set; }
        public string Product_Category_Codes { get; set; }

        public virtual ICollection<TB_Invitation_Area> TB_Invitation_Areas { get; set; }
        public virtual ICollection<TB_Template_Area> TB_Template_Areas { get; set; }
    }
}
