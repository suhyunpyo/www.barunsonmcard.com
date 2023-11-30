using Microsoft.EntityFrameworkCore;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MobileInvitation.Data.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly barunsonContext Entity_db;

        public OrderRepository(barunsonContext ctx)
        {
            this.Entity_db = ctx;
        }
                
        public TB_Order TB_Order_EntityFromInvitation_ID(int Invitation_ID)
        {
            var entity = from o in Entity_db.TB_Orders
                         join i in Entity_db.TB_Invitations on o.Order_ID equals i.Order_ID
                         where i.Invitation_ID == Invitation_ID
                         select o;
            return entity.FirstOrDefault();
        }
        public int TB_Order_Insert_Sql(TB_Order order)
        {
            try
            {
                order.Order_Code = Get_Order_Code();
                order.PG_ID = order.Order_Code;

                Entity_db.TB_Orders.Add(order);
                Entity_db.SaveChanges();
            }
            catch (Exception ex) {
                if (ex.InnerException != null && ex.InnerException is Microsoft.Data.SqlClient.SqlException)
                {
                    var sqlError = (Microsoft.Data.SqlClient.SqlException)ex.InnerException;
                    if(sqlError.Number == 2601)
                    {
                        TB_Order_Insert_Sql(order);
                    }
                }
            }

            return order.Order_ID;
        }

        public int TB_Order_Update_Sql(TB_Order order)
        {
            var entity = Entity_db.Set<TB_Order>().Where(x => x.Order_ID == order.Order_ID).FirstOrDefault();

            //로그인 사용자 검사, 관리자 로그인시 문제
            //주문이 없거나, 기존 주문이 비회원이 아니고 수정하는 회원정보와 ID가 일치 하지 않을 경우 예외 발생.
            if (entity == null || (!string.IsNullOrEmpty(entity.User_ID) && entity.User_ID != order.User_ID))
                throw new Exception("주문정보가 없거나 인증정보가 잘못되었습니다.");

            entity.User_ID = order.User_ID;
            entity.Name = order.Name;
            entity.CellPhone_Number = order.CellPhone_Number;
            entity.Email = order.Email;
            entity.Order_Status_Code = "OSC01";

            entity.Update_User_ID = order.Update_User_ID;
            entity.Update_DateTime = DateTime.Now;
            entity.Update_IP = order.Update_IP;

            Entity_db.TB_Orders.Update(entity);
            Entity_db.SaveChanges();

            return order.Order_ID;
        }

        /// <summary>
        /// 일단 삭제보다는 Order_Status_Code값을 주문 취소로 변경
        /// </summary>
        /// <param name="Order_Id"></param>
        public void User_Order_Del(int Order_Id)
        {
            var Update_Order = Entity_db.TB_Orders.Where(n => n.Order_ID.Equals(Order_Id)).SingleOrDefault();

            Update_Order.Order_Status_Code = "OSC02"; // 주문취소

            Entity_db.Entry(Update_Order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Entity_db.SaveChanges();

            User_Invitation_URL_UpDate_Sql(Order_Id);

        }


        /// <summary>
        /// 제작중인 주문건 삭제시 모바일 초대장 URL 초기화 
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <param name="Phone"></param>
        public string User_Invitation_URL_UpDate_Sql(int Order_Id)
        {
            string ReturnValue = "F";
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = _connectionString;
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_INVITATION_URL_UPDATE";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter OrderParam = new SqlParameter("@ORDER_ID", SqlDbType.Int);
                OrderParam.Value = Order_Id;
                cmd.Parameters.Add(OrderParam);

                cmd.ExecuteNonQuery();

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

            return ReturnValue;


        }

        public int TB_Order_Product_Insert_Sql(TB_Order_Product order_product)
        {
            Entity_db.TB_Order_Products.Add(order_product);
            Entity_db.SaveChanges();

            return order_product.Order_ID;
        }

        public string Get_Order_Code()
        {
            string result = string.Empty;

            string match_key = "M" + DateTime.Now.ToString("yyMMdd");

            var entity = Entity_db.Set<TB_Order>().Where(x => x.Order_Code.Contains(match_key)).OrderByDescending(x => x.Order_Code).FirstOrDefault();


            int num = 1;

            if (entity != null)
            {
                num = Int32.Parse(entity.Order_Code.Substring(7, 4)) + 1;
            }

            string c3 = string.Format("{0:D4}", num);

            string c2 = DateTime.Now.ToString("yyMMdd");

            result = "M" + c2 + c3;

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Order_Id"></param>
        /// <param name="Phone"></param>
        public string User_Invitation_Mms_Send_Sql(int Order_Id, string Phone)
        {
            string ReturnValue = "F";
            SqlConnection con = new SqlConnection();
            //con.ConnectionString = _connectionString;
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_MMS_SEND";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter OrderParam = new SqlParameter("@ORDER_ID", SqlDbType.Int);
                OrderParam.Value = Order_Id;
                cmd.Parameters.Add(OrderParam);

                SqlParameter Recv_Param = new SqlParameter("@RECV_PNUM", SqlDbType.VarChar, 20);
                Recv_Param.Value = Phone;
                cmd.Parameters.Add(Recv_Param);

                SqlParameter Send_Param = new SqlParameter("@SEND_PNUM", SqlDbType.VarChar, 20);
                Send_Param.Value = "16440708";
                cmd.Parameters.Add(Send_Param);


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds, "Order_Info");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ReturnValue = ds.Tables[0].Rows[i]["Result"].ToString();


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

            return ReturnValue;


        }

        /// <summary>
        ///  주문(결제) 완료시 SMS 발송 / 결제완료시 자동쿠폰 발급
        /// </summary>
        /// <param name="Order_Code"></param>
        /// <returns></returns>
        public string User_Order_Sms_Send_Sql(string Order_Code, string Order_Path)
        {
            string ReturnValue = "T";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_SEND_ORDER_BIZTALK";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter OrderParam = new SqlParameter("@ORDER_CDOE", SqlDbType.VarChar, 25);
                OrderParam.Value = Order_Code;
                cmd.Parameters.Add(OrderParam);

                SqlParameter PathParam = new SqlParameter("@ORDER_PATH", SqlDbType.NVarChar, 100);
                PathParam.Value = Order_Path;
                cmd.Parameters.Add(PathParam);


                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // _Transation.Rollback();
                ReturnValue = "F";
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return ReturnValue;


        }
        
    }
}
