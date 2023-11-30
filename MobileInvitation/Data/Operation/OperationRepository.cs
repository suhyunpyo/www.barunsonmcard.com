//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MobileInvitation.Data.Operation
{
    public class OperationRepository : IOperationRepository
    {
        private barunsonContext Entity_db;

        public OperationRepository(barunsonContext _Entity_db/*string connectionString*/)
        {
            Entity_db = _Entity_db;
        }

        public List<Dictionary<string, object>> Admin_Banner_Add_List_Entity2(string Search_Banner_Category_Name, HttpRequest request)
        {
            var query = from banner in Entity_db.Set<TB_Banner>()
                        join banner_category in Entity_db.Set<TB_Banner_Category>().Where(x => x.Banner_Category_Name.Equals(Search_Banner_Category_Name))
                        on banner.Banner_Category_ID equals banner_category.Banner_Category_ID
                        
                        join Item in Entity_db.Set<TB_Banner_Item>()//.Where(x => x.Banner_ID.Equals(Banner_Id) && x.Banner_Type_Code.Equals(string.IsNullOrEmpty(BannerGubun) ? x.Banner_Type_Code : BannerGubun))
                        on banner.Banner_ID equals Item.Banner_ID
                        //join Item in Entity_db.Set<TB_Banner_Item>().Where(x => x.Banner_Type_Code.Equals(BannerGubun)) on banner.Banner_ID equals Item.Banner_ID
                        select new
                        {
                            banner,
                            banner_category,
                            Item
                        };

            query = query.OrderBy(x => x.Item.Sort);

            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (var item in query)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                string Banner_Type_Code = "";

                if (Search_Banner_Category_Name.Equals("main banner 4"))
                {
                    if (MobileInvitation.FunctionHelper.UrlHelper.IsMobile(request))
                    {
                        Banner_Type_Code = "BTC02";
                    }
                    else
                    {
                        Banner_Type_Code = "BTC01";
                    }
                }
               
                if(!string.IsNullOrEmpty(Banner_Type_Code))
                {
                    if(Banner_Type_Code.Equals(item.Item.Banner_Type_Code))
                    {
                        dic.Add("Banner_ID", item.banner.Banner_ID);
                        dic.Add("Banner_Category_ID", item.banner.Banner_Category_ID);
                        dic.Add("Banner_Item_ID", item.Item.Banner_Item_ID);
                        dic.Add("Banner_Type_Code", item.Item.Banner_Type_Code);
                        dic.Add("Banner_PC_YN", item.banner.Banner_PC_YN);
                        dic.Add("Banner_Mobile_YN", item.banner.Banner_Mobile_YN);
                        dic.Add("Sort", item.Item.Sort);
                        dic.Add("Image_URL", item.Item.Image_URL);
                        dic.Add("Image_URL2", !string.IsNullOrEmpty(item.Item.Image_URL2) ? item.Item.Image_URL2 : "");
                        dic.Add("Link_URL", item.Item.Link_URL);
                        dic.Add("Deadline_Type_Code", item.Item.Deadline_Type_Code);
                        dic.Add("Regist_Date", item.Item.Regist_DateTime);
                        dic.Add("Start_Date", item.Item.Start_Date);
                        dic.Add("Start_Time", item.Item.Start_Time);
                        dic.Add("End_Date", item.Item.End_Date);
                        dic.Add("End_Time", item.Item.End_Time);
                        string NewPage_YN = string.Equals(item.Item.NewPage_YN.ToString(), "Y") ? "_Blank" : "_self";
                        dic.Add("NewPage_YN", NewPage_YN);
                        int? Click_Count = string.IsNullOrEmpty(item.Item.Click_Count.ToString()) ? 0 : item.Item.Click_Count;
                        dic.Add("Click_Count", Click_Count);
                        dic.Add("Banner_Main_Description", item.Item.Banner_Main_Description);
                        dic.Add("Banner_Add_Description", item.Item.Banner_Add_Description);

                        if (!string.Equals(item.Item.Deadline_Type_Code, "PTC02"))
                        {
                            //경과일수 : 시작일 - 종료일?? OR  @( ((DateTime.Now - m.RegDate).Days + 1).ToStringCurrency() 
                            //DateTime StartDate = Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00");
                            //DateTime EndDate = Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00");
                            //TimeSpan dateDiff = StartDate - EndDate;

                            ////int diffDay = dateDiff.Days;
                            ////int diffHour = dateDiff.Hours;
                            //int diffMinute = dateDiff.Minutes;
                            ////int diffSecond = dateDiff.Seconds;

                            //string Elapsed_Time = ((StartDate - EndDate).Days + 1).ToString();// Convert.ToInt32(diffMinute);
                            //dic.Add("Elapsed_Time", Elapsed_Time);
                            dic.Add("Elapsed_Time", DateTimeHelper.DateCheck(Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00"), Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00"),
                                "1"));
                            dic.Add("Status", DateTimeHelper.DateCheck(Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00"), Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00"),
                             "2"));

                            ////상태
                            //string NowDate = DateTime.Now.ToString("yyyy-MM-dd  HH:00:00");
                            //int start = DateTime.Compare(StartDate, Convert.ToDateTime(NowDate));
                            //int end = DateTime.Compare(EndDate, Convert.ToDateTime(NowDate));

                            //if (start <= 0)
                            //{
                            //    if (end > 0) dic.Add("Status", "진행");
                            //    else if (end <= 0) dic.Add("Status", "종료");
                            //}
                            //else if (start > 0)
                            //{
                            //    dic.Add("Status", "예약");

                            //}
                        }
                        else // 무제한
                        {
                            dic.Add("Elapsed_Time", "무제한");
                            dic.Add("Status", "진행");
                        }
                        result.Add(dic);
                    }
                }
                else
                {
                    dic.Add("Banner_ID", item.banner.Banner_ID);
                    dic.Add("Banner_Category_ID", item.banner.Banner_Category_ID);
                    dic.Add("Banner_Item_ID", item.Item.Banner_Item_ID);
                    dic.Add("Banner_Type_Code", item.Item.Banner_Type_Code);
                    dic.Add("Banner_PC_YN", item.banner.Banner_PC_YN);
                    dic.Add("Banner_Mobile_YN", item.banner.Banner_Mobile_YN);
                    dic.Add("Sort", item.Item.Sort);
                    dic.Add("Image_URL", item.Item.Image_URL);
                    dic.Add("Image_URL2", !string.IsNullOrEmpty(item.Item.Image_URL2) ? item.Item.Image_URL2 : "");
                    dic.Add("Link_URL", item.Item.Link_URL);
                    dic.Add("Deadline_Type_Code", item.Item.Deadline_Type_Code);
                    dic.Add("Regist_Date", item.Item.Regist_DateTime);
                    dic.Add("Start_Date", item.Item.Start_Date);
                    dic.Add("Start_Time", item.Item.Start_Time);
                    dic.Add("End_Date", item.Item.End_Date);
                    dic.Add("End_Time", item.Item.End_Time);
                    string NewPage_YN = string.Equals(item.Item.NewPage_YN.ToString(), "Y") ? "_Blank" : "_self";
                    dic.Add("NewPage_YN", NewPage_YN);
                    int? Click_Count = string.IsNullOrEmpty(item.Item.Click_Count.ToString()) ? 0 : item.Item.Click_Count;
                    dic.Add("Click_Count", Click_Count);
                    dic.Add("Banner_Main_Description", item.Item.Banner_Main_Description);
                    dic.Add("Banner_Add_Description", item.Item.Banner_Add_Description);

                    if (!string.Equals(item.Item.Deadline_Type_Code, "PTC02"))
                    {
                        //경과일수 : 시작일 - 종료일?? OR  @( ((DateTime.Now - m.RegDate).Days + 1).ToStringCurrency() 
                        //DateTime StartDate = Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00");
                        //DateTime EndDate = Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00");
                        //TimeSpan dateDiff = StartDate - EndDate;

                        ////int diffDay = dateDiff.Days;
                        ////int diffHour = dateDiff.Hours;
                        //int diffMinute = dateDiff.Minutes;
                        ////int diffSecond = dateDiff.Seconds;

                        //string Elapsed_Time = ((StartDate - EndDate).Days + 1).ToString();// Convert.ToInt32(diffMinute);
                        //dic.Add("Elapsed_Time", Elapsed_Time);
                        dic.Add("Elapsed_Time", DateTimeHelper.DateCheck(Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00"), Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00"),
                            "1"));
                        dic.Add("Status", DateTimeHelper.DateCheck(Convert.ToDateTime(item.Item.Start_Date + " " + item.Item.Start_Time + ":00:00"), Convert.ToDateTime(item.Item.End_Date + " " + item.Item.End_Time + ":00:00"),
                         "2"));

                        ////상태
                        //string NowDate = DateTime.Now.ToString("yyyy-MM-dd  HH:00:00");
                        //int start = DateTime.Compare(StartDate, Convert.ToDateTime(NowDate));
                        //int end = DateTime.Compare(EndDate, Convert.ToDateTime(NowDate));

                        //if (start <= 0)
                        //{
                        //    if (end > 0) dic.Add("Status", "진행");
                        //    else if (end <= 0) dic.Add("Status", "종료");
                        //}
                        //else if (start > 0)
                        //{
                        //    dic.Add("Status", "예약");

                        //}
                    }
                    else // 무제한
                    {
                        dic.Add("Elapsed_Time", "무제한");
                        dic.Add("Status", "진행");
                    }
                    result.Add(dic);
                }

                //result.Add(dic);
            }

            //  Entity_db.Dispose();
            return result;
        }

        /// <summary>
        /// 프런트 - 마이페이지 - Qna 
        /// </summary>
        /// <param name="User_Id"></param>
        /// <returns></returns>
        public List<VW_User_QNA> User_Qna_List_Entity(string User_Id)
        {

            var query = (
                            from user_qna in Entity_db.Set<VW_User_QNA>().Where(x => x.USERID.Equals(User_Id)
                                                                                && x.STAT !="S4" && x.STAT != "S5")
                            select new { user_qna }
                        );
            query = query.OrderByDescending(e => e.user_qna.REGIST_DATETIME);

            //var union_query = query.Union(query2).ToList();

            var list = new List<VW_User_QNA>();

            foreach (var item in query)
            {
                list.Add(new VW_User_QNA()
                {
                    QNA_ID = item.user_qna.QNA_ID,
                    USERID = item.user_qna.USERID,
                    NAME = item.user_qna.NAME,
                    ORDER_ID = item.user_qna.ORDER_ID,
                    TITLE = item.user_qna.TITLE,
                    CONTENT = item.user_qna.CONTENT,
                    REGIST_DATETIME = item.user_qna.REGIST_DATETIME,
                    ANSWER_CONTENT = item.user_qna.ANSWER_CONTENT,
                    ANSWER_DATETIME = item.user_qna.ANSWER_DATETIME,
                    STAT = item.user_qna.STAT,
                    ADMIN_UPFILE1 = item.user_qna.ADMIN_UPFILE1,
                    UPFILE_1 = item.user_qna.UPFILE_1,
                    UPFILE_2 = item.user_qna.UPFILE_2

                }); ;
            }

            return list;

        }


        /// <summary>
        /// 프런트 - 마이페이지 - Qna 뷰
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserQNAVIewModel User_Qna_Detail_Entity(int Id)
        {
            UserQNAVIewModel result = null;
            var query = (
                           from user_qna in Entity_db.Set<VW_User_QNA>().Where(x => x.QNA_ID.Equals(Id))
                           select new { user_qna }
                       );
            var item = query.FirstOrDefault();

            if (item != null)
            {
                result = new UserQNAVIewModel()
                {
                    QNA_ID = item.user_qna.QNA_ID,
                    NAME = item.user_qna.NAME,
                    TITLE = item.user_qna.TITLE,
                    Q_KIND = item.user_qna.Q_KIND,
                    ADMIN_NAME = item.user_qna.ADMIN_NAME,
                    ANSWER_CONTENT = item.user_qna.ANSWER_CONTENT,
                    ADMIN_UPFILE1 = item.user_qna.ADMIN_UPFILE1,
                    CONTENT = item.user_qna.CONTENT,
                    REGIST_DATETIME = item.user_qna.REGIST_DATETIME,
                    ORDER_ID = item.user_qna.ORDER_ID,
                    ANSWER_DATETIME = item.user_qna.ANSWER_DATETIME,
                    UPFILE_1 = item.user_qna.UPFILE_1,
                    UPFILE_2 = item.user_qna.UPFILE_2
                };

                string Order_Code = "";
                if (item.user_qna.ORDER_ID > 0 && !string.IsNullOrEmpty(item.user_qna.ORDER_ID.ToString()))
                {
                    TB_Order order = Entity_db.TB_Orders.Where(x => x.Order_Code.Equals("M" + item.user_qna.ORDER_ID) && x.User_ID.Equals(item.user_qna.USERID)).FirstOrDefault();
                    if (order != null)
                    {
                        Order_Code = order.Order_Code;
                        result.ORDER_ID = order.Order_ID;
                    }
                }
                else
                {
                    result.ORDER_ID = 0;
                }

                result.ORDER_CODE = Order_Code;
            }
            else
            {
                result = new UserQNAVIewModel();
            }
            return result;
        }

        public string User_Qna_FileUpladName_Entity(int Id, int num)
        {
            string FileName = "";

            var query = (
                          from user_qna in Entity_db.Set<VW_User_QNA>().Where(x => x.QNA_ID.Equals(Id))
                          select new { user_qna }
                      );
            foreach (var item in query)
            {
                if (num == 1)
                {
                    FileName = item.user_qna.UPFILE_1;
                }
                else if (num == 2)
                {
					FileName = item.user_qna.UPFILE_2;
				}
                else if (num == 3)
                {
                    FileName = item.user_qna.ADMIN_UPFILE1;
                }
            }

            return FileName;
        }
        
        /// <summary>
        /// 마이페이지 - 1대1 문의 저장 
        /// </summary>
        /// <param name="model"></param>
        public void User_Qna_Save_Sql(VW_User_QNA model, string Gubun)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_USER_QNA_INSERT";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter Save_Gubun = new SqlParameter("@GUBUN", SqlDbType.Char, 1);
                Save_Gubun.Value = Gubun;
                cmd.Parameters.Add(Save_Gubun);

                SqlParameter Sales_Gubun = new SqlParameter("@SALES_GUBUN", SqlDbType.VarChar, 2);
                Sales_Gubun.Value = model.SALES_GUBUN;
                cmd.Parameters.Add(Sales_Gubun);

                SqlParameter Company_Seq = new SqlParameter("@COMPANY_SEQ", SqlDbType.Int);
                Company_Seq.Value = model.COMPANY_SEQ;
                cmd.Parameters.Add(Company_Seq);

                SqlParameter Order_Seq = new SqlParameter("@ORDER_SEQ", SqlDbType.Int);
                Order_Seq.Value = model.ORDER_ID;
                cmd.Parameters.Add(Order_Seq);

                SqlParameter Member_Id = new SqlParameter("@MEMBER_ID", SqlDbType.VarChar, 50);
                Member_Id.Value = model.USERID ;
                cmd.Parameters.Add(Member_Id);

                SqlParameter Member_Name = new SqlParameter("@MEMBER_NAME", SqlDbType.VarChar, 50);
                Member_Name.Value = model.NAME;
                cmd.Parameters.Add(Member_Name);

                SqlParameter E_Mail = new SqlParameter("@E_MAIL", SqlDbType.VarChar, 50);
                E_Mail.Value = string.IsNullOrEmpty(model.EMAIL) ? "" : model.EMAIL;
                cmd.Parameters.Add(E_Mail);

                SqlParameter Q_Title = new SqlParameter("@Q_TITLE", SqlDbType.VarChar, 50);
                Q_Title.Value = model.TITLE;
                cmd.Parameters.Add(Q_Title);

                SqlParameter Q_Content = new SqlParameter("@Q_CONTENT", SqlDbType.Text);
                Q_Content.Value = model.CONTENT;
                cmd.Parameters.Add(Q_Content);      

                SqlParameter User_Upfile1 = new SqlParameter("@USER_UPFILE1", SqlDbType.VarChar, 100);
                User_Upfile1.Value = model.UPFILE_1;
                cmd.Parameters.Add(User_Upfile1);

                SqlParameter User_Upfile2 = new SqlParameter("@USER_UPFILE2", SqlDbType.VarChar, 100);
                User_Upfile2.Value = model.UPFILE_2;
                cmd.Parameters.Add(User_Upfile2);


                SqlParameter Q_Kind = new SqlParameter("@Q_KIND", SqlDbType.VarChar, 10);
                Q_Kind.Value = model.Q_KIND;
                cmd.Parameters.Add(Q_Kind);

                if (!string.IsNullOrEmpty(model.QNA_ID.ToString()))
                {
                    SqlParameter Qa_IID = new SqlParameter("@QA_IID", SqlDbType.VarChar, 100);
                    Qa_IID.Value = model.QNA_ID;
                    cmd.Parameters.Add(Qa_IID);
                }


                SqlParameter Stat_Upfile1 = new SqlParameter("@STAT", SqlDbType.VarChar, 100);
                Stat_Upfile1.Value = model.STAT;
                cmd.Parameters.Add(Stat_Upfile1);

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
        }

        public List<TB_Board> User_Faq_List_Entity(string SearchTxt)
        {

            var query = (
                            from faq in Entity_db.Set<TB_Board>().Where(x => x.Board_Category.Equals("F") && x.Display_YN.Equals("Y")).OrderByDescending(x => x.Regist_DateTime)
                            select new { faq }
                        );


            if (!String.IsNullOrEmpty(SearchTxt))
                query = query.Where(x => x.faq.Title.Contains(SearchTxt) || x.faq.Content.Contains(SearchTxt));

            //var union_query = query.Union(query2).ToList();

            var list = new List<TB_Board>();

            foreach (var item in query)
            {
                list.Add(new TB_Board()
                {
                    Board_ID = item.faq.Board_ID,
                    Title = item.faq.Title,
                    Content= item.faq.Content,
                    Hits = item.faq.Hits,
                    Top_YN = item.faq.Top_YN,
                    Display_YN = item.faq.Display_YN,
                    Regist_DateTime = item.faq.Regist_DateTime,
                    Update_DateTime = item.faq.Update_DateTime
                });
            }

            return list;

        }


        public List<TB_Board> User_Notice_List_Entity(string Top_YN)
        {

            var query = (
                            from notice in Entity_db.Set<TB_Board>().Where(x => x.Board_Category.Equals("N") && x.Display_YN.Equals("Y") && x.Top_YN.Equals(Top_YN)).OrderByDescending(x => x.Regist_DateTime)
                            select new { notice }
                        );

            var list = new List<TB_Board>();
           
            foreach (var item in query)
            {
                list.Add(new TB_Board()
                {
                    Board_ID = item.notice.Board_ID,
                    Title = item.notice.Title,
                    Hits = item.notice.Hits,
                    Top_YN = item.notice.Top_YN,
                    Display_YN = item.notice.Display_YN,
                    Regist_DateTime = item.notice.Regist_DateTime,
                    Update_DateTime = item.notice.Update_DateTime
                });
            }

            return list;

        }


        public List<TB_Board> Admin_Notice_View_Entity(int id)
        {
            var query = from notice in Entity_db.Set<TB_Board>().Where(x => x.Board_ID.Equals(id)) select new { notice };

            var list = new List<TB_Board>();
            foreach (var item in query)
            {
                list.Add(new TB_Board()
                {
                    Board_ID = item.notice.Board_ID,
                    Title = item.notice.Title,
                    Content = item.notice.Content,
                    Hits = item.notice.Hits,
                    Top_YN = item.notice.Top_YN,
                    Display_YN = item.notice.Display_YN,
                    Regist_DateTime = item.notice.Regist_DateTime,
                    Update_DateTime = item.notice.Update_DateTime,
                });
            }

            return list;

        }


        /// <summary>
        /// 배너 업데이트 
        /// </summary>
        /// <param name="Banner_Item_Id"></param>
        /// <returns></returns>
        public string User_Banner_Click_Update_Entity(int Banner_Item_Id)
        {
            try
            {
                int? Click = 0;

               var query = from banner in Entity_db.Set<TB_Banner_Item>().Where(x => x.Banner_Item_ID.Equals(Banner_Item_Id)) select new { banner };

                var list = new List<TB_Banner_Item>();
                foreach (var item in query)
                {
                    Click = string.IsNullOrEmpty(item.banner.Click_Count.ToString()) ? 0 : item.banner.Click_Count;

                    //list.Add(new TB_Board()
                    //{
                    //    Board_ID = item.banner.Click_Count
                    //    Title = item.notice.Title,
                    //    Content = item.notice.Content,
                    //    Hits = item.notice.Hits,
                    //    Top_YN = item.notice.Top_YN,
                    //    Display_YN = item.notice.Display_YN,
                    //    Regist_DateTime = item.notice.Regist_DateTime,
                    //    Update_DateTime = item.notice.Update_DateTime,
                    //});
                }
            

                // int Click = Entity_db.TB_Banner_Items.Where(x => x.Banner_Item_ID.Equals(Convert.ToInt32(Banner_Item_Id))).Select(p => Convert.ToInt32(p.Click_Count));

                // int? Click_Sum = Entity_db.TB_Banner_Items.AsEnumerable().Where(x => x.Banner_Item_ID.Equals(Banner_Item_Id)).Select(p => p.Click_Count).DefaultIfEmpty(0).Max();

                var Update_Banner_Item = Entity_db.TB_Banner_Items.Where(n => n.Banner_Item_ID.Equals(Banner_Item_Id)).SingleOrDefault();

                Update_Banner_Item.Click_Count = Convert.ToInt32(Click + 1);

                Entity_db.Entry(Update_Banner_Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Entity_db.SaveChanges();

                return "S";
            }
            catch (Exception ex)
            {
                return "F";
            }
            

        }

        /// <summary>
        /// 공지사항/ FAQ 클릭수 업데이트 
        /// </summary>
        /// <param name="Banner_Item_Id"></param>
        /// <returns></returns>
        public string User_Notice_Faq_Click_Update_Entity(int Id, string Board_Category)
        {
            try
            {
                int? Click = 0;

                var query = from board in Entity_db.Set<TB_Board>().Where(x => x.Board_Category.Equals(Board_Category) && x.Board_ID.Equals(Id)) select new { board };

                var list = new List<TB_Board>();
                foreach (var item in query)
                {
                    Click = string.IsNullOrEmpty(item.board.Hits.ToString()) ? 0 : item.board.Hits;
                }


                // int Click = Entity_db.TB_Banner_Items.Where(x => x.Banner_Item_ID.Equals(Convert.ToInt32(Banner_Item_Id))).Select(p => Convert.ToInt32(p.Click_Count));

                // int? Click_Sum = Entity_db.TB_Banner_Items.AsEnumerable().Where(x => x.Banner_Item_ID.Equals(Banner_Item_Id)).Select(p => p.Click_Count).DefaultIfEmpty(0).Max();

                var Update_Board_Item = Entity_db.TB_Boards.Where(n => n.Board_Category.Equals(Board_Category) && n.Board_ID.Equals(Id)).SingleOrDefault();

                Update_Board_Item.Hits = Convert.ToInt32(Click + 1);

                Entity_db.Entry(Update_Board_Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Entity_db.SaveChanges();

                return "S";
            }
            catch (Exception ex)
            {
                return "F";
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Popup_Title"></param>
        /// <param name="Popup_Type">PTC01 : PC / PTC02 : 모바일</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Popup_List_Entity(string Popup_Title, string Popup_Type)
        {
           
            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
                //Entity_db = new barunsonContext();
                //this.Entity_db = new barunsonContext();

                var query = from popup in Entity_db.Set<TB_Popup>()//.Where(x => x.Popup_Title.Equals(Popup_Title))
                            join popup_item in Entity_db.Set<TB_Popup_Item>().Where(x => x.Popup_Type_Code.Equals(Popup_Type)) on popup.Popup_ID equals popup_item.Popup_ID
                            select new { popup, popup_item }; //Entity_db.TB_Banner_Items.Delete();
          
                foreach (var item in query)
                {
                    string NowDate = DateTime.Now.ToString("yyyy-MM-dd  HH:00:00");

                    int start = 0;

                    try
                    {
                        start = DateTime.Compare(Convert.ToDateTime(item.popup_item.Start_Date + " " + item.popup_item.Start_Time + ":00:00"), Convert.ToDateTime(NowDate));
                    }
                    catch (Exception)
                    {

                    }
                   
                    
                    int end = 0;

                    try
                    {
                        end = DateTime.Compare(Convert.ToDateTime(item.popup_item.End_Date + " " + item.popup_item.End_Time + ":00:00"), Convert.ToDateTime(NowDate));
                    }
                    catch (Exception)
                    {

                    }
                  

                    if ((start <= 0 && end > 0) || item.popup_item.Period_Type_Code.Equals("PTC02"))
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();

                        dic.Add("Popup_Title", item.popup.Popup_Title);
                        dic.Add("Popup_PC_YN", item.popup.Popup_PC_YN);
                        dic.Add("Image_Url", item.popup_item.Image_URL);
                        dic.Add("Popup_Mo_YN", item.popup.Popup_Mobile_YN);
                        dic.Add("Popup_Location_Left", item.popup.Popup_Location_Left);
                        dic.Add("Popup_Location_Top", item.popup.Popup_Location_Top);
                        dic.Add("Popup_Height", item.popup.Popup_Height);
                        dic.Add("Popup_Width", item.popup.Popup_Width);
                        dic.Add("Link_URL", item.popup_item.Link_URL);
                        result.Add(dic);
                    }
                       
                }
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        /// <summary>
        /// 어드민 - 사이트메뉴 리스트 
        /// </summary>
        /// <param name="Menu_Type"></param>
        /// <returns></returns>
        public List<TB_Common_Menu> Admin_Menu_List_Entity(/*string Menu_Type*/)
        {
            // Entity_db = new barunsonContext();
            //Entity_db.Entry(Entity_db.TB_Common_Menus).Reload();


            List<TB_Common_Menu> Common_Menu = new List<TB_Common_Menu>();

            try
            {
                var query = from menu in Entity_db.Set<TB_Common_Menu>().Where(x => x.Display_YN.Equals("Y"))//.AsNoTracking()//.Where(x => x.Menu_Type_Code.Equals(Menu_Type))
                            select new { menu }; //Entity_db.TB_Banner_Items.Delete();
                query = query.OrderBy(o => o.menu.Sort);


                //var list = new List<string>();
                //List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                foreach (var item in query)
                {

                    Common_Menu.Add(new TB_Common_Menu
                    {
                        Menu_ID = item.menu.Menu_ID,
                        Menu_Step = item.menu.Menu_Step,
                        Sort = item.menu.Sort,
                        Menu_Name = item.menu.Menu_Name,
                        Parent_Menu_ID = item.menu.Parent_Menu_ID,
                        Menu_Type_Code = item.menu.Menu_Type_Code,
                        Display_YN = item.menu.Display_YN,
                        Menu_URL = item.menu.Menu_URL,
                        Image_URL = item.menu.Image_URL
                    });

                }
            }
            catch (Exception)
            {

            }
           
                return Common_Menu;
           
           

        }

        /// <summary>
        /// 프런트 - 
        /// 카테고리명 
        /// </summary>
        /// <param name="Category_Type">CTC01: 메인 / CTC02: 카테고리</param>
        /// <param name="Parent_Category_Id">부모카테고리ID</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> User_Menu_List_Entity(string Category_Type, int Parent_Category_Id)
        {
            var list = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {
               // Entity_db = new barunsonContext();
                //this.Entity_db = new barunsonContext();

                var query = from category_menu in Entity_db.Set<TB_Category>().Where(x => x.Category_Type_Code.Equals(Category_Type) && x.Display_YN.Equals("Y"))
                            select new { category_menu }; //Entity_db.TB_Banner_Items.Delete();

                if (Parent_Category_Id > 0)
                {
                    //중분류명만 가져오기 
                    query = query.Where(x => x.category_menu.Parent_Category_ID.Equals(Parent_Category_Id));

                }

                query = query.OrderBy(o => o.category_menu.Sort);
           

                foreach (var item in query)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic.Add("Parent_Category_Id", item.category_menu.Parent_Category_ID);
                    dic.Add("Category_ID", item.category_menu.Category_ID);
                    dic.Add("Category_Name", item.category_menu.Category_Name);
                    dic.Add("Category_Type_Code", item.category_menu.Category_Type_Code);
                    dic.Add("Category_Name_PC_URL", item.category_menu.Category_Name_PC_URL);
                    dic.Add("Sort", item.category_menu.Sort);
                    result.Add(dic);
                }
            }
            catch (Exception)
            {

                throw;
            }
           

            return result;
        }

        /// <summary>
        /// 프런트 - 회원의 위시 리스트 개수 
        /// </summary>
        /// <param name="User_Id"></param>
        /// <returns></returns>
        public int User_Wish_Total_Cnt_Entity(string User_Id)
        {
            int cnt = 0;

            try
            {
               Entity_db = new barunsonContext();

                //TB_Wish_List Wish_List = new TB_Wish_List();
                //Entity_db.Entry(Wish_List).Reload();

                cnt = (from wish_list in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID.Equals(User_Id))
                       join product in Entity_db.Set<TB_Product>() on wish_list.Product_ID equals product.Product_ID
                       select new { wish_list }).Count();

               

                //foreach (var item in query)
                //{
                //    cnt++;
                //}
            }
            catch (Exception ex)
            {
                //  throw;
            }
            

            return cnt;
        }

        public string User_Wish_Save_Entity(TB_Wish_List list, string Gubun)
        {
            //int cnt = 0;

            if (string.Equals(Gubun, "I"))
            {
                Entity_db.Add(list);
            }
            else
            {
                var Del_Wish_List = Entity_db.TB_Wish_Lists.Where(n => n.User_ID.Equals(list.User_ID) && n.Product_ID.Equals(list.Product_ID));
                Entity_db.TB_Wish_Lists.RemoveRange(Del_Wish_List);


            }
         
            Entity_db.SaveChanges();

            int Total_Cnt = (from wish_list in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID.Equals(list.User_ID))
                          join product in Entity_db.Set<TB_Product>() on wish_list.Product_ID equals product.Product_ID
                          select new { wish_list }).Count();

            int Member_Cnt = (from wish_list in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID.Equals(list.User_ID) && x.Product_ID.Equals(list.Product_ID))
                                join product in Entity_db.Set<TB_Product>() on wish_list.Product_ID equals product.Product_ID
                                select new { wish_list }).Count();
           return Total_Cnt + "_" + Member_Cnt;

            //return cnt = (from wish_list in Entity_db.Set<TB_Wish_List>().Where(x => x.User_ID.Equals(list.User_ID))
            //              join product in Entity_db.Set<TB_Product>() on wish_list.Product_ID equals product.Product_ID
            //              select new { wish_list }).Count();


        }


        public void Error_Save_Sql(String Error_Content, string Method_Name, string User_Id, string User_Name)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Entity_db.Database.GetConnectionString();

            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = _connectionString;

            try
            {
                if (con.State == ConnectionState.Closed) con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DBO.SP_T_ERROR_CONTENT";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter Error_Param = new SqlParameter("@ERROR_CONTENT", SqlDbType.Text);
                Error_Param.Value = Error_Content;
                cmd.Parameters.Add(Error_Param);

                SqlParameter ID_Param = new SqlParameter("@ID", SqlDbType.VarChar, 50);
                ID_Param.Value = User_Id;
                cmd.Parameters.Add(ID_Param);

                SqlParameter Name_Param = new SqlParameter("@USER_NAME", SqlDbType.NVarChar, 100);
                Name_Param.Value = User_Name;
                cmd.Parameters.Add(Name_Param);

                SqlParameter Method = new SqlParameter("@METHOD_NAME", SqlDbType.VarChar, 50);
                Method.Value = Method_Name;
                cmd.Parameters.Add(Method);

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

        }
        
        public List<TB_PolicyInfo> PolicyInfo_History_List_Entity(string policyDiv)
        {
            List<TB_PolicyInfo> policyInfos = new List<TB_PolicyInfo>();
            
            string currDate = DateTime.Now.ToString("yyyy-MM-dd");

            var query = from o in Entity_db.TB_PolicyInfos
                        where o.PolicyDiv == policyDiv
                        && string.Compare(currDate, o.EndDate) > 0
                        orderby o.StartDate ascending
                        select o;

            foreach (var item in query)
            {

                policyInfos.Add(new TB_PolicyInfo
                {
                    Seq = item.Seq,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate
                });

            }
            return policyInfos;
        }

        public TB_PolicyInfo PolicyInfo_View_Entity(string policyDiv, int seq=0)
        {
            TB_PolicyInfo policyInfo = null;

            if (seq > 0)
            {
                policyInfo = (from o in Entity_db.TB_PolicyInfos
                              where o.Seq == seq && o.PolicyDiv== policyDiv
                              orderby o.StartDate descending
                              select o).FirstOrDefault();
            }
            else
            {
                string currDate = DateTime.Now.ToString("yyyy-MM-dd");

                policyInfo = (from o in Entity_db.TB_PolicyInfos
                              where o.PolicyDiv == policyDiv
                              && string.Compare(currDate, o.StartDate) >= 0
                              && string.Compare(currDate, o.EndDate) <= 0                              
                              orderby o.StartDate descending
                              select o).FirstOrDefault();
            }            

            return policyInfo;
        }


    }
}
