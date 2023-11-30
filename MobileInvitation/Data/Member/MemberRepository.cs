using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace MobileInvitation.Data.Member
{
    public class MemberRepository : IMemberRepository
    {
        private readonly barunsonContext Entity_db;

        public MemberRepository(barunsonContext ctx)
        {
            this.Entity_db = ctx;

        }

        /// <summary>
        /// 1대1문의 - 주문완료 리스트 
        /// </summary>
        /// <param name="Order_Code"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Seach_MyOrderList_Sql(string Gubun, TB_Order model)
        {
            string ProcStr = "DBO.SP_S_USER_ORDER_LIST";

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

                cmd.Parameters.AddWithValue("@GUBUN", Gubun);
                cmd.Parameters.AddWithValue("@USER_ID", model.User_ID);
                cmd.Parameters.AddWithValue("@USER_NAME", model.Name);
                cmd.Parameters.AddWithValue("@USER_EMAIL", model.Email);
                cmd.Parameters.AddWithValue("@ORDER_CODE", "");

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Detail");



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    var Order_Id = ds.Tables[0].Rows[i]["ORDER_ID"];
                    var Order_Code = ds.Tables[0].Rows[i]["ORDER_CODE"];
                    var Product_Category = ds.Tables[0].Rows[i]["PRODUCT_CATEGORY"];
                    var Product_Code = ds.Tables[0].Rows[i]["PRODUCT_CODE"];
                    var Product_Name = ds.Tables[0].Rows[i]["PRODUCT_NAME"];
                    var Regist_Datetime = DateTimeHelper.HHmm(ds.Tables[0].Rows[i]["REGIST_DATETIME"].ToString());
                    var Order_Datetime = DateTimeHelper.HHmm(ds.Tables[0].Rows[i]["ORDER_DATETIME"].ToString());
                    var Order_Price = ds.Tables[0].Rows[i]["ORDER_PRICE"];
                    var Preview_Image_Url = ds.Tables[0].Rows[i]["PREVIEW_IMAGE_URL"];
                    var Main_Image_Url = ds.Tables[0].Rows[i]["MAIN_IMAGE_URL"];
                    var Product_Id = ds.Tables[0].Rows[i]["PRODUCT_ID"];
                    var Invitation_Id = ds.Tables[0].Rows[i]["INVITATION_ID"];
                    var Finance_Name = ds.Tables[0].Rows[i]["FINANCE_NAME"];
                    var Invitation_Display_Yn = !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["INVITATION_DISPLAY_YN"].ToString()) ? ds.Tables[0].Rows[i]["INVITATION_DISPLAY_YN"] : "Y";
                    var Payment_Status_Code = ds.Tables[0].Rows[i]["Payment_Status_Code"];
                    string WeddingYY = ds.Tables[0].Rows[i]["WeddingYY"].ToString();
                    string WeddingMM = ds.Tables[0].Rows[i]["WeddingMM"].ToString();
                    string WeddingDD = ds.Tables[0].Rows[i]["WeddingDD"].ToString();

                    string Payment_Method_Name = ds.Tables[0].Rows[i]["Payment_Method_Name"].ToString();
                    string Account_Number = ds.Tables[0].Rows[i]["Account_Number"].ToString();

                   
                    int Date_Result = 0;

                    if (!string.IsNullOrEmpty(WeddingYY) && !string.IsNullOrEmpty(WeddingMM) && !string.IsNullOrEmpty(WeddingDD))
                    {
                        DateTime WeddingDate = DateTime.Parse(WeddingYY + "-" + WeddingMM + "-" + WeddingDD);
                        WeddingDate = WeddingDate.AddMonths(3);

                        DateTime NowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        Date_Result = DateTime.Compare(WeddingDate, NowDate);
                    }
                    else
                    {
                        Date_Result = 1;
                    }


                    dic.Add("Order_Id", Order_Id);
                    dic.Add("Order_Code", Order_Code);
                    dic.Add("Product_Category", Product_Category);
                    dic.Add("Product_Code", Product_Code);
                    dic.Add("Product_Name", Product_Name);
                    dic.Add("Regist_Datetime", Regist_Datetime);
                    dic.Add("Order_Price", Order_Price);
                    dic.Add("Preview_Image_Url", Preview_Image_Url);
                    dic.Add("Main_Image_Url", Main_Image_Url);
                    dic.Add("Product_Id", Product_Id);
                    dic.Add("Invitation_Id", Invitation_Id);
                    dic.Add("Finance_Name", Finance_Name);
                    dic.Add("Invitation_Display_Yn", Invitation_Display_Yn);
                    dic.Add("Payment_Status_Code", Payment_Status_Code);
                    dic.Add("Payment_Method_Name", Payment_Method_Name);
                    dic.Add("Account_Number", Account_Number);
                    dic.Add("Order_Datetime", Order_Datetime);

                    if (Invitation_Display_Yn.Equals("N"))
                    {
                        dic.Add("Badge_Ment", "노출중지");  //고객이 비공개 설정한 상태
                        dic.Add("Badge_Status", "type03");
                    }
                    else if (Invitation_Display_Yn.Equals("Y")) // 모바일초대장 활성화 상태
                    {
                        if (Payment_Status_Code.Equals("PSC02")) //결제 완료 
                        {
                            if (Date_Result < 0)
                            {
                                // 기간만료 : 예식일로부터 3개월이 지난 상태
                                dic.Add("Badge_Ment", "기간만료");
                                dic.Add("Badge_Status", "type02");
                            }
                            else
                            {
                                dic.Add("Badge_Ment", "노출");
                                dic.Add("Badge_Status", "type01");
                            }



                        }
                        else if (Payment_Status_Code.Equals("PSC04")) //입금대기 
                        {
                            dic.Add("Badge_Ment", "결제대기");
                            dic.Add("Badge_Status", "type02");
                        }


                    }



                    result.Add(dic);

                }




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


    }
}
