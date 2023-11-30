using System.Text.RegularExpressions;
using System;

namespace MobileInvitation.FunctionHelper
{
    public static class StringHelper
    {
        public static string RemoveHTML(string str)
        {
            //return Regex.Replace(str, "<[^>]*>", string.Empty);
            return Regex.Replace(str, "<[A-z|/]+[^<>]*>", string.Empty);
        }

        public static bool IsNumber(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                    return false;
                else
                    return Regex.IsMatch(str, "^[0-9]*$");
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static string Alert(this string script, string massage)
        {
            return string.Format("{0} alert('{1}');", script, Uri.EscapeDataString(massage));
        }

        /// <summary>
        /// Javascript history.back() 생성
        /// </summary>
        /// <param name="script"></param>
        /// <returns>history.back(0);</returns>
        public static string HistoryBack(this string script)
        {
            return string.Format("{0} history.back(0);", script);
        }

        /// <summary>
        /// 해당 문자열을 스크립트 블록으로 생성
        /// </summary>
        /// <param name="script"></param>
        //public static string ToScript(this string script)
        //{

        //    //Content(string.Format("<script type=\"text/javascript\">{0}</script>", script));
        //    //return string.Empty;
        //}
        public static string Alert(string message)
        {
            return Alert(string.Empty, message);
        }
        public static string HtmlDecode(string str)
        {
            return System.Net.WebUtility.HtmlDecode(str);
        }
        public static string HtmlEncode(string str)
        {
            return System.Net.WebUtility.HtmlEncode(str);
        }

        public static string HtmlEncode2(string str)
        {
            // Create regular expressions
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(<br>|<br />|<br/>|</ br>|</br>)");

            // Replace new line with <br/> tag
            str = regex.Replace(str, "\r\n");

            return str;
        }

        public static string QnA_Status_Str(string str)
        {
            string Return = "";

            switch (str)
            {
                case "S1":
                    Return = "접수중";
                    break;
                case "S2":
                    Return = "접수/처리중";
                    break;
                case "S3":
                    Return = "답변완료";
                    break;
                case "S5":
                    Return = "내부처리";
                    break;
                default:
                    Return = "접수중";
                    break;
            }

            return Return;
        }


        public static string CutLength(string str, int len, string expression = "...")
        {
            bool state = false;
            string result = CutLength(str, len, out state);

            return string.Format("{0}{1}", result, (state) ? expression : string.Empty);
        }
        private static string CutLength(string str, int len, out bool state)
        {
            string result = str;

            if (str.Length <= len)
            {
                state = false;
            }
            else
            {
                state = true;
                result = str.Substring(0, len);
            }

            return result;
        }

        public static string GetSiteName(string ProductBrandCode)
        {
            string result = "";

            switch (ProductBrandCode.ToUpper())
            {
                case "PBC01":
                    result = "바른손";
                    break;
                case "PBC02":
                    result = "비핸즈";
                    break;
                case "PBC03":
                    result = "더카드";
                    break;
                case "PBC04":
                    result = "프리미어페이퍼";
                    break;
                default:
                    break;
            }

            return result;
        }


        public static string GetMemberRegistGubun(string SiteName)
        {
            string result = "";

            if (SiteName.Trim().Equals("바른손스토어"))
            {
                result = "M";
            }
            else result = "F";

            return result;
        }


        public static string GetDisplayStatus(string YN)
        {
            string result = "";

            switch (YN.ToUpper())
            {
                case "Y":
                    result = "진열함";
                    break;
                case "N":
                    result = "진열안함";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static string GetAdminType(string gubun)
        {
            string result = "";

            switch (gubun.ToUpper())
            {
                case "2":
                    result = "관리자";
                    break;
                default:
                    result = "운영자";
                    break;
            }

            return result;
        }

    }
}
