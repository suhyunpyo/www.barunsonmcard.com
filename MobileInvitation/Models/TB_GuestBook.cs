using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_GuestBook
    {
        public int GuestBook_ID { get; set; }
        public int Invitation_ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string Display_YN { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Invitation Invitation { get; set; }
    }
}
