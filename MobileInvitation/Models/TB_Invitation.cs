using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation
    {
        public TB_Invitation()
        {
            TB_Account_Extras = new HashSet<TB_Account_Extra>();
            TB_Accounts = new HashSet<TB_Account>();
            TB_Galleries = new HashSet<TB_Gallery>();
            TB_GuestBooks = new HashSet<TB_GuestBook>();
            TB_Invitation_Accounts = new HashSet<TB_Invitation_Account>();
            TB_Invitation_Areas = new HashSet<TB_Invitation_Area>();
            TB_Invitation_Detail_Etcs = new HashSet<TB_Invitation_Detail_Etc>();
            TB_Invitation_Items = new HashSet<TB_Invitation_Item>();
        }

        public int Invitation_ID { get; set; }
        public int Order_ID { get; set; }
        public int Template_ID { get; set; }
        public string User_ID { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Invitation_Display_YN { get; set; }

        public virtual TB_Order Order { get; set; }
        public virtual TB_Template Template { get; set; }
        public virtual TB_Invitation_Detail TB_Invitation_Detail { get; set; }
        public virtual ICollection<TB_Account_Extra> TB_Account_Extras { get; set; }
        public virtual ICollection<TB_Account> TB_Accounts { get; set; }
        public virtual ICollection<TB_Gallery> TB_Galleries { get; set; }
        public virtual ICollection<TB_GuestBook> TB_GuestBooks { get; set; }
        public virtual ICollection<TB_Invitation_Account> TB_Invitation_Accounts { get; set; }
        public virtual ICollection<TB_Invitation_Area> TB_Invitation_Areas { get; set; }
        public virtual ICollection<TB_Invitation_Detail_Etc> TB_Invitation_Detail_Etcs { get; set; }
        public virtual ICollection<TB_Invitation_Item> TB_Invitation_Items { get; set; }
    }
}
