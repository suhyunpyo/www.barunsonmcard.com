using Microsoft.AspNetCore.Http;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Operation
{
    public interface IOperationRepository
    {

        /// <summary>
        /// 마이페이지 - 1대1문의 저장 
        /// </summary>
        /// <param name="model"></param>
        void User_Qna_Save_Sql(VW_User_QNA model, string Gubun);

        UserQNAVIewModel User_Qna_Detail_Entity(int Id);
        
        string User_Qna_FileUpladName_Entity(int Id, int num);

        List<TB_Common_Menu> Admin_Menu_List_Entity(/*string Menu_Type*/);
        
        List<TB_Board> User_Notice_List_Entity(string Top_YN);

        /// <summary>
        /// 프런트 - 대분류카테고리명
        /// </summary>
        /// <param name="Category_Type">1: 메인 / 2: 카테고리</param>
        /// <returns></returns>
        List<Dictionary<string, object>> User_Menu_List_Entity(string Category_Type, int Parent_Category_Id);

        string User_Notice_Faq_Click_Update_Entity(int Id, string Board_Category);

        string User_Banner_Click_Update_Entity(int Banner_Item_Id);
        List<Dictionary<string, object>> Admin_Banner_Add_List_Entity2(string Search_Banner_Category_Name, HttpRequest request);

        List<VW_User_QNA> User_Qna_List_Entity(string User_Id);

        List<TB_Board> User_Faq_List_Entity(string SearchTxt);

        /// <summary>
        /// 프런트 - 메인 팝업 노출 
        /// </summary>
        /// <param name="Popup_Title"></param>
        /// <param name="Popup_Type">PTC01 : PC / PTC02 : 모바일</param>
        /// <returns></returns>
        /// 
        List<Dictionary<string, object>> User_Popup_List_Entity(string Popup_Title, string Popup_Type);


        List<TB_Board> Admin_Notice_View_Entity(int id);        

        string User_Wish_Save_Entity(TB_Wish_List list, string Gubun);

        int User_Wish_Total_Cnt_Entity(string User_Id);
        List<TB_PolicyInfo> PolicyInfo_History_List_Entity(string policyDiv);
        TB_PolicyInfo PolicyInfo_View_Entity(string policyDiv, int seq = 0);       

        void Error_Save_Sql(String Error_Content, string Method_Name, string User_Id, string User_Name);

    }
}
