using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Template_Detail
    {
        public int Template_ID { get; set; }
        public string Greetings { get; set; }
        public string Groom_Name { get; set; }
        public string Groom_EngName { get; set; }
        public string Groom_Phone { get; set; }
        public string Bride_Name { get; set; }
        public string Bride_EngName { get; set; }
        public string Bride_Phone { get; set; }
        public string Groom_Parents1_Name { get; set; }
        public string Groom_Parents1_Phone { get; set; }
        public string Groom_Parents2_Name { get; set; }
        public string Groom_Parents2_Phone { get; set; }
        public string Bride_Parents1_Name { get; set; }
        public string Bride_Parents1_Phone { get; set; }
        public string Bride_Parents2_Name { get; set; }
        public string Bride_Parents2_Phone { get; set; }
        public string WeddingDate { get; set; }
        public string WeddingHHmm { get; set; }
        public string Time_Type_Code { get; set; }
        public string Time_Type_Eng_YN { get; set; }
        public string WeddingYY { get; set; }
        public string WeddingMM { get; set; }
        public string WeddingDD { get; set; }
        public string WeddingWeek { get; set; }
        public string WeddingWeek_Eng_YN { get; set; }
        public string WeddingHour { get; set; }
        public string WeddingMin { get; set; }
        public string Weddinghall_Name { get; set; }
        public string WeddingHallDetail { get; set; }
        public string Weddinghall_Address { get; set; }
        public string Weddinghall_PhoneNumber { get; set; }
        public string Etc_Bus_Information { get; set; }
        public string Etc_Car_Information { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public string Baby_Name { get; set; }
        public DateTime? Baby_Birthday { get; set; }
        public string RepeatData { get; set; }

        public virtual TB_Template Template { get; set; }
    }
}
