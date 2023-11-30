using Microsoft.EntityFrameworkCore;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MobileInvitation.Data.Coupon
{
    public class CouponRepository : ICouponRepository
    {
        private barunsonContext Entity_db { get; set; }

        public CouponRepository(barunsonContext ctx)
        {
            this.Entity_db = ctx;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="User_Id"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Coupon_List_Entity_New(string User_Id, string Product_Code)
        {
            string ProcStr = "DBO.SP_S_USER_ORDER_COUPON_lIST";

            SqlConnection con = new SqlConnection();
            //con.ConnectionString = _connectionString;
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

                cmd.Parameters.AddWithValue("@USER_ID", User_Id);
                cmd.Parameters.AddWithValue("@PRODUCT_CODE", Product_Code);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Coupon");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("Coupon_ID", ds.Tables[0].Rows[i]["Coupon_ID"].ToString());
                    dic.Add("Coupon_Publish_ID", ds.Tables[0].Rows[i]["Coupon_Publish_ID"].ToString());
                    dic.Add("Coupon_Name", ds.Tables[0].Rows[i]["Coupon_Name"].ToString());
                    dic.Add("Description", ds.Tables[0].Rows[i]["Description"].ToString());
                    string Retrieve_DateTime = ds.Tables[0].Rows[i]["Retrieve_DateTime"].ToString();
                    dic.Add("Retrieve_DateTime", !string.IsNullOrEmpty(Retrieve_DateTime) ? Retrieve_DateTime : "");
                    dic.Add("Discount_Method_Code", ds.Tables[0].Rows[i]["Discount_Method_Code"].ToString());
                    dic.Add("Discount_Price", !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Discount_Price"].ToString()) ? ds.Tables[0].Rows[i]["Discount_Price"].ToString() : "");
                    dic.Add("Discount_Rate", !string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Discount_Rate"].ToString()) ? ds.Tables[0].Rows[i]["Discount_Rate"].ToString() : "");

                    switch (ds.Tables[0].Rows[i]["Discount_Method_Code"].ToString()) //할인방식
                    {
                        case "DMC01": //금액
                            dic.Add("Discount_View", String.Format("{0:#,##0}", int.Parse(ds.Tables[0].Rows[i]["Discount_Price"].ToString())) + "원");
                            break;
                        case "DMC02": //  % 
                            dic.Add("Discount_View", ds.Tables[0].Rows[i]["Discount_Rate"].ToString() + "%");
                            break;
                        case "DMC03":
                            dic.Add("Discount_View", "전액");
                            break;
                    }
                    switch (ds.Tables[0].Rows[i]["Period_Method_Code"].ToString()) //기간방식
                    {
                        case "PMC01": //기간입력

                            string Publish_Start_Date = ds.Tables[0].Rows[i]["Publish_Start_Date"].ToString().Split("-")[0] + "." + ds.Tables[0].Rows[i]["Publish_Start_Date"].ToString().Split("-")[1] + "." + ds.Tables[0].Rows[i]["Publish_Start_Date"].ToString().Split("-")[2];
                            string Publish_End_Date = ds.Tables[0].Rows[i]["Publish_End_Date"].ToString().Split("-")[0] + "." + ds.Tables[0].Rows[i]["Publish_End_Date"].ToString().Split("-")[1] + "." + ds.Tables[0].Rows[i]["Publish_End_Date"].ToString().Split("-")[2];

                            dic.Add("Date_Display_View", Publish_Start_Date + "~" + Publish_End_Date);
                            break;
                        case "PMC02": // 발행일로부터 X일


                            string Regist_DateTime = ((DateTime?)ds.Tables[0].Rows[i]["Regist_DateTime"])?.ToString("yyyy-MM-dd");
                            string Expiration_Date = ds.Tables[0].Rows[i]["Expiration_Date"].ToString();

                            dic.Add("Date_Display_View", Regist_DateTime + "~" + Expiration_Date);
                            break;
                        case "PMC03": //무제한
                            dic.Add("Date_Display_View", "사용기간 제한 없음 ");
                            break;
                    }


                    //유효일 체크 
                    dic.Add("Expiration_YN", string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Expiration_Date"].ToString()) ? "on" : DateTimeHelper.DateCheck2(Convert.ToDateTime(ds.Tables[0].Rows[i]["Expiration_Date"].ToString())));
                    dic.Add("Standard_Purchase_Price", ds.Tables[0].Rows[i]["Standard_Purchase_Price"].ToString());

                    //dic.Add("Discount_Method_Code", item.coupon.Discount_Method_Code); //할인방식 : 금액 - DMC01 / % - DMC02 / 전액할인 - DMC03
                    //dic.Add("Discount_Price", item.coupon.Discount_Price); //할인금액
                    //dic.Add("Discount_Rate", item.coupon.Discount_Rate);  //할인율

                    dic.Add("Coupon_Apply_Code", ds.Tables[0].Rows[i]["Coupon_Apply_Code"].ToString()); // CET01 상품전체 CET02 지정 상품 적용 CET03 지정 상품 제외
                    dic.Add("Coupon_Apply_Product_ID", ds.Tables[0].Rows[i]["Coupon_Apply_Product_ID"].ToString()); //
                    dic.Add("Coupon_Product_Yn", ds.Tables[0].Rows[i]["COUPON_PRODUCT_YN"].ToString()); //

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

        public int SetCouponProcess(TB_Serial_Coupon_Publish serial_coupon_publish)
        {
            int result = 0;

            string Current_Date = DateTime.Now.ToString("yyyy-MM-dd");

            //사용자가 매핑되지 않고, 사용하지 않고, 회수하지 않고,  만료일이 지나지 않은 쿠폰

            //엑셀업로드(CPTC03),난수번호(CPTC01)
            var query = from coupon in Entity_db.Set<TB_Serial_Coupon>().Where(x => !x.Coupon_Type_Code.Equals("CPTC02"))
                        join coupon_publish in Entity_db.Set<TB_Serial_Coupon_Publish>().Where(x => x.Coupon_Number.Equals(serial_coupon_publish.Coupon_Number) &&
                            string.IsNullOrEmpty(x.Retrieve_DateTime.ToString()) && x.Use_YN.Equals("N") && x.User_ID == null
                            ) on coupon.Coupon_ID equals coupon_publish.Coupon_ID
                        select new
                        {
                            coupon,
                            coupon_publish
                        };
            if (query.Count() > 0)
            {
                DateTime Expired_Date = Convert.ToDateTime(query.First().coupon_publish.Expiration_Date);


                bool _pass = query.First().coupon.Period_Method_Code == "PMC03" ? true : false; //무제한

                if (_pass || Expired_Date >= Convert.ToDateTime(Current_Date))
                {
                    //TB_Serial_Coupon_Publish 업데이트
                    var entity = Entity_db.Set<TB_Serial_Coupon_Publish>().Where(x => x.Coupon_Number == serial_coupon_publish.Coupon_Number).FirstOrDefault();
                    entity.Coupon_ID = query.First().coupon.Coupon_ID;
                    entity.User_ID = serial_coupon_publish.Regist_User_ID;
                    entity.Regist_DateTime = serial_coupon_publish.Regist_DateTime;
                    entity.Regist_IP = serial_coupon_publish.Regist_IP;
                    entity.Regist_User_ID = serial_coupon_publish.Regist_User_ID;
                    entity.Update_DateTime = serial_coupon_publish.Update_DateTime;
                    entity.Update_IP = serial_coupon_publish.Update_IP;
                    entity.Update_User_ID = serial_coupon_publish.Update_User_ID;
                    entity.Use_YN = "N";
                    Entity_db.TB_Serial_Coupon_Publishes.Update(entity);
                    Entity_db.SaveChanges();
                    return entity.Coupon_Publish_ID;
                }
            }
            else
            {
                //기등록한 동일쿠폰번호가 없는 경우
                var pre_data = Entity_db.Set<TB_Serial_Coupon_Publish>().Where(x => x.Coupon_Number.Equals(serial_coupon_publish.Coupon_Number) && x.User_ID.Equals(serial_coupon_publish.Regist_User_ID)).ToList();

                if (pre_data.Count() < 1)
                {
                    //동일번호(CPTC02)
                    var query2 = from coupon in Entity_db.Set<TB_Serial_Coupon>().Where(x => x.Serial_Coupon_Number.Equals(serial_coupon_publish.Coupon_Number) && x.Coupon_Type_Code.Equals("CPTC02"))
                                 select new
                                 {
                                     coupon
                                 };

                    if (query2.Count() > 0)
                    {

                        if ("PMC01".Equals(query2.First().coupon.Period_Method_Code)) // 날짜 지정 
                        {
                            DateTime Expired_Date = Convert.ToDateTime(query2.First().coupon.Publish_End_Date);

                            if (Expired_Date >= Convert.ToDateTime(Current_Date))
                            {
                                TB_Serial_Coupon_Publish entity = new TB_Serial_Coupon_Publish();
                                entity.Coupon_ID = query2.First().coupon.Coupon_ID;
                                entity.Coupon_Number = serial_coupon_publish.Coupon_Number;
                                entity.User_ID = serial_coupon_publish.Regist_User_ID;
                                entity.Expiration_Date = query2.First().coupon.Publish_End_Date;
                                entity.Regist_DateTime = serial_coupon_publish.Regist_DateTime;
                                entity.Regist_IP = serial_coupon_publish.Regist_IP;
                                entity.Regist_User_ID = serial_coupon_publish.Regist_User_ID;
                                entity.Update_DateTime = serial_coupon_publish.Update_DateTime;
                                entity.Update_IP = serial_coupon_publish.Update_IP;
                                entity.Update_User_ID = serial_coupon_publish.Update_User_ID;
                                entity.Use_YN = "N";
                                Entity_db.TB_Serial_Coupon_Publishes.Add(entity);
                                Entity_db.SaveChanges();
                                return entity.Coupon_Publish_ID;
                            }
                        }
                        else if ("PMC02".Equals(query2.First().coupon.Period_Method_Code)) //발행일로부터
                        {
                            TB_Common_Code code = Entity_db.TB_Common_Codes.Where(s => s.Code_Group == "Publish_Period_Code" && s.Code == query2.First().coupon.Publish_Period_Code).FirstOrDefault();

                            DateTime Expired_Date = Convert.ToDateTime(Convert.ToDateTime(query2.First().coupon.Publish_Start_Date).AddDays(Convert.ToInt32(code.Code_Name)).ToString().Substring(0, 10));

                            if (Expired_Date >= Convert.ToDateTime(Current_Date))
                            {
                                TB_Serial_Coupon_Publish entity = new TB_Serial_Coupon_Publish();
                                entity.Coupon_ID = query2.First().coupon.Coupon_ID;
                                entity.Coupon_Number = serial_coupon_publish.Coupon_Number;
                                entity.User_ID = serial_coupon_publish.Regist_User_ID;
                                entity.Expiration_Date = Expired_Date.ToString("yyyy-MM-dd");
                                entity.Regist_DateTime = serial_coupon_publish.Regist_DateTime;
                                entity.Regist_IP = serial_coupon_publish.Regist_IP;
                                entity.Regist_User_ID = serial_coupon_publish.Regist_User_ID;
                                entity.Update_DateTime = serial_coupon_publish.Update_DateTime;
                                entity.Update_IP = serial_coupon_publish.Update_IP;
                                entity.Update_User_ID = serial_coupon_publish.Update_User_ID;
                                entity.Use_YN = "N";
                                Entity_db.TB_Serial_Coupon_Publishes.Add(entity);
                                Entity_db.SaveChanges();
                                return entity.Coupon_Publish_ID;
                            }
                        }
                        else  //무제한
                        {
                            TB_Serial_Coupon_Publish entity = new TB_Serial_Coupon_Publish();
                            entity.Coupon_ID = query2.First().coupon.Coupon_ID;
                            entity.Coupon_Number = serial_coupon_publish.Coupon_Number;
                            entity.User_ID = serial_coupon_publish.Regist_User_ID;
                            entity.Regist_DateTime = serial_coupon_publish.Regist_DateTime;
                            entity.Regist_IP = serial_coupon_publish.Regist_IP;
                            entity.Regist_User_ID = serial_coupon_publish.Regist_User_ID;
                            entity.Update_DateTime = serial_coupon_publish.Update_DateTime;
                            entity.Update_IP = serial_coupon_publish.Update_IP;
                            entity.Update_User_ID = serial_coupon_publish.Update_User_ID;
                            entity.Use_YN = "N";
                            Entity_db.TB_Serial_Coupon_Publishes.Add(entity);
                            Entity_db.SaveChanges();
                            return entity.Coupon_Publish_ID;
                        }
                    }
                }
            }

            return result;

        }
    }
}
