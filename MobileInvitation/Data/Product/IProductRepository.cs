using MobileInvitation.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Product
{
    public interface IProductRepository
    {
        #region 공통 SELECT

        #region ************코드리스트********************


        /// <summary>
        /// 공통코드리스트 - Entity Framework
        /// </summary>
        /// <returns></returns>
        List<TB_Common_Code> Common_CodeList_Entity(string Code_Gubun);


        /// <summary>
        /// 공통코드리스트 - Dapper
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        List<TB_Common_Code> Common_CodeList_Dapper(string Code_Gubun);



        /// <summary>
        /// 공통코드리스트 - Sql
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        List<TB_Common_Code> Common_CodeList_Sql(string Code_Gubun);


        #endregion 공통 End



        #region ************대/소분류 리스트************

        /// <summary>
        ///  대/소분류 리스트 추출 - Sql
        /// </summary>
        /// <param name="Gubun"></param>1 : 대분류 / 2 : 소분류 
        /// <param name="Parent_CategoryId"></param> 상위 대분류 카테고리번호 
        /// <param name="DetailViewYn"></param>카테고리 상세 리스트 디스플레이 Y/N
        /// <param name="CategoryId"></param>선택한 카테고리의 번호 
        /// <returns></returns>
        List<TB_Category> CateGoryList_Sql(int Gubun, int? Parent_CategoryId, string DetailViewYn, int? CategoryId);



        /// <summary>
        /// 대/소분류 리스트 추출 - Dapper
        /// </summary>
        /// <param name="Gubun"></param>1 : 대분류 / 2 : 소분류 
        /// <param name="Parent_CategoryId"></param> 상위 대분류 카테고리번호 
        /// <param name="DetailViewYn"></param>카테고리 상세 리스트 디스플레이 Y/N
        /// <param name="CategoryId"></param>선택한 카테고리의 번호 
        /// <returns></returns>
        List<TB_Category> CateGoryList_Dapper(int Gubun, int Parent_CategoryId, string DetailViewYn, int? CategoryId);

        #endregion



        #endregion



        #region 상품 관련 INSERT / UPDATE

        /// <summary>
        ///  분류관리 - 메인/카테고리 분류 -> 저장 (Entity Framework)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Category_Save_Entity(TB_Category model);

        string Category_Save_Sql_Test(string SaveGubun, TB_Category model, List<Dictionary<string, object>> Depth2_List2, string Category_id_And_Sort, string User_Id, string Uset_Ip);

        string Product_Categoty_Save_Sql(string Gubun, string Display_Yn, string Product_Id_And_Category_Id, string UserID, string UserIP/* string Product_Id, int Category_Id*/);

        /// <summary>
        ///  분류관리 - 메인/카테고리 분류 -> 저장 (Sql)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Category_Nm1"></param>
        /// <param name="Category_Nm2"></param>
        /// <returns></returns>
        string Category_Save_Sql(string SaveGubun, TB_Category model, JObject Depth2, TB_Category model2, string Category_id_And_Sort);




        /// <summary>
        ///  분류관리 - 메인/카테고리 분류 -> 삭제 (Sql)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Type_Code"></param> //메인 CTC01 / 카테고리 CTC02
        /// <param name="Category_Id"></param>// 카테고리 ID
        /// <returns></returns>
        string Category_Del_Sql(string Type_Code, int Category_Id);





        #endregion



        #region 상품 관련 SELECT

        /// <summary>
        /// 진열관리 - 메인/카테고리 상품추가 화면에 뿌려질 상품 검색 리스트 
        /// </summary>
        /// <param name="Category_Type_Code">CTC01 - 메인 진열 / CTC02 - 카테고리 진열</param>
        /// <param name="Product_Category_Code">청첩장, 감사장 등등..</param>
        /// <param name="Product_Brand_Code">바른손, 더카드 등등..</param>
        /// <param name="Category_Id"></param>
        /// <param name="Searchtxt"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> Display_AddProductList_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int Category_Id, string Searchtxt);

        /// <summary>
        ///  분류관리 - 메인/카테고리 분류 -> 중분류 리스트 (Sql)
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        List<TB_Category> Product_CategoryList_Sql(string Category_Type_Code);



        /// <summary>
        /// 진열관리 - 메인/카테고리 진열 검색
        /// </summary>
        /// <param name="SearchKind"></param>
        /// <param name="SearchBrand"></param>
        /// <param name="SearchViewYn"></param>
        /// <param name="Searchtxt"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> Display_ProductList_Entity(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn);


        List<Dictionary<string, object>> Display_ProductList_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn, string User_ID);

        /// <summary>
        /// 상품/템플릿 전체 리스트 및 검색
        /// </summary>
        /// <param name="SearchKind">분류 - 전체, M청첩장, M감사장</param>
        /// <param name="SearchBrand">브랜드 - 전체 , 바른손, 비핸즈, 더카드, 프리미어페이퍼</param>
        /// <param name="SearchViewYn">진열 - 전체, 진열, 미진열</param>
        /// <returns></returns>
        List<Dictionary<string, object>> ProductList_Entity(string SearchKind, string SearchBrand, string SearchViewYn, string Searchtxt);

        #endregion

        List<Dictionary<string, object>> User_Product_Icon_List_Entity();
        List<TB_Category> CateGory_Depth2Info_Sql(int? Parent_CategoryId);
        //Dictionary<string, string> CateGory_Depth2Info_Depper(int? Parent_CategoryId);

        List<TB_Product_Image> User_Product_Detail_Img(int Product_ID /*, string Image_Type_Code*/);
        string Categoty_Update_Sql(string Gubun, string Product_Id_And_Category_Id, int Category_Id, string UserID, string UserIP);


        #region 상품 등록 관련 SELECT

        List<TB_Category> TB_CategoryList_Entity(string Category_Type_Code, int Category_Step);
        List<TB_Category> TB_Sub_CategoryList_Entity(string Category_Type_Code, int Category_Step, int Parent_Category_ID);

        List<Dictionary<string, object>> TB_IconList_Entity(int Product_Id);

        string IconList_Save_Entity(TB_Icon model);

        string TB_Product_Image_Save_Entity(TB_Product_Image model);

        string IconList_Delete_Entity(TB_Icon model);

        string Product_Code_New_Entity(string Product_Brand_Code);

        #endregion

        #region 상품 등록(상세) 관련 SELECT

        List<TB_Product> TB_Product_Entity(int Product_ID);
        List<Dictionary<string, object>> TB_Product_Template_Entity(int Product_ID);
        List<Dictionary<string, object>> TB_Product_Category_Entity(int Product_ID);

        List<Dictionary<string, object>> TB_Main_Product_Category_Entity(int Product_ID);

        public List<TB_Product_Image> TB_Product_Image_List_Entity(int Product_ID);

        public List<Dictionary<string, object>> TB_Product_Used_Image_LIst(int Product_ID);

        #endregion

        #region 상품 등록 관련 INSERT / UPDATE

        public int TB_Product_Update_Sql(TB_Product product);

        public int Main_Image_Update_Sql(TB_Product product);

        public int TB_Product_Insert_Sql(TB_Product product);

        public string TB_Product_Category_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Category> product_categories);
        public string TB_Product_Icon_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Icon> product_icons);

        public string TB_Product_Image_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Image> product_Images);

        #endregion

        string Admin_Product_Category_Sort_Update_Entity(List<TB_Product_Category> model, string UserIP);

        List<Dictionary<string, object>> Display_ProductList_Search_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn);
        /// <summary>
        /// 프런트 - 상품 상세 
        /// </summary>
        /// <param name="Product_Id"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> User_Product_Detail_Entity(int Product_Id, string User_ID);

        // string User_Product_Detail_Img(int Product_ID, string Image_Type_Code);
        /// <summary>
        /// 프런트 - 상품 카테고리 리스트 
        /// </summary>
        /// <param name="User_Id"></param>
        /// <param name="Category_Id"></param>
        /// <param name="Search_Gubun"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> User_Product_List_Sql(string User_Id, int Category_Id, int Search_Gubun, string SearchCategoryList, string SearchBrandList);

        /// <summary>
        /// 프런트 - 상품리스트 -> 클릭한 상품의 기본 정보 
        /// </summary>
        /// <param name="Product_Id"></param>
        /// <returns></returns>
        List<Dictionary<string, object>> User_Product_PreViewImg_Entity(int Product_Id);

        /// <summary>
        /// 프런트 - 상품 검색
        /// </summary>
        /// <param name="SearchKeyword">상품코드 OR 상품명</param>
        /// <returns></returns>
        List<Dictionary<string, object>> User_Search_ProductList_Sql(string User_Id, string SearchKeyword, int Search_Gubun);

        string Get_Product_PreView_Url(int Product_ID);


        int Product_Detail_Total_Wish_Cnt(int Product_ID);


        List<Dictionary<string, object>> Product_MainImage_List_Entity();


        int User_Order_Chk_Entity(string gubun, TB_Order model);


        int Product_Detail_Member_Wish_Cnt(int Product_ID, string User_ID);

    }
}
