using Microsoft.AspNetCore.Mvc.Rendering;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 1:1 문의
    /// </summary>
    public class UserQNAVIewModel : VW_User_QNA
    {
        public string ORDER_CODE { get; set; }

        public string Q_KIND_NAME
        {
            get
            {               
                try
                {
                    return Q_KIND_LIST.Where(p => p.Value == Q_KIND).First().Text;
                }
                catch
                {
                    return "";
                }
            }
          
        }

        public IEnumerable<SelectListItem> Q_KIND_LIST => new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "분류", Value = "none" },
                    new SelectListItem { Text = "주문방법", Value = "AMC0401" },
                    new SelectListItem { Text = "주문수정/취소", Value = "AMC0402" },
                    new SelectListItem { Text = "환불", Value = "AMC0403" },
                    new SelectListItem { Text = "불만", Value = "AMC0404" },
                    new SelectListItem { Text = "쿠폰", Value = "AMC0405" },
                    new SelectListItem { Text = "회원연동", Value = "AMC0406" }
            }
            , "Value", "Text"
        );
    }

}
