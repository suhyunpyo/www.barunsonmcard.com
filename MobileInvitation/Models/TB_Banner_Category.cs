using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Banner_Category
    {
        public TB_Banner_Category()
        {
            TB_Banners = new HashSet<TB_Banner>();
        }

        public int Banner_Category_ID { get; set; }
        public string Banner_Category_Name { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual ICollection<TB_Banner> TB_Banners { get; set; }
    }
}
