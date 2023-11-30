using System.Web;
using System;

namespace MobileInvitation.FunctionHelper
{
    public static class DateTimeHelper
    {
        public static String configPath = "C:\\lgdacom";
        public static string HHmm(object date)
        {
            try
            {
                return Convert.ToDateTime(date).ToString("yyyy-MM-dd  HH:mm");
            }
            catch (Exception ex)
            {
                return date.ToString();
                throw new Exception(ex.ToString());
            }

        }

        public static string Int_Format(string Num)
        {
            string ReturnValue = "";

            try
            {
                if (!string.IsNullOrEmpty(Num))
                {
                    ReturnValue = String.Format("{0:#,##0}", int.Parse(Num));
                }
            }
            catch (Exception ex)
            {
                ReturnValue = "0";
                throw new Exception(ex.ToString());
            }

            return ReturnValue;


        }

        public static string DateCheck(DateTime StartDateTime, DateTime EndDateTime, string Gubun)
        {
            string ReturnValue = "";

            if (Gubun.Equals("1")) // 경과일 
            {
                TimeSpan dateDiff = StartDateTime - EndDateTime;

                //int diffDay = dateDiff.Days;
                //int diffHour = dateDiff.Hours;
                int diffMinute = dateDiff.Minutes;
                //int diffSecond = dateDiff.Seconds;

                string Elapsed_Time = ((StartDateTime - EndDateTime).Days + 1).ToString(); ;// Convert.ToInt32(diffMinute);

                ReturnValue = Elapsed_Time;
            }
            else // 상태 
            {
                string NowDate = DateTime.Now.ToString("yyyy-MM-dd  HH:00:00");
                int start = DateTime.Compare(StartDateTime, Convert.ToDateTime(NowDate));
                int end = DateTime.Compare(EndDateTime, Convert.ToDateTime(NowDate));

                // 1 - 진행 / 2 - 예약 / 3 - 종료 
                if (start <= 0)
                {
                    if (end >= 0) ReturnValue = "진행";
                    else if (end < 0) ReturnValue = "종료";
                }
                else if (start > 0)
                {
                    ReturnValue = "예약";
                }


            }
            return ReturnValue;
        }

        public static string DateCheck2(DateTime ComPage_Time)
        {
            string ReturnValue = "";

            string NowDate = DateTime.Now.ToString("yyyy-MM-dd");
            int start = DateTime.Compare(ComPage_Time, Convert.ToDateTime(NowDate));

            // T : 진행 / F : 종료 

            // 유효날짜가 현재날짜를 지나지 않은 경우 

            if (start < 0)
            {
                ReturnValue = "off";
            }
            else
            {

                ReturnValue = "on";
            }


            return ReturnValue;
        }

        public static string HHmm(DateTime date)
        {
            return Convert.ToDateTime(date).ToString("yyyy-MM-dd  HH:mm");
        }

        public static string StartDate(string date)
        {
            return date + ", 00:00:00";
        }
        public static string EndDate(string date)
        {
            return date + ", 23:59:59";
        }
        public static string Test(string aa)
        {
            return HttpUtility.UrlDecode(aa);

        }

    }
}
