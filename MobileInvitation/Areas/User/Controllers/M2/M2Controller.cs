using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobileInvitation.Areas.User.Controllers.M2
{
    [Area("User")]
    public class M2Controller : Controller
    {
        private readonly barunsonContext barunson;
        private readonly BarunnConfig _barunnConfig;
        private readonly Uri CdnUri;
        private readonly Uri SiteUri;

        #region 생성자
        public M2Controller(barunsonContext context, BarunnConfig barunnConfig)
        {
            barunson = context;
            _barunnConfig = barunnConfig;
            CdnUri = _barunnConfig.Sites.CDNUrl;
            SiteUri = _barunnConfig.Sites.Url;
        }
        #endregion

        #region Main Page
        /// <summary>
        /// 초대장 메인 페이지
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        [Route("m/{Path}")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index(string Path)
        {
            M2ViewModel model = new M2ViewModel();
            try
            {
                var query = from id in barunson.TB_Invitation_Details
                            join i in barunson.TB_Invitations on id.Invitation_ID equals i.Invitation_ID
                            join o in barunson.TB_Orders on i.Order_ID equals o.Order_ID
                            join op in barunson.TB_Order_Products on o.Order_ID equals op.Order_ID
                            join p in barunson.TB_Products on op.Product_ID equals p.Product_ID
                            join t in barunson.TB_Templates on i.Template_ID equals t.Template_ID
                            where id.Invitation_URL == Path
                            select new M2DBDataModel
                            {
                                InvitationDetail = id,
                                Invitation = i,
                                Order = o,
                                Product = p,
                                Template = t
                            };
                var item = await query.FirstOrDefaultAsync();
                if (item != null)
                {
                    model.InvitaionId = item.InvitationDetail.Invitation_ID;
                    model.ProductCategoryCode = item.Product.Product_Category_Code;
                    model.IsUser = !string.IsNullOrWhiteSpace(item.Invitation.User_ID);

                    //표시여부 판단.
                    if (CheckDisplay(item.InvitationDetail, item.Invitation, item.Order))
                    {
                        #region Header, Meta, 공통 영역
                        model.FullUrl = new Uri(HttpContext.Request.GetEncodedUrl());
                        model.Title = item.InvitationDetail.Invitation_Title;
                        model.EventDateTime = "";
                        model.EventDate = "";
                        if (!string.IsNullOrEmpty(item.InvitationDetail.SNS_Image_URL))
                        {
                            model.SNSImageInfo = new M2ImageInfoViewModel
                            {
                                ImageUrl = GetResourceAbsoluteUrl(item.InvitationDetail.SNS_Image_URL),
                                Height = (int?)item.InvitationDetail.SNS_Image_Height,
                                Width = (int?)item.InvitationDetail.SNS_Image_Width
                            };
                        }
                        model.GalleryPreventPhotoYN = (item.InvitationDetail.GalleryPreventPhoto_YN == "Y");

                        if (!string.IsNullOrEmpty(item.Template.Attached_File1_URL))
                            model.CustomCssUrl = GetResourceAbsoluteUrl(item.Template.Attached_File1_URL);
                        if (!string.IsNullOrEmpty(item.Template.Attached_File2_URL))
                            model.CustomJsUrl = GetResourceAbsoluteUrl(item.Template.Attached_File2_URL);

                        var reserveWords = await GetReserveWord(item.Product.Product_Category_Code, item.InvitationDetail);
                        var areas = await GetAreas(item.Invitation, item.InvitationDetail, reserveWords);

                        //이벤트 날짜시간 생성
                        if (!string.IsNullOrEmpty(item.InvitationDetail.WeddingDate))
                        {
                            var eventDateTime = DateTime.Parse(item.InvitationDetail.WeddingDate);
                            int.TryParse(item.InvitationDetail.WeddingHour, out int hour);
                            if (item.InvitationDetail.Time_Type_Code == "오후")
                            {
                                if (hour < 12) { hour += 12; }
                            }
                            eventDateTime = eventDateTime.AddHours(hour);
                            int.TryParse(item.InvitationDetail.WeddingMin, out int min);
                            eventDateTime = eventDateTime.AddMinutes(min);

                            if(eventDateTime.Minute == 0)
                            {
                                model.EventDateTime = eventDateTime.ToString("yyyy-MM-dd dddd tt h'시'", CultureInfo.GetCultureInfo("ko-KR"));
                            } 
                            else
                            {
                                model.EventDateTime = eventDateTime.ToString("yyyy-MM-dd dddd tt h'시' mm'분'", CultureInfo.GetCultureInfo("ko-KR"));
                            }
                            
                            model.EventDate = item.InvitationDetail.WeddingDate;
                        }
                        #endregion

                        #region 청첩장
                        if (item.Product.Product_Category_Code == "PCC01")
                        {
                            //청첩장 모델 작성
                            model.PCC01DataModel = await GetPCC01ViewModel(model.InvitaionId, areas, item, model.IsUser);
                            
                        }
                        #endregion

                            #region 감사장
                        if (item.Product.Product_Category_Code == "PCC02")
                        {
                            model.PCC02DataModel = GetPCC02ViewModel(model.InvitaionId, areas, item);
                        }
                        #endregion

                        #region 돌잔치
                       
                        if (item.Product.Product_Category_Code == "PCC03")
                        {
                            //돌잔치 모델 작성
                            model.PCC03DataModel = await GetPCC03ViewModel(model.InvitaionId, areas, item, model.IsUser);
                        }

                        #endregion

                        //마지막으로 표시 설정
                        model.IsDisplay = true;
                    }
                }
            }
            catch (Exception ex)
            {
                model.IsDisplay = false;
            }
            if (model.IsDisplay)
                return View(model);
            else
                return View("NotDisplay");
            
        }
        #endregion

        #region Sub Pages
        
        /// <summary>
        /// 네이버 지도 표시
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="lat"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("m2/navermap")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NaverMap(double lot, double lat, string name)
        {
            var model = new NaverMapViewModel
            {
                ApiId = _barunnConfig.Map.NaverCloudId,
                ApiKey= _barunnConfig.Map.NaverCloudKey,
                Lot = lot,
                Lat= lat,
                Name = name
            };
            return View(model);
        }

        /// <summary>
        /// 방명록 작성
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        [Route("m2/GuestBookSave")]
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GuestBookSave(int InvitationId, string Name, string Password, string Message)
        {
            var result = new JsonReusltStatusModel { status = false, message = "등록되지않았습니다. 특수문자가 없는지 확인해 보시고 계속 등록이 안될시 관리자에게 문의하세요." };
            try
            {
                if (InvitationId > 0)
                {
                    //초대장 유효성 검토
                    var query = from id in barunson.TB_Invitation_Details
                                join i in barunson.TB_Invitations on id.Invitation_ID equals i.Invitation_ID
                                join o in barunson.TB_Orders on i.Order_ID equals o.Order_ID
                                where id.Invitation_ID == InvitationId
                                select new
                                {
                                    InvitationDetail = id,
                                    Invitation = i,
                                    Order = o,
                                };
                    var item = await query.FirstOrDefaultAsync();
                    if (CheckDisplay(item.InvitationDetail, item.Invitation, item.Order))
                    {
                        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Message))
                        {
                            result.message = "이름, 비밀번호, 축하메시지 <br> 모두 입력 하셔야 등록 가능합니다.";
                        }
                        else
                        {
                            string ip = HttpContext.Connection.RemoteIpAddress.ToString();

                            var addItem = new TB_GuestBook
                            {
                                Invitation_ID = InvitationId,
                                Name = Name,
                                Password = Password,
                                Message = Message,
                                Regist_IP = ip,
                                Regist_DateTime = DateTime.Now
                            };

                        
                            barunson.TB_GuestBooks.Add(addItem);
                            await barunson.SaveChangesAsync();

                            result.status = true;
                            result.message = "등록되었습니다.";
                        }
                    }
                    else
                    {
                        result.message = "이용기간이 만료되었거나 비공개된 초대장입니다";
                    }
                }
            }
            catch
            {

            }
            return Json(result);
        }

        /// <summary>
        /// 방명록 삭제
        /// </summary>
        /// <param name="InvitationId"></param>
        /// <param name="idx"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [Route("m2/GuestBookDelete")]
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GuestBookDelete(int InvitationId, int idx, string Password)
        {
            var result = new JsonReusltStatusModel { status = false, message = "비밀번호가 올바르지 않거나 <br> 정보가 잘못되었습니다." };
            try
            {
                if (InvitationId > 0)
                {
                    var delQuery = from a in barunson.TB_GuestBooks
                                   where a.Invitation_ID == InvitationId && a.GuestBook_ID == idx
                                   && a.Password == Password
                                   select a;

                    var delItem = await delQuery.FirstOrDefaultAsync();

                    if (delItem != null)
                    {
                        barunson.TB_GuestBooks.Remove(delItem);
                        await barunson.SaveChangesAsync();
                        result.status = true;
                        result.message = "삭제 되었습니다.";
                    }
                }
            }
            catch
            { }
            return Json(result);
        }

        /// <summary>
        /// 방명록
        /// </summary>
        /// <param name="InvitationId"></param>
        /// <param name="Type"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        [Route("m2/GuestBook")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GuestBook(int InvitationId, string Type, int idx)
        {
            var model = new List<M2GuestBookListModel>();
            List<TB_GuestBook> GuestBooks;
            var query = from a in barunson.TB_GuestBooks
                        where a.Invitation_ID == InvitationId && a.Display_YN == "Y"
                        && (idx == 0 || a.GuestBook_ID < idx)
                        orderby a.GuestBook_ID descending
                        select a;
            if (Type == "all")
                GuestBooks = await query.ToListAsync();
            else
                GuestBooks = await query.Take(10).ToListAsync();

            foreach(var g in GuestBooks)
            {
                model.Add(new M2GuestBookListModel
                {
                    GuestBookID = g.GuestBook_ID,
                    Message= g.Message,
                    Name= g.Name,
                    RegistDatetime = g.Regist_DateTime.Value
                });
            }
            ViewData["InvitationId"] = InvitationId;
            return View("GuestBookList_Partial", model);
        }
        #endregion

        #region Private 함수

        #region 공통

        /// <summary>
        /// 초대장 표시 여부
        /// 만료, 결제등 확인
        /// </summary>
        /// <param name="id"></param>
        /// <param name="i"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CheckDisplay(TB_Invitation_Detail id, TB_Invitation i, TB_Order o)
        {

            //표시 여부
            var disp = id.Invitation_Display_YN == "Y" && i.Invitation_Display_YN == "Y";

            //결제 여부확인
            //결제 완료는 모두 표시
            //결제 취소는 모두 표시 않함.
            //본인 로그인시는 취소가 아닐경우 표시
            var paystatus = false;
            if (o.Payment_Status_Code == "PSC02")
                paystatus = true;
            else if (o.Payment_Status_Code == "PSC03" || o.Payment_Status_Code == "PSC05")
            {
                paystatus = false;
            }
            else if (User.Identity.IsAuthenticated)
            {
                string UserID = User.FindFirst("Id").Value;
                if (UserID == o.User_ID)
                {
                    paystatus = true;
                }
            }

            //청첩장, 돌잔치 - 예식일(행사일)로부터 3개월이 지나거나 
            //감사장 - 구매일 기준로부터 3개월이 지나거나
            var checkday = false;
            DateTime NowDate = DateTime.Now.Date;
            DateTime checkDate = o.Regist_DateTime.Value;
            if (o.Order_DateTime.HasValue)
                checkDate = o.Order_DateTime.Value;

            if (!string.IsNullOrEmpty(id.WeddingDate))
                DateTime.TryParse(id.WeddingDate, out checkDate);
            checkday = checkDate.AddMonths(3) > NowDate;

            //모든 조건이 True 일경우 표시
            return disp & paystatus & checkday;

        }

        /// <summary>
        /// Area 모델 생성,
        /// 각 리소스의 선언된 변수를 실제 데이터로 변경하여 결과 출력.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="id"></param>
        /// <param name="reserveWords"></param>
        /// <returns></returns>
        private async Task<List<M2AreaViewModel>> GetAreas(TB_Invitation i, TB_Invitation_Detail id, Dictionary<string, string> reserveWords)
        {
            #region Area 
            var Areas = await(from a in barunson.TB_Invitation_Areas
                              where a.Invitation_ID == i.Invitation_ID
                              select new M2AreaViewModel
                              {
                                  AreaID = a.Area_ID,
                                  Height = a.Size_Height.Value,
                                  Width = a.Size_Width.Value,
                                  Color = a.Color
                              }).ToListAsync();

            var AreaItems = await(from ii in barunson.TB_Invitation_Items
                                  join r in barunson.TB_Item_Resources on ii.Resource_ID equals r.Resource_ID
                                  where ii.Invitation_ID == i.Invitation_ID
                                  select new 
                                  {
                                      AreaID = ii.Area_ID.Value,
                                      Sort = r.Sort ?? 0,
                                      itemId = ii.Item_ID,
                                      ResourceId = ii.Resource_ID,
                                      ItemType = ii.Item_Type_Code,
                                      Top = ii.Location_Top,
                                      Left = ii.Location_Left,
                                      Height = ii.Size_Height,
                                      Width = ii.Size_Width,
                                      Text = r.CharacterSet,
                                      Font = r.Font,
                                      FontSize = r.Character_Size,
                                      FontColor = r.Color,
                                      BackgroundColor = r.Background_Color,
                                      IsBold = (r.Bold_YN == "Y"),
                                      IsItalic = (r.Italic_YN == "Y"),
                                      IsUnderline = (r.Underline_YN == "Y"),
                                      LetterSpacing = r.BetweenText,
                                      LineHeight = r.BetweenLine,
                                      VerticalAlign = r.Vertical_Alignment,
                                      HorizontalAlign = r.Horizontal_Alignment,
                                      ResourceUrl = r.Resource_URL
                                  }).ToListAsync();

            foreach (var Area in Areas)
            {
                Area.Items = new List<M2AreaItemViewModel>();
                var aitems = AreaItems.Where(m => m.AreaID == Area.AreaID).OrderBy(x => x.Sort);
                foreach (var aitem in aitems)
                {
                    var areaitem = new M2AreaItemViewModel
                    {
                        Sort = aitem.Sort,
                        itemId = aitem.itemId,
                        ResourceId = aitem.ResourceId,
                        ItemType = aitem.ItemType,
                        Top = aitem.Top,
                        Left = aitem.Left,
                        Height = aitem.Height,
                        Width = aitem.Width,
                        Font = aitem.Font,
                        FontSize = aitem.FontSize,
                        FontColor = aitem.FontColor,
                        BackgroundColor = aitem.BackgroundColor,
                        IsBold = aitem.IsBold,
                        IsItalic = aitem.IsItalic,
                        IsUnderline = aitem.IsUnderline,
                        LetterSpacing = aitem.LetterSpacing,
                        LineHeight = aitem.LineHeight,
                        ResourceUrl = aitem.ResourceUrl,
                        Text = aitem.Text
                    };
                    // 템플릿 Text 변환
                    if (!string.IsNullOrEmpty(aitem.Text) && aitem.Text.Contains('#'))
                    {
                        var word = aitem.Text;
                        word = reserveWords.Aggregate(word, (result, s) => result.Replace(s.Key, s.Value));

                        areaitem.Text = word;
                    }
                    //Content URL 설정
                    if (!string.IsNullOrEmpty(aitem.ResourceUrl))
                    {
                        var resUrl = aitem.ResourceUrl;

                        //photo 일경우 TB_Invitation_Detail의 Delegate_Image_URL을 사용함. 리소스의 이미지는 템플릿 이미지 임..
                        if (aitem.ItemType == "ITC03" && !string.IsNullOrEmpty(id.Delegate_Image_URL)) 
                            resUrl = id.Delegate_Image_URL;

                        areaitem.ResourceAbsoluteUrl = GetResourceAbsoluteUrl(resUrl);

                    }

                    //표시정렬
                    if (!string.IsNullOrEmpty(aitem.VerticalAlign))
                    {
                        if (aitem.VerticalAlign == "T") areaitem.VerticalAlign = "flex-start";
                        else if (aitem.VerticalAlign == "M") areaitem.VerticalAlign = "center";
                        else if (aitem.VerticalAlign == "B") areaitem.VerticalAlign = "flex-end";
                    }
                    if (!string.IsNullOrEmpty(aitem.HorizontalAlign))
                    {
                        if (aitem.HorizontalAlign == "C") areaitem.HorizontalAlign = "center";
                        else if (aitem.HorizontalAlign == "R") areaitem.HorizontalAlign = "right";
                        else if (aitem.HorizontalAlign == "L") areaitem.HorizontalAlign = "left";
                    }

                    Area.Items.Add(areaitem);
                }
            }
            #endregion

            return Areas;
        }

        /// <summary>
        /// 선언된 변수 텍스트에 대한 실제 입력 정보 텍스트를 키,값 형식으로 출력
        /// TB_Invitation_Detail에 포함된 데이터만 가능
        /// </summary>
        /// <remarks>
        /// 확장성을 위하여 데이터 구조 재설계 필요
        /// </remarks>
        /// <returns></returns>
        private async Task<Dictionary<string,string>> GetReserveWord(string ProductCategoryCode, 
            TB_Invitation_Detail detailData)
        {
            var result = new Dictionary<string, string>();
            var query = from m in barunson.TB_ReservationWords
                        where m.Product_Category_Codes.Contains(ProductCategoryCode)
                        && m.Mapping_YN == "Y"
                        select new { m.ReserveWord, m.MappingField };
            var items = await query.ToListAsync();

           
            foreach ( var item in items )
            {
                if (result.ContainsKey($"#{item.ReserveWord}#"))
                    continue;

                var value = item.ReserveWord;
                //각 ReserveWord 에 해당하는 Data 값 설정...  
                var prop = detailData.GetType().GetProperty(item.MappingField);
                if ( prop != null )
                {
                    value = prop.GetValue(detailData, null)?.ToString();
                    if (value != null)
                    {
                        value = value.Replace("\n", "<br>");

                        #region 타임,요일 영문 -> 하드코딩으로 밖에 적용 못함.. DB 구조 개선필요.
                        if (!string.IsNullOrEmpty(value) && item.MappingField == "WeddingWeek" && detailData.WeddingWeek_Eng_YN == "Y")
                        {
                            switch (value)
                            {
                                case "월":
                                    value = "MON";
                                    break;
                                case "화":
                                    value = "TUE";
                                    break;
                                case "수":
                                    value = "WED";
                                    break;
                                case "목":
                                    value = "THU";
                                    break;
                                case "금":
                                    value = "FRI";
                                    break;
                                case "토":
                                    value = "SAT";
                                    break;
                                case "일":
                                    value = "SUN";
                                    break;
                            }
                        }
                        if (!string.IsNullOrEmpty(value) && item.MappingField == "Time_Type_Code" && detailData.Time_Type_Eng_YN == "Y")
                        {
                            value = (value == "오후") ? "PM" : "AM";
                        }
                        #endregion
                    }
                    result.Add($"#{item.ReserveWord}#", value);
                }
                else
                {
                    //D-DAY는 이벤트 날짜로 계산 해야 됨.
                    if (item.ReserveWord == "D-DAY" && !string.IsNullOrEmpty(detailData.WeddingDate))
                    {
                        var today = DateTime.Today;
                        var eventDate = DateTime.Parse(detailData.WeddingDate);
                        var ddayvalue = "";
                        if (today == eventDate)
                            ddayvalue = "D-Day";
                        else if (today < eventDate)
                            ddayvalue = $"D-{(eventDate - today).Days}";

                        result.Add($"#{item.ReserveWord}#", ddayvalue);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 연락처 추가
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private M2ContectViewModel GetContect(TB_Invitation_Detail model)
        {
            var result = new M2ContectViewModel();
            var gPhone = (model.Groom_Global_Phone_YN == "Y") ? model.Groom_Global_Phone_Number + model.Groom_Phone : model.Groom_Phone;
            if (!string.IsNullOrWhiteSpace(gPhone))
            {
                result.MainContects.Add(new M2ContectViewItemModel
                {
                    Title = "신랑에게 연락하기",
                    Name = model.Groom_Name,
                    TelNo = gPhone
                });
            }
            var bPhone = (model.Bride_Global_Phone_YN == "Y") ? model.Bride_Global_Phone_Number + model.Bride_Phone : model.Bride_Phone;
            if (!string.IsNullOrWhiteSpace(bPhone))
            {
                result.MainContects.Add(new M2ContectViewItemModel
                {
                    Title ="신부에게 연락하기",
                    Name = model.Bride_Name,
                    TelNo = bPhone
                });
            }
            if (model.Parents_Information_Use_YN== "Y")
            {
                var gp1Phone = (model.Groom_Parents1_Global_Phone_Number_YN == "Y") ? model.Groom_Parents1_Global_Phone_Number + model.Groom_Parents1_Phone : model.Groom_Parents1_Phone;
                if (!string.IsNullOrWhiteSpace(gp1Phone) || !string.IsNullOrWhiteSpace(model.Groom_Parents1_Name))
                {
                    result.Extra1Contects.Add(
                        new M2ContectViewItemModel {
                            Title = (model.Groom_Parents1_Title == "입력안함") ? "" : model.Groom_Parents1_Title,
                            Name = model.Groom_Parents1_Name,
                            TelNo = gp1Phone,
                        }
                    );
                }
                var gp2Phone = (model.Groom_Parents2_Global_Phone_Number_YN == "Y") ? model.Groom_Parents2_Global_Phone_Number + model.Groom_Parents2_Phone : model.Groom_Parents2_Phone;
                if (!string.IsNullOrWhiteSpace(gp2Phone) || !string.IsNullOrWhiteSpace(model.Groom_Parents2_Name))
                {
                    result.Extra1Contects.Add(
                        new M2ContectViewItemModel
                        {
                            Title = (model.Groom_Parents2_Title == "입력안함") ? "" : model.Groom_Parents2_Title,
                            Name= model.Groom_Parents2_Name,
                            TelNo = gp2Phone,
                        }
                    );
                }
                var bp1Phone = (model.Bride_Parents1_Global_Phone_Number_YN == "Y") ? model.Bride_Parents1_Global_Phone_Number + model.Bride_Parents1_Phone : model.Bride_Parents1_Phone;
                if (!string.IsNullOrWhiteSpace(bp1Phone) || !string.IsNullOrWhiteSpace(model.Bride_Parents1_Name))
                {
                    result.Extra2Contects.Add(
                        new M2ContectViewItemModel
                        {
                            Title = (model.Bride_Parents1_Title == "입력안함") ? "" : model.Bride_Parents1_Title,
                            Name = model.Bride_Parents1_Name,
                            TelNo = bp1Phone,
                        }
                    );
                }

                var bp2Phone = (model.Bride_Parents2_Global_Phone_Number_YN == "Y") ? model.Bride_Parents2_Global_Phone_Number + model.Bride_Parents2_Phone : model.Bride_Parents2_Phone;
                if (!string.IsNullOrWhiteSpace(bp2Phone) || !string.IsNullOrWhiteSpace(model.Bride_Parents2_Name))
                {
                    result.Extra2Contects.Add(
                        new M2ContectViewItemModel
                        {
                            Title = (model.Bride_Parents2_Title == "입력안함") ? "" : model.Bride_Parents2_Title,
                            Name= model.Bride_Parents2_Name,
                            TelNo = bp2Phone,
                        }
                    );
                }

            }
            return result;
        }

        /// <summary>
        /// 갤러리 Area 모델 생성
        /// </summary>
        /// <param name="invitaionId"></param>
        /// <param name="galleryTypeCode"></param>
        /// <param name="titelArea"></param>
        /// <returns></returns>
        private async Task<M2GalleryViewModel> GetGallery(int invitaionId, string galleryTypeCode, M2AreaViewModel titelArea)
        {
            var result = new M2GalleryViewModel
            {
                GalleryTypeCode = galleryTypeCode,
                GalleryTitleArea = titelArea,
                GalleryItems = new Dictionary<int, M2ImageInfoViewModel>()
            };
            //갤러리 아이템 채우기
            var sortid = 0;

            var query = from a in barunson.TB_Galleries
                        where a.Invitation_ID == invitaionId
                        orderby a.Sort ascending, a.Gallery_ID ascending
                        select new
                        {
                            a.Sort,
                            a.Image_URL,
                            a.Image_Height,
                            a.Image_Width
                        };
            var items = await query.ToListAsync();

            foreach (var item in items)
            {
                var image = new M2ImageInfoViewModel
                {
                    Width = item.Image_Width,
                    Height = item.Image_Height
                };
                image.ImageUrl = GetResourceAbsoluteUrl(item.Image_URL);

                result.GalleryItems.Add(sortid, image);

                sortid++;
            }
            return result;
        }

        /// <summary>
        /// 동영상 Area 모델 생성
        /// </summary>
        /// <param name="invitationVideoUrl"></param>
        /// <param name="videoTypeCode"></param>
        /// <param name="titelArea"></param>
        /// <returns></returns>
        private M2VideoViewModel GetVideo(string invitationVideoUrl, string videoTypeCode,  M2AreaViewModel titelArea)
        {
            var result = new M2VideoViewModel
            {
                VideoTypeCode = videoTypeCode,
                VideoTitleArea = titelArea,
                VideoUri = invitationVideoUrl
            };
            var videourl = invitationVideoUrl;
            if (videoTypeCode == "VTC01")    //Youtube
            {
                Regex r = new Regex(@"youtu(?:\.be|be\.com)/(?:shorts/)?(?:watch\?v=)?([a-zA-Z0-9-_]+)",
                         RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(150));
                Match m = r.Match(videourl);
                if (m.Success)
                {
                    var replacement = "<iframe src=\"https://www.youtube.com/embed/{0}\" frameborder=\"0\" class=\"embed-container\" allowfullscreen></iframe>";
                    videourl = string.Format(replacement, m.Groups[1].Value);
                }

            }
            else
            {
                //"VTC02" Vimeo, "VTC03" FEELMAKER
                // 데이터에 iframe 으로 경로가 입력되어 있음.
                if (videourl.StartsWith("<iframe") && !videourl.EndsWith("/iframe>"))
                    videourl += "</iframe>";

            }
            result.VideoUri = videourl;

            return result;
        }
        
        /// <summary>
        /// 오시는길 Area 모델 생성
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="titelArea"></param>
        /// <returns></returns>
        private M2LocationVIewModel GetLocation(TB_Invitation_Detail detail, M2AreaViewModel titelArea)
        {
            //행사장 이름, 상세, 전화번호, 주소값이 없으면 Null 리턴
            if (string.IsNullOrWhiteSpace(detail.Weddinghall_Name) && string.IsNullOrWhiteSpace(detail.WeddingHallDetail) &&
                string.IsNullOrWhiteSpace(detail.Weddinghall_Address) && string.IsNullOrWhiteSpace(detail.Weddinghall_PhoneNumber))
            {
                return null;
            }

            var result = new M2LocationVIewModel
            {
                OutlineTypeCode = detail.Outline_Type_Code,
                LocationTitleArea = titelArea,
                Name = detail.Weddinghall_Name,
                DetailName = detail.WeddingHallDetail,
                IsDetailNewLine = detail.DetailNewLineYN == "Y",
                Address = detail.Weddinghall_Address,
                TelNo = detail.Weddinghall_PhoneNumber,
                Lat = detail.Location_LAT,
                Lot = detail.Location_LOT,
                LocationUrl = null
            };
            if (detail.Outline_Type_Code == "OTC01" && !string.IsNullOrWhiteSpace(detail.Weddinghall_Address)) //네이버 지도
            {
                result.LocationUrl = new Uri(
                    SiteUri,
                    Url.Action("NaverMap", "M2",
                    new
                    {
                        lot = detail.Location_LOT ?? 0,
                        lat = detail.Location_LAT ?? 0,
                        name = detail.Weddinghall_Name
                    }
                    ));
            }
            else if (!string.IsNullOrEmpty(detail.Outline_Image_URL))
            {
                result.LocationUrl = GetResourceAbsoluteUrl(detail.Outline_Image_URL);
            }

            return result;
        }

        /// <summary>
        /// 기타정보
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<TB_Invitation_Detail_Etc>> GetETCs(int id)
        {
            var query = from a in barunson.TB_Invitation_Detail_Etcs
                        where a.Invitation_ID == id
                        orderby a.Sort
                        select a;
            return await query.ToListAsync();
        }

        /// <summary>
        /// 계좌번호 Area 모델 생성
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="isUser"></param>
        /// <param name="titelArea"></param>
        /// <returns></returns>
        private async Task<M2AccountViewModel> GetAccount(TB_Invitation_Detail detail, bool isUser, M2AreaViewModel titelArea)
        {
            if ((detail.MoneyAccount_Div_Use_YN == "Y") || (detail.MoneyAccount_Remit_Use_YN == "Y") ||
                (isUser && (detail.MoneyGift_Remit_Use_YN == "Y")))
            {
                var result = new M2AccountViewModel
                {
                    AccountTitleArea = titelArea,
                    UseAccountDiv = (detail.MoneyAccount_Div_Use_YN == "Y"),
                    UseAccountRemit = (detail.MoneyAccount_Remit_Use_YN == "Y"),
                    UseGiftRemit = isUser && (detail.MoneyGift_Remit_Use_YN == "Y"),
                    Accounts = await GetRemitAccounts(detail.Invitation_ID),
                    GiftRemitUrl = new Uri(Url.ActionLink("RemitAccounts", "KakaoRemit", new { id = detail.Invitation_ID }))
                };

                return result;
            }
            else
                return null;
          
        }
        /// <summary>
        /// 계좌 번호 목록 모델 생성
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<M2AccountListModel>> GetRemitAccounts(int id)
        {
            var Accounts = new List<M2AccountListModel>();


            //신랑, 신부 계좌 번호
            var query1 = from a in barunson.TB_Invitation_Accounts
                         join b in barunson.TB_Banks on a.Bank_Code equals b.Bank_Code into g
                         from b in g.DefaultIfEmpty()
                         where a.Invitation_ID == id 
                         select new M2AccountListModel
                         {
                             Category = a.Category,
                             Sort = a.Sort,
                             Name = a.Send_Name,
                             BankName = b.Bank_Name ?? "",
                             AccountNumber = a.Account_Number,
                             AccountHolder = a.Account_Holder
                         };
            Accounts.AddRange(await query1.ToListAsync());

            //기타 계좌
            var query2 = from a in barunson.TB_Account_Extras
                         join b in barunson.TB_Banks on a.Bank_Code equals b.Bank_Code into g
                         from b in g.DefaultIfEmpty()
                         where a.Invitation_ID == id 
                         select new M2AccountListModel
                         {
                             Category = -1,
                             Sort = a.Sort,
                             Name = a.Send_Name,
                             BankName = b.Bank_Name ?? "",
                             AccountNumber = a.Account_Number,
                             AccountHolder = a.Account_Holder
                         };
            Accounts.AddRange(await query2.ToListAsync());

            return Accounts;
        }

        /// <summary>
        /// 리소스 절대 URL 생성 -
        /// 이미지등 정적 콘텐츠
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Uri GetResourceAbsoluteUrl(string url)
        {
            Uri result = null;
            if (url.StartsWith("/upload/", StringComparison.InvariantCultureIgnoreCase))
                result = new Uri(CdnUri, url);
            else if (url.StartsWith("/img/", StringComparison.InvariantCultureIgnoreCase))
                result = new Uri(SiteUri, url);
            else if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                result = new Uri(url);
           
            return result;
        }
        #endregion

        #region 청첩장 PCC01
        /// <summary>
        /// 청첩장 PCC01 뷰 모델 생성
        /// </summary>
        /// <param name="InvitaionId"></param>
        /// <param name="areas"></param>
        /// <param name="item"></param>
        /// <param name="IsUser"></param>
        /// <returns></returns>
        private async Task<PCC01ViewModel> GetPCC01ViewModel(int InvitaionId, List<M2AreaViewModel> areas, M2DBDataModel item, bool IsUser)
        {
            //청첩장 모델 작성
            var model = new PCC01ViewModel();
            //커버 모델
            model.CoverArea = areas.FirstOrDefault(m => m.AreaID == 1);
            //인사말 모델
            model.GreetingsArea = areas.FirstOrDefault(m => m.AreaID == 2);
            //연락처 모델
            model.ContectArea = GetContect(item.InvitationDetail);

            //갤러리 모델
            if (item.InvitationDetail.Gallery_Use_YN == "Y")
            {
                model.GalleryArea = await GetGallery(InvitaionId, item.InvitationDetail.Gallery_Type_Code, areas.FirstOrDefault(m => m.AreaID == 5));
            }

            //동영상 모델
            if (item.InvitationDetail.Invitation_Video_Use_YN == "Y")
            {
                model.VideoArea = GetVideo(item.InvitationDetail.Invitation_Video_URL, item.InvitationDetail.Invitation_Video_Type_Code, areas.FirstOrDefault(m => m.AreaID == 7));
            }

            //오시는길 모델
            model.LocationArea = GetLocation(item.InvitationDetail, areas.FirstOrDefault(m => m.AreaID == 9));

            //기타 정보
            if (item.InvitationDetail.Etc_Information_Use_YN == "Y")
            {
                model.ETCs = await GetETCs(InvitaionId);
            }

            //계좌 번호
            model.AccountArea = await GetAccount(item.InvitationDetail, IsUser, areas.FirstOrDefault(m => m.AreaID == 12));

            //화환선물
            //웨딩홀 이름, 주소가 있을 경우만 표시
            if (item.InvitationDetail.Flower_gift_YN == "Y"  && !string.IsNullOrWhiteSpace(item.InvitationDetail.Weddinghall_Address))
            {
                model.FlowerGiftUri = new Uri($"https://www.barunsonflower.com/?barunid={item.Order.Order_ID}");
            }

            //방명록
            if (item.InvitationDetail.GuestBook_Use_YN == "Y")
            {
                model.GuestBookArea = new M2GuestBookViewModel
                {
                    InvitaionId = InvitaionId,
                    GuestBookTitleArea = areas.FirstOrDefault(m => m.AreaID == 14),
                };
            }

            return model;
        }
        #endregion

        #region 감사장 PCC02
        /// <summary>
        /// 감사장 PCC02 뷰 모델 생성
        /// </summary>
        /// <param name="InvitaionId"></param>
        /// <param name="areas"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private PCC02ViewModel GetPCC02ViewModel(int InvitaionId, List<M2AreaViewModel> areas, M2DBDataModel item)
        {
            return new PCC02ViewModel
            {
                CoverArea = areas.FirstOrDefault(m => m.AreaID == 1),
                GreetingsArea = areas.FirstOrDefault(m => m.AreaID == 2),
                ContectArea = GetContect(item.InvitationDetail)
            };
        }
        #endregion

        #region 돌잔치 PCC03
        /// <summary>
        /// 돌잔치 PCC03 뷰 모델 생성
        /// </summary>
        /// <param name="InvitaionId"></param>
        /// <param name="areas"></param>
        /// <param name="item"></param>
        /// <param name="IsUser"></param>
        /// <returns></returns>
        private async Task<PCC03ViewModel> GetPCC03ViewModel(int InvitaionId, List<M2AreaViewModel> areas, M2DBDataModel item, bool IsUser)
        {
            var model = new PCC03ViewModel();
            //커버 모델
            model.CoverArea = areas.FirstOrDefault(m => m.AreaID == 1);
            //인사말 모델
            model.GreetingsArea = areas.FirstOrDefault(m => m.AreaID == 2);

            #region 아기정보 모델
            var babyInfos = JsonConvert.DeserializeObject<List<BabyFirstBirthViewModel>>(item.InvitationDetail.ExtendData);
            var babyInfosViewModel = new List<M2BabyInfoModel>();
            foreach (var babyInfo in babyInfos.OrderBy(m => m.idx))
            {
                var titleArea = areas.FirstOrDefault(m => m.AreaID == 17);

                var m2BabyInfoModel = new M2BabyInfoModel
                {
                    idx = babyInfo.idx,
                    TitleArea = GetPCC03Area(17, areas, babyInfo),
                    BodyArea = GetPCC03Area(18, areas, babyInfo),
                    infoArea = new List<M2AreaViewModel>(),
                    FooterArea = GetPCC03Area(20, areas, babyInfo),
                };
                //areaid 19, 아기 항목은 리스트로 Area를 복사하여 사용
                if (babyInfo.ExtraInfos != null)
                {
                    var area19 = areas.FirstOrDefault(m => m.AreaID == 19);
                    foreach (var extraInfo in babyInfo.ExtraInfos)
                    {
                        var cloneArea = area19.Copy();
                        foreach (var aitem in cloneArea.Items)
                        {
                            // 템플릿 Text 변환
                            if (!string.IsNullOrEmpty(aitem.Text) && aitem.Text.Contains('#') && aitem.ItemType == "ITC01")
                            {
                                var word = aitem.Text;
                                word = word.Replace("#아기정보키#", extraInfo.Title);
                                word = word.Replace("#아기정보값#", extraInfo.Value);
                                aitem.Text = word;
                            }

                        }
                        m2BabyInfoModel.infoArea.Add(cloneArea);
                    }
                }
                babyInfosViewModel.Add(m2BabyInfoModel);
            }
            model.BabyInfosArea = babyInfosViewModel;
            #endregion

            //연락처 모델
            model.ContectArea = GetPCC03Contect(item.InvitationDetail);

            //갤러리 모델
            if (item.InvitationDetail.Gallery_Use_YN == "Y")
            {
                model.GalleryArea = await GetGallery(InvitaionId, item.InvitationDetail.Gallery_Type_Code, areas.FirstOrDefault(m => m.AreaID == 5));
            }
            //동영상 모델
            if (item.InvitationDetail.Invitation_Video_Use_YN == "Y")
            {
                model.VideoArea = GetVideo(item.InvitationDetail.Invitation_Video_URL, item.InvitationDetail.Invitation_Video_Type_Code, areas.FirstOrDefault(m => m.AreaID == 7));
            }

            //오시는길 모델
            model.LocationArea = GetLocation(item.InvitationDetail, areas.FirstOrDefault(m => m.AreaID == 9));

            //기타 정보
            if (item.InvitationDetail.Etc_Information_Use_YN == "Y")
            {
                model.ETCs = await GetETCs(InvitaionId);
            }

            //계좌 번호
            model.AccountArea = await GetAccount(item.InvitationDetail, IsUser, areas.FirstOrDefault(m => m.AreaID == 12));

            //방명록
            if (item.InvitationDetail.GuestBook_Use_YN == "Y")
            {
                model.GuestBookArea = new M2GuestBookViewModel
                {
                    InvitaionId = InvitaionId,
                    GuestBookTitleArea = areas.FirstOrDefault(m => m.AreaID == 14),
                };
            }

            return model;
        }
        /// <summary>
        /// 돌잔치 연락처
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private M2ContectViewModel GetPCC03Contect(TB_Invitation_Detail model)
        {
            var result = new M2ContectViewModel();
            var gPhone = (model.Groom_Global_Phone_YN == "Y") ? model.Groom_Global_Phone_Number + model.Groom_Phone : model.Groom_Phone;
            if (!string.IsNullOrWhiteSpace(gPhone))
            {
                result.MainContects.Add(new M2ContectViewItemModel
                {
                    Title = "아빠에게 연락하기",
                    Name = model.Groom_Name,
                    TelNo = gPhone
                });
            }
            var bPhone = (model.Bride_Global_Phone_YN == "Y") ? model.Bride_Global_Phone_Number + model.Bride_Phone : model.Bride_Phone;
            if (!string.IsNullOrWhiteSpace(bPhone))
            {
                result.MainContects.Add(new M2ContectViewItemModel
                {
                    Title = "엄마에게 연락하기",
                    Name = model.Bride_Name,
                    TelNo = bPhone
                });
            }

            return result;
        }

        /// <summary>
        /// 아기정보 영역은 다른 기준으로 데이터 맵핑
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="areas"></param>
        /// <param name="babyInfo"></param>
        /// <returns></returns>
        private M2AreaViewModel GetPCC03Area(int areaId, List<M2AreaViewModel> areas, BabyFirstBirthViewModel babyInfo)
        {
            var area = areas.FirstOrDefault(m => m.AreaID == areaId);
            if (area != null)
            {
                //템플릿 Area를 List로 사용하기 위하여는 객체 복사가 필요
                var cloneArea = area.Copy();

                foreach (var aitem in cloneArea.Items)
                {
                    // 템플릿 Text 변환
                    if (!string.IsNullOrEmpty(aitem.Text) && aitem.Text.Contains('#') && aitem.ItemType == "ITC01")
                    {
                        var word = aitem.Text;
                        word = word.Replace("#아기이름#", babyInfo.Name);
                        word = word.Replace("#탄생일자#", babyInfo.Birthday.ToString("yyyy.MM.dd"));
                        word = word.Replace("#탄생년#", babyInfo.Birthday.ToString("yyyy"));
                        word = word.Replace("#탄생월#", babyInfo.Birthday.ToString("MM"));
                        word = word.Replace("#탄생일#", babyInfo.Birthday.ToString("dd"));
                        aitem.Text = word;
                    }
                    if (aitem.ItemType == "ITC04")
                    {
                        aitem.ResourceAbsoluteUrl = GetResourceAbsoluteUrl(babyInfo.Image_URL);
                    }
                }
                return cloneArea;
            }
            return area;
        }
        #endregion

        #endregion
    }
}
