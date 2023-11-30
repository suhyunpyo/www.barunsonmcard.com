using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Item
    {
        public int Item_ID { get; set; }
        public int Invitation_ID { get; set; }
        public int Resource_ID { get; set; }
        public int? Area_ID { get; set; }
        public string Item_Type_Code { get; set; }
        public double? Location_Top { get; set; }
        public double? Location_Left { get; set; }
        public double? Size_Height { get; set; }
        public double? Size_Width { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
        public virtual TB_Item_Resource Resource { get; set; }
    }
}
