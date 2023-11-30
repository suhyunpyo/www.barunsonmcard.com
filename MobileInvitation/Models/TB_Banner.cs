using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Banner
    {
        public TB_Banner()
        {
            TB_Banner_Items = new HashSet<TB_Banner_Item>();
        }

        public int Banner_ID { get; set; }
        public int Banner_Category_ID { get; set; }
        public string Banner_PC_YN { get; set; }
        public string Banner_Mobile_YN { get; set; }
        public string Banner_Name { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Banner_Category Banner_Category { get; set; }
        public virtual ICollection<TB_Banner_Item> TB_Banner_Items { get; set; }
    }
}
