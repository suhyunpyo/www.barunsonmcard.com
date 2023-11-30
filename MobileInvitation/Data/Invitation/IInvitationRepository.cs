using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MobileInvitation.Areas.User.Controllers.Invitaion.InvitationController;

namespace MobileInvitation.Data.Invitation
{
    public interface IInvitationRepository
    {

        #region
        public List<TB_Invitation> TB_Invitation_Entity(int Order_ID);


        public int TB_Invitation_Insert_Sql(TB_Invitation invitation);

        public int TB_Invitation_Detail_Insert_Sql(TB_Invitation_Detail invitation_detail);

        public int TB_Invitation_Detail_Update_Sql(TB_Invitation_Detail invitation_detail);

        public int TB_Invitation_Detail_Update_For_Admin_Sql(TB_Invitation_Detail invitation_detail);

        public int TB_Invitation_Area_Update_For_Admin_Sql(TB_Invitation_Area invatation_area);

        public bool TB_Invitation_Item_Resource_Delete_For_Admin_Sql(List<Invitation_Item_Resource> invitation_item_resources, int Invitation_ID);

        public int TB_Invitation_Item_Insert_Sql(TB_Invitation_Item invitation_item);

        public int TB_Invitation_Item_Update_Sql(TB_Invitation_Item invitation_item);

        public int TB_Item_Resource_Insert_Sql(TB_Item_Resource item_resource);

        public int TB_Item_Resource_Update_Sql(TB_Item_Resource item_resource);

        public int TB_Invitation_Area_Insert_Sql(TB_Invitation_Area inivation_area);

        public List<Invitation_Item_Resource> TB_Invitation_Item_Resource_LIst(int Invitation_ID);

        public List<TB_Invitation_Area> TB_Invitation_Area_Entity(int Invitation_ID);

        public List<TB_Invitation_Detail> TB_Invitation_Detail_Entity(int Invitation_ID);

        public List<TB_Gallery> TB_Gallery_List(int Invitation_ID);

        public int TB_Gallery_Insert_Sql(TB_Gallery gallery);

        public TB_Gallery TB_Gallery_Entity(int Gallery_ID);

        public int Gallery_Sort_Extra_Update_Sql(int Invitation_ID, int add_Value, int Start_Value, int End_Value);

        public int Gallery_Sort_Update_Sql(int Gallery_ID, int newSort);

        public int Gallery_Sort_Reset_Update_Sql(int Invitation_ID, int Sort);

        public int TB_Gallery_Delete_Entity(int Gallery_ID);

        public int SNS_Image_Update_Sql(TB_Invitation_Detail invitation_detail);

        public int Delegate_Image_Update_Sql(TB_Invitation_Detail invitation_detail);

        public int Gallery_Image_Update_Sql(TB_Gallery gallery);

        public TB_Invitation_Detail CheckDuplicateURL_Entity(string invitation_url);
        /// <summary>
        /// 초대장 URL 중복 체크
        /// </summary>
        /// <param name="invitation_url"></param>
        /// <returns>true 사용가능, false 사용불가</returns>
        public bool CheckDuplicateURL(string invitation_url);

        public int Video_Gallery_Update_Sql(TB_Invitation_Detail invitation_detail);

        public Invitation_Item_Resource PhotoImage_Entity(int Invitation_Id);

        public int TB_Invitation_Detail_Etc_Delete_Sql(int Invitation_ID);

        public int TB_Account_Extra_Delete_Sql(int Invitation_ID);

        public int TB_Invitation_Account_Delete_Sql(int Invitation_ID);
        public int TB_Invitation_Detail_Etc_Sql(TB_Invitation_Detail_Etc invitation_detail_etc);

        public int TB_Account_Extra_Sql(TB_Account_Extra invitation_account);

        public int TB_Invitation_Account_Sql(TB_Invitation_Account invitation_account);

        public string Get_Send_Name(string Send_Code);
        public List<TB_Invitation_Detail_Etc> TB_Invitation_Detail_Etc_List(int Invitation_ID);

        public List<Dictionary<string, object>> TB_Account_Extra_List(int Invitation_ID);

        public List<Dictionary<string, object>> TB_Invitation_Account_List(int Invitation_ID, int Category);

        public List<TB_Bank> TB_Bank_List();

        public List<TB_Common_Code> TB_Account_List(string Product_Category_Code);

        public List<TB_Common_Code> TB_Account_List_By_Extra_Code(string extra_code);
        public List<TB_Product> TB_Product_By_Invitation_ID(int Invitation_ID);

        public void SP_T_TAX_Save_Sql(int Invitation_ID);

        public int TB_Invitation_Detail_Update_For_Thanks_Sql(TB_Invitation_Detail invitation_detail);

        public TB_Order StepOrder_Sql(int Order_Id, string Product_Category_Code);

        public T Get_TB_Invitation_Detail_ExtendData<T>(int Invitation_ID);
        public void Set_TB_Invitation_Detail_ExtendData<T>(int Invitation_ID, T extendData);
        #endregion
    }
}
