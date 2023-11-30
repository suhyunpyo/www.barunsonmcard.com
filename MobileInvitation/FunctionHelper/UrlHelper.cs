using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Net;
using System;
using System.Linq;

namespace MobileInvitation.FunctionHelper
{
    public class UrlHelper
    {
        public static string GetIP(IPAddress remoteIpAddress)
        {
            string ip_ = "";
            IPAddress ipaddr = remoteIpAddress;// Request.HttpContext.Connection.RemoteIpAddress;
            if (ipaddr != null)
            {
                if (ipaddr.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ipaddr = Dns.GetHostEntry(ipaddr).AddressList
                        .First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }
                ip_ = ipaddr.ToString();
            }
            return ip_;

            

        }

        public static Boolean IsMobile(HttpRequest Request)
        {
            bool IsMobile = false;
            string Mobile = Request.Cookies["IsMobile"];
            string Agent = Request.Headers["User-Agent"].ToString();

            if (Mobile == null)
            {
                string[] browser = { "iphone", "ipod", "ipad", "android", "blackberry", "windows ce", "nokia", "webos", "opera mini", "sonyericsson", "opera mobi", "iemobile", "windows phone" };
                for (int i = 0; i < browser.Length; i++)
                {
                    if (Agent.ToLower().Contains(browser[i]))
                    {
                        IsMobile = true;
                        break;
                    }
                }
            }
            else
            {
                IsMobile = Mobile.ToLower().Equals("true") ? true : false;
            }

            return IsMobile;
        }
    }
}
