using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Icon
    {
        public TB_Icon()
        {
            TB_Product_Icons = new HashSet<TB_Product_Icon>();
        }

        public int Icon_ID { get; set; }
        public string Icon_URL { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual ICollection<TB_Product_Icon> TB_Product_Icons { get; set; }
    }
}
