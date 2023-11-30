using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using X.PagedList;

namespace MobileInvitation.Data.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly barunsonContext Entity_db;
        //BarunnSonContext context;

        public ProductRepository(barunsonContext ctx)
        {
            this.Entity_db = ctx;



        }

        #region 공통 SELECT

        #region ************코드리스트********************


        /// <summary>
        /// 공통코드리스트 - Entity Framework
        /// </summary>
        /// <returns></returns>
        public List<TB_Common_Code> Common_CodeList_Entity(string Code_Gubun)
        {
            //  TB_Common_Code aa = new TB_Common_Code;

            //  Entity_db.Update(Entity_db);
            // Entity_db.Entry(entity).Reload()
            //  Entity_db.Entry("rwer").Reload();
            //TB_Product_Category aa = new TB_Product_Category();
            //Entity_db.Entry(aa).GetDatabaseValues();
            //var e = new TB_Common_Code();
            //Entity_db.Update(e);


            var query = from common_code in Entity_db.Set<TB_Common_Code>().Where(x => x.Code_Group == Code_Gubun)
                        join common_code_group in Entity_db.Set<TB_Common_Code_Group>()
                            on common_code.Code_Group equals common_code_group.Code_Group
                        select new { common_code };

            if (Code_Gubun.Equals("Refund_Status_Code")) // 환불철회는 제외
            {
                query = query.Where(x => x.common_code.Code != "RSC03");
            }

            var list = new List<TB_Common_Code>();
            foreach (var item in query)
            {

                list.Add(new TB_Common_Code()
                {
                    Code = item.common_code.Code,
                    Code_Name = item.common_code.Code_Name,
                    //Code_Name = Code_Gubun.Equals("Refund_Status_Code") && item.common_code.Code_Name.Equals("환불신청") ? "환불전" : item.common_code.Code_Name,
                    Sort = item.common_code.Sort,
                });
            }

            return list;
            // return (List<TB_Common_Code>)(IEnumerable<TB_Common_Code>)query.AsEnumerable().FirstOrDefault();
            //   List<TB_Common_Code> brand_code = new List<TB_Common_Code>();
            //  var codes = new List<TB_Common_Code>();
            //codes = (TB_Common_Code)query.ToList();
            ////brand_code.Add((TB_Common_Code)query);
            //return codes;



        }

        /// <summary>
        /// 공통코드리스트 - Dapper
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        public List<TB_Common_Code> Common_CodeList_Dapper(string Code_Gubun)
        {
            var query = from c in Entity_db.TB_Common_Codes
                        join g in Entity_db.TB_Common_Code_Groups on c.Code_Group equals g.Code_Group
                        where g.Code_Group == Code_Gubun
                        select c;

            return query.ToList();
        }



        /// <summary>
        /// 공통코드리스트 - Sql
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        public List<TB_Common_Code> Common_CodeList_Sql(string Code_Gubun)
        {
            string ProcStr = "DBO.SP_S_ADMIN_COMMON_CODE";

            //string QueryStr = @"SELECT CODE, CODE_NAME, SORT FROM TB_COMMON_CODE A INNER
            //                JOIN TB_COMMON_CODE_GROUP B  ON A.CODE_GROUP = B.CODE_GROUP
            //                WHERE A.CODE_GROUP = '" + Code_Gubun + "' ";//' ORDER BY 3";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Common_Code> brand_code = new List<TB_Common_Code>();
            //SqlTransaction _Transation = null;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CODE_GROUP", Code_Gubun);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Common_Code");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Code = /*Convert.ToInt32*/(ds.Tables[0].Rows[i]["CODE"].ToString());
                    var CodeName = ds.Tables[0].Rows[i]["CODE_NAME"].ToString();
                    int Sort = int.Parse(ds.Tables[0].Rows[i]["SORT"].ToString());

                    brand_code.Add(new TB_Common_Code
                    {
                        Code = Code,
                        Code_Name = CodeName,
                        Sort = Sort
                    });
                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return brand_code;

        }




        #endregion 공통 End



        #region ************대/소분류 리스트************

        /// <summary>
        ///  대/소분류 리스트 추출 - Sql
        /// </summary>
        /// <param name="Gubun"></param> 1 : 메인대분류 / 2 : 카테고리대분류  / 3 : 카테고리중분류 
        /// <param name="Parent_CategoryId"></param> 상위 대분류 카테고리번호 
        /// <param name="DetailViewYn"></param>카테고리 상세 리스트 디스플레이 Y/N
        /// <param name="CategoryId"></param>선택한 카테고리의 번호 
        /// <returns></returns>
        public List<TB_Category> CateGoryList_Sql(int Gubun, int? Parent_CategoryId, string DetailViewYn, int? CategoryId)
        {
            string ProcStr = "DBO.SP_S_ADMIN_CATEGORY";


            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> CateGory = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GUBUN", Gubun);
                cmd.Parameters.AddWithValue("@DETAILVIEW_YN", DetailViewYn);
                cmd.Parameters.AddWithValue("@PARENT_CATEGORY_ID", Parent_CategoryId);
                cmd.Parameters.AddWithValue("@CATEGORY_ID", CategoryId);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Category_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString());
                    var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString();
                    var Category_Name = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                    var Category_Type_Code = ds.Tables[0].Rows[i]["CATEGORY_TYPE_CODE"].ToString();
                    var Category_Name_Type_Code = ds.Tables[0].Rows[i]["CATEGORY_NAME_TYPE_CODE"].ToString();
                    var Category_Name_PC = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC"].ToString();
                    var Category_Name_PC_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC_URL"].ToString();
                    //var Category_Name_Mobile = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE"].ToString();
                    //var Category_Name_Mobile_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE_URL"].ToString();
                    var Category_Step = ds.Tables[0].Rows[i]["CATEGORY_STEP"].ToString() == "" ? "1" : ds.Tables[0].Rows[i]["CATEGORY_STEP"].ToString();
                    var Sort = Convert.ToInt32(ds.Tables[0].Rows[i]["SORT"].ToString());
                    var Display_YN = ds.Tables[0].Rows[i]["DISPLAY_YN"].ToString();

                    CateGory.Add(new TB_Category
                    {
                        Category_ID = Category_ID,
                        Parent_Category_ID = Convert.ToInt32(Parent_Category_ID),
                        Category_Name = Category_Name,
                        Category_Type_Code = Category_Type_Code,
                        Category_Name_Type_Code = Category_Name_Type_Code,
                        Category_Name_PC = Category_Name_PC,
                        Category_Name_PC_URL = Category_Name_PC_URL,
                        //Category_Name_Mobile = Category_Name_Mobile,
                        //Category_Name_Mobile_URL = Category_Name_Mobile_URL,
                        Category_Step = Convert.ToInt32(Category_Step),
                        Sort = Sort,
                        Display_YN = Display_YN

                    });
                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return CateGory;
        }

        /// <summary>
        /// 카테고리 중분류 
        /// </summary>
        /// <param name="Parent_CategoryId"></param>
        /// <returns></returns>
        public List<TB_Category> CateGory_Depth2Info_Sql(int? Parent_CategoryId)
        {
            string ProcStr = "DBO.SP_S_ADMIN_CATEGORY_DEPTH2INFO";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> Category = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PARENT_CATEGORYID", Parent_CategoryId);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Category_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString());
                    // object Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] ?? 1;
                    //var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() ??= "1";
                    var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString();
                    var Category_Name = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString() + "(" + ds.Tables[0].Rows[i]["CNT"].ToString() + ")";
                    var Type_Code = ds.Tables[0].Rows[i]["CATEGORY_TYPE_CODE"].ToString();
                    var Category_Name_Type_Code = ds.Tables[0].Rows[i]["CATEGORY_NAME_TYPE_CODE"].ToString();
                    var Category_Name_PC = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC"].ToString();
                    var Category_Name_PC_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC_URL"].ToString();
                    var Category_Name_Mobile = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE"].ToString();
                    var Category_Name_Mobile_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE_URL"].ToString();
                    var Category_Step = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_STEP"].ToString());
                    var Sort = Convert.ToInt32(ds.Tables[0].Rows[i]["SORT"].ToString());
                    var Display_YN = ds.Tables[0].Rows[i]["DISPLAY_YN"].ToString();

                    Category.Add(new TB_Category
                    {
                        Category_ID = Category_ID,

                        Parent_Category_ID = Convert.ToInt32(Parent_Category_ID),
                        Category_Name = Category_Name,
                        Category_Type_Code = Type_Code,
                        Category_Name_Type_Code = Category_Name_Type_Code,
                        Category_Name_PC = Category_Name_PC,
                        Category_Name_PC_URL = Category_Name_PC_URL,
                        Category_Name_Mobile = Category_Name_Mobile,
                        Category_Name_Mobile_URL = Category_Name_Mobile_URL,
                        Category_Step = Category_Step,
                        Sort = Sort,
                        Display_YN = Display_YN

                    });
                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return Category;

        }

        /// <summary>
        /// 대/소분류 리스트 추출 - Dapper
        /// </summary>
        /// <param name="Gubun"></param>1 : 대분류 / 2 : 소분류 
        /// <param name="Parent_CategoryId"></param> 상위 대분류 카테고리번호 
        /// <param name="DetailViewYn"></param>카테고리 상세 리스트 디스플레이 Y/N
        /// <param name="CategoryId"></param>선택한 카테고리의 번호 
        /// <returns></returns>
        public List<TB_Category> CateGoryList_Dapper(int Gubun, int Parent_CategoryId, string DetailViewYn, int? CategoryId)
        {
            //SP_S_ADMIN_CATEGORY 2, 'N', NULL, NULL
            return Entity_db.TB_Categories
                .FromSqlInterpolated($"EXECUTE DBO.SP_S_ADMIN_CATEGORY @GUBUN={Gubun}, @DETAILVIEW_YN={DetailViewYn}, @PARENT_CATEGORY_ID={Parent_CategoryId},@CATEGORY_ID={CategoryId}")
                .ToList();
        }

        #endregion



        #endregion

        #region 상품 관련 INSERT / UPDATE

        /// <summary>
        ///  분류 관리 - 메인/카테고리 분류 -> 저장 (Entity Framework)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // [HttpPost]
        public string Category_Save_Entity(TB_Category model)
        {
            // model.Category_Type_Code = "CTC01";
            // model.Category_Name_Type_Code = "CNC01";
            Entity_db.Add(model);
            Entity_db.SaveChanges();

            return "Aa";


        }

        public string Product_Categoty_Save_Sql(string Gubun, string Display_Yn, string Product_Id_And_Category_Id, string UserID, string UserIP/* string Product_Id, int Category_Id*/)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Product_id", typeof(int));
                dt.Columns.Add("Category_id", typeof(int));

                if (!string.IsNullOrEmpty(Product_Id_And_Category_Id)) //1_3,2_3
                {
                    if (Product_Id_And_Category_Id.IndexOf(',') > 0)
                    {
                        for (int i = 0; i < Product_Id_And_Category_Id.Split(',').Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Product_Id_And_Category_Id.Split(',')[i])) //1_3
                            {
                                //dt.Rows.Add(Convert.ToInt32(Product_Id.Split(',')[i].ToString()));
                                DataRow newRow = dt.Rows.Add();
                                newRow.SetField("Product_id", Convert.ToInt32(Product_Id_And_Category_Id.Split(',')[i].Split('_')[0].ToString()));
                                newRow.SetField("Category_id", Convert.ToInt32(Product_Id_And_Category_Id.Split(',')[i].Split('_')[1].ToString()));
                                // dt.NewRow();
                                //dt.Rows.Add(Category_Id);
                            }
                        }
                    }
                    //else
                    //{
                    //    dt.Rows.Add(Convert.ToInt32(Product_Id));
                    //    dt.Rows.Add(Category_Id);
                    //}
                }

                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ADMIN_PRODUCT_CATEGORY";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter GubunParam = new SqlParameter("@GUBUN", SqlDbType.Char, 1);
                GubunParam.Value = Gubun;
                cmd.Parameters.Add(GubunParam);

                SqlParameter IdParam = new SqlParameter("@ID", SqlDbType.Structured);
                IdParam.Value = dt;
                cmd.Parameters.Add(IdParam);

                SqlParameter Display_YnParam = new SqlParameter("@DISPLAY_YN", SqlDbType.Char, 1);
                Display_YnParam.Value = Display_Yn;
                cmd.Parameters.Add(Display_YnParam);

                SqlParameter User_Id = new SqlParameter("@USER_ID", SqlDbType.VarChar, 50);
                User_Id.Value = UserID;
                cmd.Parameters.Add(User_Id);

                SqlParameter User_Ip = new SqlParameter("@IP", SqlDbType.VarChar, 15);
                User_Ip.Value = UserIP;
                cmd.Parameters.Add(User_Ip);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return "";
        }
        /// <summary>
        ///  분류 관리 - 메인/카테고리 분류 -> 저장 (Sql)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Category_Nm1"></param>
        /// <param name="Category_Nm2"></param>
        /// <returns></returns>
        public string Category_Save_Sql(string SaveGubun, TB_Category model, JObject Depth2, TB_Category model2, string Category_id_And_Sort)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                DataTable dt = new DataTable();
                dt.Columns.Add("Category_id", typeof(int));
                dt.Columns.Add("Sort", typeof(int));

                if (!string.IsNullOrEmpty(Category_id_And_Sort)) //4_1,9_2,11_3,
                {
                    if (Category_id_And_Sort.IndexOf(',') > 0)
                    {
                        for (int i = 0; i < Category_id_And_Sort.Split(',').Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Category_id_And_Sort.Split(',')[i])) //1_3
                            {
                                DataRow newRow = dt.Rows.Add();
                                newRow.SetField("Category_id", Convert.ToInt32(Category_id_And_Sort.Split(',')[i].Split('_')[0].ToString()));
                                newRow.SetField("Sort", Convert.ToInt32(Category_id_And_Sort.Split(',')[i].Split('_')[1].ToString()));
                            }


                        }


                    }

                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ADMIN_CATEGORY_NEW";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter Gubun = new SqlParameter("@GUBUN", SqlDbType.Char, 1);
                Gubun.Value = SaveGubun;
                cmd.Parameters.Add(Gubun);

                SqlParameter Category_Type_Code = new SqlParameter("@CATEGORY_TYPE_CODE", SqlDbType.VarChar, 50);
                Category_Type_Code.Value = model.Category_Type_Code;
                cmd.Parameters.Add(Category_Type_Code);

                SqlParameter Category_Name1 = new SqlParameter("@CATEGORY_NAME1", SqlDbType.VarChar, 100);
                Category_Name1.Value = model.Category_Name;
                cmd.Parameters.Add(Category_Name1);

                SqlParameter Category_Name_Type_Code = new SqlParameter("@CATEGORY_NAME_TYPE_CODE1", SqlDbType.VarChar, 50);
                Category_Name_Type_Code.Value = model.Category_Name_Type_Code;
                cmd.Parameters.Add(Category_Name_Type_Code);

                SqlParameter Category_Name_Pc = new SqlParameter("@CATEGORY_NAME_PC1", SqlDbType.VarChar, 100);
                Category_Name_Pc.Value = model.Category_Name_PC;
                cmd.Parameters.Add(Category_Name_Pc);

                SqlParameter Category_Name_Pc_Url = new SqlParameter("@CATEGORY_NAME_PC_URL1", SqlDbType.VarChar, 1000);
                Category_Name_Pc_Url.Value = model.Category_Name_PC_URL;
                cmd.Parameters.Add(Category_Name_Pc_Url);

                SqlParameter Category_Name_Mobile = new SqlParameter("@CATEGORY_NAME_MOBILE1", SqlDbType.VarChar, 100);
                Category_Name_Mobile.Value = model.Category_Name_Mobile;
                cmd.Parameters.Add(Category_Name_Mobile);

                SqlParameter Category_Name_Mobile_Url = new SqlParameter("@CATEGORY_NAME_MOBILE_URL1", SqlDbType.VarChar, 1000);
                Category_Name_Mobile_Url.Value = model.Category_Name_Mobile_URL;
                cmd.Parameters.Add(Category_Name_Mobile_Url);

                SqlParameter Category_Step = new SqlParameter("@CATEGORY_STEP1", SqlDbType.Int);
                Category_Step.Value = model.Category_Step;
                cmd.Parameters.Add(Category_Step);

                SqlParameter Sort = new SqlParameter("@SORT", SqlDbType.Int);
                Sort.Value = model.Sort;
                cmd.Parameters.Add(Sort);

                SqlParameter Display_Yn = new SqlParameter("@DISPLAY_YN1", SqlDbType.Char, 1);
                Display_Yn.Value = model.Display_YN;
                cmd.Parameters.Add(Display_Yn);


                SqlParameter Del_Up_Category_Id = new SqlParameter("@DEL_UP_CATEGORY_ID1", SqlDbType.Int);
                Del_Up_Category_Id.Value = model.Category_ID;
                cmd.Parameters.Add(Del_Up_Category_Id);

                if (Depth2 != null && Depth2.Count > 0)
                //if (model2 != null)
                {
                    SqlParameter Category_Name2 = new SqlParameter("@CATEGORY_NAME2", SqlDbType.VarChar, 100);
                    Category_Name2.Value = model2.Category_Name;
                    cmd.Parameters.Add(Category_Name2);

                    SqlParameter Category_Name_Type_Code2 = new SqlParameter("@CATEGORY_NAME_TYPE_CODE2", SqlDbType.VarChar, 50);
                    Category_Name_Type_Code2.Value = model2.Category_Name_Type_Code;
                    cmd.Parameters.Add(Category_Name_Type_Code2);

                    SqlParameter Category_Name_Pc2 = new SqlParameter("@CATEGORY_NAME_PC2", SqlDbType.VarChar, 100);
                    Category_Name_Pc2.Value = model2.Category_Name_PC;
                    cmd.Parameters.Add(Category_Name_Pc2);

                    SqlParameter Category_Name_Pc_Url2 = new SqlParameter("@CATEGORY_NAME_PC_URL2", SqlDbType.VarChar, 1000);
                    Category_Name_Pc_Url2.Value = model2.Category_Name_PC_URL;
                    cmd.Parameters.Add(Category_Name_Pc_Url2);

                    SqlParameter Category_Name_Mobile2 = new SqlParameter("@CATEGORY_NAME_MOBILE2", SqlDbType.VarChar, 100);
                    Category_Name_Mobile2.Value = model2.Category_Name_Mobile;
                    cmd.Parameters.Add(Category_Name_Mobile2);

                    SqlParameter Category_Name_Mobile_Url2 = new SqlParameter("@CATEGORY_NAME_MOBILE_URL2", SqlDbType.VarChar, 1000);
                    Category_Name_Mobile_Url2.Value = model2.Category_Name_Mobile_URL;
                    cmd.Parameters.Add(Category_Name_Mobile_Url2);

                    SqlParameter Category_Step2 = new SqlParameter("@CATEGORY_STEP2", SqlDbType.Int);
                    Category_Step2.Value = model2.Category_Step;
                    cmd.Parameters.Add(Category_Step2);

                    SqlParameter Depth2_Cnt = new SqlParameter("@DEPTH2_CNT", SqlDbType.Char, 1);
                    Depth2_Cnt.Value = "Y";
                    cmd.Parameters.Add(Depth2_Cnt);

                    SqlParameter Display_Yn2 = new SqlParameter("@DISPLAY_YN2", SqlDbType.Char, 1);
                    Display_Yn2.Value = model2.Display_YN;
                    cmd.Parameters.Add(Display_Yn2);


                    SqlParameter Del_Up_Category_Id2 = new SqlParameter("@DEL_UP_CATEGORY_ID2", SqlDbType.Int);
                    Del_Up_Category_Id2.Value = model2.Category_ID;
                    cmd.Parameters.Add(Del_Up_Category_Id2);

                }



                SqlParameter IdParam = new SqlParameter("@ID", SqlDbType.Structured);
                IdParam.Value = dt;
                cmd.Parameters.Add(IdParam);


                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return "Aa";

        }


        public string Category_Save_Sql_Test(string SaveGubun, TB_Category model, List<Dictionary<string, object>> Depth2_List2, string Category_id_And_Sort, string User_Id, string Uset_Ip)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                //정렬값 리스트 
                DataTable dt = new DataTable();
                dt.Columns.Add("Category_id", typeof(int));
                dt.Columns.Add("Sort", typeof(int));

                if (!string.IsNullOrEmpty(Category_id_And_Sort)) //4_1,9_2,11_3,
                {
                    if (Category_id_And_Sort.IndexOf(',') > 0)
                    {
                        for (int i = 0; i < Category_id_And_Sort.Split(',').Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Category_id_And_Sort.Split(',')[i])) //1_3
                            {
                                DataRow newRow = dt.Rows.Add();
                                newRow.SetField("Category_id", Convert.ToInt32(Category_id_And_Sort.Split(',')[i].Split('_')[0].ToString()));
                                newRow.SetField("Sort", Convert.ToInt32(Category_id_And_Sort.Split(',')[i].Split('_')[1].ToString()));
                            }


                        }


                    }

                }

                // 중분류리스트 
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Category_ID", typeof(int));
                dt2.Columns.Add("Parent_Category_Id", typeof(int));
                dt2.Columns.Add("Category_Name", typeof(string));
                dt2.Columns.Add("Category_Type_Code", typeof(string));
                dt2.Columns.Add("Category_Name_Type_Code", typeof(string));
                dt2.Columns.Add("Category_Name_PC", typeof(string));
                dt2.Columns.Add("Category_Name_PC_URL", typeof(string));
                dt2.Columns.Add("Category_Name_Mobile", typeof(string));
                dt2.Columns.Add("Category_Name_Mobile_URL", typeof(string));
                dt2.Columns.Add("Category_Step", typeof(int));
                dt2.Columns.Add("Sort", typeof(int));
                dt2.Columns.Add("Display_YN", typeof(string));

                int Depth2_Yn = 0;

                if (Depth2_List2.Count > 0)
                {
                    Depth2_Yn = 1;

                    foreach (Dictionary<string, object> list in Depth2_List2)
                    {
                        DataRow newRow2 = dt2.Rows.Add();
                        newRow2.SetField("Category_Id", Convert.ToInt32(list["Category_Id"].ToString()));
                        newRow2.SetField("Parent_Category_Id", 0);
                        newRow2.SetField("Category_Name", list["Category_Name"].ToString());
                        newRow2.SetField("Category_Type_Code", "CTC02"); //(메인 CTC01 / 카테고리 CTC02)
                        newRow2.SetField("Category_Name_Type_Code", list["Category_Name_Type_Code"].ToString());
                        newRow2.SetField("Category_Name_PC", list["Category_Name_PC"].ToString());

                        newRow2.SetField("Category_Name_PC_URL", list["Category_Name_PC_URL"].ToString());
                        newRow2.SetField("Category_Name_Mobile", "");
                        newRow2.SetField("Category_Name_Mobile_URL", "");
                        newRow2.SetField("Category_Step", Convert.ToInt32(list["Category_Step"].ToString()));
                        newRow2.SetField("Sort", Convert.ToInt32(list["Sort"].ToString()));
                        newRow2.SetField("Display_YN", list["Display_YN"].ToString());
                    }

                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ADMIN_CATEGORY_TEST";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter Gubun = new SqlParameter("@GUBUN", SqlDbType.Char, 1);
                Gubun.Value = SaveGubun;
                cmd.Parameters.Add(Gubun);

                SqlParameter Category_Type_Code = new SqlParameter("@CATEGORY_TYPE_CODE", SqlDbType.VarChar, 50);
                Category_Type_Code.Value = model.Category_Type_Code;
                cmd.Parameters.Add(Category_Type_Code);

                SqlParameter Category_Name1 = new SqlParameter("@CATEGORY_NAME1", SqlDbType.VarChar, 100);
                Category_Name1.Value = model.Category_Name;
                cmd.Parameters.Add(Category_Name1);

                SqlParameter Category_Name_Type_Code = new SqlParameter("@CATEGORY_NAME_TYPE_CODE1", SqlDbType.VarChar, 50);
                Category_Name_Type_Code.Value = model.Category_Name_Type_Code;
                cmd.Parameters.Add(Category_Name_Type_Code);

                SqlParameter Category_Name_Pc = new SqlParameter("@CATEGORY_NAME_PC1", SqlDbType.VarChar, 100);
                Category_Name_Pc.Value = model.Category_Name_PC;
                cmd.Parameters.Add(Category_Name_Pc);

                SqlParameter Category_Name_Pc_Url = new SqlParameter("@CATEGORY_NAME_PC_URL1", SqlDbType.VarChar, 1000);
                Category_Name_Pc_Url.Value = model.Category_Name_PC_URL;
                cmd.Parameters.Add(Category_Name_Pc_Url);

                SqlParameter Category_Name_Mobile = new SqlParameter("@CATEGORY_NAME_MOBILE1", SqlDbType.VarChar, 100);
                Category_Name_Mobile.Value = model.Category_Name_Mobile;
                cmd.Parameters.Add(Category_Name_Mobile);

                SqlParameter Category_Name_Mobile_Url = new SqlParameter("@CATEGORY_NAME_MOBILE_URL1", SqlDbType.VarChar, 1000);
                Category_Name_Mobile_Url.Value = model.Category_Name_Mobile_URL;
                cmd.Parameters.Add(Category_Name_Mobile_Url);

                SqlParameter Category_Step = new SqlParameter("@CATEGORY_STEP1", SqlDbType.Int);
                Category_Step.Value = model.Category_Step;
                cmd.Parameters.Add(Category_Step);

                SqlParameter Sort = new SqlParameter("@SORT", SqlDbType.Int);
                Sort.Value = model.Sort;
                cmd.Parameters.Add(Sort);

                SqlParameter Display_Yn = new SqlParameter("@DISPLAY_YN1", SqlDbType.Char, 1);
                Display_Yn.Value = model.Display_YN;
                cmd.Parameters.Add(Display_Yn);


                SqlParameter Del_Up_Category_Id = new SqlParameter("@DEL_UP_CATEGORY_ID1", SqlDbType.Int);
                Del_Up_Category_Id.Value = model.Category_ID;
                cmd.Parameters.Add(Del_Up_Category_Id);

                SqlParameter Depth2_Yn_Param = new SqlParameter("@DEPTH2_YN", SqlDbType.Int);
                Depth2_Yn_Param.Value = Depth2_Yn;
                cmd.Parameters.Add(Depth2_Yn_Param);


                if (Depth2_Yn > 0)
                {

                    SqlParameter Depth2List_Param = new SqlParameter("@DEPTH2", SqlDbType.Structured);
                    Depth2List_Param.Value = dt2;
                    cmd.Parameters.Add(Depth2List_Param);

                    //SqlParameter Del_Up_Category_Id2 = new SqlParameter("@DEL_UP_CATEGORY_ID2", SqlDbType.Int);
                    //Del_Up_Category_Id2.Value = model2.Category_ID;
                    //cmd.Parameters.Add(Del_Up_Category_Id2);

                }



                SqlParameter IdParam = new SqlParameter("@ID", SqlDbType.Structured);
                IdParam.Value = dt;
                cmd.Parameters.Add(IdParam);


                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return "Aa";

        }

        public string Admin_Product_Category_Sort_Update_Entity(List<TB_Product_Category> model, string UserIP)
        {
            foreach (var item in model)
            {
                int Category_ID = item.Category_ID;
                int Product_ID = item.Product_ID;
                int Sort = (int)item.Sort;
                DateTime Date = Convert.ToDateTime(item.Update_DateTime);
                string Update_User_ID = item.Update_User_ID;

                var Update_Product_Category_Item = Entity_db.TB_Product_Categories.Where(n => n.Category_ID.Equals(Category_ID) && n.Product_ID.Equals(Product_ID)).SingleOrDefault();
                if (Update_Product_Category_Item != null)
                {
                    Update_Product_Category_Item.Sort = Sort;
                    Update_Product_Category_Item.Update_DateTime = DateTime.Now;
                    Update_Product_Category_Item.Update_IP = UserIP;
                    Update_Product_Category_Item.Update_User_ID = Update_User_ID;
                    Entity_db.Entry(Update_Product_Category_Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    Entity_db.SaveChanges();
                }

            }
            return "";

        }

        /// <summary>
        ///  분류 관리 - 메인/카테고리 분류 -> 삭제 (Sql)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Type_Code"></param> //메인 CTC01 / 카테고리 CTC02
        /// <param name="Category_Id"></param>// 카테고리 ID
        /// <returns></returns>
        public string Category_Del_Sql(string Type_Code, int Category_Id)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ADMIN_CATEGORY_NEW";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter Gubun = new SqlParameter("@GUBUN", SqlDbType.Char, 1);
                Gubun.Value = "D";
                cmd.Parameters.Add(Gubun);

                SqlParameter Category_Type_Code = new SqlParameter("@CATEGORY_TYPE_CODE", SqlDbType.VarChar, 50);
                Category_Type_Code.Value = Type_Code;
                cmd.Parameters.Add(Category_Type_Code);

                SqlParameter Del_Up_Category_Id1 = new SqlParameter("@DEL_UP_CATEGORY_ID1", SqlDbType.Int);
                Del_Up_Category_Id1.Value = Category_Id;
                cmd.Parameters.Add(Del_Up_Category_Id1);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return "Aa";

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Gubun"></param>
        /// <param name="Product_Id_And_Category_Id"></param>
        /// <param name="Category_Id"></param>
        /// <returns></returns>
        public string Categoty_Update_Sql(string Gubun, string Product_Id_And_Category_Id, int Category_Id, string UserID, string UserIP)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Product_id", typeof(int));
                dt.Columns.Add("Category_id", typeof(int));

                if (!string.IsNullOrEmpty(Product_Id_And_Category_Id)) //1_3,2_3
                {
                    if (Product_Id_And_Category_Id.IndexOf(',') > 0)
                    {
                        for (int i = 0; i < Product_Id_And_Category_Id.Split(',').Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Product_Id_And_Category_Id.Split(',')[i])) //1_3
                            {
                                //dt.Rows.Add(Convert.ToInt32(Product_Id.Split(',')[i].ToString()));
                                DataRow newRow = dt.Rows.Add();
                                newRow.SetField("Product_id", Convert.ToInt32(Product_Id_And_Category_Id.Split(',')[i].Split('_')[0].ToString()));
                                newRow.SetField("Category_id", Convert.ToInt32(Product_Id_And_Category_Id.Split(',')[i].Split('_')[1].ToString()));
                                // dt.NewRow();
                                //dt.Rows.Add(Category_Id);
                            }
                        }
                    }
                    //else
                    //{
                    //    dt.Rows.Add(Convert.ToInt32(Product_Id));
                    //    dt.Rows.Add(Category_Id);
                    //}
                }

                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ADMIN_PRODUCT_CATEGORY";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter GubunParam = new SqlParameter("@GUBUN", SqlDbType.Char, 2);
                GubunParam.Value = Gubun;
                cmd.Parameters.Add(GubunParam);

                SqlParameter IdParam = new SqlParameter("@ID", SqlDbType.Structured);
                IdParam.Value = dt;
                cmd.Parameters.Add(IdParam);

                SqlParameter Category_Id_Param = new SqlParameter("@CATEGORY_ID", SqlDbType.Int, 1);
                Category_Id_Param.Value = Category_Id;
                cmd.Parameters.Add(Category_Id_Param);

                SqlParameter User_Id = new SqlParameter("@USER_ID", SqlDbType.VarChar, 50);
                User_Id.Value = UserID;
                cmd.Parameters.Add(User_Id);

                SqlParameter User_Ip = new SqlParameter("@IP", SqlDbType.VarChar, 15);
                User_Ip.Value = UserIP;
                cmd.Parameters.Add(User_Ip);


                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return "";
        }


        #endregion

        #region 상품 관련 SELECT

        /// <summary>
        /// [진열관리 - 메인,카테고리 진열] - 메인/카테고리 진열설정된 상품 검색
        /// </summary>
        /// <param name="Category_Type_Code"></param>
        /// <param name="Product_Category_Code"></param>
        /// <param name="Product_Brand_Code"></param>
        /// <param name="Category_Id"></param>
        /// <param name="Searchtxt"></param>
        /// <param name="Searchtxt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Display_ProductList_Entity(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn)
        {
            //  Entity_db.Entry(entity).Reload()
            //  Entity_db.Entry("rwer").Reload();
            //TB_Product_Category aa = new TB_Product_Category();
            //Entity_db.Entry(aa).GetDatabaseValues();
            //Entity_db.Update();
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Display_YN.Equals("Y"))
                        join product_category in Entity_db.Set<TB_Product_Category>()
                            on product.Product_ID equals product_category.Product_ID

                        join template in Entity_db.Set<TB_Template>() on product.Template_ID equals template.Template_ID

                        // into products
                        //from p1 in products.DefaultIfEmpty()
                        join category in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Display_YN.Equals("Y"))
                            on product_category.Category_ID equals category.Category_ID
                        join category_code1 in Entity_db.Set<TB_Common_Code>() on product.Product_Category_Code equals category_code1.Code
                        join category_code2 in Entity_db.Set<TB_Common_Code>() on product.Product_Brand_Code equals category_code2.Code

                        select new
                        {
                            product,
                            product_category,
                            category,
                            category_code1,
                            category_code2,
                            template//,
                            //category_test

                        };

            if (!String.IsNullOrEmpty(Product_Category_Code))
                query = query.Where(x => x.product.Product_Category_Code == Product_Category_Code);

            if (!String.IsNullOrEmpty(Product_Brand_Code))
                query = query.Where(x => x.product.Product_Brand_Code == Product_Brand_Code);

            if (!String.IsNullOrEmpty(Category_Id1.ToString()) && Category_Id1 > 0)
                query = query.Where(x => x.category.Category_ID == Category_Id1);

            if (!String.IsNullOrEmpty(Searchtxt))
                query = query.Where(x => x.product.Product_Name.Contains(Searchtxt) || x.product.Product_Code.Contains(Searchtxt));

            //if (!String.IsNullOrEmpty(SearchViewYn) && !SearchViewYn.Equals("ALL"))
            //    query = query.Where(x => x.product_category.Display_YN == SearchViewYn);


            // query = query.Where(x => x.product_category.Display_YN == "Y");
            //   Entity_db.Entry(TB_Product_Category).Reload();

            query = query.OrderByDescending(x => x.product_category.Regist_DateTime);

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                int Product_ID = item.product.Product_ID;
                string Product_Pay_Gubun = Order_Product_Pay_Gubun(Product_ID);

                //dic.Add("Preview_Image_URL", item.template.Preview_Image_URL);
                dic.Add("Product_ID", item.product.Product_ID);
                dic.Add("Brand_Code", item.product.Product_Brand_Code);
                //dic.Add("brandname", brandname);
                dic.Add("Product_Name", item.product.Product_Name);
                dic.Add("Product_Code", item.product.Product_Code);
                dic.Add("Price", DateTimeHelper.Int_Format(item.product.Price.ToString()));
                dic.Add("Category_Id", item.category.Category_ID);
                dic.Add("Category_Name1", item.category.Category_Name);
                dic.Add("Category_Name2", item.category.Category_Name);

                //if(item.category_test.Category_ID > 0)
                //{
                //    dic.Add("Category_Name1", item.category_test.Category_Name);
                //}
                //else
                //{
                //    dic.Add("Category_Name1", item.category.Category_Name);
                //}

                //if (item.category_test.Category_ID > 0)
                //{
                //    dic.Add("Category_Name2", item.category.Category_Name);
                //}
                //else
                //{
                //    dic.Add("Category_Name2","");
                //}

                //  dic.Add("Category_Name3", item.category.Parent_Category_ID == item.category2.Select( ? "111" : "222");
                //dic.Add("Category_Name4", item.category.Parent_Category_ID == item.category.Category_ID ?  "333" : "444");
                // dic.Add("Category_Name122", model..Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_ID == item.category.Parent_Category_ID).Count() > 0 ? category category22.Category_Name);
                //context.Customer.Select(c => new { c.Id, c.First, Last = c.LocationId == 2 ? c.Last : "" });


                //var qq1 = from tt in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_ID == item.category.Parent_Category_ID).Select(c => new { Category_Name = c.Category_Name != "" ? c.Category_Name : item.category.Category_Name }) select new { tt.Category_Name };
                //foreach (var item1 in qq1)
                //{
                //    dic.Add("Category_Name122", item1.Category_Name);

                //}


                //var qq2 = from tt in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_ID == item.category.Parent_Category_ID).Select(c => new { Category_Name = c.Category_Name != "" ? item.category.Category_Name : "" }) select new { tt.Category_Name };
                //foreach (var item2 in qq2)
                //{
                //    dic.Add("Category_Name234", item2.Category_Name);

                // }

                //dic.Add("Category_Name122", from tt in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_ID == item.category.Parent_Category_ID).Select(c => new { Category_Name = c.Category_Name != "" ? c.Category_Name : item.category.Category_Name }) select new { tt.Category_Name });
                //dic.Add("Category_Name234", from tt in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_ID == item.category.Parent_Category_ID).Select(c => new { Category_Name = c.Category_Name != "" ? item.category.Category_Name : "" }) select new { tt.Category_Name });
                ////dic.Add("Category_Name2", item.category.Category_Name);

                dic.Add("DateTime", DateTimeHelper.HHmm(item.product_category.Regist_DateTime));
                dic.Add("Main_Img_Url", item.product.Main_Image_URL);
                dic.Add("Preview_Img_Url", item.product.Preview_Image_URL);
                dic.Add("Display_Yn", "Y");
                dic.Add("Pay_Cnt", DateTimeHelper.Int_Format(Product_Pay_Gubun.Split("_")[0].ToString()));
                dic.Add("Free_Cnt", DateTimeHelper.Int_Format(Product_Pay_Gubun.Split("_")[1].ToString()));
                result.Add(dic);


            }

            return result;

            #region 쿼리
            //SELECT*
            //FROM TB_PRODUCT A INNER JOIN

            //     TB_PRODUCT_CATEGORY B ON A.PRODUCT_ID = B.PRODUCT_ID INNER JOIN

            //     TB_CATEGORY C ON B.CATEGORY_ID = C.CATEGORY_ID INNER JOIN

            //     TB_COMMON_CODE D ON A.PRODUCT_CATEGORY_CODE = D.CODE INNER JOIN

            //     TB_COMMON_CODE E ON A.PRODUCT_BRAND_CODE = E.CODE

            //WHERE C.CATEGORY_TYPE_CODE = 'CTC01' AND

            //      A.PRODUCT_CATEGORY_CODE = @PRODUCT_CATEGORY_CODE AND

            //      A.PRODUCT_BRAND_CODE = @PRODUCT_BRAND_CODE AND

            //      C.CATEGORY_ID = @CATEGORY_ID AND
            //     (A.PRODUCT_NAME LIKE  '%' + @SEARCHTXT + '%' OR A.PRODUCT_CODE LIKE '%' + @SEARCHTXT + '%')
            #endregion

        }


        /// <summary>
        /// 주문한 상품(결재완료)의 유/무료 카운트 
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        private string Order_Product_Pay_Gubun(int Product_ID)
        {
            //var query = from order in Entity_db.Set<TB_Order>().Where(x => x.Payment_Status_Code.Equals("PSC02")) //결제완료
            //            join order_product in Entity_db.Set<TB_Order_Product>().Where(x => x.Product_ID.Equals(Product_ID)) 
            //                on order.Order_ID equals order_product.Order_ID

            // 유료구매카운트
            int Pay_Cnt = (from order in Entity_db.Set<TB_Order>().Where(x => x.Payment_Status_Code.Equals("PSC02") && x.Payment_Price > 0) //결제완료
                           join order_product in Entity_db.Set<TB_Order_Product>().Where(x => x.Product_ID.Equals(Product_ID))
                               on order.Order_ID equals order_product.Order_ID
                           select order).Count();
            // 무료구매카운트
            int Free_Cnt = (from order in Entity_db.Set<TB_Order>().Where(x => x.Payment_Status_Code.Equals("PSC02") && (x.Payment_Price.Equals(0) || string.IsNullOrEmpty(x.Payment_Price.ToString()))) //결제완료
                            join order_product in Entity_db.Set<TB_Order_Product>().Where(x => x.Product_ID.Equals(Product_ID))
                                on order.Order_ID equals order_product.Order_ID
                            select order).Count();

            return Pay_Cnt.ToString() + "_" + Free_Cnt.ToString();


        }

        /// <summary>
        /// [진열관리 - 메인,카테고리 진열] - 메인/카테고리 진열설정된 상품 검색
        /// </summary>
        /// <param name="Category_Type_Code"></param>
        /// <param name="Product_Category_Code"></param>
        /// <param name="Product_Brand_Code"></param>
        /// <param name="Category_Id1"></param>
        /// <param name="Category_Id2"></param>
        /// <param name="Searchtxt"></param>
        /// <param name="SearchViewYn"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Display_ProductList_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn, string User_ID)
        {
            string ProcStr = "DBO.SP_S_ADMIN_PRODUCT_CATEGORY";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CATEGORY_TYPE_CODE", Category_Type_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_CODE", Product_Category_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_BRAND_CODE", Product_Brand_Code);
                cmd.Parameters.AddWithValue("@SEARCHTXT", Searchtxt);
                cmd.Parameters.AddWithValue("@SEARCHVIEWYN", SearchViewYn);
                cmd.Parameters.AddWithValue("@CATEGORY_ID", Category_Id1);

                if (!string.IsNullOrEmpty(User_ID)) cmd.Parameters.AddWithValue("@USER_ID", User_ID);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"];
                    var Category_Name1 = ds.Tables[0].Rows[i]["CATEGORY_NAME1"];
                    var Category_Name2 = ds.Tables[0].Rows[i]["CATEGORY_NAME2"];
                    var Product_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString());
                    var Category_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString());
                    var Parent_Category_Id = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] != null ? ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] : 0;

                    var Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    var Display_Yn = ds.Tables[0].Rows[i]["DISPLAY_YN"].ToString();
                    var Price = Convert.ToInt32(ds.Tables[0].Rows[i]["PRICE"].ToString());
                    var Regist_Datetime = ds.Tables[0].Rows[i]["REGIST_DATETIME"].ToString();
                    var Main_Img_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    var Product_Description = ds.Tables[0].Rows[i]["Product_Description"].ToString();
                    var Pay_Cnt = ds.Tables[0].Rows[i]["PAY_CNT"].ToString();
                    var Free_Cnt = ds.Tables[0].Rows[i]["FREE_CNT"].ToString();
                    var Wish_Cnt = ds.Tables[0].Rows[i]["WISH_CNT"].ToString();

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Category_Name1", Category_Name1);
                    dic.Add("Category_Name2", Category_Name2);
                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Category_Id", Category_Id);
                    dic.Add("Parent_Category_Id", Parent_Category_Id);
                    dic.Add("Brand_Code", Brand_Code);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Display_Yn", Display_Yn);
                    dic.Add("Price", DateTimeHelper.Int_Format(Price.ToString()));
                    dic.Add("Regist_Datetime", DateTimeHelper.HHmm(Regist_Datetime));
                    dic.Add("Main_Img_Url", Main_Img_Url);
                    dic.Add("Product_Description", Product_Description);

                    dic.Add("Pay_Cnt", DateTimeHelper.Int_Format(Pay_Cnt.ToString()));
                    dic.Add("Free_Cnt", DateTimeHelper.Int_Format(Free_Cnt.ToString()));
                    dic.Add("Wish_Cnt", DateTimeHelper.Int_Format(Wish_Cnt.ToString()));

                    result.Add(dic);

                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return result;

        }



        public List<Dictionary<string, object>> Display_ProductList_Search_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int? Category_Id1, int? Category_Id2, string Searchtxt, string SearchViewYn)
        {
            string ProcStr = "DBO.SP_S_ADMIN_PRODUCT_SEARCH_CATEGORY";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CATEGORY_TYPE_CODE", Category_Type_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_CODE", Product_Category_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_BRAND_CODE", Product_Brand_Code);
                cmd.Parameters.AddWithValue("@SEARCHTXT", Searchtxt);
                cmd.Parameters.AddWithValue("@SEARCHVIEWYN", SearchViewYn);
                cmd.Parameters.AddWithValue("@CATEGORY_ID1", Category_Id1);
                cmd.Parameters.AddWithValue("@CATEGORY_ID2", Category_Id2);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"];
                    var Category_Name1 = ds.Tables[0].Rows[i]["CATEGORY_NAME1"];
                    var Category_Name2 = ds.Tables[0].Rows[i]["CATEGORY_NAME2"];
                    var Product_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString());
                    var Category_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString());
                    var Parent_Category_Id = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] != null ? ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] : 0;

                    var Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    var Display_Yn = ds.Tables[0].Rows[i]["DISPLAY_YN"].ToString();
                    var Price = Convert.ToInt32(ds.Tables[0].Rows[i]["PRICE"].ToString());
                    var Regist_Datetime = ds.Tables[0].Rows[i]["REGIST_DATETIME"].ToString();
                    var Main_Img_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    var Product_Description = ds.Tables[0].Rows[i]["Product_Description"].ToString();
                    var Pay_Cnt = ds.Tables[0].Rows[i]["PAY_CNT"];
                    var Free_Cnt = ds.Tables[0].Rows[i]["FREE_CNT"];

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Category_Name1", Category_Name1);
                    dic.Add("Category_Name2", Category_Name2);
                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Category_Id", Category_Id);
                    dic.Add("Parent_Category_Id", Parent_Category_Id);
                    dic.Add("Brand_Code", Brand_Code);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Display_Yn", Display_Yn);
                    dic.Add("Price", DateTimeHelper.Int_Format(Price.ToString()));
                    dic.Add("Regist_Datetime", DateTimeHelper.HHmm(Regist_Datetime));
                    dic.Add("Main_Img_Url", Main_Img_Url);
                    dic.Add("Product_Description", Product_Description);
                    dic.Add("Pay_Cnt", DateTimeHelper.Int_Format(Pay_Cnt.ToString()));
                    dic.Add("Free_Cnt", DateTimeHelper.Int_Format(Free_Cnt.ToString()));
                    result.Add(dic);

                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return result;

        }


        /// <summary>
        /// [메인진열] - 진열관리 - 메인/카테고리 상품추가 화면에 뿌려질 상품 검색 리스트 
        /// </summary>
        /// <param name="Category_Type_Code">CTC01 - 메인 진열 / CTC02 - 카테고리 진열</param>
        /// <param name="Product_Category_Code">청첩장, 감사장 등등..</param>
        /// <param name="Product_Brand_Code">바른손, 더카드 등등..</param>
        /// <param name="Category_Id"></param>
        /// <param name="Searchtxt"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Display_AddProductList_Sql(string Category_Type_Code, string Product_Category_Code, string Product_Brand_Code, int Category_Id, string Searchtxt)
        {
            string ProcStr = "DBO.SP_S_ADMIN_PRODUCT_DISPLAY_NO";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CATEGORY_TYPE_CODE", Category_Type_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_CATEGORY_CODE", Product_Category_Code);
                cmd.Parameters.AddWithValue("@PRODUCT_BRAND_CODE", Product_Brand_Code);
                cmd.Parameters.AddWithValue("@SEARCHTXT", Searchtxt);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Product_Id = Convert.ToInt32(ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString());
                    // object Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] ?? 1;
                    //var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() ??= "1";
                    //var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString();
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
                    var Category_Code = ds.Tables[0].Rows[i]["PRODUCT_CATEGORY_CODE"].ToString();
                    var Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    var Regist_DateTime = ds.Tables[0].Rows[i]["REGIST_DATETIME"].ToString();
                    var Price = ds.Tables[0].Rows[i]["PRICE"].ToString();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Category_Code", Category_Code);
                    dic.Add("Brand_Code", Brand_Code);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Main_Image_Url", Main_Image_Url);
                    dic.Add("Regist_DateTime", Regist_DateTime);
                    dic.Add("Price", Price);

                    result.Add(dic);

                }
                //_Transation.Commit();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return result;

        }
        /// <summary>
        /// 상품/템플릿 전체 리스트 및 검색
        /// </summary>
        /// <param name="SearchKind">분류 - 전체, M청첩장, M감사장</param>
        /// <param name="SearchBrand">브랜드 - 전체 , 바른손, 비핸즈, 더카드, 프리미어페이퍼</param>
        /// <param name="SearchViewYn">진열 - 전체, 진열, 미진열</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ProductList_Entity(string SearchKind, string SearchBrand, string SearchViewYn, string Searchtxt)
        {
            var query = from product in Entity_db.Set<TB_Product>()

                        join template in Entity_db.Set<TB_Template>()
                        on product.Template_ID equals template.Template_ID
                        //let brandname = (
                        //         product.Product_Brand_Code.Equals("PBC01") ? "바른손" :
                        //         product.Product_Brand_Code.Equals("PBC02") ? "비핸즈" :
                        //         product.Product_Brand_Code.Equals("PBC03") ? "더카드" :DisplayUpdate
                        //         product.Product_Brand_Code.Equals("PBC04") ? "프리미어페이퍼" :
                        //       "")
                        //group product by new { brandname } into aa
                        //select new { aa.Key.brandname, product, template };
                        select new { product, template };

            //if (!String.IsNullOrEmpty(SearchKind) && !SearchKind.ToUpper().Equals("ALL"))
            if (!String.IsNullOrEmpty(SearchKind))// && SearchKind.IndexOf('_') > 0)
            {
                query = query.Where(x => SearchKind.Contains(x.product.Product_Category_Code));
            }

            if (!String.IsNullOrEmpty(SearchBrand) && SearchBrand.IndexOf('_') > 0)
                query = query.Where(x => SearchBrand.Contains(x.product.Product_Brand_Code));

            if (!String.IsNullOrEmpty(SearchViewYn) && !SearchViewYn.Equals("ALL"))
                query = query.Where(x => x.product.Display_YN == SearchViewYn);

            if (!String.IsNullOrEmpty(Searchtxt))
                query = query.Where(x => x.product.Product_Name.Contains(Searchtxt) || x.product.Product_Code.Contains(Searchtxt));

            query = query.OrderByDescending(x => x.product.Regist_DateTime);

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                //string brandname = "";
                //switch (item.product.Product_Brand_Code)
                //{
                //    case "PBC01":
                //        brandname = "바른손";
                //        break;
                //    case "PBC02":
                //        brandname = "비핸즈";
                //        break;
                //    case "PBC03":
                //        brandname = "더카드";
                //        break;
                //    case "PBC04":
                //        brandname = "프리미어페이퍼";
                //        break;
                //    default:
                //        break;
                //}
                dic.Add("Product_ID", item.product.Product_ID);
                dic.Add("Main_Image_URL", item.product.Main_Image_URL);
                dic.Add("Brand_Code", item.product.Product_Brand_Code);
                //dic.Add("brandname", brandname);
                dic.Add("Product_Name", item.product.Product_Name);
                dic.Add("Product_Code", item.product.Product_Code);
                dic.Add("Price", item.product.Price.ToString());
                dic.Add("Display_YN", item.product.Display_YN);
                dic.Add("Regist_DateTime", item.product.Regist_DateTime.ToString());
                dic.Add("Update_DateTime", item.product.Update_DateTime.ToString());

                result.Add(dic);
            }

            return result;
        }



        /// <summary>
        ///  분류 관리 - 메인/카테고리 분류 -> 중분류 리스트 (Sql)
        /// </summary>
        /// <param name="Code_Gubun"></param>
        /// <returns></returns>
        public List<TB_Category> Product_CategoryList_Sql(string Category_Type_Code)
        {
            string ProcStr = "DBO.SP_S_ADMIN_CATEGORY_DISPLAYCNT";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> Category = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CATEGORY_TYPE_CODE", Category_Type_Code);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Category_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString());
                    // object Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"] ?? 1;
                    //var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() ??= "1";
                    var Parent_Category_ID = ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[i]["PARENT_CATEGORY_ID"].ToString();
                    var Category_Name = ds.Tables[0].Rows[i]["CATEGORY_NAME"].ToString() + "(" + ds.Tables[0].Rows[i]["CNT"].ToString() + ")";

                    // if (Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_STEP"].ToString()) == 2) Category_Name = "↳" + Category_Name;

                    var Type_Code = ds.Tables[0].Rows[i]["CATEGORY_TYPE_CODE"].ToString();
                    var Category_Name_Type_Code = ds.Tables[0].Rows[i]["CATEGORY_NAME_TYPE_CODE"].ToString();
                    var Category_Name_PC = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC"].ToString();
                    var Category_Name_PC_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_PC_URL"].ToString();
                    var Category_Name_Mobile = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE"].ToString();
                    var Category_Name_Mobile_URL = ds.Tables[0].Rows[i]["CATEGORY_NAME_MOBILE_URL"].ToString();
                    var Category_Step = Convert.ToInt32(ds.Tables[0].Rows[i]["CATEGORY_STEP"].ToString());
                    var Sort = Convert.ToInt32(ds.Tables[0].Rows[i]["SORT"].ToString());
                    var Display_YN = ds.Tables[0].Rows[i]["DISPLAY_YN"].ToString();

                    Category.Add(new TB_Category
                    {
                        Category_ID = Category_ID,

                        Parent_Category_ID = Convert.ToInt32(Parent_Category_ID),
                        Category_Name = Category_Name,
                        Category_Type_Code = Type_Code,
                        Category_Name_Type_Code = Category_Name_Type_Code,
                        Category_Name_PC = Category_Name_PC,
                        Category_Name_PC_URL = Category_Name_PC_URL,
                        Category_Name_Mobile = Category_Name_Mobile,
                        Category_Name_Mobile_URL = Category_Name_Mobile_URL,
                        Category_Step = Category_Step,
                        Sort = Sort,
                        Display_YN = Display_YN

                    });
                }
                //_Transation.Commit(); 
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return Category;

        }







        #endregion

        #region 상품 등록 관련 SELECT

        public List<TB_Category> TB_CategoryList_Entity(string Category_Type_Code, int Category_Step)
        {

            var query = from catecory in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_Step == Category_Step)

                        select new { catecory };

            var list = new List<TB_Category>();
            foreach (var item in query)
            {
                list.Add(new TB_Category()
                {
                    Category_ID = item.catecory.Category_ID,
                    Category_Name = item.catecory.Category_Name
                });
            }

            return list;
        }

        public List<TB_Category> TB_Sub_CategoryList_Entity(string Category_Type_Code, int Category_Step, int Parent_Category_ID)
        {

            var query = from catecory in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == Category_Type_Code && x.Category_Step == Category_Step && x.Parent_Category_ID == Parent_Category_ID)

                        select new { catecory };

            var list = new List<TB_Category>();
            foreach (var item in query)
            {
                list.Add(new TB_Category()
                {
                    Category_ID = item.catecory.Category_ID,
                    Category_Name = item.catecory.Category_Name
                });
            }

            return list;
        }

        public List<Dictionary<string, object>> TB_IconList_Entity(int Product_Id)
        {

            var query = from icon in Entity_db.Set<TB_Icon>()
                        join product_icon in Entity_db.Set<TB_Product_Icon>().Where(x => x.Product_ID == Product_Id) on icon.Icon_ID equals product_icon.Icon_ID into Icon_ID
                        from p in Icon_ID.DefaultIfEmpty()
                        select new { icon, p };

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Checked", item.p == null ? false : true);
                dic.Add("Icon_ID", item.icon.Icon_ID);
                dic.Add("Icon_URL", item.icon.Icon_URL);

                result.Add(dic);
            }
            return result;

        }

        public string IconList_Save_Entity(TB_Icon model)
        {
            Entity_db.Add(model);
            Entity_db.SaveChanges();

            return "Aa";
        }

        public string TB_Product_Image_Save_Entity(TB_Product_Image model)
        {
            Entity_db.Add(model);
            Entity_db.SaveChanges();

            return "Aa";
        }

        public string IconList_Delete_Entity(TB_Icon model)
        {
            var query = Entity_db.Set<TB_Product_Icon>().Where(x => x.Icon_ID == model.Icon_ID).ToList();

            foreach (var item in query)
            {
                Entity_db.Remove(item);
                Entity_db.SaveChanges();
            }

            var query2 = Entity_db.Set<TB_Icon>().Where(x => x.Icon_ID == model.Icon_ID).FirstOrDefault();

            Entity_db.Remove(query2);
            Entity_db.SaveChanges();

            return "Aa";
        }

        public string Get_Product_Code_Seq(string Product_Code)
        {
            string seq = string.Empty;

            int n = Product_Code == null ? 0 : int.Parse(Product_Code.Substring(4, 2));

            seq = string.Format("{0:D2}", n + 1);

            return seq;
        }

        public string Product_Code_New_Entity(string Product_Brand_Code)
        {

            string code = "";

            string result = "";

            string year = DateTime.Now.Year.ToString();

            string y = year.Substring(year.Length - 1, 1);

            var query = Entity_db.Set<TB_Product>().Where(x => x.Product_Brand_Code == Product_Brand_Code && x.Original_Product_Code.Substring(2, 1) == y).OrderByDescending(x => x.Original_Product_Code).FirstOrDefault();

            string Product_Code = query == null ? null : query.Original_Product_Code;

            switch (Product_Brand_Code)
            {
                case "PBC01":
                    code = "2";
                    break;
                case "PBC02":
                    code = "0";
                    break;
                case "PBC03":
                    code = "4";
                    break;
                case "PBC04":
                    code = "6";
                    break;
            }

            result = "MC" + y + code + Get_Product_Code_Seq(Product_Code);

            return result;
        }


        #endregion

        #region 상품 등록 (상세)관련 SELECT

        public List<TB_Product> TB_Product_Entity(int Product_ID)
        {

            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID)
                        select new { product };

            var list = new List<TB_Product>();
            foreach (var item in query)
            {
                list.Add(new TB_Product()
                {
                    Product_ID = item.product.Product_ID,
                    Template_ID = item.product.Template_ID,
                    Product_Code = item.product.Product_Code,
                    Original_Product_Code = item.product.Original_Product_Code,
                    Product_Category_Code = item.product.Product_Category_Code,
                    Product_Brand_Code = item.product.Product_Brand_Code,
                    Product_Name = item.product.Product_Name,
                    Product_Description = item.product.Product_Description,
                    Price = item.product.Price,
                    Display_YN = item.product.Display_YN,
                    Main_Image_URL = item.product.Main_Image_URL,
                    Preview_Image_URL = item.product.Preview_Image_URL,
                });
            }

            return list;
        }

        public List<Dictionary<string, object>> TB_Product_Template_Entity(int Product_ID)
        {
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID)
                        join template in Entity_db.Set<TB_Template>() on product.Template_ID equals template.Template_ID
                        select new { product, template };

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                {
                    dic.Add("Product_ID", item.product.Product_ID);
                    dic.Add("Template_ID", item.product.Template_ID);
                    dic.Add("Product_Code", item.product.Product_Code);
                    dic.Add("Original_Product_Code", item.product.Original_Product_Code);
                    dic.Add("Product_Category_Code", item.product.Product_Category_Code);
                    dic.Add("Product_Brand_Code", item.product.Product_Brand_Code);
                    dic.Add("Product_Name", item.product.Product_Name);
                    dic.Add("Product_Description", item.product.Product_Description);
                    dic.Add("Price", item.product.Price);
                    dic.Add("Display_YN", item.product.Display_YN);
                    dic.Add("Main_Image_URL", item.product.Main_Image_URL);
                    dic.Add("Preview_Image_URL", item.product.Preview_Image_URL);
                    dic.Add("Photo_YN", item.template.Photo_YN);
                    dic.Add("SetCard_URL", item.product.SetCard_URL);
                    dic.Add("SetCard_Display_YN", item.product.SetCard_Display_YN);

                    result.Add(dic);
                }
            }

            return result;
        }

        public List<Dictionary<string, object>> TB_Product_Category_Entity(int Product_ID)
        {
            var query = from product_category in Entity_db.Set<TB_Product_Category>().Where(x => x.Product_ID == Product_ID)
                        join category in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == "CTC02") on product_category.Category_ID equals category.Category_ID
                        join category_p in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == "CTC02") on category.Parent_Category_ID equals category_p.Category_ID into product_category_name
                        from p in product_category_name.DefaultIfEmpty()
                        select new { product_category, category, p };

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Product_ID", item.product_category.Product_ID);
                dic.Add("Category_ID", item.product_category.Category_ID);
                dic.Add("Product_Category_Name", (item.p != null ? item.p.Category_Name + " > " : "") + item.category.Category_Name);
                dic.Add("Sort", item.product_category.Sort);

                result.Add(dic);
            }
            return result;
        }

        public List<Dictionary<string, object>> TB_Main_Product_Category_Entity(int Product_ID)
        {

            var query = from category in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code == "CTC01" && x.Category_Step == 1)
                        join product_category in Entity_db.Set<TB_Product_Category>().Where(x => x.Product_ID == Product_ID) on category.Category_ID equals product_category.Category_ID into Category_ID
                        from p in Category_ID.DefaultIfEmpty()
                        select new { category, p };


            var list = new List<string>();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Chekced", item.p == null ? false : true);
                dic.Add("Category_ID", item.category.Category_ID);
                dic.Add("Category_Name", item.category.Category_Name);
                dic.Add("Sort", item.category.Sort);

                result.Add(dic);
            }
            return result;
        }

        public List<TB_Product_Image> TB_Product_Image_List_Entity(int Product_ID)
        {

            var query = from product_image in Entity_db.Set<TB_Product_Image>().Where(x => x.Product_ID == Product_ID).ToList()

                        select new { product_image };

            var list = new List<TB_Product_Image>();
            foreach (var item in query)
            {
                list.Add(new TB_Product_Image()
                {
                    Image_ID = item.product_image.Image_ID,
                    Product_ID = item.product_image.Product_ID,
                    Image_URL = item.product_image.Image_URL,
                    Image_Type_Code = item.product_image.Image_Type_Code
                });
            }

            return list;
        }

        public List<Dictionary<string, object>> TB_Product_Used_Image_LIst(int Product_ID)
        {
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID)
                        join product_image in Entity_db.Set<TB_Product_Image>() on product.Product_ID equals product_image.Product_ID
                        select new { product, product_image };

            var entity = Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID).FirstOrDefault();

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Resource_URL", item.product_image.Image_URL);

                result.Add(dic);
            }

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.Preview_Image_URL))
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Resource_URL", entity.Preview_Image_URL);
                    result.Add(dic);
                }
            }

            return result;
        }

        #endregion

        #region 상품 등록 관련 INSERT / UPDATE

        public int TB_Product_Update_Sql(TB_Product product)
        {
            var entity = Entity_db.Set<TB_Product>().Where(x => x.Product_ID == product.Product_ID).FirstOrDefault();

            entity.Product_ID = product.Product_ID;
            entity.Template_ID = product.Template_ID;
            entity.Product_Category_Code = product.Product_Category_Code;
            entity.Product_Code = product.Product_Code;
            entity.Product_Brand_Code = product.Product_Brand_Code;
            entity.Product_Name = product.Product_Name;
            entity.Product_Description = product.Product_Description;
            entity.Price = product.Price;
            entity.Display_YN = product.Display_YN;
            entity.SetCard_URL = product.SetCard_URL;
            entity.SetCard_Display_YN = product.SetCard_Display_YN;
            entity.Update_User_ID = product.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = product.Update_IP;

            Entity_db.TB_Products.Update(entity);
            Entity_db.SaveChanges();

            return entity.Product_ID;
        }

        public int Main_Image_Update_Sql(TB_Product product)
        {
            var entity = Entity_db.Set<TB_Product>().Where(x => x.Product_ID == product.Product_ID).FirstOrDefault();

            entity.Main_Image_URL = product.Main_Image_URL;
            entity.Update_User_ID = product.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = product.Update_IP;

            Entity_db.TB_Products.Update(entity);
            Entity_db.SaveChanges();

            return entity.Product_ID;
        }

        public int TB_Product_Insert_Sql(TB_Product product)
        {
            Entity_db.TB_Products.Add(product);
            Entity_db.SaveChanges();

            return product.Product_ID;
        }

        public string TB_Product_Category_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Category> product_categories)
        {

            var query = from product in Entity_db.Set<TB_Product_Category>().Where(x => x.Product_ID == product_id).ToList()
                        select new { product };

            var sort = (from product_category in Entity_db.Set<TB_Product_Category>()
                        join product in Entity_db.Set<TB_Product>() on product_category.Product_ID equals product.Product_ID
                        join category in Entity_db.Set<TB_Category>() on product_category.Category_ID equals category.Category_ID
                        select product_category.Sort).Max();

            sort = sort == null ? 0 : sort;

            //삭제
            foreach (var item in query)
            {
                bool result = true;

                foreach (var product_category in product_categories)
                {
                    if (product_category.Category_ID == item.product.Category_ID)
                        result = false;
                }

                if (result)
                {
                    Entity_db.Remove(item.product);
                    Entity_db.SaveChanges();
                }
            }

            //추가
            foreach (var product_category in product_categories)
            {
                sort = sort + 1;
                bool result = true;

                product_category.Product_ID = product_id;
                product_category.Sort = sort;

                foreach (var item in query)
                {
                    if (product_category.Category_ID == item.product.Category_ID)
                        result = false;
                }

                if (result)
                {
                    product_category.Regist_User_ID = user_id;
                    product_category.Regist_DateTime = DateTime.Now;
                    product_category.Regist_IP = user_ip;
                    product_category.Update_User_ID = user_id;
                    product_category.Update_DateTime = DateTime.Now;
                    product_category.Update_IP = user_ip;

                    Entity_db.Add(product_category);
                    Entity_db.SaveChanges();
                }
            }
            return "Aa";
        }

        public string TB_Product_Icon_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Icon> product_icons)
        {
            //전체삭제
            var query = Entity_db.Set<TB_Product_Icon>().Where(x => x.Product_ID == product_id).ToList();

            foreach (var item in query)
            {
                Entity_db.Remove(item);
                Entity_db.SaveChanges();
            }

            //추가
            foreach (var product_icon in product_icons)
            {
                product_icon.Product_ID = product_id;

                product_icon.Regist_User_ID = user_id;
                product_icon.Regist_DateTime = DateTime.Now;
                product_icon.Regist_IP = user_ip;
                product_icon.Update_User_ID = user_id;
                product_icon.Update_DateTime = DateTime.Now;
                product_icon.Update_IP = user_ip;

                Entity_db.Add(product_icon);
                Entity_db.SaveChanges();
            }

            return "Aa";
        }

        public string TB_Product_Image_Update_Sql(int product_id, string user_id, string user_ip, List<TB_Product_Image> product_Images)
        {

            var image_ids = product_Images.Select(x => x.Image_ID).ToList();

            //삭제 대상 조회
            var query = Entity_db.Set<TB_Product_Image>().Where(x => x.Product_ID == product_id && !image_ids.Contains(x.Image_ID)).ToList();

            foreach (var item in query)
            {
                Entity_db.TB_Product_Images.Remove(item);
                Entity_db.SaveChanges();
            }

            //추가
            foreach (var product_Image in product_Images)
            {
                if (product_Image.Image_ID == 0)
                {
                    product_Image.Product_ID = product_id;
                    product_Image.Regist_User_ID = user_id;
                    product_Image.Regist_DateTime = DateTime.Now;
                    product_Image.Regist_IP = user_ip;
                    product_Image.Update_User_ID = user_id;
                    product_Image.Update_DateTime = DateTime.Now;
                    product_Image.Update_IP = user_ip;
                    Entity_db.Add(product_Image);
                    Entity_db.SaveChanges();
                }
            }

            return "Aa";
        }

        #endregion


        /// <summary>
        ///  프런트 - 상세 - 상품 기본 정보 
        /// </summary>
        /// <param name="Product_Id"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Product_Detail_Entity(int Product_Id, string User_ID)
        {
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID.Equals(Product_Id))
                        join template in Entity_db.Set<TB_Template>()
                            on product.Template_ID equals template.Template_ID

                        join category_code in Entity_db.Set<TB_Common_Code>() on product.Product_Brand_Code equals category_code.Code
                        //join category_code2 in Entity_db.Set<TB_Common_Code>() on product.Product_Brand_Code equals category_code2.Code

                        select new
                        {
                            product,
                            template,
                            category_code
                        };
            int wishYn = 0;
            //해당 상품을 위시리스트에 담은 총 카운트 
            int Wish_Cnt = (from Wish in Entity_db.Set<TB_Wish_List>().Where(x => x.Product_ID.Equals(Product_Id)) select Wish).Count();
            //int Wish_Cnt =  (from Wish in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID == User_ID && x.Product_ID.Equals(Product_Id)) select Wish).Count();

            if (User_ID != null && User_ID.Length > 0)
            {
                wishYn = (from WishList in Entity_db.Set<TB_Wish_List>().Where(x => x.Product_ID.Equals(Product_Id) && x.User_ID.Equals(User_ID)) select WishList).Count();

            }

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("Product_Category_Code", item.product.Product_Category_Code); //PCC01 : M청첩장 / PCC02 ; M감사장 
                dic.Add("Product_Id", item.product.Product_ID);
                dic.Add("Brand_Code", item.category_code.Code_Name);
                //dic.Add("brandname", brandname);
                dic.Add("Product_Name", item.product.Product_Name);
                dic.Add("Product_Code", item.product.Product_Code);
                dic.Add("COUPON_EXCEPTION_PRODUCT_YN", COUPON_EXCEPTION_PRODUCT_YN(item.product.Product_Code));
                dic.Add("Price", item.product.Price.ToString());
                dic.Add("Preview_Image_URL", item.product.Preview_Image_URL);
                dic.Add("Main_Image_URL", item.product.Main_Image_URL);
                dic.Add("Display_Yn", item.product.Display_YN);
                dic.Add("Wish_Cnt", Wish_Cnt);
                dic.Add("Wish_Yn", wishYn > 0 ? "Y" : "N");
                dic.Add("SetCard_URL", item.product.SetCard_URL);
                dic.Add("SetCard_Mobile_URL", item.product.SetCard_Mobile_URL);
                dic.Add("SetCard_Display_YN", item.product.SetCard_Display_YN);
                result.Add(dic);
            }

            return result;
        }



        private int COUPON_EXCEPTION_PRODUCT_YN(string Product_Code)
        {
            int cnt = 0;

            string ProcStr = "DBO.SP_S_COUPON_EXCEPTION_PRODUCT";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> Category = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PRODUCT_CODE", Product_Code);

                cnt = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }


            return cnt;

        }




        /// <summary>
        /// 특정 상품의 총 위시리스트 개수 구하기 
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        public int Product_Detail_Total_Wish_Cnt(int Product_ID)
        {
            //해당 상품을 위시리스트에 담은 총 카운트 
            return (from Wish in Entity_db.Set<TB_Wish_List>().Where(x => x.Product_ID.Equals(Product_ID)) select Wish).Count();
        }

        /// <summary>
        ///  특정 회원당 특정 상품의 위시리스트 개수 구하기
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        public int Product_Detail_Member_Wish_Cnt(int Product_ID, string User_ID)
        {
            //특정 회원당 특정 상품의 위시리스트 개수 구하기
            return (from Wish in Entity_db.Set<TB_Wish_List>().Where(x => x.Product_ID.Equals(Product_ID) && x.User_ID.Equals(User_ID)) select Wish).Count();
        }

        public List<Dictionary<string, object>> User_Product_Icon_List_Entity()
        {
            var query = from product_icon in Entity_db.Set<TB_Product_Icon>()//.Where(x => x.Product_ID.Equals(Product_Id))
                        join ico in Entity_db.Set<TB_Icon>()
                            on product_icon.Icon_ID equals ico.Icon_ID

                        select new
                        {
                            product_icon,
                            ico
                        };

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();

                //dic.Add("Preview_Image_URL", item.template.Preview_Image_URL);
                dic.Add("Product_Id", item.product_icon.Product_ID);
                dic.Add("Icon_Url", item.ico.Icon_URL);
                result.Add(dic);

            }

            return result;
        }


        public List<TB_Product_Image> User_Product_Detail_Img(int Product_ID /*, string Image_Type_Code*/)
        {

            var query = from product in Entity_db.Set<TB_Product_Image>().Where(x => x.Product_ID.Equals(Product_ID))
                        select new
                        {
                            product
                        };

            var list = new List<TB_Product_Image>();
            foreach (var item in query)
            {
                list.Add(new TB_Product_Image()
                {
                    Image_URL = item.product.Image_URL,
                    Image_Type_Code = item.product.Image_Type_Code
                });
            }

            return list;

        }


        /// <summary>
        /// 프런트 - List 페이지 -> 각 카테고리에 매칭된 상품 리스트 (중복 제거)
        /// </summary>
        /// <param name="User_Id"></param>
        /// <param name="Category_Id"></param>
        /// <param name="Search_Gubun"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Product_List_Sql(string User_Id, int Category_Id, int Search_Gubun, string SearchCategoryList, string SearchBrandList)
        {
            string ProcStr = "DBO.SP_S_USER_PRODUCT_LIST_NEW";
            //string ProcStr = "DBO.SP_S_USER_PRODUCT_LIST_test";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> Category = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(SearchCategoryList))
                {
                    if (SearchCategoryList.IndexOf('_') > 0) //98_103_
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Category_id", typeof(int));
                        dt.Columns.Add("temp", typeof(int));
                        for (int i = 0; i < SearchCategoryList.Split('_').Length; i++)
                        {
                            if (!string.IsNullOrEmpty(SearchCategoryList.Split('_')[i])) //1_3
                            {
                                DataRow newRow = dt.Rows.Add();
                                newRow.SetField("Category_id", Convert.ToInt32(SearchCategoryList.Split('_')[i].ToString()));
                                newRow.SetField("temp", 0);
                            }


                        }
                        SqlParameter IdParam = new SqlParameter("@ID", SqlDbType.Structured);
                        IdParam.Value = dt;
                        cmd.Parameters.Add(IdParam);

                    }


                }


                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                cmd.Parameters.AddWithValue("@CATEGORY_ID", Category_Id);
                cmd.Parameters.AddWithValue("@SORT_GUBUN", Search_Gubun);
                if (!string.IsNullOrEmpty(SearchBrandList)) cmd.Parameters.AddWithValue("@SEARCHBRAND_CODE", SearchBrandList);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");

                string TempProduct_Id = "";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Update_Datetime = ds.Tables[0].Rows[i]["UPDATE_DATETIME"].ToString();
                    var Price = ds.Tables[0].Rows[i]["PRICE"].ToString();
                    var Code_name = ds.Tables[0].Rows[i]["CODE_NAME"].ToString();
                    var Product_Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    var Cate_Id = ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                    var Product_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString();
                    var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
                    var Wishcnt = ds.Tables[0].Rows[i]["WISHCNT"].ToString();
                    var Totalsale = ds.Tables[0].Rows[i]["TOtaLSALE"].ToString();

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Update_Datetime", Update_Datetime);
                    dic.Add("Price", Price);
                    dic.Add("Code_name", Code_name);
                    dic.Add("Product_Brand_Code", Product_Brand_Code);
                    dic.Add("Cate_Id", Cate_Id);
                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Main_Image_Url", Main_Image_Url);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Wishcnt", Wishcnt);
                    dic.Add("Totalsale", Totalsale);

                    result.Add(dic);

                    //if (!string.IsNullOrEmpty(SearchCategoryList) && SearchCategoryList.Contains(ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString()))
                    //{
                    //    var Update_Datetime = ds.Tables[0].Rows[i]["UPDATE_DATETIME"].ToString();
                    //    var Price = ds.Tables[0].Rows[i]["PRICE"].ToString();
                    //    var Code_name = ds.Tables[0].Rows[i]["CODE_NAME"].ToString();
                    //    var Product_Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    //    var Cate_Id = ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                    //    var Product_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString();
                    //    var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    //    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    //    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
                    //    var Wishcnt = ds.Tables[0].Rows[i]["WISHCNT"].ToString();
                    //    var Totalsale = ds.Tables[0].Rows[i]["TOtaLSALE"].ToString();

                    //    Dictionary<string, object> dic = new Dictionary<string, object>();
                    //    dic.Add("Update_Datetime", Update_Datetime);
                    //    dic.Add("Price", Price);
                    //    dic.Add("Code_name", Code_name);
                    //    dic.Add("Product_Brand_Code", Product_Brand_Code);
                    //    dic.Add("Cate_Id", Cate_Id);
                    //    dic.Add("Product_Id", Product_Id);
                    //    dic.Add("Main_Image_Url", Main_Image_Url);
                    //    dic.Add("Product_Name", Product_Name);
                    //    dic.Add("Product_Code", Product_Code);
                    //    dic.Add("Wishcnt", Wishcnt);
                    //    dic.Add("Totalsale", Totalsale);

                    //   result.Add(dic);
                    //}
                    //else if (string.IsNullOrEmpty(SearchCategoryList)) //전체검색
                    //{
                    //    //if(!TempProduct_Id.Equals(ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString()))
                    //    //{
                    //       // TempProduct_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString();

                    //        var Update_Datetime = ds.Tables[0].Rows[i]["UPDATE_DATETIME"].ToString();
                    //        var Price = ds.Tables[0].Rows[i]["PRICE"].ToString();
                    //        var Code_name = ds.Tables[0].Rows[i]["CODE_NAME"].ToString();
                    //        var Product_Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    //        var Cate_Id = ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                    //        var Product_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString();
                    //        var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    //        var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    //        var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
                    //        var Wishcnt = ds.Tables[0].Rows[i]["WISHCNT"].ToString();
                    //        var Totalsale = ds.Tables[0].Rows[i]["TOtaLSALE"].ToString();

                    //        Dictionary<string, object> dic = new Dictionary<string, object>();
                    //        dic.Add("Update_Datetime", Update_Datetime);
                    //        dic.Add("Price", Price);
                    //        dic.Add("Code_name", Code_name);
                    //        dic.Add("Product_Brand_Code", Product_Brand_Code);
                    //        dic.Add("Cate_Id", Cate_Id);
                    //        dic.Add("Product_Id", Product_Id);
                    //        dic.Add("Main_Image_Url", Main_Image_Url);
                    //        dic.Add("Product_Name", Product_Name);
                    //        dic.Add("Product_Code", Product_Code);
                    //        dic.Add("Wishcnt", Wishcnt);
                    //        dic.Add("Totalsale", Totalsale);
                    //        result.Add(dic);
                    //    //}

                    //}


                }
                //_Transation.Commit(); 
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return result;
        }



        public List<Dictionary<string, object>> Product_MainImage_List_Entity()
        {
            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_Category_Code.Equals("PCC01") && x.Display_YN.Equals("Y"))
                        join common_code in Entity_db.Set<TB_Common_Code>().Where(x => x.Code_Group.Equals("Product_Category_code"))
                        on product.Product_Category_Code equals common_code.Code

                        select new
                        {
                            product
                        };

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Product_Id", item.product.Product_ID);
                dic.Add("Main_Image_URL", item.product.Main_Image_URL);
                result.Add(dic);
            }


            return result;
        }

        /// <summary>
        /// 프런트 - 상품리스트 -> 클릭한 상품의 기본 정보 
        /// </summary>
        /// <param name="Product_Id"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Product_PreViewImg_Entity(int Product_Id)
        {
            // string PreView_Url = "";

            var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID.Equals(Product_Id)).AsNoTracking()
                        join template in Entity_db.Set<TB_Template>().AsNoTracking()
                        on product.Template_ID equals template.Template_ID
                        join category_code in Entity_db.Set<TB_Common_Code>().AsNoTracking() on product.Product_Brand_Code equals category_code.Code
                        select new
                        {
                            product,
                            template,
                            category_code
                        };
            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            string PreView_Url = "";

            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                //PreView_Url = !string.IsNullOrEmpty(item.product.Main_Image_URL) ? item.product.Main_Image_URL : item.product.Preview_Image_URL;
                PreView_Url = !string.IsNullOrEmpty(item.product.Preview_Image_URL) ? item.product.Preview_Image_URL : "";
                //dic.Add("Preview_Image_URL", item.template.Preview_Image_URL);
                dic.Add("Product_Id", item.product.Product_ID);
                dic.Add("PreView_Url", PreView_Url);
                dic.Add("Price", item.product.Price);
                dic.Add("Product_Name", StringHelper.CutLength(item.product.Product_Name, 19, "..."));
                dic.Add("Brand_Name", item.category_code.Code_Name);

                result.Add(dic);
            }


            return result;

        }



        /// <summary>
        /// 프런트 - 상품 검색
        /// </summary>
        /// <param name="SearchKeyword">상품코드 OR 상품명</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Search_ProductList_Sql(string User_Id, string SearchKeyword, int Search_Gubun)
        {
            string ProcStr = "DBO.SP_S_USER_PRODUCT_SEARCH";

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();
            List<TB_Category> Category = new List<TB_Category>();
            //SqlTransaction _Transation = null;

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = ProcStr;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                cmd.Parameters.AddWithValue("@SEARCH_KEYWORD", SearchKeyword);

                cmd.Parameters.AddWithValue("@SORT_GUBUN", Search_Gubun);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "TB_Category");


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var Update_Datetime = ds.Tables[0].Rows[i]["UPDATE_DATETIME"].ToString();
                    var Code_name = ds.Tables[0].Rows[i]["CODE_NAME"].ToString();
                    var Product_Brand_Code = ds.Tables[0].Rows[i]["PRODUCT_BRAND_CODE"].ToString();
                    // var Cate_Id = ds.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                    var Product_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"].ToString();
                    var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"].ToString();
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"].ToString();
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"].ToString();
                    var Wishcnt = ds.Tables[0].Rows[i]["WISHCNT"].ToString();
                    var Totalsale = ds.Tables[0].Rows[i]["TOtaLSALE"].ToString();
                    var Price = DateTimeHelper.Int_Format(ds.Tables[0].Rows[i]["Price"].ToString());

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Update_Datetime", Update_Datetime);
                    dic.Add("Price", Price);
                    dic.Add("Code_name", Code_name);
                    dic.Add("Product_Brand_Code", Product_Brand_Code);

                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Main_Image_Url", Main_Image_Url);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Wishcnt", Wishcnt);
                    dic.Add("Totalsale", Totalsale);
                    result.Add(dic);


                }
                //_Transation.Commit(); 
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return result;


        }



        public string Get_Product_PreView_Url(int Product_ID)
        {
            string imgUrl = "";

            // int Wish_Cnt = (from Wish in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID == User_ID && x.Product_ID.Equals(Product_Id)) select Wish).Count();

            string Preview_Image_URL = (from Product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID && x.Product_ID.Equals(Product_ID)) select Product.Preview_Image_URL).Single();
            Entity_db.Dispose();
            //var query = from product in Entity_db.Set<TB_Product>().Where(x => x.Product_ID == Product_ID).ToList()
            //            select new { product };

            return Preview_Image_URL;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gubun">1 - 주문번호 / 2- 초대장번호 </param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int User_Order_Chk_Entity(string gubun, TB_Order model)
        {
            int Order_Cnt = 0;

            if (gubun.Equals("1"))
            {
                Order_Cnt = (from Order in Entity_db.Set<TB_Order>().Where(x => x.Order_ID.Equals(model.Order_ID) &&
                                        (
                                            (x.User_ID.Equals(model.User_ID) /*&& x.Name.Equals(model.Name)*/) ||
                                            (x.Name.Equals(model.Name) && x.Email.Equals(model.Email))
                                        ))
                             select Order).Count();
            }
            else
            {
                int Order_ID = (from Invitation in Entity_db.Set<TB_Invitation>().Where(x => x.Invitation_ID.Equals(model.Previous_Order_ID)) select Invitation.Order_ID).FirstOrDefault();

                Order_Cnt = (from Order in Entity_db.Set<TB_Order>().Where(x => x.Order_ID.Equals(Order_ID) &&
                                      (
                                          (x.User_ID.Equals(model.User_ID) /*&& x.Name.Equals(model.Name)*/) ||
                                          (x.Name.Equals(model.Name) && x.Email.Equals(model.Email))
                                      ))
                             select Order).Count();
            }

            return Order_Cnt;

        }



















    }

}
