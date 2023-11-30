using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Invitation_Detail
    {
        public int Invitation_ID { get; set; }
        public string Invitation_URL { get; set; }
        public string Invitation_Title { get; set; }
        public string Greetings { get; set; }
        public string Sender { get; set; }
        public string Delegate_Image_URL { get; set; }
        public string SNS_Image_URL { get; set; }
        public string GuestBook_Use_YN { get; set; }
        public string Groom_Name { get; set; }
        public string Groom_EngName { get; set; }
        public string Groom_Global_Phone_YN { get; set; }
        public string Groom_Global_Phone_Number { get; set; }
        public string Groom_Phone { get; set; }
        public string Bride_Name { get; set; }
        public string Bride_EngName { get; set; }
        public string Bride_Global_Phone_YN { get; set; }
        public string Bride_Global_Phone_Number { get; set; }
        public string Bride_Phone { get; set; }
        public string Parents_Information_Use_YN { get; set; }
        public string Groom_Parents1_Title { get; set; }
        public string Groom_Parents1_Name { get; set; }
        public string Groom_Parents1_Global_Phone_Number_YN { get; set; }
        public string Groom_Parents1_Global_Phone_Number { get; set; }
        public string Groom_Parents1_Phone { get; set; }
        public string Groom_Parents2_Title { get; set; }
        public string Groom_Parents2_Name { get; set; }
        public string Groom_Parents2_Global_Phone_Number_YN { get; set; }
        public string Groom_Parents2_Global_Phone_Number { get; set; }
        public string Groom_Parents2_Phone { get; set; }
        public string Bride_Parents1_Title { get; set; }
        public string Bride_Parents1_Name { get; set; }
        public string Bride_Parents1_Global_Phone_Number_YN { get; set; }
        public string Bride_Parents1_Global_Phone_Number { get; set; }
        public string Bride_Parents1_Phone { get; set; }
        public string Bride_Parents2_Title { get; set; }
        public string Bride_Parents2_Name { get; set; }
        public string Bride_Parents2_Global_Phone_Number_YN { get; set; }
        public string Bride_Parents2_Global_Phone_Number { get; set; }
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
        public string WeddingMin { get; set; }
        public string Weddinghall_Name { get; set; }
        public string WeddingHallDetail { get; set; }
        public string Weddinghall_Address { get; set; }
        public string Weddinghall_PhoneNumber { get; set; }
        public string Outline_Type_Code { get; set; }
        public string Outline_Image_URL { get; set; }
        public double? Location_LAT { get; set; }
        public double? Location_LOT { get; set; }
        public string Etc_Information_Use_YN { get; set; }
        public string Invitation_Video_Use_YN { get; set; }
        public string Invitation_Video_Type_Code { get; set; }
        public string Invitation_Video_URL { get; set; }
        public string Gallery_Use_YN { get; set; }
        public string Gallery_Type_Code { get; set; }
        public string MoneyGift_Remit_Use_YN { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string WeddingHour { get; set; }
        public double? Delegate_Image_Height { get; set; }
        public double? Delegate_Image_Width { get; set; }
        public double? SNS_Image_Height { get; set; }
        public double? SNS_Image_Width { get; set; }
        public string MMS_Send_YN { get; set; }
        public string Invitation_Display_YN { get; set; }
        public string MoneyAccount_Remit_Use_YN { get; set; }
        public string MoneyAccount_Div_Use_YN { get; set; }
        public string DetailNewLineYN { get; set; }
        public string Conf_KaKaoPay_YN { get; set; }
        public string Conf_Remit_YN { get; set; }
        public string ExtendData { get; set; }

        public string Flower_gift_YN { get; set; }
        public virtual TB_Invitation Invitation { get; set; }

        public string GalleryPreventPhoto_YN { get; set; }
    }
}
