using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Board
    {
        public int Board_ID { get; set; }
        public string Board_Category { get; set; }
        public string Top_YN { get; set; }
        public string Display_YN { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Hits { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
    }
}
