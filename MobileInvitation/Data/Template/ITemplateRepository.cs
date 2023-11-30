using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Template
{
    public interface ITemplateRepository
    {

        #region 템플릿 등록 관련 SELECT

        public List<TB_Template> TB_Template_Entity(int Template_ID);

        public List<TB_Template> TB_Template_By_Invitation_ID(int Invitation_ID);
        public List<TB_ReservationWord> TB_Template_Detail_Default_Info_New();

        public List<TB_Template_Detail> TB_Template_Detail_Entity(int Template_ID);

        public List<TB_ReservationWord> TB_ReservationWord_Entity();

        public List<Template_Item_Resource> TB_Template_Item_Resource_LIst(int Template_ID);

        public List<TB_Area> TB_Area_Entity(bool isWeddCard);

        public List<Dictionary<string, object>> TB_Template_Area_LIst(int Template_ID, bool isWeddCard);
        public List<Dictionary<string, object>> TB_Template_Area_LIst(int Template_ID, string Product_Category_Code);
        public List<TB_Template_Area> TB_Template_Area_Entity(int Template_ID);

        public List<Dictionary<string, object>> TB_Template_Used_Image_LIst(int Template_ID, string Product_Code);

        public List<Dictionary<string, object>> TB_Template_LIst(string Product_Category_Code, string Photo_YN);

        public List<TB_Template_Item> TB_Template_Item_LIst(int Template_ID);

        public List<TB_Item_Resource> TB_Item_Resource_LIst(int Resource_ID);

        #endregion

        #region 상품 등록 관련 INSERT / UPDATE\
        public int TB_Template_Update_Sql(TB_Template template);

        public int TB_Template_Insert_Sql(TB_Template template);

        public int TB_Template_Detail_Update_Sql(TB_Template_Detail template_detail);

        public int TB_Template_Detail_Insert_Sql(TB_Template_Detail template_detail);

        public int TB_Template_Area_Update_Sql(TB_Template_Area template_area);

        public int TB_Template_Area_Insert_Sql(TB_Template_Area template_area);

        public int TB_Item_Resource_Update_Sql(TB_Item_Resource item_resource);

        public int TB_Item_Resource_Insert_Sql(TB_Item_Resource item_resource);

        public int TB_Template_Item_Update_Sql(TB_Template_Item template_item);

        public int TB_Template_Item_Insert_Sql(TB_Template_Item template_item);

        public bool TB_Template_Item_Resource_Delete_Sql(List<Template_Item_Resource> template_item_resources, int Template_ID);

        public bool Main_Image_URL_Update_Sql(int TemplateID, string Main_Image_URL);


        #endregion
    }
}
