using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using X.PagedList;
using static MobileInvitation.Areas.User.Controllers.Invitaion.InvitationController;

namespace MobileInvitation.Data.Invitation
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly barunsonContext Entity_db;


        public InvitationRepository(barunsonContext _Entity_db)
        {
            Entity_db = _Entity_db;
        }

        #region
        public List<TB_Invitation> TB_Invitation_Entity(int Order_ID)
        {
            var query = from invitation in Entity_db.Set<TB_Invitation>().Where(x => x.Order_ID == Order_ID)

                        select new { invitation };

            var list = new List<TB_Invitation>();

            foreach (var item in query)
            {
                list.Add(new TB_Invitation()
                {
                    Template_ID = item.invitation.Template_ID,
                    Invitation_ID = item.invitation.Invitation_ID
                });
            }

            return list;
        }

        public int TB_Invitation_Insert_Sql(TB_Invitation invitation)
        {
            Entity_db.TB_Invitations.Add(invitation);
            Entity_db.SaveChanges();

            return invitation.Invitation_ID;
        }

        public int TB_Invitation_Detail_Insert_Sql(TB_Invitation_Detail invitation_detail)
        {
            Entity_db.TB_Invitation_Details.Add(invitation_detail);
            Entity_db.SaveChanges();

            return invitation_detail.Invitation_ID;
        }

        public int TB_Invitation_Detail_Update_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();
            //화환 배너 노출 여부를 위한 예시장 제외 정보 읽기, Allowed == false가 배너 노출 안함임.
            var keywords = new List<string>();
            var weddKeywordItems = Entity_db.TB_FlaBannerManage.Where(x => x.Allowed == false).Select(x => x.Keywords).ToList();
            foreach ( var keyword in weddKeywordItems)
            {
                var keyItems = JsonConvert.DeserializeObject<List<string>>(keyword);
                keywords.AddRange(keyItems);
            }

            entity.Invitation_Title = invitation_detail.Invitation_Title;
            entity.Invitation_URL = invitation_detail.Invitation_URL;
            entity.Greetings = invitation_detail.Greetings;
            entity.Groom_Name = invitation_detail.Groom_Name;
            entity.Groom_Phone = invitation_detail.Groom_Phone;
            entity.Bride_Name = invitation_detail.Bride_Name;
            entity.Bride_Phone = invitation_detail.Bride_Phone;
            entity.Groom_Parents1_Name = invitation_detail.Groom_Parents1_Name;
            entity.Groom_Parents1_Phone = invitation_detail.Groom_Parents1_Phone;
            entity.Groom_Parents2_Name = invitation_detail.Groom_Parents2_Name;
            entity.Groom_Parents2_Phone = invitation_detail.Groom_Parents2_Phone;
            entity.Bride_Parents1_Name = invitation_detail.Bride_Parents1_Name;
            entity.Bride_Parents1_Phone = invitation_detail.Bride_Parents1_Phone;
            entity.Bride_Parents2_Name = invitation_detail.Bride_Parents2_Name;
            entity.Bride_Parents2_Phone = invitation_detail.Bride_Parents2_Phone;

            entity.Groom_Global_Phone_YN = invitation_detail.Groom_Global_Phone_YN;
            entity.Groom_Global_Phone_Number = invitation_detail.Groom_Global_Phone_Number;
            entity.Bride_Global_Phone_YN = invitation_detail.Bride_Global_Phone_YN;
            entity.Bride_Global_Phone_Number = invitation_detail.Bride_Global_Phone_Number;
            entity.Groom_Parents1_Global_Phone_Number_YN = invitation_detail.Groom_Parents1_Global_Phone_Number_YN;
            entity.Groom_Parents1_Global_Phone_Number = invitation_detail.Groom_Parents1_Global_Phone_Number;
            entity.Groom_Parents2_Global_Phone_Number_YN = invitation_detail.Groom_Parents2_Global_Phone_Number_YN;
            entity.Groom_Parents2_Global_Phone_Number = invitation_detail.Groom_Parents2_Global_Phone_Number;

            entity.Bride_Parents1_Global_Phone_Number_YN = invitation_detail.Bride_Parents1_Global_Phone_Number_YN;
            entity.Bride_Parents1_Global_Phone_Number = invitation_detail.Bride_Parents1_Global_Phone_Number;
            entity.Bride_Parents2_Global_Phone_Number_YN = invitation_detail.Bride_Parents2_Global_Phone_Number_YN;
            entity.Bride_Parents2_Global_Phone_Number = invitation_detail.Bride_Parents2_Global_Phone_Number;

            entity.Groom_Parents1_Title = invitation_detail.Groom_Parents1_Title;
            entity.Groom_Parents2_Title = invitation_detail.Groom_Parents2_Title;
            entity.Bride_Parents1_Title = invitation_detail.Bride_Parents1_Title;
            entity.Bride_Parents2_Title = invitation_detail.Bride_Parents2_Title;

            entity.WeddingDate = invitation_detail.WeddingDate;
            entity.WeddingHHmm = invitation_detail.WeddingHHmm;
            entity.Time_Type_Code = invitation_detail.Time_Type_Code;
            entity.WeddingYY = invitation_detail.WeddingYY;
            entity.WeddingMM = invitation_detail.WeddingMM;
            entity.WeddingDD = invitation_detail.WeddingDD;
            entity.WeddingWeek = invitation_detail.WeddingWeek;
            entity.WeddingHour = invitation_detail.WeddingHour;
            entity.WeddingMin = invitation_detail.WeddingMin;
            entity.Weddinghall_Name = invitation_detail.Weddinghall_Name ?? "";
            entity.WeddingHallDetail = invitation_detail.WeddingHallDetail;
            entity.Weddinghall_Address = invitation_detail.Weddinghall_Address;
            entity.Weddinghall_PhoneNumber = invitation_detail.Weddinghall_PhoneNumber;

            entity.Location_LAT = invitation_detail.Location_LAT;
            entity.Location_LOT = invitation_detail.Location_LOT;

            entity.Outline_Type_Code = invitation_detail.Outline_Type_Code;
            entity.Outline_Image_URL = invitation_detail.Outline_Image_URL;

            entity.GuestBook_Use_YN = invitation_detail.GuestBook_Use_YN;
            entity.Etc_Information_Use_YN = invitation_detail.Etc_Information_Use_YN;
            entity.Parents_Information_Use_YN = invitation_detail.Parents_Information_Use_YN;
            entity.MoneyGift_Remit_Use_YN = invitation_detail.MoneyGift_Remit_Use_YN;
            entity.MoneyAccount_Remit_Use_YN = invitation_detail.MoneyAccount_Remit_Use_YN;
            entity.MoneyAccount_Div_Use_YN = invitation_detail.MoneyAccount_Div_Use_YN;

            entity.DetailNewLineYN = invitation_detail.DetailNewLineYN;

            entity.Conf_KaKaoPay_YN = entity.Conf_KaKaoPay_YN == "N" ? invitation_detail.Conf_KaKaoPay_YN : entity.Conf_KaKaoPay_YN;

            entity.Conf_Remit_YN = entity.Conf_Remit_YN == "N" ? invitation_detail.Conf_Remit_YN : entity.Conf_Remit_YN;

            //화환 배너 노출 여부 검사, 관리자가 등록한 예식장 명으로 노출 여부 설정
            //웨딩홀 이름은 공백, 대소문자등 정확한 문자가 일치할경우에만 적용
            if (keywords.Any(m => m == invitation_detail.Weddinghall_Name))
                invitation_detail.Flower_gift_YN = "N";
            else
                invitation_detail.Flower_gift_YN = "Y";

            if (entity.Flower_gift_YN == "Y" && invitation_detail.Flower_gift_YN == "N")
            {
                // 취소
                entity.Flower_gift_YN = "C";
            }
            //중요: 노출 안함 상태는 다시 업데이트 하지 않음... 다시 업데이트 필요시 아래 주석 제거
            //else if (!(entity.Flower_gift_YN == "C" && invitation_detail.Flower_gift_YN == "N") &&
            //    (entity.Flower_gift_YN != invitation_detail.Flower_gift_YN))
            //{
            //    //기존값이 C(취소)이고 입력값이 N이 아니고, 기존 값과 입력값이 다를 경우만 업데이트
            //    entity.Flower_gift_YN = invitation_detail.Flower_gift_YN;
            //}

            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            if (!string.IsNullOrEmpty(invitation_detail.ExtendData))
                entity.ExtendData = invitation_detail.ExtendData;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            if (!string.IsNullOrEmpty(entity.Invitation_URL))
                ChangeInvitationUrl(entity.Invitation_URL);

            return invitation_detail.Invitation_ID;
        }

        public int TB_Invitation_Detail_Update_For_Admin_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();

            entity.Invitation_Title = invitation_detail.Invitation_Title;
            entity.Invitation_URL = invitation_detail.Invitation_URL;
            entity.Greetings = invitation_detail.Greetings;
            entity.Groom_Name = invitation_detail.Groom_Name;
            entity.Groom_Phone = invitation_detail.Groom_Phone;
            entity.Bride_Name = invitation_detail.Bride_Name;
            entity.Bride_Phone = invitation_detail.Bride_Phone;
            entity.Groom_Parents1_Name = invitation_detail.Groom_Parents1_Name;
            entity.Groom_Parents1_Phone = invitation_detail.Groom_Parents1_Phone;
            entity.Groom_Parents2_Name = invitation_detail.Groom_Parents2_Name;
            entity.Groom_Parents2_Phone = invitation_detail.Groom_Parents2_Phone;
            entity.Bride_Parents1_Name = invitation_detail.Bride_Parents1_Name;
            entity.Bride_Parents1_Phone = invitation_detail.Bride_Parents1_Phone;
            entity.Bride_Parents2_Name = invitation_detail.Bride_Parents2_Name;
            entity.Bride_Parents2_Phone = invitation_detail.Bride_Parents2_Phone;

            entity.WeddingDate = invitation_detail.WeddingDate;
            if (invitation_detail.WeddingHHmm != null)
            {
                entity.WeddingHHmm = invitation_detail.WeddingHHmm;
            }
            entity.Time_Type_Code = invitation_detail.Time_Type_Code;
            entity.WeddingYY = invitation_detail.WeddingYY;
            entity.WeddingMM = invitation_detail.WeddingMM;
            entity.WeddingDD = invitation_detail.WeddingDD;
            entity.WeddingWeek = invitation_detail.WeddingWeek;
            entity.WeddingHour = invitation_detail.WeddingHour;
            entity.WeddingMin = invitation_detail.WeddingMin;
            if (invitation_detail.Weddinghall_Name != null)
            {
                entity.Weddinghall_Name = invitation_detail.Weddinghall_Name;
            }
            entity.WeddingHallDetail = invitation_detail.WeddingHallDetail;
            entity.Weddinghall_Address = invitation_detail.Weddinghall_Address;
            entity.Weddinghall_PhoneNumber = invitation_detail.Weddinghall_PhoneNumber;
            entity.WeddingWeek_Eng_YN = invitation_detail.WeddingWeek_Eng_YN;
            entity.Time_Type_Eng_YN = invitation_detail.Time_Type_Eng_YN;

            if (entity.Delegate_Image_URL != null)
            {
                entity.Delegate_Image_URL = invitation_detail.Delegate_Image_URL;
                entity.Delegate_Image_Height = invitation_detail.Delegate_Image_Height;
                entity.Delegate_Image_Width = invitation_detail.Delegate_Image_Width;
            }

            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            if (!string.IsNullOrEmpty(entity.Invitation_URL))
                ChangeInvitationUrl(entity.Invitation_URL);

            return invitation_detail.Invitation_ID;
        }

        public int TB_Invitation_Area_Update_For_Admin_Sql(TB_Invitation_Area invatation_area)
        {
            var entity = Entity_db.Set<TB_Invitation_Area>().Where(x => x.Invitation_ID == invatation_area.Invitation_ID && x.Area_ID == invatation_area.Area_ID).FirstOrDefault();

            entity.Size_Height = invatation_area.Size_Height;
            entity.Size_Width = invatation_area.Size_Width;
            entity.Color = invatation_area.Color;
            entity.Sort = invatation_area.Sort;
            entity.Update_User_ID = invatation_area.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = invatation_area.Update_IP;

            Entity_db.TB_Invitation_Areas.Update(entity);
            Entity_db.SaveChanges();

            return entity.Area_ID;
        }

        public bool TB_Invitation_Item_Resource_Delete_For_Admin_Sql(List<Invitation_Item_Resource> invitation_item_resources, int Invitation_ID)
        {
            try
            {
                var resource_ids = invitation_item_resources.Select(x => x.resource_id).ToList();

                var query = Entity_db.Set<TB_Invitation_Item>().Where(x => x.Invitation_ID == Invitation_ID && !resource_ids.Contains(x.Resource_ID)).ToList();

                foreach (var item in query)
                {
                    Entity_db.TB_Invitation_Items.Remove(item);
                    Entity_db.SaveChanges();

                    var entity = Entity_db.Set<TB_Item_Resource>().Where(x => x.Resource_ID == item.Resource_ID).FirstOrDefault();

                    Entity_db.TB_Item_Resources.Remove(entity);
                    Entity_db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public int TB_Invitation_Item_Insert_Sql(TB_Invitation_Item invitation_item)
        {
            Entity_db.TB_Invitation_Items.Add(invitation_item);
            Entity_db.SaveChanges();

            return invitation_item.Item_ID;
        }

        public int TB_Invitation_Item_Update_Sql(TB_Invitation_Item invitation_item)
        {
            var entity = Entity_db.Set<TB_Invitation_Item>().Where(x => x.Resource_ID == invitation_item.Resource_ID).FirstOrDefault();

            entity.Area_ID = invitation_item.Area_ID;
            entity.Item_Type_Code = invitation_item.Item_Type_Code;
            entity.Location_Top = invitation_item.Location_Top;
            entity.Location_Left = invitation_item.Location_Left;
            entity.Size_Height = invitation_item.Size_Height;
            entity.Size_Width = invitation_item.Size_Width;

            entity.Update_User_ID = invitation_item.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = invitation_item.Update_IP;

            Entity_db.TB_Invitation_Items.Update(entity);
            Entity_db.SaveChanges();

            return entity.Item_ID;
        }


        public int TB_Item_Resource_Insert_Sql(TB_Item_Resource item_resource)
        {
            Entity_db.TB_Item_Resources.Add(item_resource);
            Entity_db.SaveChanges();

            return item_resource.Resource_ID;
        }

        public int TB_Item_Resource_Update_Sql(TB_Item_Resource item_resource)
        {
            var entity = Entity_db.Set<TB_Item_Resource>().Where(x => x.Resource_ID == item_resource.Resource_ID).FirstOrDefault();

            entity.CharacterSet = item_resource.CharacterSet;
            entity.Character_Size = item_resource.Character_Size;
            entity.Color = item_resource.Color;
            entity.Background_Color = item_resource.Background_Color;
            entity.Bold_YN = item_resource.Bold_YN;
            entity.Italic_YN = item_resource.Italic_YN;
            entity.Underline_YN = item_resource.Underline_YN;
            entity.BetweenText = item_resource.BetweenText;
            entity.BetweenLine = item_resource.BetweenLine;
            entity.Vertical_Alignment = item_resource.Vertical_Alignment;
            entity.Horizontal_Alignment = item_resource.Horizontal_Alignment;
            entity.Sort = item_resource.Sort;
            entity.Font = item_resource.Font;
            entity.Resource_URL = item_resource.Resource_URL;
            entity.Resource_Height = item_resource.Resource_Height;
            entity.Resource_Width = item_resource.Resource_Width;
            entity.Resource_Type_Code = item_resource.Resource_Type_Code;

            entity.Update_User_ID = item_resource.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = item_resource.Update_IP;

            Entity_db.TB_Item_Resources.Update(entity);
            Entity_db.SaveChanges();

            return entity.Resource_ID;
        }

        public int TB_Invitation_Area_Insert_Sql(TB_Invitation_Area inivation_area)
        {
            Entity_db.TB_Invitation_Areas.Add(inivation_area);
            Entity_db.SaveChanges();

            return inivation_area.Invitation_ID;
        }


        public List<Invitation_Item_Resource> TB_Invitation_Item_Resource_LIst(int Invitation_ID)
        {
            var query = from invitation_item in Entity_db.Set<TB_Invitation_Item>().Where(x => x.Invitation_ID == Invitation_ID)
                        join item_resource in Entity_db.Set<TB_Item_Resource>()
                        on invitation_item.Resource_ID equals item_resource.Resource_ID
                        orderby item_resource.Sort
                        select new
                        {
                            invitation_item,
                            item_resource
                        };

            var list = new List<Invitation_Item_Resource>();

            foreach (var item in query)
            {
                list.Add(new Invitation_Item_Resource()
                {
                    item_id = item.invitation_item.Item_ID,
                    resource_id = item.item_resource.Resource_ID,
                    pid = "area" + item.invitation_item.Area_ID,
                    id = "item_" + item.item_resource.Sort,
                    type = CodeNameHelper.ItemTypeCodeToResourceTypeCode(item.invitation_item.Item_Type_Code),
                    top = item.invitation_item.Location_Top,
                    left = item.invitation_item.Location_Left,
                    height = item.invitation_item.Size_Height,
                    width = item.invitation_item.Size_Width,
                    chracterset = item.item_resource.CharacterSet,
                    fontsize = item.item_resource.Character_Size,
                    fontcolor = item.item_resource.Color,
                    bgcolor = item.item_resource.Background_Color,
                    bold_yn = item.item_resource.Bold_YN == "Y" ? true : false,
                    italic_yn = item.item_resource.Italic_YN == "Y" ? true : false,
                    underline_yn = item.item_resource.Underline_YN == "Y" ? true : false,
                    between_text = item.item_resource.BetweenText,
                    between_line = item.item_resource.BetweenLine,
                    vertical_align = item.item_resource.Vertical_Alignment,
                    horizontal_align = item.item_resource.Horizontal_Alignment,
                    zindex = item.item_resource.Sort,
                    font = item.item_resource.Font,
                    resource_url = item.item_resource.Resource_URL,
                    org_height = item.item_resource.Resource_Height,
                    org_width = item.item_resource.Resource_Width
                });
            }
            return list;
        }

        public List<TB_Invitation_Area> TB_Invitation_Area_Entity(int Invitation_ID)
        {
            var query = from invitation_area in Entity_db.Set<TB_Invitation_Area>().Where(x => x.Invitation_ID == Invitation_ID)
                        select new { invitation_area };


            var list = new List<TB_Invitation_Area>();

            foreach (var item in query)
            {
                list.Add(new TB_Invitation_Area()
                {
                    Invitation_ID = item.invitation_area.Invitation_ID,
                    Area_ID = item.invitation_area.Area_ID,
                    Size_Height = item.invitation_area.Size_Height,
                    Size_Width = item.invitation_area.Size_Width,
                    Sort = item.invitation_area.Sort,
                    Color = item.invitation_area.Color
                });
            }

            return list;
        }

        public List<TB_Invitation_Detail> TB_Invitation_Detail_Entity(int Invitation_ID)
        {
            var query = from invitation_datail in Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == Invitation_ID).ToList()
                        select new { invitation_datail };

            var list = new List<TB_Invitation_Detail>();

            foreach (var item in query)
            {
                list.Add(new TB_Invitation_Detail()
                {
                    Invitation_ID = item.invitation_datail.Invitation_ID,
                    Invitation_Title = item.invitation_datail.Invitation_Title,
                    Invitation_URL = item.invitation_datail.Invitation_URL,
                    Greetings = item.invitation_datail.Greetings,
                    Groom_Name = item.invitation_datail.Groom_Name,
                    Groom_Global_Phone_YN = item.invitation_datail.Groom_Global_Phone_YN,//
                    Groom_Global_Phone_Number = item.invitation_datail.Groom_Global_Phone_Number,//
                    Groom_Phone = item.invitation_datail.Groom_Phone,
                    Bride_Name = item.invitation_datail.Bride_Name,
                    Bride_Global_Phone_YN = item.invitation_datail.Bride_Global_Phone_YN,//
                    Bride_Global_Phone_Number = item.invitation_datail.Bride_Global_Phone_Number,//
                    Bride_Phone = item.invitation_datail.Bride_Phone,
                    Groom_Parents1_Name = item.invitation_datail.Groom_Parents1_Name,
                    Groom_Parents1_Global_Phone_Number_YN = item.invitation_datail.Groom_Parents1_Global_Phone_Number_YN,//
                    Groom_Parents1_Global_Phone_Number = item.invitation_datail.Groom_Parents1_Global_Phone_Number,//
                    Groom_Parents1_Phone = item.invitation_datail.Groom_Parents1_Phone,
                    Groom_Parents2_Name = item.invitation_datail.Groom_Parents2_Name,
                    Groom_Parents2_Global_Phone_Number_YN = item.invitation_datail.Groom_Parents2_Global_Phone_Number_YN,//
                    Groom_Parents2_Global_Phone_Number = item.invitation_datail.Groom_Parents2_Global_Phone_Number,//
                    Groom_Parents2_Phone = item.invitation_datail.Groom_Parents2_Phone,
                    Bride_Parents1_Name = item.invitation_datail.Bride_Parents1_Name,
                    Bride_Parents1_Global_Phone_Number_YN = item.invitation_datail.Bride_Parents1_Global_Phone_Number_YN,//
                    Bride_Parents1_Global_Phone_Number = item.invitation_datail.Bride_Parents1_Global_Phone_Number,//
                    Bride_Parents1_Phone = item.invitation_datail.Bride_Parents1_Phone,
                    Bride_Parents2_Name = item.invitation_datail.Bride_Parents2_Name,
                    Bride_Parents2_Global_Phone_Number_YN = item.invitation_datail.Bride_Parents2_Global_Phone_Number_YN,//
                    Bride_Parents2_Global_Phone_Number = item.invitation_datail.Bride_Parents2_Global_Phone_Number,//
                    Bride_Parents2_Phone = item.invitation_datail.Bride_Parents2_Phone,

                    Groom_Parents1_Title = item.invitation_datail.Groom_Parents1_Title,
                    Groom_Parents2_Title = item.invitation_datail.Groom_Parents2_Title,
                    Bride_Parents1_Title = item.invitation_datail.Bride_Parents1_Title,
                    Bride_Parents2_Title = item.invitation_datail.Bride_Parents2_Title,


                    WeddingDate = item.invitation_datail.WeddingDate,
                    WeddingHHmm = item.invitation_datail.WeddingHHmm,
                    Time_Type_Code = item.invitation_datail.Time_Type_Code,
                    WeddingYY = item.invitation_datail.WeddingYY,
                    WeddingMM = item.invitation_datail.WeddingMM,
                    WeddingDD = item.invitation_datail.WeddingDD,
                    WeddingWeek = item.invitation_datail.WeddingWeek,
                    WeddingHour = item.invitation_datail.WeddingHour,
                    WeddingMin = item.invitation_datail.WeddingMin,
                    Weddinghall_Name = item.invitation_datail.Weddinghall_Name,
                    WeddingHallDetail = item.invitation_datail.WeddingHallDetail,
                    Weddinghall_Address = item.invitation_datail.Weddinghall_Address,
                    Weddinghall_PhoneNumber = item.invitation_datail.Weddinghall_PhoneNumber,
                    Location_LAT = item.invitation_datail.Location_LAT,
                    Location_LOT = item.invitation_datail.Location_LOT,
                    Outline_Type_Code = item.invitation_datail.Outline_Type_Code,
                    Outline_Image_URL = item.invitation_datail.Outline_Image_URL,

                    GuestBook_Use_YN = item.invitation_datail.GuestBook_Use_YN, //방명록
                    Etc_Information_Use_YN = item.invitation_datail.Etc_Information_Use_YN, //기타정보
                    Parents_Information_Use_YN = item.invitation_datail.Parents_Information_Use_YN, //혼주정보
                    MoneyGift_Remit_Use_YN = item.invitation_datail.MoneyGift_Remit_Use_YN, //축의금 송금(카카오페이)
                    MoneyAccount_Remit_Use_YN = item.invitation_datail.MoneyAccount_Remit_Use_YN, //축의금 송금(계좌이체)
                    MoneyAccount_Div_Use_YN = item.invitation_datail.MoneyAccount_Div_Use_YN, //축의금 송금(계좌이체)
                    Invitation_Video_Use_YN = item.invitation_datail.Invitation_Video_Use_YN, //초대영상
                    Gallery_Use_YN = item.invitation_datail.Gallery_Use_YN, //이미지관리 
                    Flower_gift_YN = item.invitation_datail.Flower_gift_YN, //화환선물하기
                    Gallery_Type_Code = item.invitation_datail.Gallery_Type_Code,
                    Invitation_Video_Type_Code = item.invitation_datail.Invitation_Video_Type_Code,
                    Invitation_Video_URL = item.invitation_datail.Invitation_Video_URL,

                    SNS_Image_URL = item.invitation_datail.SNS_Image_URL,
                    SNS_Image_Height = item.invitation_datail.SNS_Image_Height,
                    SNS_Image_Width = item.invitation_datail.SNS_Image_Width,
                    Delegate_Image_URL = item.invitation_datail.Delegate_Image_URL,
                    Delegate_Image_Height = item.invitation_datail.Delegate_Image_Height,
                    Delegate_Image_Width = item.invitation_datail.Delegate_Image_Width,

                    Time_Type_Eng_YN = item.invitation_datail.Time_Type_Eng_YN,
                    WeddingWeek_Eng_YN = item.invitation_datail.WeddingWeek_Eng_YN,
                    Sender = item.invitation_datail.Sender,
                    DetailNewLineYN = item.invitation_datail.DetailNewLineYN != null ? item.invitation_datail.DetailNewLineYN : "N",
                    GalleryPreventPhoto_YN = item.invitation_datail.GalleryPreventPhoto_YN != null ? item.invitation_datail.GalleryPreventPhoto_YN : "N",
                    ExtendData = item.invitation_datail.ExtendData
                }); 
            }

            return list;
        }

        public List<TB_Gallery> TB_Gallery_List(int Invitation_ID)
        {
            var query = from gallery in Entity_db.Set<TB_Gallery>().Where(x => x.Invitation_ID == Invitation_ID).ToList()
                        orderby gallery.Sort
                        select new { gallery };

            var list = new List<TB_Gallery>();

            foreach (var item in query)
            {
                list.Add(new TB_Gallery()
                {
                    Gallery_ID = item.gallery.Gallery_ID,
                    Invitation_ID = item.gallery.Invitation_ID,
                    Sort = item.gallery.Sort,
                    Image_URL = item.gallery.Image_URL,
                    Image_Height = item.gallery.Image_Height,
                    Image_Width = item.gallery.Image_Width,
                    SmallImage_URL = item.gallery.SmallImage_URL,

                });
            }

            return list;
        }

        public int TB_Gallery_Insert_Sql(TB_Gallery gallery)
        {
            Entity_db.TB_Galleries.Add(gallery);
            Entity_db.SaveChanges();

            return gallery.Gallery_ID;
        }

        public TB_Gallery TB_Gallery_Entity(int Gallery_ID)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Gallery_ID == Gallery_ID).FirstOrDefault();
                      
            return entity;
        }

        public int Gallery_Sort_Extra_Update_Sql(int Invitation_ID, int add_Value, int Start_Value, int End_Value)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Invitation_ID == Invitation_ID && x.Sort >= Start_Value && x.Sort <= End_Value).ToList();

            foreach (var item in entity)
            {
                item.Sort = item.Sort + add_Value;

                Entity_db.TB_Galleries.Update(item);
                Entity_db.SaveChanges();
            }

            return End_Value;
        }

        public int Gallery_Sort_Update_Sql(int Gallery_ID, int newSort)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Gallery_ID == Gallery_ID).FirstOrDefault();

            entity.Sort = newSort;

            Entity_db.TB_Galleries.Update(entity);
            Entity_db.SaveChanges();
            
            return newSort;
        }

        public int Gallery_Sort_Reset_Update_Sql(int Invitation_ID, int Sort)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Invitation_ID == Invitation_ID && x.Sort > Sort).ToList();

            foreach (var item in entity)
            {
                item.Sort = item.Sort - 1;

                Entity_db.TB_Galleries.Update(item);
                Entity_db.SaveChanges();
            }

            return Invitation_ID;
        }

        public int TB_Gallery_Delete_Entity(int Gallery_ID)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Gallery_ID == Gallery_ID).FirstOrDefault();
           
            Entity_db.Remove(entity);
            Entity_db.SaveChanges();

            return Gallery_ID;
        }


        public int SNS_Image_Update_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();

            entity.SNS_Image_URL = invitation_detail.SNS_Image_URL;
            entity.SNS_Image_Height = invitation_detail.SNS_Image_Height;
            entity.SNS_Image_Width = invitation_detail.SNS_Image_Width;

            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            return invitation_detail.Invitation_ID;
        }

        public int Delegate_Image_Update_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();

            entity.Delegate_Image_URL = invitation_detail.Delegate_Image_URL;
            entity.Delegate_Image_Height = invitation_detail.Delegate_Image_Height;
            entity.Delegate_Image_Width = invitation_detail.Delegate_Image_Width;

            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            return invitation_detail.Invitation_ID;
        }

        public int Gallery_Image_Update_Sql(TB_Gallery gallery)
        {
            var entity = Entity_db.Set<TB_Gallery>().Where(x => x.Gallery_ID == gallery.Gallery_ID).FirstOrDefault();

            entity.Image_URL = gallery.Image_URL;
            entity.Image_Width = gallery.Image_Width;
            entity.Image_Height = gallery.Image_Height;
            entity.SmallImage_URL = gallery.SmallImage_URL;

            entity.Update_User_ID = gallery.Update_User_ID;
            entity.Update_DateTime = gallery.Update_DateTime;
            entity.Update_IP = gallery.Update_IP;

            Entity_db.TB_Galleries.Update(entity);
            Entity_db.SaveChanges();

            return gallery.Gallery_ID;
        }


        public TB_Invitation_Detail CheckDuplicateURL_Entity(string invitation_url)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_URL == invitation_url).FirstOrDefault();

            return entity;
        }
        /// <summary>
        /// 초대장 URL 중복 체크
        /// </summary>
        /// <param name="invitation_url"></param>
        /// <returns>true 사용가능, false 사용불가</returns>
        public bool CheckDuplicateURL(string invitation_url)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_URL == invitation_url).Count();

            return (entity == 0);
        }

        public int Video_Gallery_Update_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();


            entity.Invitation_Video_Use_YN = invitation_detail.Invitation_Video_Use_YN;
            entity.Invitation_Video_Type_Code = invitation_detail.Invitation_Video_Type_Code;
            entity.Invitation_Video_URL = invitation_detail.Invitation_Video_URL;

            entity.Gallery_Use_YN = invitation_detail.Gallery_Use_YN;
            entity.Gallery_Type_Code = invitation_detail.Gallery_Type_Code;
            entity.GalleryPreventPhoto_YN = String.IsNullOrEmpty(invitation_detail.GalleryPreventPhoto_YN) ? "N" : invitation_detail.GalleryPreventPhoto_YN;

            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            return invitation_detail.Invitation_ID;
        }


        public Invitation_Item_Resource PhotoImage_Entity(int Invitation_Id)
        {
            var query = from invitation_item in Entity_db.Set<TB_Invitation_Item>().Where(x => x.Invitation_ID == Invitation_Id)
                        join item_resource in Entity_db.Set<TB_Item_Resource>().Where(x => x.Resource_Type_Code == "photo")
                        on invitation_item.Resource_ID equals item_resource.Resource_ID
                        orderby item_resource.Sort
                        select new
                        {
                            invitation_item,
                            item_resource
                        };

            var list = new List<Invitation_Item_Resource>();

            foreach (var item in query)
            {
                list.Add(new Invitation_Item_Resource()
                {
                    width = item.invitation_item.Size_Width,
                    height = item.invitation_item.Size_Height
                });
            }

            return list[0];
        }

        public int TB_Invitation_Detail_Etc_Delete_Sql(int Invitation_ID)
        {
            var query = Entity_db.Set<TB_Invitation_Detail_Etc>().Where(x => x.Invitation_ID == Invitation_ID).ToList();

            foreach (var item in query)
            {
                Entity_db.Remove(item);
                Entity_db.SaveChanges();
            }

            return Invitation_ID;
        }

        public int TB_Account_Extra_Delete_Sql(int Invitation_ID)
        {
            var query = Entity_db.Set<TB_Account_Extra>().Where(x => x.Invitation_ID == Invitation_ID).ToList();

            foreach (var item in query)
            {
                Entity_db.Remove(item);
                Entity_db.SaveChanges();
            }

            return Invitation_ID;
        }

        public int TB_Invitation_Account_Delete_Sql(int Invitation_ID)
        {
            var query = Entity_db.Set<TB_Invitation_Account>().Where(x => x.Invitation_ID == Invitation_ID).ToList();

            foreach (var item in query)
            {
                Entity_db.Remove(item);
                Entity_db.SaveChanges();
            }

            return Invitation_ID;
        }

        public int TB_Invitation_Detail_Etc_Sql(TB_Invitation_Detail_Etc invitation_detail_etc)
        {
            Entity_db.TB_Invitation_Detail_Etcs.Add(invitation_detail_etc);
            Entity_db.SaveChanges();

            return invitation_detail_etc.Invitation_ID;
        }




        public List<TB_Invitation_Detail_Etc> TB_Invitation_Detail_Etc_List(int Invitation_ID)
        {
            var query = from invitation_datail_etc in Entity_db.Set<TB_Invitation_Detail_Etc>().Where(x => x.Invitation_ID == Invitation_ID).ToList()
                        select new { invitation_datail_etc };

            var list = new List<TB_Invitation_Detail_Etc>();

            foreach (var item in query)
            {
                list.Add(new TB_Invitation_Detail_Etc()
                {
                    Invitation_ID = item.invitation_datail_etc.Invitation_ID,
                    Sort = item.invitation_datail_etc.Sort,
                    Etc_Title = item.invitation_datail_etc.Etc_Title,
                    Information_Content = item.invitation_datail_etc.Information_Content

                });
            }

            return list;
        }


        public int TB_Account_Extra_Sql(TB_Account_Extra invitation_account)
        {
            Entity_db.TB_Account_Extras.Add(invitation_account);
            Entity_db.SaveChanges();

            return invitation_account.Invitation_ID;
        }

        public int TB_Invitation_Account_Sql(TB_Invitation_Account invitation_account)
        {
            Entity_db.TB_Invitation_Accounts.Add(invitation_account);
            Entity_db.SaveChanges();

            return invitation_account.Invitation_ID;
        }

        public string Get_Send_Name(string Send_Code)
        {
            var query = from m in Entity_db.TB_Common_Codes
                        where m.Code_Group == "Account_Type_Code" && m.Code == Send_Code
                        select m.Code_Name;

            return query.FirstOrDefault() ?? "";
  
        }


        public List<Dictionary<string, object>> TB_Account_Extra_List(int Invitation_ID)
        {
            var query = from invitation_account in Entity_db.Set<TB_Account_Extra>().Where(x => x.Invitation_ID == Invitation_ID)
                        join bank in Entity_db.Set<TB_Bank>().Where(x => x.Use_YN == "Y")
                        on invitation_account.Bank_Code equals bank.Bank_Code into Bank_Code
                        from b in Bank_Code.DefaultIfEmpty()
                        select new { invitation_account, b };

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Invitation_ID", item.invitation_account.Invitation_ID);
                dic.Add("Sort", item.invitation_account.Sort);
                dic.Add("Send_Target_Code", item.invitation_account.Send_Target_Code);
                dic.Add("Send_Name", item.invitation_account.Send_Name !=null ? item.invitation_account.Send_Name : "" );
                dic.Add("Bank_Code", item.invitation_account.Bank_Code);
                dic.Add("Bank_Name", item.b != null ? item.b.Bank_Name : "");
                dic.Add("Account_Number", item.invitation_account.Account_Number != null ? item.invitation_account.Account_Number : "");
                dic.Add("Account_Holder", item.invitation_account.Account_Holder != null ? item.invitation_account.Account_Holder : "");
                result.Add(dic);
            }

            return result;
        }

        public List<Dictionary<string, object>> TB_Invitation_Account_List(int Invitation_ID, int Category)
        {
            var query = from invitation_account in Entity_db.Set<TB_Invitation_Account>().Where(x => x.Invitation_ID == Invitation_ID && x.Category == Category)
                        join bank in Entity_db.Set<TB_Bank>().Where(x => x.Use_YN == "Y")
                        on invitation_account.Bank_Code equals bank.Bank_Code into Bank_Code
                        from b in Bank_Code.DefaultIfEmpty()
                        select new { invitation_account, b };

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Invitation_ID", item.invitation_account.Invitation_ID);
                dic.Add("Sort", item.invitation_account.Sort);
                dic.Add("Category", item.invitation_account.Category);
                dic.Add("Send_Target_Code", item.invitation_account.Send_Target_Code);
                dic.Add("Send_Name", item.invitation_account.Send_Name != null ? item.invitation_account.Send_Name : "");
                dic.Add("Bank_Code", item.invitation_account.Bank_Code);
                dic.Add("Bank_Name", item.b != null ? item.b.Bank_Name : "");
                dic.Add("Account_Number", item.invitation_account.Account_Number != null ? item.invitation_account.Account_Number : "");
                dic.Add("Account_Holder", item.invitation_account.Account_Holder != null ? item.invitation_account.Account_Holder : "");
                result.Add(dic);
            }

            return result;
        }


        public List<TB_Bank> TB_Bank_List()
        {
            var query = from bank in Entity_db.Set<TB_Bank>().Where(x => x.Use_YN == "Y").ToList()
                        orderby bank.Sort
                        select new { bank };

            var list = new List<TB_Bank>();

            foreach (var item in query)
            {
                list.Add(new TB_Bank()
                {
                    Bank_Code = item.bank.Bank_Code,
                    Bank_Name = item.bank.Bank_Name

                });
            }

            return list;
        }

        public List<TB_Common_Code> TB_Account_List(string Product_Category_Code = "PCC01")
        {
            List<string> extra_code = new List<string>() { "0" };
            if (Product_Category_Code == "PCC01" || Product_Category_Code == "PCC02")
            {
                extra_code.Add("1");
                extra_code.Add("2");
            }
            else if (Product_Category_Code == "PCC03")
            {
                extra_code.Add("3");
            }

            var query = from account in Entity_db.Set<TB_Common_Code>().Where(x => x.Code_Group == "Account_Type_Code" && extra_code.Contains(x.Extra_Code)).ToList()
                        orderby account.Sort
                        select new { account };

            var list = new List<TB_Common_Code>();

            foreach (var item in query)
            {
                list.Add(new TB_Common_Code()
                {
                    Code = item.account.Code,
                    Code_Name = item.account.Code_Name

                });
            }

            return list;
        }

        public List<TB_Common_Code> TB_Account_List_By_Extra_Code(string extra_code)
        {
            var query = from account in Entity_db.Set<TB_Common_Code>().Where(x => x.Code_Group == "Account_Type_Code" && (x.Extra_Code == extra_code || x.Extra_Code == "0")).ToList()
                        orderby account.Sort
                        select new { account };

            var list = new List<TB_Common_Code>();

            foreach (var item in query)
            {
                list.Add(new TB_Common_Code()
                {
                    Code = item.account.Code,
                    Code_Name = item.account.Code_Name

                });
            }

            return list;
        }


        public List<TB_Product> TB_Product_By_Invitation_ID(int Invitation_ID)
        {
            var query = from product in Entity_db.Set<TB_Product>()
                        join order_product in Entity_db.Set<TB_Order_Product>()
                        on product.Product_ID equals order_product.Product_ID
                        join invitation in Entity_db.Set<TB_Invitation>().Where(x=> x.Invitation_ID == Invitation_ID)
                        on order_product.Order_ID equals invitation.Order_ID
                        select new { product};

            var list = new List<TB_Product>();

            foreach (var item in query)
            {
                list.Add(new TB_Product()
                {
                    Product_ID = item.product.Product_ID,
                    Product_Category_Code = item.product.Product_Category_Code,
                    Product_Name = item.product.Product_Name,
                    Product_Code = item.product.Product_Code,
                    Original_Product_Code = item.product.Original_Product_Code
                });
            }




            return list;
        }

        public void SP_T_TAX_Save_Sql(int Invitation_ID)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

        
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_TAX";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter INVITATION_ID = new SqlParameter("@INVITATION_ID", SqlDbType.Int);
                INVITATION_ID.Value = Invitation_ID;
                cmd.Parameters.Add(INVITATION_ID);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        public int TB_Invitation_Detail_Update_For_Thanks_Sql(TB_Invitation_Detail invitation_detail)
        {
            var entity = Entity_db.Set<TB_Invitation_Detail>().Where(x => x.Invitation_ID == invitation_detail.Invitation_ID).FirstOrDefault();

            entity.Invitation_Title = invitation_detail.Invitation_Title;
            entity.Invitation_URL = invitation_detail.Invitation_URL;
            entity.Greetings = invitation_detail.Greetings;
            entity.Sender = invitation_detail.Sender;

            entity.Groom_Name = invitation_detail.Groom_Name;
            entity.Groom_Phone = invitation_detail.Groom_Phone;
            entity.Bride_Name = invitation_detail.Bride_Name;
            entity.Bride_Phone = invitation_detail.Bride_Phone;
            entity.Groom_Global_Phone_YN = invitation_detail.Groom_Global_Phone_YN;
            entity.Groom_Global_Phone_Number = invitation_detail.Groom_Global_Phone_Number;
            entity.Bride_Global_Phone_YN = invitation_detail.Bride_Global_Phone_YN;
            entity.Bride_Global_Phone_Number = invitation_detail.Bride_Global_Phone_Number;


            entity.Update_User_ID = invitation_detail.Update_User_ID;
            entity.Update_DateTime = invitation_detail.Update_DateTime;
            entity.Update_IP = invitation_detail.Update_IP;

            Entity_db.TB_Invitation_Details.Update(entity);
            Entity_db.SaveChanges();

            if (!string.IsNullOrEmpty(entity.Invitation_URL))
                ChangeInvitationUrl(entity.Invitation_URL);


            return invitation_detail.Invitation_ID;
        }


        public TB_Order StepOrder_Sql(int Order_Id, string Product_Category_Code)
        {
            var query = from order in Entity_db.TB_Orders
                        join order_product in Entity_db.TB_Order_Products on order.Order_ID equals order_product.Order_ID
                        join product in Entity_db.TB_Products on order_product.Product_ID equals product.Product_ID
                        where order.Order_ID == Order_Id 
                        && product.Product_Category_Code == Product_Category_Code
                        && product.Display_YN == "Y"
                        select new { order.Order_ID, order.Name, order.CellPhone_Number, order.Order_Code, order.Email, order.User_ID };

            var item = query.FirstOrDefault();

            if (item != null)
                return new TB_Order()
                {
                    Order_ID = item.Order_ID,
                    Name = item.Name,
                    CellPhone_Number = item.CellPhone_Number,
                    Order_Code = item.Order_Code,
                    Email = item.Email,
                    User_ID = item.User_ID
                };
            else
                return null;
        }
        
        public T Get_TB_Invitation_Detail_ExtendData<T>(int Invitation_ID)
        {
            var result = default(T);
            var query = from invitation_datail in Entity_db.TB_Invitation_Details
                        where invitation_datail.Invitation_ID == Invitation_ID
                        select invitation_datail.ExtendData;
            var item = query.FirstOrDefault();
            if (!string.IsNullOrEmpty(item))
            {
                result = JsonConvert.DeserializeObject<T>(item);
            }

            return result;
        }
        public void Set_TB_Invitation_Detail_ExtendData<T>(int Invitation_ID, T extendData)
        {
            var query = from invitation_datail in Entity_db.TB_Invitation_Details
                        where invitation_datail.Invitation_ID == Invitation_ID
                        select invitation_datail;
            var item = query.FirstOrDefault();
            if (item != null)
            {
                item.ExtendData = JsonConvert.SerializeObject(extendData);
                Entity_db.SaveChanges();
            }
        }

        private void ChangeInvitationUrl(string url)
        {
            var item = new TB_Kakao_Cache
            {
                Cache_URL = "https://www.barunsonmcard.com/m/" + url,
                Progress_YN = "N",
                Regist_DateTime = DateTime.Now
            };
            Entity_db.TB_Kakao_Cache.Add(item);
            Entity_db.SaveChanges();
        }
        #endregion

    }

}
