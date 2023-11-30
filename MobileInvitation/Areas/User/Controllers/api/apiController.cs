using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileInvitation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace MobileInvitation.Areas.User.Controllers.api
{
    [Area("User")]
    [AllowAnonymous]
    public class apiController : Controller
    {
        private readonly barunsonContext Entity_db;
        public apiController(barunsonContext _Entity_db)
        {
            Entity_db = _Entity_db;

        }

        /// <summary>
        /// 클라이언트 접속 허용 여부
        /// </summary>
        /// <param name="allowList"></param>
        /// <returns></returns>
        private bool ClientIpCheck(List<IPAddress> allowList)
        {
            //기본 허용 IP 추가
            allowList.Add(IPAddress.Loopback);
            allowList.Add(IPAddress.Parse("112.169.30.150"));
            for(int ip = 150; ip < 180; ip++)
            {
                allowList.Add(IPAddress.Parse($"112.169.30.{ip}"));
            }

            var badIp = true;
            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }
            foreach (var address in allowList)
            {
                if (address.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }
            //역 출력
            return !badIp;
        }

        #region Flower 선물하기(Fla system 용 API)

        /// <summary>
        /// 플라에서 주문정보 확인용 api
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [Route("api/pla/orderinfo/{orderid}")]
        [HttpGet]
        public async Task<IActionResult> flaOrderInfoAsync(int orderid)
        {
            var result = new OrderInfo
            {
                OrderId = orderid,
                Message = "인증되지 않은 접근입니다."
            };
            var allowList = new List<IPAddress>
            {
                 IPAddress.Parse("58.103.130.28")
            };

            if (ClientIpCheck(allowList))
            {
                var query = from m in Entity_db.TB_Invitation_Details
                            join i in Entity_db.TB_Invitations on m.Invitation_ID equals i.Invitation_ID
                            join o in Entity_db.TB_Orders on i.Order_ID equals o.Order_ID
                            where o.Order_ID == orderid
                            select new { m, o.Order_Status_Code, o.Payment_Status_Code };
                var item = await query.FirstOrDefaultAsync();

                //주문 상태확인 주문완료, 결제완료: Order_Status_Code = OSC01, PAYMENT_STATUS_CODE=PSC02
                //화환 서비스 이용 여부 확인 필요
                if (item != null && (item.Order_Status_Code == "OSC01" && item.Payment_Status_Code == "PSC02" && item.m.Flower_gift_YN == "Y"))
                {
                    result.GroomInfo = new PersonInfo();
                    result.GroomInfo.Title = "신랑";
                    result.GroomInfo.Name = item.m.Groom_Name;
                    result.GroomInfo.Phone = item.m.Groom_Phone;
                    result.GroomInfo.Parents = new List<PersonInfo>
                    {
                        new PersonInfo
                        {
                            Name = item.m.Groom_Parents1_Name,
                            Phone = item.m.Groom_Parents1_Phone,
                            Title = item.m.Groom_Parents1_Title
                        },
                        new PersonInfo
                        {
                            Name = item.m.Groom_Parents2_Name,
                            Phone = item.m.Groom_Parents2_Phone,
                            Title = item.m.Groom_Parents2_Title
                        }
                    };

                    result.BrideInfo = new PersonInfo();
                    result.BrideInfo.Title = "신부";
                    result.BrideInfo.Name = item.m.Bride_Name;
                    result.BrideInfo.Phone = item.m.Bride_Phone;
                    result.BrideInfo.Parents = new List<PersonInfo>
                    {
                        new PersonInfo
                        {
                            Name = item.m.Bride_Parents1_Name,
                            Phone = item.m.Bride_Parents1_Phone,
                            Title = item.m.Bride_Parents1_Title
                        },
                        new PersonInfo
                        {
                            Name = item.m.Bride_Parents2_Name,
                            Phone = item.m.Bride_Parents2_Phone,
                            Title = item.m.Bride_Parents2_Title
                        }
                    };

                    var wdate = DateTime.Parse(item.m.WeddingDate);
                    if (!string.IsNullOrEmpty(item.m.WeddingHour))
                    {
                        var h = int.Parse(item.m.WeddingHour.Trim());
                        if (item.m.Time_Type_Code == "오후" && h < 12)
                            h += 12;

                        wdate = wdate.AddHours(h);
                    }
                    if (!string.IsNullOrEmpty(item.m.WeddingMin))
                        wdate = wdate.AddMinutes(int.Parse(item.m.WeddingMin.Trim()));

                    result.WeddingDate = wdate.ToString("yyyy-MM-dd HH:mm:ss");
                    result.WeddingHall = item.m.Weddinghall_Name;
                    result.WeddingHallDetal = item.m.WeddingHallDetail;
                    result.WeddingHallAddress = item.m.Weddinghall_Address;
                    result.WeddingHallPhone = item.m.Weddinghall_PhoneNumber;

                    result.Message = "";
                }
                else
                {
                    result.Message = "주문 정보를 찾을 수 없습니다.";
                }
            }
            var jsonStr = JsonConvert.SerializeObject(result);
            return Content(jsonStr, "application/json");

        }
        #endregion

        #region 주문 정보 모델

        /// <summary>
        /// 주문정보
        /// </summary>
        public class OrderInfo
        {
            public int OrderId { get; set; }
            /// <summary>
            /// 신랑정보
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public PersonInfo GroomInfo{ get; set; }

            /// <summary>
            /// 신부정보
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public PersonInfo BrideInfo { get; set; }

            /// <summary>
            /// 결혼 날짜시간
            /// yyyy-MM-dd {오전:오후} hh:mm
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public string WeddingDate { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public string WeddingHall { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public string WeddingHallDetal { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public string WeddingHallAddress { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] 
            public string WeddingHallPhone { get; set; }

            public string Message { get; set; }
        }
        /// <summary>
        /// 인물 정보
        /// </summary>
        public class PersonInfo
        {
            /// <summary>
            /// 호칭
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 이름
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 전화번호
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// 부모정보
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<PersonInfo> Parents { get; set; }
        }

        #endregion

        #region Test
        [Route("api/testapi")]
        [HttpGet]
        public IActionResult Test()
        {
            var result = new
            {
                cip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                cip2 = MobileInvitation.FunctionHelper.UrlHelper.GetIP(HttpContext.Connection.RemoteIpAddress),
                ciph = HttpContext.Request.Headers["X-Forwarded-For"]
            };
            var jsonStr = JsonConvert.SerializeObject(result);
            return Content(jsonStr, "application/json");
        }
        #endregion
    }
}
