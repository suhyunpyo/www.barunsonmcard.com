using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Item_Resource
    {
        public TB_Item_Resource()
        {
            TB_Invitation_Items = new HashSet<TB_Invitation_Item>();
            TB_Template_Items = new HashSet<TB_Template_Item>();
        }

        public int Resource_ID { get; set; }
        public string CharacterSet { get; set; }
        public double? Character_Size { get; set; }
        public string Color { get; set; }
        public string Background_Color { get; set; }
        public string Bold_YN { get; set; }
        public string Italic_YN { get; set; }
        public string Underline_YN { get; set; }
        public double? BetweenText { get; set; }
        public double? BetweenLine { get; set; }
        public string Vertical_Alignment { get; set; }
        public string Horizontal_Alignment { get; set; }
        public int? Sort { get; set; }
        public string Font { get; set; }
        public string Resource_URL { get; set; }
        public double? Resource_Height { get; set; }
        public double? Resource_Width { get; set; }
        public string Resource_Type_Code { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual ICollection<TB_Invitation_Item> TB_Invitation_Items { get; set; }
        public virtual ICollection<TB_Template_Item> TB_Template_Items { get; set; }
    }
}
