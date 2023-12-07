using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Gallery
    {
        public int Gallery_ID { get; set; }
        public int? Invitation_ID { get; set; }
        public int? Sort { get; set; }
        public string Image_URL { get; set; }
        public int? Image_Height { get; set; }
        public int? Image_Width { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public long? FileSize { get; set; }
        [StringLength(1000)]
        public string SmallImage_URL { get; set; }
        public virtual TB_Invitation Invitation { get; set; }
    }
}
