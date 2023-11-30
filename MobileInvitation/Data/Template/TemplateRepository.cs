using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using X.PagedList;

namespace MobileInvitation.Data.Template
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly barunsonContext Entity_db;


        public TemplateRepository(barunsonContext _Entity_db)
        {
            Entity_db = _Entity_db;
        }

        #region 템플릿 등록 관련 SELECT

        /// <summary>
        /// 템플릿 신규 등록 시 TB_Template 테이블 에서 기본정보 가져오기
        /// </summary>
        /// <param name="Template_ID"></param>
        /// <returns></returns>
        public List<TB_Template> TB_Template_Entity(int Template_ID)
        {

            var query = from template in Entity_db.Set<TB_Template>().Where(x => x.Template_ID == Template_ID)

                        select new { template };

            var list = new List<TB_Template>();

            foreach (var item in query)
            {
                list.Add(new TB_Template()
                {
                    Template_ID = item.template.Template_ID,
                    Template_Name = item.template.Template_Name,
                    Preview_URL = item.template.Preview_URL,
                    Background_Color = item.template.Background_Color,
                    Photo_YN = item.template.Photo_YN,
                    Attached_File1_URL = item.template.Attached_File1_URL,
                    Attached_File2_URL = item.template.Attached_File2_URL
                });
            }

            return list;
        }

        public List<TB_Template> TB_Template_By_Invitation_ID(int Invitation_ID)
        {


            var query = from template in Entity_db.Set<TB_Template>()
                        join invitation in Entity_db.Set<TB_Invitation>().Where(x => x.Invitation_ID == Invitation_ID)
                        on template.Template_ID equals invitation.Template_ID
                        select new
                        {
                            template
                        };

            var list = new List<TB_Template>();

            foreach (var item in query)
            {
                list.Add(new TB_Template()
                {
                    Template_ID = item.template.Template_ID,
                    Template_Name = item.template.Template_Name,
                    Preview_URL = item.template.Preview_URL,
                    Background_Color = item.template.Background_Color,
                    Photo_YN = item.template.Photo_YN,
                    Attached_File1_URL = item.template.Attached_File1_URL,
                    Attached_File2_URL = item.template.Attached_File2_URL
                });
            }

            return list;
        }

        /// <summary>
        ///  템플릿 신규 등록 시 TB_ReservationWord 테이블의 DefaultValue에서 템플릿의 기본정보 가져오기
        /// </summary>
        /// <returns></returns>
        public List<TB_ReservationWord> TB_Template_Detail_Default_Info_New()
        {
            var query = from reservationword in Entity_db.Set<TB_ReservationWord>().ToList()
                        select new { reservationword };

            var list = new List<TB_ReservationWord>();

            foreach (var item in query)
            {
                list.Add(new TB_ReservationWord()
                {
                    ReserveWord = item.reservationword.ReserveWord,
                    MappingField = item.reservationword.MappingField,
                    DefaultValue = item.reservationword.DefaultValue,
                    Product_Category_Codes = item.reservationword.Product_Category_Codes
                });
            }

            return list;
        }

        /// <summary>
        /// 템플릿 수정 시TB_Template_Detail 테이블에서 템플릿의 기본정보 가져오기
        /// </summary>
        /// <param name="Template_ID"></param>
        /// <returns></returns>
        public List<TB_Template_Detail> TB_Template_Detail_Entity(int Template_ID)
        {
            var query = from template_detail in Entity_db.Set<TB_Template_Detail>().Where(x => x.Template_ID == Template_ID).ToList()
                        select new { template_detail };

            var list = new List<TB_Template_Detail>();

            foreach (var item in query)
            {
                list.Add(new TB_Template_Detail()
                {
                    Template_ID = item.template_detail.Template_ID,
                    Greetings = item.template_detail.Greetings,
                    Groom_Name = item.template_detail.Groom_Name,
                    Groom_Phone = item.template_detail.Groom_Phone,
                    Bride_Name = item.template_detail.Bride_Name,
                    Bride_Phone = item.template_detail.Bride_Phone,
                    Groom_Parents1_Name = item.template_detail.Groom_Parents1_Name,
                    Groom_Parents1_Phone = item.template_detail.Groom_Parents1_Phone,
                    Groom_Parents2_Name = item.template_detail.Groom_Parents2_Name,
                    Groom_Parents2_Phone = item.template_detail.Groom_Parents2_Phone,
                    Bride_Parents1_Name = item.template_detail.Bride_Parents1_Name,
                    Bride_Parents1_Phone = item.template_detail.Bride_Parents1_Phone,
                    Bride_Parents2_Name = item.template_detail.Bride_Parents2_Name,
                    Bride_Parents2_Phone = item.template_detail.Bride_Parents2_Phone,
                    WeddingDate = item.template_detail.WeddingDate,
                    WeddingHHmm = item.template_detail.WeddingHHmm,
                    Time_Type_Code = item.template_detail.Time_Type_Code,
                    WeddingYY = item.template_detail.WeddingYY,
                    WeddingMM = item.template_detail.WeddingMM,
                    WeddingDD = item.template_detail.WeddingDD,
                    WeddingWeek = item.template_detail.WeddingWeek,
                    WeddingHour = item.template_detail.WeddingHour,
                    WeddingMin = item.template_detail.WeddingMin,
                    Weddinghall_Name = item.template_detail.Weddinghall_Name,
                    WeddingHallDetail = item.template_detail.WeddingHallDetail,
                    Weddinghall_Address = item.template_detail.Weddinghall_Address,
                    Weddinghall_PhoneNumber = item.template_detail.Weddinghall_PhoneNumber,
                    Etc_Bus_Information = item.template_detail.Etc_Bus_Information,
                    Etc_Car_Information = item.template_detail.Etc_Car_Information,
                    WeddingWeek_Eng_YN = item.template_detail.WeddingWeek_Eng_YN,
                    Time_Type_Eng_YN = item.template_detail.Time_Type_Eng_YN,
                    Baby_Birthday = item.template_detail.Baby_Birthday,
                    Baby_Name = item.template_detail.Baby_Name,
                    RepeatData = item.template_detail.RepeatData
                });
            }

            return list;
        }

        /// <summary>
        /// 매칭정보 추가 드롭다운리스트 바인딩
        /// </summary>
        /// <returns></returns>
        public List<TB_ReservationWord> TB_ReservationWord_Entity()
        {
            var query = from reservationword in Entity_db.Set<TB_ReservationWord>().Where(x => x.Mapping_YN == "Y").ToList()
                        select new { reservationword };

            var list = new List<TB_ReservationWord>();

            foreach (var item in query)
            {
                list.Add(new TB_ReservationWord()
                {
                    ReserveWord = "#" + item.reservationword.ReserveWord + "#",
                    MappingField = item.reservationword.MappingField,
                    DefaultValue = item.reservationword.DefaultValue,
                    Product_Category_Codes = item.reservationword.Product_Category_Codes
                });
            }

            return list;
        }


        /// <summary>
        /// 템플릿 수정 시 TB_Template_Item ,TB_Item_Resource  테이블에서 이미 저장한 템플릿의 오브젝트 정보 가져오기
        /// </summary>
        /// <param name="Template_ID"></param>
        /// <returns></returns>
        public List<Template_Item_Resource> TB_Template_Item_Resource_LIst(int Template_ID)
        {
            var query = from template_item in Entity_db.Set<TB_Template_Item>().Where(x => x.Template_ID == Template_ID)
                        join item_resource in Entity_db.Set<TB_Item_Resource>()
                        on template_item.Resource_ID equals item_resource.Resource_ID
                        orderby item_resource.Sort
                        select new
                        {
                            template_item,
                            item_resource
                        };

            var list = new List<Template_Item_Resource>();

            foreach (var item in query)
            {
                list.Add(new Template_Item_Resource()
                {
                    item_id = item.template_item.Item_ID,
                    resource_id = item.item_resource.Resource_ID,
                    pid = "area" + item.template_item.Area_ID,
                    id = "item_" + item.item_resource.Sort,
                    type = CodeNameHelper.ItemTypeCodeToResourceTypeCode(item.template_item.Item_Type_Code),
                    top = item.template_item.Location_Top,
                    left = item.template_item.Location_Left,
                    height = item.template_item.Size_Height,
                    width = item.template_item.Size_Width,
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

        /// <summary>
        /// 신규등록 시 TB_Area_Entity에서 영역정보 가져오기
        /// </summary>
        /// <param name="isWeddCard"></param>
        /// <returns></returns>
        public List<TB_Area> TB_Area_Entity(bool isWeddCard)
        {
            var query = from area in Entity_db.Set<TB_Area>().ToList()
                        select new { area };

            if (isWeddCard)
            {
                query = query.Where(x => x.area.WeddingCard_YN == "Y");
            }
            else
            {
                query = query.Where(x => x.area.ThanksCard_YN == "Y");
            }


            var list = new List<TB_Area>();

            foreach (var item in query)
            {
                list.Add(new TB_Area()
                {
                    Area_ID = item.area.Area_ID,
                    Area_Name = item.area.Area_Name,
                    Edit_YN = item.area.Edit_YN
                });
            }

            return list;
        }

        /// <summary>
        /// 수정 시 TB_Template_Area에서 템플릿 영역정보 가져오기
        /// </summary>
        /// <param name="Template_ID"></param>
        /// <param name="isWeddCard"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> TB_Template_Area_LIst(int Template_ID, bool isWeddCard)
        {
            var query = from area in Entity_db.Set<TB_Area>()
                        join template_area in Entity_db.Set<TB_Template_Area>().Where(x => x.Template_ID == Template_ID) on area.Area_ID equals template_area.Area_ID into Area_ID
                        from t in Area_ID.DefaultIfEmpty()
                        select new { area, t };


            if (isWeddCard)
            {
                query = query.Where(x => x.area.WeddingCard_YN == "Y");
            }
            else
            {
                query = query.Where(x => x.area.ThanksCard_YN == "Y");
            }

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Template_ID", item.t == null ? 0 : item.t.Template_ID);
                dic.Add("Area_ID", item.area.Area_ID);
                dic.Add("Area_Name", item.area.Area_Name);
                dic.Add("Edit_YN", item.area.Edit_YN);
                dic.Add("Size_Height", item.t == null ? 100 : item.t.Size_Height);
                dic.Add("Size_Width", item.t == null ? 800 : item.t.Size_Width);
                if (item.area.Area_ID == 14)
                {
                    if (item.t != null)
                    {
                        dic.Add("Color", item.t.Color == null ? "#F7F8F9" : item.t.Color);
                    }
                    else
                    {
                        dic.Add("Color", item.t == null ? "" : item.t.Color);
                    }
                }
                else
                {
                    dic.Add("Color", item.t == null ? "" : item.t.Color);
                }
                dic.Add("Sort", item.t == null ? 0 : item.t.Sort);
                dic.Add("Product_Category_Codes", item.area.Product_Category_Codes);
                result.Add(dic);
            }
            return result;
        }
        public List<Dictionary<string, object>> TB_Template_Area_LIst(int Template_ID, string Product_Category_Code)
        {
            var query = from area in Entity_db.Set<TB_Area>()
                        join template_area in Entity_db.Set<TB_Template_Area>().Where(x => x.Template_ID == Template_ID) on area.Area_ID equals template_area.Area_ID into Area_ID
                        from t in Area_ID.DefaultIfEmpty()
                        where area.Product_Category_Codes.Contains(Product_Category_Code)
                        select new { area, t };


            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Template_ID", item.t == null ? 0 : item.t.Template_ID);
                dic.Add("Area_ID", item.area.Area_ID);
                dic.Add("Area_Name", item.area.Area_Name);
                dic.Add("Edit_YN", item.area.Edit_YN);
                dic.Add("Size_Height", item.t == null ? 100 : item.t.Size_Height);
                dic.Add("Size_Width", item.t == null ? 800 : item.t.Size_Width);
                if (item.area.Area_ID == 14)
                {
                    if (item.t != null)
                    {
                        dic.Add("Color", item.t.Color == null ? "#F7F8F9" : item.t.Color);
                    }
                    else
                    {
                        dic.Add("Color", item.t == null ? "" : item.t.Color);
                    }
                }
                else
                {
                    dic.Add("Color", item.t == null ? "" : item.t.Color);
                }
                dic.Add("Sort", item.t == null ? 0 : item.t.Sort);
                dic.Add("Product_Category_Codes", item.area.Product_Category_Codes);
                result.Add(dic);
            }
            return result;
        }
        public List<TB_Template_Area> TB_Template_Area_Entity(int Template_ID)
        {
            var query = from template_area in Entity_db.Set<TB_Template_Area>().Where(x => x.Template_ID == Template_ID)
                        select new { template_area };


            var list = new List<TB_Template_Area>();

            foreach (var item in query)
            {
                list.Add(new TB_Template_Area()
                {
                    Template_ID = item.template_area.Template_ID,
                    Area_ID = item.template_area.Area_ID,
                    Size_Height = item.template_area.Size_Height,
                    Size_Width = item.template_area.Size_Width,
                    Color = item.template_area.Color,
                    Sort = item.template_area.Sort
                });
            }
            return list;
        }

        public List<Dictionary<string, object>> TB_Template_Used_Image_LIst(int Template_ID, string Product_Code)
        {
            var query = from template in Entity_db.Set<TB_Template>().Where(x => x.Template_ID == Template_ID)
                        join template_item in Entity_db.Set<TB_Template_Item>().Where(x => x.Item_Type_Code == "ITC02" || x.Item_Type_Code == "ITC03" || x.Item_Type_Code == "ITC04") on template.Template_ID equals template_item.Template_ID
                        join item_resource in Entity_db.Set<TB_Item_Resource>() on template_item.Resource_ID equals item_resource.Resource_ID
                        select new { template, template_item, item_resource };

            var entity = Entity_db.Set<TB_Product>().Where(x => x.Original_Product_Code == Product_Code).FirstOrDefault();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Resource_URL", item.item_resource.Resource_URL);

                result.Add(dic);
            }

            //상품등록 이전
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Main_Image_URL))
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Resource_URL", entity.Main_Image_URL);
                    result.Add(dic);
                }
            }

            return result;
        }

        public List<Dictionary<string, object>> TB_Template_LIst(string Product_Category_Code, string Photo_YN)
        {
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_Category_Code == Product_Category_Code && x.Product_ID > 40)
                        join template in Entity_db.Set<TB_Template>().Where(x => x.Photo_YN == Photo_YN) on product.Template_ID equals template.Template_ID
                        orderby template.Template_ID
                        select new
                        {
                            product,
                            template
                        };

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Template_ID", item.product.Template_ID);
                dic.Add("Product_Code", item.product.Original_Product_Code);
                dic.Add("Product_Category_Code", item.product.Product_Category_Code);
                dic.Add("Main_Image_URL", item.product.Main_Image_URL);

                result.Add(dic);
            }
            return result;
        }

        public List<TB_Template_Item> TB_Template_Item_LIst(int Template_ID)
        {
            var query = from template_item in Entity_db.Set<TB_Template_Item>().Where(x => x.Template_ID == Template_ID)

                        select new { template_item };

            var list = new List<TB_Template_Item>();

            foreach (var item in query)
            {
                list.Add(new TB_Template_Item()
                {
                    Item_ID = item.template_item.Item_ID,
                    Template_ID = item.template_item.Template_ID,
                    Resource_ID = item.template_item.Resource_ID,
                    Area_ID = item.template_item.Area_ID,
                    Item_Type_Code = item.template_item.Item_Type_Code,
                    Location_Top = item.template_item.Location_Top,
                    Location_Left = item.template_item.Location_Left,
                    Size_Height = item.template_item.Size_Height,
                    Size_Width = item.template_item.Size_Width
                });
            }

            return list;
        }

        public List<TB_Item_Resource> TB_Item_Resource_LIst(int Resource_ID)
        {
            var query = from item_resource in Entity_db.Set<TB_Item_Resource>().Where(x => x.Resource_ID == Resource_ID)

                        select new { item_resource };

            var list = new List<TB_Item_Resource>();

            foreach (var item in query)
            {
                list.Add(new TB_Item_Resource()
                {
                    Resource_ID = item.item_resource.Resource_ID,
                    CharacterSet = item.item_resource.CharacterSet,
                    Character_Size = item.item_resource.Character_Size,
                    Color = item.item_resource.Color,
                    Background_Color = item.item_resource.Background_Color,
                    Bold_YN = item.item_resource.Bold_YN,
                    Italic_YN = item.item_resource.Italic_YN,
                    Underline_YN = item.item_resource.Underline_YN,
                    BetweenText = item.item_resource.BetweenText,
                    BetweenLine = item.item_resource.BetweenLine,
                    Vertical_Alignment = item.item_resource.Vertical_Alignment,
                    Horizontal_Alignment = item.item_resource.Horizontal_Alignment,
                    Sort = item.item_resource.Sort,
                    Font = item.item_resource.Font,
                    Resource_URL = item.item_resource.Resource_URL,
                    Resource_Height = item.item_resource.Resource_Height,
                    Resource_Width = item.item_resource.Resource_Width,
                    Resource_Type_Code = item.item_resource.Resource_Type_Code
                });
            }

            return list;
        }


        #endregion

        #region 템플릿 등록 관련 INSERT / UPDATE
        /// <summary>
        /// TB_Template 테이블 수정
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        /// 
        public int TB_Template_Update_Sql(TB_Template template)
        {
            var entity = Entity_db.Set<TB_Template>().Where(x => x.Template_ID == template.Template_ID).FirstOrDefault();

            entity.Template_ID = template.Template_ID;
            entity.Template_Name = template.Template_Name;
            entity.Preview_URL = template.Preview_URL;
            entity.Background_Color = template.Background_Color;
            entity.Attached_File1_URL = template.Attached_File1_URL;
            entity.Attached_File2_URL = template.Attached_File2_URL;
            entity.Update_User_ID = template.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = template.Update_IP;

            Entity_db.TB_Templates.Update(entity);
            Entity_db.SaveChanges();

            return entity.Template_ID;
        }

        /// <summary>
        /// TB_Template_Insert 등록
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public int TB_Template_Insert_Sql(TB_Template template)
        {
            Entity_db.TB_Templates.Add(template);
            Entity_db.SaveChanges();

            return template.Template_ID;
        }

        /// <summary>
        /// TB_Template_Detail 수정
        /// </summary>
        /// <param name="template_detail"></param>
        /// <returns></returns>
        public int TB_Template_Detail_Update_Sql(TB_Template_Detail template_detail)
        {
            var entity = Entity_db.Set<TB_Template_Detail>().Where(x => x.Template_ID == template_detail.Template_ID).FirstOrDefault();

            entity.Template_ID = template_detail.Template_ID;
            entity.Greetings = template_detail.Greetings;
            entity.Groom_Name = template_detail.Groom_Name;
            entity.Groom_Phone = template_detail.Groom_Phone;
            entity.Bride_Name = template_detail.Bride_Name;
            entity.Bride_Phone = template_detail.Bride_Phone;
            entity.Groom_Parents1_Name = template_detail.Groom_Parents1_Name;
            entity.Groom_Parents1_Phone = template_detail.Groom_Parents1_Phone;
            entity.Groom_Parents2_Name = template_detail.Groom_Parents2_Name;
            entity.Groom_Parents2_Phone = template_detail.Groom_Parents2_Phone;
            entity.Bride_Parents1_Name = template_detail.Bride_Parents1_Name;
            entity.Bride_Parents1_Phone = template_detail.Bride_Parents1_Phone;
            entity.Bride_Parents2_Name = template_detail.Bride_Parents2_Name;
            entity.Bride_Parents2_Phone = template_detail.Bride_Parents2_Phone;
            entity.WeddingDate = template_detail.WeddingDate;
            entity.WeddingHHmm = template_detail.WeddingHHmm;
            entity.Time_Type_Code = template_detail.Time_Type_Code;
            entity.WeddingYY = template_detail.WeddingYY;
            entity.WeddingMM = template_detail.WeddingMM;
            entity.WeddingDD = template_detail.WeddingDD;
            entity.WeddingWeek = template_detail.WeddingWeek;
            entity.WeddingHour = template_detail.WeddingHour;
            entity.WeddingMin = template_detail.WeddingMin;
            entity.Weddinghall_Name = template_detail.Weddinghall_Name;
            entity.WeddingHallDetail = template_detail.WeddingHallDetail;
            entity.Weddinghall_Address = template_detail.Weddinghall_Address;
            entity.Weddinghall_PhoneNumber = template_detail.Weddinghall_PhoneNumber;
            entity.Etc_Bus_Information = template_detail.Etc_Bus_Information;
            entity.Etc_Car_Information = template_detail.Etc_Car_Information;
            entity.WeddingWeek_Eng_YN = template_detail.WeddingWeek_Eng_YN;
            entity.Time_Type_Eng_YN = template_detail.Time_Type_Eng_YN;
            entity.Update_User_ID = template_detail.Update_User_ID;
            entity.Update_DateTime = template_detail.Update_DateTime;
            entity.Update_IP = template_detail.Update_IP;

            Entity_db.TB_Template_Details.Update(entity);
            Entity_db.SaveChanges();

            return entity.Template_ID;
        }

        /// <summary>
        /// TB_Template_Detail 등록ㅣ
        /// </summary>
        /// <param name="template_detail"></param>
        /// <returns></returns>
        public int TB_Template_Detail_Insert_Sql(TB_Template_Detail template_detail)
        {
            Entity_db.TB_Template_Details.Add(template_detail);
            Entity_db.SaveChanges();

            return template_detail.Template_ID;
        }

        /// <summary>
        /// TB_Template_Area 수정
        /// </summary>
        /// <param name="template_area"></param>
        /// <returns></returns>
        public int TB_Template_Area_Update_Sql(TB_Template_Area template_area)
        {
            var entity = Entity_db.Set<TB_Template_Area>().Where(x => x.Template_ID == template_area.Template_ID && x.Area_ID == template_area.Area_ID).FirstOrDefault();

            entity.Size_Height = template_area.Size_Height;
            entity.Size_Width = template_area.Size_Width;
            entity.Color = template_area.Color;
            entity.Sort = template_area.Sort;
            entity.Update_User_ID = template_area.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = template_area.Update_IP;

            Entity_db.TB_Template_Areas.Update(entity);
            Entity_db.SaveChanges();

            return entity.Area_ID;
        }

        /// <summary>
        /// TB_Template_Area 등록
        /// </summary>
        /// <param name="template_Area"></param>
        /// <returns></returns>
        public int TB_Template_Area_Insert_Sql(TB_Template_Area template_Area)
        {
            Entity_db.TB_Template_Areas.Add(template_Area);
            Entity_db.SaveChanges();

            return template_Area.Area_ID;
        }

        /// <summary>
        /// TB_Item_Resource 수정
        /// </summary>
        /// <param name="item_resource"></param>
        /// <returns></returns>
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

        /// <summary>
        /// TB_Item_Resource 등록
        /// </summary>
        /// <param name="item_resource"></param>
        /// <returns></returns>
        public int TB_Item_Resource_Insert_Sql(TB_Item_Resource item_resource)
        {
            Entity_db.TB_Item_Resources.Add(item_resource);
            Entity_db.SaveChanges();

            return item_resource.Resource_ID;
        }

        /// <summary>
        /// TB_Template_Item 수정
        /// </summary>
        /// <param name="template_item"></param>
        /// <returns></returns>
        public int TB_Template_Item_Update_Sql(TB_Template_Item template_item)
        {
            var entity = Entity_db.Set<TB_Template_Item>().Where(x => x.Resource_ID == template_item.Resource_ID).FirstOrDefault();

            entity.Area_ID = template_item.Area_ID;
            entity.Item_Type_Code = template_item.Item_Type_Code;
            entity.Location_Top = template_item.Location_Top;
            entity.Location_Left = template_item.Location_Left;
            entity.Size_Height = template_item.Size_Height;
            entity.Size_Width = template_item.Size_Width;

            entity.Update_User_ID = template_item.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = template_item.Update_IP;

            Entity_db.TB_Template_Items.Update(entity);
            Entity_db.SaveChanges();

            return entity.Item_ID;
        }

        /// <summary>
        /// TB_Template_Item 등록
        /// </summary>
        /// <param name="template_item"></param>
        /// <returns></returns>
        public int TB_Template_Item_Insert_Sql(TB_Template_Item template_item)
        {
            Entity_db.TB_Template_Items.Add(template_item);
            Entity_db.SaveChanges();

            return template_item.Item_ID;
        }

        /// <summary>
        /// 수정 시 (오브젝트 삭제 되었을 경우) TB_Template_Item와 Ite_Resource 테이블 삭제
        /// </summary>
        /// <param name="template_item_resources"></param>
        /// <param name="Template_ID"></param>
        /// <returns></returns>
        public bool TB_Template_Item_Resource_Delete_Sql(List<Template_Item_Resource> template_item_resources, int Template_ID)
        {
            try
            {
                var resource_ids = template_item_resources.Select(x => x.resource_id).ToList();

                var query = Entity_db.Set<TB_Template_Item>().Where(x => x.Template_ID == Template_ID && !resource_ids.Contains(x.Resource_ID)).ToList();

                foreach (var item in query)
                {
                    Entity_db.TB_Template_Items.Remove(item);
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

        public bool Main_Image_URL_Update_Sql(int TemplateID, string Main_Image_URL)
        {
            var entity = Entity_db.Set<TB_Product>().Where(x => x.Template_ID == TemplateID).FirstOrDefault();

            var result = false;
            if (entity != null)
            {
                entity.Main_Image_URL = Main_Image_URL;

                Entity_db.TB_Products.Update(entity);
                Entity_db.SaveChanges();

                result = true;
            }
            return result;
        }


        #endregion
    }

}
