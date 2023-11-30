using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.FunctionHelper
{
    public static class CodeNameHelper
    {

        public static string ItemTypeCodeToResourceTypeCode(string itc)
        {
            var result = "";
            switch (itc)
            {
                case "ITC01":
                    result = "txt";
                    break;
                case "ITC02":
                    result = "img";
                    break;
                case "ITC03":
                    result = "photo";
                    break;
                case "ITC04":
                    result = "profile";
                    break;
                default:
                    break;
            }
            return result;
        }
        public static string ResourceTypeCodeToItemTypeCode(string rtc)
        {
            var result = "";
            switch (rtc)
            {
                case "txt":
                    result = "ITC01";
                    break;
                case "img":
                    result = "ITC02";
                    break;
                case "photo":
                    result = "ITC03";
                    break;
                case "profile":
                    result = "ITC04";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
