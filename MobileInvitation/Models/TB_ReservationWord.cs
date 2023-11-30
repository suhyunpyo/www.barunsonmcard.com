using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_ReservationWord
    {
        public int ReserveWord_ID { get; set; }
        public string ReserveWord { get; set; }
        public string MappingField { get; set; }
        public string DefaultValue { get; set; }
        public string Mapping_YN { get; set; }
        public int Sort { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public string Product_Category_Codes { get; set; }
    }
}
