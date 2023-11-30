using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Template_Area
    {
        public int Template_ID { get; set; }
        public int Area_ID { get; set; }
        public double? Size_Height { get; set; }
        public double? Size_Width { get; set; }
        public int? Sort { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Color { get; set; }

        public virtual TB_Area Area { get; set; }
        public virtual TB_Template Template { get; set; }
    }
}
