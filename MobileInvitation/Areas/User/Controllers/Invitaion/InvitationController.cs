using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.Data.Invitation;
using MobileInvitation.Data.Product;
using MobileInvitation.Data.Template;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


namespace MobileInvitation.Areas.User.Controllers.Invitaion
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class InvitationController : PathController
    {
        private readonly ITemplateRepository _t_repository;
        private readonly IProductRepository _p_repository;
        private readonly IInvitationRepository _repository;


        public InvitationController(IInvitationRepository repository, ITemplateRepository t_repository, IProductRepository p_repository, 
            IWebHostEnvironment environment, IHttpContextAccessor accessor,  
            barunsonContext barunsonContext, BarunnConfig barunnConfig)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _repository = repository;
            _t_repository = t_repository;
            _p_repository = p_repository;
        }


        [Route("Invitation/{Invitation_ID?}/{Step?}")]
        public IActionResult Index(int Invitation_ID, int Step)
        {
            TB_Order model = new TB_Order();
            model.Previous_Order_ID = Invitation_ID;

            string User_ID = !string.IsNullOrEmpty(User.FindFirst("ID").Value) ? User.FindFirst("ID").Value : "";
            string User_Name = !string.IsNullOrEmpty(User.FindFirst("Name").Value) ? User.FindFirst("Name").Value : "";
            string User_Email = !string.IsNullOrEmpty(User.FindFirst("Email").Value) ? User.FindFirst("Email").Value : "";

            model.User_ID = User_ID;
            model.Name = User_Name;
            model.Email = User_Email;


            ViewData["User_ID"] = User_ID;
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl;

            int User_Invitation_YN = _p_repository.User_Order_Chk_Entity("2", model);

            if (User_Invitation_YN.Equals(0))
            {
                return LocalRedirect("/");
            }

            if (Invitation_ID > 0)
            {
                ViewData["TB_Area"] = JsonConvert.SerializeObject(_repository.TB_Invitation_Area_Entity(Invitation_ID));
                ViewData["Objects"] = JsonConvert.SerializeObject(_repository.TB_Invitation_Item_Resource_LIst(Invitation_ID));
                ViewData["ETCs"] = JsonConvert.SerializeObject(_repository.TB_Invitation_Detail_Etc_List(Invitation_ID));
                ViewData["Accounts"] = JsonConvert.SerializeObject(_repository.TB_Account_Extra_List(Invitation_ID));
                ViewData["GroomAccounts"] = JsonConvert.SerializeObject(_repository.TB_Invitation_Account_List(Invitation_ID, 1));
                ViewData["BrideAccounts"] = JsonConvert.SerializeObject(_repository.TB_Invitation_Account_List(Invitation_ID, 2));
                ViewData["Step"] = Step;

                var defaultInfo = _t_repository.TB_Template_Detail_Default_Info_New();
                ViewData["defaultInfos"] = defaultInfo;

                ViewBag.Product = _repository.TB_Product_By_Invitation_ID(Invitation_ID);

                ViewBag.Template = _t_repository.TB_Template_By_Invitation_ID(Invitation_ID);


                if (ViewBag.Product.Count > 0)
                {
                    ViewData["Product_Category_Code"] = ViewBag.Product[0].Product_Category_Code;
                }

                if (ViewBag.Template.Count > 0)
                {
                    ViewData["Attached_File1_URL"] = ViewBag.Template[0].Attached_File1_URL;
                    ViewData["Attached_File2_URL"] = ViewBag.Template[0].Attached_File2_URL;
                }

                ViewBag.Invitation_Detail = _repository.TB_Invitation_Detail_Entity(Invitation_ID);

                if (ViewBag.Invitation_Detail.Count > 0)
                {
                    ViewData["Invitation_Title"] = ViewBag.Invitation_Detail[0].Invitation_Title;


                    ViewData["Greetings"] = ViewBag.Invitation_Detail[0].Greetings;
                    ViewData["Groom_Name"] = ViewBag.Invitation_Detail[0].Groom_Name;
                    ViewData["Groom_Global_Phone_YN"] = ViewBag.Invitation_Detail[0].Groom_Global_Phone_YN;
                    ViewData["Groom_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Groom_Global_Phone_Number;
                    ViewData["Groom_Phone"] = ViewBag.Invitation_Detail[0].Groom_Phone;
                    ViewData["Bride_Name"] = ViewBag.Invitation_Detail[0].Bride_Name;
                    ViewData["Bride_Global_Phone_YN"] = ViewBag.Invitation_Detail[0].Bride_Global_Phone_YN;
                    ViewData["Bride_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Bride_Global_Phone_Number;
                    ViewData["Bride_Phone"] = ViewBag.Invitation_Detail[0].Bride_Phone;

                    ViewData["Groom_Parents1_Name"] = ViewBag.Invitation_Detail[0].Groom_Parents1_Name;
                    ViewData["Groom_Parents1_Global_Phone_Number_YN"] = ViewBag.Invitation_Detail[0].Groom_Parents1_Global_Phone_Number_YN;
                    ViewData["Groom_Parents1_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Groom_Parents1_Global_Phone_Number;
                    ViewData["Groom_Parents1_Phone"] = ViewBag.Invitation_Detail[0].Groom_Parents1_Phone;
                    ViewData["Groom_Parents2_Name"] = ViewBag.Invitation_Detail[0].Groom_Parents2_Name;
                    ViewData["Groom_Parents2_Global_Phone_Number_YN"] = ViewBag.Invitation_Detail[0].Groom_Parents2_Global_Phone_Number_YN;
                    ViewData["Groom_Parents2_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Groom_Parents2_Global_Phone_Number;
                    ViewData["Groom_Parents2_Phone"] = ViewBag.Invitation_Detail[0].Groom_Parents2_Phone;
                    ViewData["Bride_Parents1_Name"] = ViewBag.Invitation_Detail[0].Bride_Parents1_Name;
                    ViewData["Bride_Parents1_Global_Phone_Number_YN"] = ViewBag.Invitation_Detail[0].Bride_Parents1_Global_Phone_Number_YN;
                    ViewData["Bride_Parents1_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Bride_Parents1_Global_Phone_Number;
                    ViewData["Bride_Parents1_Phone"] = ViewBag.Invitation_Detail[0].Bride_Parents1_Phone;
                    ViewData["Bride_Parents2_Name"] = ViewBag.Invitation_Detail[0].Bride_Parents2_Name;
                    ViewData["Bride_Parents2_Global_Phone_Number_YN"] = ViewBag.Invitation_Detail[0].Bride_Parents2_Global_Phone_Number_YN;
                    ViewData["Bride_Parents2_Global_Phone_Number"] = ViewBag.Invitation_Detail[0].Bride_Parents2_Global_Phone_Number;
                    ViewData["Bride_Parents2_Phone"] = ViewBag.Invitation_Detail[0].Bride_Parents2_Phone;

                    if (ViewBag.invitation_detail[0].WeddingDate == "1900-01-01")
                    {
                        ViewData["WeddingDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                        string[] WDArray = ViewData["WeddingDate"].ToString().Split('-');
                        string WeddingYY = WDArray[0];
                        string WeddingMM = WDArray[1];
                        string WeddingDD = WDArray[2];
                        ViewData["WeddingYY"] = WeddingYY;
                        ViewData["WeddingMM"] = WeddingMM;
                        ViewData["WeddingDD"] = WeddingDD;
                        ViewData["WeddingWeek"] = getDayOfWeek(DateTime.Now);
                    }
                    else
                    {
                        ViewData["WeddingDate"] = ViewBag.invitation_detail[0].WeddingDate;
                        ViewData["WeddingYY"] = ViewBag.invitation_detail[0].WeddingYY;
                        ViewData["WeddingMM"] = ViewBag.invitation_detail[0].WeddingMM;
                        ViewData["WeddingDD"] = ViewBag.invitation_detail[0].WeddingDD;
                        ViewData["WeddingWeek"] = ViewBag.invitation_detail[0].WeddingWeek;

                    }
                    ViewData["Time_Type_Code"] = ViewBag.Invitation_Detail[0].Time_Type_Code;
                    ViewData["Time_Type_Eng_YN"] = ViewBag.Invitation_Detail[0].Time_Type_Eng_YN;
                    if (ViewData["Time_Type_Eng_YN"].ToString() == "Y")
                    {
                        //영문
                        ViewData["Time_Type_Name"] = ViewData["Time_Type_Code"].ToString() == "오후" ? "PM" : "AM";
                    }
                    else
                    {
                        //한글
                        ViewData["Time_Type_Name"] = ViewData["Time_Type_Code"].ToString();
                    }

                    ViewData["WeddingWeek_Eng_YN"] = ViewBag.Invitation_Detail[0].WeddingWeek_Eng_YN;
                    if (ViewData["WeddingWeek_Eng_YN"].ToString() == "Y")
                    {
                        if (ViewData["WeddingWeek"] != null)
                        {
                            //영문
                            switch (ViewData["WeddingWeek"].ToString())
                            {
                                case "월":
                                    ViewData["WeddingWeekName"] = "MON";
                                    break;
                                case "화":
                                    ViewData["WeddingWeekName"] = "THE";
                                    break;
                                case "수":
                                    ViewData["WeddingWeekName"] = "WED";
                                    break;
                                case "목":
                                    ViewData["WeddingWeekName"] = "THU";
                                    break;
                                case "금":
                                    ViewData["WeddingWeekName"] = "FRI";
                                    break;
                                case "토":
                                    ViewData["WeddingWeekName"] = "SAT";
                                    break;
                                case "일":
                                    ViewData["WeddingWeekName"] = "SUN";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //한글
                        ViewData["WeddingWeekName"] = ViewData["WeddingWeek"].ToString();
                    }

                    
                    ViewData["WeddingHour"] = ViewBag.Invitation_Detail[0].WeddingHour;
                    ViewData["WeddingMin"] = ViewBag.Invitation_Detail[0].WeddingMin;
                    ViewData["D-DAY"] = GetDDay((string)ViewData["WeddingDate"]);

                    ViewData["Weddinghall_Name"] = ViewBag.Invitation_Detail[0].Weddinghall_Name;
                    ViewData["WeddingHallDetail"] = ViewBag.Invitation_Detail[0].WeddingHallDetail;
                    ViewData["Weddinghall_Address"] = ViewBag.Invitation_Detail[0].Weddinghall_Address;
                    ViewData["Weddinghall_PhoneNumber"] = ViewBag.Invitation_Detail[0].Weddinghall_PhoneNumber;

                    ViewData["Outline_Type_Code"] = ViewBag.invitation_detail[0].Outline_Type_Code;
                    ViewData["Outline_Image_URL"] = ViewBag.invitation_detail[0].Outline_Image_URL;

                    ViewData["GuestBook_Use_YN"] = ViewBag.invitation_detail[0].GuestBook_Use_YN;
                    ViewData["Etc_Information_Use_YN"] = ViewBag.invitation_detail[0].Etc_Information_Use_YN;
                    ViewData["Parents_Information_Use_YN"] = ViewBag.invitation_detail[0].Parents_Information_Use_YN;
                    ViewData["MoneyGift_Remit_Use_YN"] = ViewBag.invitation_detail[0].MoneyGift_Remit_Use_YN;
                    ViewData["MoneyAccount_Remit_Use_YN"] = ViewBag.invitation_detail[0].MoneyAccount_Remit_Use_YN;
                    ViewData["MoneyAccount_Div_Use_YN"] = ViewBag.invitation_detail[0].MoneyAccount_Div_Use_YN;
                    ViewData["Flower_gift_YN"] = (ViewBag.invitation_detail[0].Flower_gift_YN == "C") ? "N": ViewBag.invitation_detail[0].Flower_gift_YN;

                    ViewData["Invitation_Video_Use_YN"] = ViewBag.invitation_detail[0].Invitation_Video_Use_YN;
                    ViewData["Gallery_Use_YN"] = ViewBag.invitation_detail[0].Gallery_Use_YN;
                    ViewData["Gallery_Type_Code"] = ViewBag.invitation_detail[0].Gallery_Type_Code;
                    ViewData["Invitation_Video_Type_Code"] = ViewBag.invitation_detail[0].Invitation_Video_Type_Code;
                    ViewData["Invitation_Video_URL"] = ViewBag.invitation_detail[0].Invitation_Video_URL;

                    ViewData["SNS_Image_URL"] = ViewBag.invitation_detail[0].SNS_Image_URL;
                    ViewData["SNS_Image_Height"] = ViewBag.invitation_detail[0].SNS_Image_Height;
                    ViewData["SNS_Image_Width"] = ViewBag.invitation_detail[0].SNS_Image_Width;

                    ViewData["Delegate_Image_URL"] = ViewBag.invitation_detail[0].Delegate_Image_URL;
                    ViewData["Delegate_Image_Height"] = ViewBag.invitation_detail[0].Delegate_Image_Height;
                    ViewData["Delegate_Image_Width"] = ViewBag.invitation_detail[0].Delegate_Image_Width;

                    ViewData["Groom_Parents1_Title"] = ViewBag.invitation_detail[0].Groom_Parents1_Title == "입력안함" ? "" : ViewBag.invitation_detail[0].Groom_Parents1_Title;
                    ViewData["Groom_Parents2_Title"] = ViewBag.invitation_detail[0].Groom_Parents2_Title == "입력안함" ? "" : ViewBag.invitation_detail[0].Groom_Parents2_Title;
                    ViewData["Bride_Parents1_Title"] = ViewBag.invitation_detail[0].Bride_Parents1_Title == "입력안함" ? "" : ViewBag.invitation_detail[0].Bride_Parents1_Title;
                    ViewData["Bride_Parents2_Title"] = ViewBag.invitation_detail[0].Bride_Parents2_Title == "입력안함" ? "" : ViewBag.invitation_detail[0].Bride_Parents2_Title;

                    ViewData["Location_LAT"] = ViewBag.invitation_detail[0].Location_LAT;
                    ViewData["Location_LOT"] = ViewBag.invitation_detail[0].Location_LOT;

                    ViewData["DetailNewLineYN"] = ViewBag.invitation_detail[0].DetailNewLineYN;

                    if (!string.IsNullOrEmpty(ViewBag.invitation_detail[0].ExtendData))
                    {
                        ViewData["BabyInfos"] = ViewBag.invitation_detail[0].ExtendData;
                    }
                    else
                    {
                        if (ViewBag.Product[0].Product_Category_Code == "PCC03")
                        {
                            var babyinfos = new List<BabyFirstBirthViewModel>();
                            babyinfos.Add(new BabyFirstBirthViewModel
                            {
                                Name = "",
                                Birthday = DateTime.Now.Date,
                                idx = 1,
                                ExtraInfos = new List<BabyFirstBirthExtraInfo>()
                            });
                            ViewData["BabyInfos"] = JsonConvert.SerializeObject(babyinfos);
                        }
                    }
                }

                ViewBag.Gallery = _repository.TB_Gallery_List(Invitation_ID);

                foreach (TB_Gallery item in ViewBag.Gallery)
                {
                    item.Image_URL = item.Image_URL;// + "?" + Guid.NewGuid();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("Invitation/NaverMap")]
        public IActionResult NaverMap()
        {
            Map map = new Map();
            map.ApiId = _barunnConfig.Map.NaverCloudId;
            map.ApiKey = _barunnConfig.Map.NaverCloudKey;
            map.DefaultMapLat = _barunnConfig.Map.DefaultMapLat;
            map.DefaultMapLot = _barunnConfig.Map.DefaultMapLot;

            ViewBag.Map = map;

            return View(nameof(NaverMap));
        }

        [HttpGet]
        [Route("Invitation/NaverMap/GeocodeForNaver")]
        public string GeocodeForNaver(string query)
        {
            string url = "https://naveropenapi.apigw.ntruss.com/map-geocode/v2/geocode?query=" + HttpUtility.UrlEncode(query); string result = "";

            WebHeaderCollection headers = new WebHeaderCollection();

            headers.Add("X-NCP-APIGW-API-KEY-ID", _barunnConfig.Map.NaverCloudId);
            headers.Add("X-NCP-APIGW-API-KEY", _barunnConfig.Map.NaverCloudKey);

            try
            {
                result = HttpGetCall(url, "", "GET", headers);
            }
            catch
            { }

            return result;
        }


        public string HttpGetCall(string url, string postData, string method = "POST", WebHeaderCollection headers = null)
        {
            string result = String.Empty;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWebRequest.Method = method.ToUpper();

                if (headers != null)
                {
                    httpWebRequest.Headers = headers;
                }

                if (httpWebRequest.Method.ToUpper() == "POST")
                {
                    byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData);
                    httpWebRequest.ContentLength = sendData.Length;
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(sendData, 0, sendData.Length);
                    requestStream.Close();
                }

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                result = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }


        [HttpGet]
        [Route("Invitation/NaverMap/ReverseGeocodeForNaver")]
        public string ReverseGeocodeForNaver(string lat, string lot)
        {
            string url = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc?encoding=utf-8&request=coordsToaddr&coord=latlng&output=json&coords=" + lot + "," + lat;
            string result = "";

            WebHeaderCollection headers = new WebHeaderCollection();

            headers.Add("X-NCP-APIGW-API-KEY-ID", _barunnConfig.Map.NaverCloudId);
            headers.Add("X-NCP-APIGW-API-KEY", _barunnConfig.Map.NaverCloudKey);
            try
            {
                result = HttpGetCall(url, "", "GET", headers);
            }
            catch
            { }

            return result;
        }

        [Route("Order/McardStep1_Upload_Outline_Image")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult McardStep1_Upload_Outline_Image(List<IFormFile> files, string upload_path)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            var filename = String.Empty;
            var resource_url = String.Empty;
            if (auth)
            {
                try
                {
                    var path = Upload_Path() + upload_path.Replace("/upload", "").Replace("/", "\\");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    long size = files.Sum(f => f.Length);

                    var ext = Path.GetExtension(files[0].FileName).ToLower();

                    filename = Guid.NewGuid().ToString() + ext;

                    string filePath = Path.Combine(path, filename);

                    message = filename;

                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                formFile.CopyTo(stream);
                            }
                        }
                    }
                    resource_url = upload_path + "/" + filename;



                    result = resource_url;

                }
                catch (Exception e)
                {

                    message = e.Message;
                }
            }


            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, resource_url = resource_url, auth = auth });
        }

        [Route("Order/McardStep1_Upload_Main_Image")] 
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult McardStep1_Upload_Main_Image(List<IFormFile> files, string upload_path, int Invitation_Id)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            var filename = String.Empty;
            var resource_url = String.Empty;
            int width = 0;
            int height = 0;
            int maxSize = _barunnConfig.MaxSize.Photo;

            if (auth)
            {
                try
                {
                    //파일은 한개만 전송됨.
                    var formFile = files.First();

                    var path = Upload_Path() + upload_path.Replace("/upload", "").Replace("/", "\\");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    long size = formFile.Length;
                    var ext = Path.GetExtension(formFile.FileName).ToLower();
                    filename = Guid.NewGuid().ToString() + ext;
                    message = filename;

                    string filePath = Path.Combine(path, filename);
                    var outSize = ResizeAndSaveImage(formFile, maxSize, filePath);
                    width = outSize.Item1;
                    height = outSize.Item2;

                    resource_url = upload_path + "/" + filename;

                    TB_Invitation_Detail invitation_detail = new TB_Invitation_Detail();

                    invitation_detail.Invitation_ID = Invitation_Id;
                    invitation_detail.Delegate_Image_URL = resource_url;
                    invitation_detail.Delegate_Image_Width = width;
                    invitation_detail.Delegate_Image_Height = height;

                    invitation_detail.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                    _repository.Delegate_Image_Update_Sql(invitation_detail);

                    result = resource_url;
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, resource_url = resource_url, width = width, height = height, auth = auth });
        }

        [Route("Order/McardStep1_Upload_SNS_Image")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult McardStep1_Upload_SNS_Image(List<IFormFile> files, string upload_path, int Invitation_Id)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            var filename = String.Empty;
            var resource_url = String.Empty;
            int width = 0;
            int height = 0;
            int maxSize = _barunnConfig.MaxSize.SNS;

            if (auth)
            {
                try
                {
                    //파일은 한개만 전송됨.
                    var formFile = files.First();

                    var path = Upload_Path() + upload_path.Replace("/upload", "").Replace("/", "\\");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    long size = formFile.Length;
                    var ext = Path.GetExtension(formFile.FileName).ToLower();
                    filename = Guid.NewGuid().ToString() + ext;
                    message = filename;

                    string filePath = Path.Combine(path, filename);
                    var outSize = ResizeAndSaveImage(formFile, maxSize, filePath);
                    width = outSize.Item1;
                    height = outSize.Item2;

                    resource_url = upload_path + "/" + filename;

                    TB_Invitation_Detail invitation_detail = new TB_Invitation_Detail();

                    invitation_detail.Invitation_ID = Invitation_Id;
                    invitation_detail.SNS_Image_URL = resource_url;
                    invitation_detail.SNS_Image_Width = width;
                    invitation_detail.SNS_Image_Height = height;

                    invitation_detail.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                    _repository.SNS_Image_Update_Sql(invitation_detail);

                    result = resource_url;
                }
                catch (Exception e)
                {

                    message = e.Message;
                }
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, resource_url = resource_url, width = width, height = height, auth = auth });
        }

        [Route("Order/MDollStep1_Upload_Prfile_Image")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult MDollStep1_Upload_Prfile_Image(List<IFormFile> files, string upload_path, int Invitation_Id, int profileidx)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            var filename = String.Empty;
            var resource_url = String.Empty;
            int width = 0;
            int height = 0;
            int maxSize = _barunnConfig.MaxSize.SNS;

            if (auth)
            {
                try
                {
                    //파일은 한개만 전송됨.
                    var formFile = files.First();

                    var path = Upload_Path() + upload_path.Replace("/upload", "").Replace("/", "\\");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    long size = formFile.Length;
                    var ext = Path.GetExtension(formFile.FileName).ToLower();
                    filename = Guid.NewGuid().ToString() + ext;
                    message = filename;

                    string filePath = Path.Combine(path, filename);
                    var outSize = ResizeAndSaveImage(formFile, maxSize, filePath);
                    width = outSize.Item1;
                    height = outSize.Item2;

                    resource_url = upload_path + "/" + filename;

                    var babyInfors = _repository.Get_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(Invitation_Id);
                    if (babyInfors == null)
                    {
                        babyInfors = new List<Models.BabyFirstBirthViewModel>();
                    }
                    var babyinfo = babyInfors.FirstOrDefault(m => m.idx == profileidx);
                    if (babyinfo == null)
                    {
                        babyinfo = new Models.BabyFirstBirthViewModel { idx = profileidx, ExtraInfos = new List<Models.BabyFirstBirthExtraInfo>() };
                        babyInfors.Add(babyinfo);
                    }
                    babyinfo.Image_URL = resource_url;
                    babyinfo.Image_Width = width;
                    babyinfo.Image_Height = height;

                    _repository.Set_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(Invitation_Id, babyInfors);

                    result = resource_url;
                }
                catch (Exception e)
                {

                    message = e.Message;
                }
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, resource_url = resource_url, width = width, height = height, profileidx = profileidx, auth = auth });
        }


        [HttpPost]
        [Route("Invitation/GalleryFileUpload")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능

        public JsonResult GalleryFileUpload(List<IFormFile> gallery_file, int Invitation_Id, string upload_path)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            
            var resource_url = String.Empty;
            int gallery_id = 0;
            int maxSize = _barunnConfig.MaxSize.Gallery;

            if (auth)
            {
                try
                {
                    var galleries = _repository.TB_Gallery_List(Invitation_Id);

                    var gal_idx = galleries.Count + 1;

                    if (gal_idx <= 18)
                    {
                        //파일은 한개만 전송됨.
                        var formFile = gallery_file.First();
                       
                        //파일 저장 위치
                        var path = Upload_Path() + upload_path.Replace("/upload", "").Replace("/", "\\");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        long size = formFile.Length;
                        var ext = Path.GetExtension(formFile.FileName).ToLower();
                        //고유 파일명 생성(원본)
                        var filename = Guid.NewGuid().ToString() + ext;
                        //고유 파일명 생성(축소)
                        var smallFilename = Guid.NewGuid().ToString() + ext;

                        message = filename;

                        //Full FIle 경로(원본)
                        var filePath = Path.Combine(path, filename);
                        //Full FIle 경로(축소)
                        var smallFilePath = Path.Combine(path, smallFilename);

                        //상대 경로(원본)
                        resource_url = upload_path + "/" + filename;
                        //상대 경로(축소)
                        string smallresource_url = null;

                        //원본 파일 저장
                        var outSize = ResizeAndSaveImage(formFile, maxSize, filePath);
                        var width = outSize.Item1;
                        var height = outSize.Item2;

                        //축소 파일 저장, 성공시 상대 경로 설정
                        if (ResizeAndCropImage(244, 244, filePath, smallFilePath))
                            smallresource_url = upload_path + "/" + smallFilename;


                        result = resource_url;

                        TB_Gallery gallery = new TB_Gallery();
                        gallery.Invitation_ID = Invitation_Id;
                        gallery.Image_URL = resource_url;
                        gallery.Image_Height = height;
                        gallery.Image_Width = width;
                        gallery.Sort = gal_idx;
                        gallery.Regist_User_ID = User.FindFirst("Id").Value;
                        gallery.Regist_DateTime = DateTime.Now;
                        gallery.Regist_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                        gallery.Update_User_ID = User.FindFirst("Id").Value;
                        gallery.Update_DateTime = DateTime.Now;
                        gallery.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                        gallery.FileSize = size;
                        gallery.SmallImage_URL = smallresource_url;

                        gallery_id = _repository.TB_Gallery_Insert_Sql(gallery);
                    }
                    else
                    {
                        message = "최대 18컷의 파일만 업로드가 가능합니다.";
                    }
                }
                catch (Exception e)
                {

                    message = e.Message;
                }
            }


            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, resource_url = resource_url, gallery_id = gallery_id , auth = auth});
        }

        [HttpPost]
        [Route("Invitation/ChangeGallerySort")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult ChangeGallerySort(int Gallery_Id, int newSort)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;

            if (auth)
            {
                try
                {
                    var gallery = _repository.TB_Gallery_Entity(Gallery_Id);
                    int Invitation_Id = Int32.Parse(gallery.Invitation_ID.ToString());

                    int preSort = Int32.Parse(gallery.Sort.ToString());

                    if (newSort > preSort)
                    {
                        _repository.Gallery_Sort_Extra_Update_Sql(Invitation_Id, -1, preSort + 1, newSort);
                    }
                    if (newSort < preSort)
                    {
                        _repository.Gallery_Sort_Extra_Update_Sql(Invitation_Id, 1, newSort, preSort - 1);
                    }
                    if (newSort != preSort)
                    {
                        _repository.Gallery_Sort_Update_Sql(Gallery_Id, newSort);
                    }
                    result = "Success";
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, auth = auth });
        }


        [HttpPost]
        [Route("Invitation/RemoveGalleryItem")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult RemoveGalleryItem(int Gallery_Id)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            if (auth)
            {
               

                System.Threading.Thread.Sleep(100); // 0.1초 sleep 파일 process 중복 사용 방지

                try
                {
                    var gallery = _repository.TB_Gallery_Entity(Gallery_Id);
                    int Invitation_Id = Int32.Parse(gallery.Invitation_ID.ToString());

                    int Sort = Int32.Parse(gallery.Sort.ToString());
                    
                    //파일삭제
                    RemoveFIle(gallery.Image_URL);
                    if (!string.IsNullOrEmpty(gallery.SmallImage_URL))
                    {
                        RemoveFIle(gallery.SmallImage_URL);
                    }

                    //Sort 재배치
                    _repository.Gallery_Sort_Reset_Update_Sql(Invitation_Id, Sort);

                    //DB삭제
                    _repository.TB_Gallery_Delete_Entity(Gallery_Id);

                    result = "Success";
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, auth = auth });
        }


        [HttpPost]
        [Route("Invitation/RemoveImage")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult RemoveImage(string filepath, int Invitation_Id, string type, int idx)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;

            if (auth)
            {
                System.Threading.Thread.Sleep(100);
                try
                {
                    //파일삭제
                    RemoveFIle(filepath);

                    //DB업데이트
                    TB_Invitation_Detail invitation_detail = new TB_Invitation_Detail();
                    invitation_detail.Invitation_ID = Invitation_Id;
                    invitation_detail.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                    invitation_detail.Update_DateTime = DateTime.Now;
                    invitation_detail.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                    if (type == "sns_img")
                    {
                        invitation_detail.SNS_Image_URL = null;
                        invitation_detail.SNS_Image_Height = null;
                        invitation_detail.SNS_Image_Width = null;

                        _repository.SNS_Image_Update_Sql(invitation_detail);
                    }
                    else if (type == "main_img")
                    {
                        invitation_detail.Delegate_Image_URL = null;
                        invitation_detail.Delegate_Image_Height = null;
                        invitation_detail.Delegate_Image_Width = null;

                        _repository.Delegate_Image_Update_Sql(invitation_detail);
                    } 
                    else if (type == "profile_img")
                    {
                        var babyInfors = _repository.Get_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(Invitation_Id);
                        if (babyInfors != null)
                        {
                            var babyinfo = babyInfors.FirstOrDefault(m => m.idx == idx);
                            if (babyinfo != null)
                            {
                                idx = babyinfo.idx;
                                babyinfo.Image_URL = "";
                                babyinfo.Image_Height = 0;
                                babyinfo.Image_Width = 0;

                                _repository.Set_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(Invitation_Id, babyInfors);
                            }
                        }
                    }

                    result = "Success";
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, auth = auth, idx=idx });
        }


        private bool RemoveFIle(string filepath){
            bool result = false;

            try
            {
                string filename = filepath.Substring(filepath.LastIndexOf('/') + 1, filepath.Length - filepath.LastIndexOf('/') - 1);

                var path = Upload_Path() + filepath.Replace("/upload", "").Replace("/" + filename, "").Replace("/", "\\");

                if (Directory.Exists(path))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(path);

                    foreach (var file in dirInfo.GetFiles())
                    {
                        bool del = false;

                        if (filename == file.Name)
                        {
                            del = true;
                        }

                        if (del)
                        {
                            file.Delete();

                            result = del;
                        }
                    }

                }
            }
            catch (Exception e)
            {
            }

            return result;
        }

 
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = 52428800)] // 50메가제한
        [Route("Invitation/CropImageUpload")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public JsonResult CropImageUpload(string imageData, string type, int id, string url, int idx=0)
        {
            bool auth = User.Identity.IsAuthenticated;
            string result = String.Empty;
            string message = String.Empty;
            string path = String.Empty;

            if (auth)
            {
                try 
                { 
                    var imageFile = ImageFileCrop(imageData, url);
                    path = imageFile.Url;

                    switch (type)
                    {
                        case "gallery":
                            //업로드된 이미지를 Crop 수정 후 저장시 동작

                            //고유 파일명 생성(축소)
                            var smallFilename = Guid.NewGuid().ToString() + ".jpg";
                            //Full FIle 경로(원본)
                            var filePath = imageFile.ImagePath;
                            //Full FIle 경로(축소)
                            var smallFilePath = filePath.Replace(imageFile.ImageName, smallFilename);
                            //상대 경로(축소)
                            string smallresource_url = null;
                            //축소 파일 저장, 성공시 상대 경로 설정
                            if (ResizeAndCropImage(244, 244, filePath, smallFilePath))
                                smallresource_url = url.Substring(0, url.LastIndexOf("/", url.Length - 1, url.Length)) + "/" + smallFilename;
                                                        
                            TB_Gallery gallery = new TB_Gallery();
                            gallery.Gallery_ID = id;
                            gallery.Image_URL = imageFile.Url;
                            gallery.Image_Height = imageFile.ImageHeight;
                            gallery.Image_Width = imageFile.ImageWidth;
                            gallery.SmallImage_URL = smallresource_url;
                            gallery.Regist_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                            gallery.Regist_DateTime = DateTime.Now;
                            gallery.Regist_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                            gallery.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                            gallery.Update_DateTime = DateTime.Now;
                            gallery.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                            result = _repository.Gallery_Image_Update_Sql(gallery).ToString();

                            break;
                        case "main":
                            TB_Invitation_Detail main = new TB_Invitation_Detail();
                            main.Invitation_ID = id;
                            main.Delegate_Image_URL = imageFile.Url;
                            main.Delegate_Image_Height = imageFile.ImageHeight;
                            main.Delegate_Image_Width = imageFile.ImageWidth;
                            main.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                            main.Update_DateTime = DateTime.Now;
                            main.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                            result = _repository.Delegate_Image_Update_Sql(main).ToString();
                            break;
                        case "sns":
                            TB_Invitation_Detail sns = new TB_Invitation_Detail();
                            sns.Invitation_ID = id;
                            sns.SNS_Image_URL = imageFile.Url;
                            sns.SNS_Image_Height = imageFile.ImageHeight;
                            sns.SNS_Image_Width = imageFile.ImageWidth;
                            sns.Update_User_ID = User.FindFirst("Id") != null ? User.FindFirst("Id").Value : "";
                            sns.Update_DateTime = DateTime.Now;
                            sns.Update_IP = UrlHelper.GetIP(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);

                            result = _repository.SNS_Image_Update_Sql(sns).ToString();
                            break;
                        case "profile":
                            var babyInfors = _repository.Get_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(id);
                            if (babyInfors != null)
                            {
                                var babyinfo = babyInfors.FirstOrDefault(m => m.idx == idx);
                                if (babyinfo != null)
                                {
                                    babyinfo.Image_URL = imageFile.Url;
                                    babyinfo.Image_Height = imageFile.ImageHeight;
                                    babyinfo.Image_Width = imageFile.ImageWidth;

                                    _repository.Set_TB_Invitation_Detail_ExtendData<List<Models.BabyFirstBirthViewModel>>(id, babyInfors);

                                    result = "Y";
                                }
                            }
                            break;
                    }
                }
                catch
                {

                }
            }

            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message, auth = auth, path = path, idx=idx });
        }

        [HttpPost]
        [Route("Invitation/CheckDuplicateURL")]
        public JsonResult CheckDuplicateURL(string invitation_url, int invitation_id)
        {
            string result = String.Empty;
            string message = String.Empty;

            var invitation_detailes = _repository.TB_Invitation_Detail_Entity(invitation_id);

            string ORG_Invitation_URL = invitation_detailes[0].Invitation_URL;

            if (ORG_Invitation_URL != invitation_url)
            {
                var invitation_detail = _repository.CheckDuplicateURL_Entity(invitation_url);

                if (invitation_detail == null)
                {
                    result = "Y";
                }
                else
                {
                    message = "duplicate";
                }
            }
            else
            {
                result = "Y";
            }
            return Json(new { success = String.IsNullOrEmpty(result) ? "N" : "Y", result = result, message = message });
        }

        
        public class ImageFile
        {
            public int ImageID { get; set; }
            public string ImagePath { get; set; }
            public string ImageName { get; set; }
            public int ImageWidth { get; set; }
            public int ImageHeight { get; set; }
            public int ImageSize { get; set; }
            public string Url { get; set; }
            public string returnCode { get; set; }
        }

        /// <summary>
        /// Invitation_Item테이블과 Item_Resource테이블을 합쳐서 활용하는 별도 클래스
        /// </summary>
        public class Invitation_Item_Resource
        {
            public int item_id { get; set; }
            public int resource_id { get; set; }
            public string pid { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public double? top { get; set; }
            public double? left { get; set; }
            public double? height { get; set; }
            public double? width { get; set; }
            public string chracterset { get; set; }
            public double? fontsize { get; set; }
            public string fontcolor { get; set; }
            public string bgcolor { get; set; }
            public bool bold_yn { get; set; }
            public bool italic_yn { get; set; }
            public bool underline_yn { get; set; }
            public double? between_text { get; set; }
            public double? between_line { get; set; }
            public string vertical_align { get; set; }
            public string horizontal_align { get; set; }
            public int? zindex { get; set; }
            public string font { get; set; }
            public string resource_url { get; set; }
            public double? org_height { get; set; }
            public double? org_width { get; set; }
        }

        public string getDayOfWeek(DateTime dt) 
        {
            string result = "";

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    result = "월";
                    break;
                case DayOfWeek.Tuesday:
                    result = "화";
                    break;
                case DayOfWeek.Wednesday:
                    result = "수";
                    break;
                case DayOfWeek.Thursday:
                    result = "목";
                    break;
                case DayOfWeek.Friday:
                    result = "금";
                    break;
                case DayOfWeek.Saturday:
                    result = "토";
                    break;
                case DayOfWeek.Sunday:
                    result = "일";
                    break;
            }

            return result;
        }
        
        private string GetDDay(string date)
        {
            var today = DateTime.Today;
            var eventDate = DateTime.Parse(date);
            var ddayvalue = "";
            if (today == eventDate)
                ddayvalue = "D-Day";
            else if (today < eventDate)
                ddayvalue = $"D-{(eventDate - today).Days}";

            return ddayvalue;
        }

        #region 이미지 저장

        /// <summary>
        /// 업로드 파일 사이즈 조정 및 저장
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="maxSize"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private (int, int) ResizeAndSaveImage(IFormFile formFile, int maxSize, string filePath)
        {
            var returnSize = (0, 0);
            if (formFile.Length > 0)
            {
                using (var stream = formFile.OpenReadStream())
                {
                    using (var image = new MagickImage(stream))
                    {
                        if (image.Width > maxSize || image.Height > maxSize)
                        {
                            int w = maxSize;
                            int h = maxSize;
                            if (image.Width > image.Height)
                                w = 0;
                            else if (image.Width < image.Height)
                                h = 0;

                            image.Resize(w, h);
                        }

                        image.Write(filePath);

                        //이미지 회전값이 왼쪽아래, 오른쪽아래로 되어있을 경우 width, height값을 바꿔줌
                        if (image.Orientation == ImageMagick.OrientationType.LeftBotom || image.Orientation == ImageMagick.OrientationType.RightBottom)
                        {
                            returnSize = (image.Height, image.Width);
                        }
                        else
                        {
                            returnSize = (image.Width, image.Height);
                        }
                    }
                }
            }
            return returnSize;
        }
        /// <summary>
        /// 사이즈 변경 및 Crop
        /// </summary>
        /// <param name="width">변경할 폭</param>
        /// <param name="height">변경할 높이</param>
        /// <param name="sourceFilePath"></param>
        /// <param name="writeFilePath"></param>
        /// <returns></returns>
        private bool ResizeAndCropImage(int width, int height, string sourceFilePath, string writeFilePath)
        {
            var result = false;
            try
            {
                using (var img = new MagickImage(sourceFilePath))
                {
                    if (img.Height != height || img.Width != width)
                    {
                        decimal result_ratio = (decimal)height / (decimal)width;
                        decimal current_ratio = (decimal)img.Height / (decimal)img.Width;

                        bool preserve_width = false;
                        if (current_ratio > result_ratio)
                        {
                            preserve_width = true;
                        }
                        int new_width = 0;
                        int new_height = 0;
                        if (preserve_width)
                        {
                            new_width = width;
                            new_height = (int)Math.Round((decimal)(current_ratio * new_width));
                        }
                        else
                        {
                            new_height = height;
                            new_width = (int)Math.Round((decimal)(new_height / current_ratio));
                        }

                        img.Resize(new_width, new_height);
                        img.Crop(width, height, Gravity.Center);
                        img.Write(writeFilePath);
                        result = true;
                    }
                }
            }
            catch
            { }
            return result;
        }

        /// <summary>
        /// 크롭 이미지 저장
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ImageFile ImageFileCrop(string imageData, string url)
        {
            var imageFile = new ImageFile();
            try
            {
                var crop_file_name = Guid.NewGuid().ToString() + ".jpg";

                var _path = Upload_Path() + url.Replace("/upload", "").Replace("/", "\\");

                var path = _path.Substring(0, _path.LastIndexOf("\\", _path.Length - 1, _path.Length)) + "\\" + crop_file_name;

                var new_url = url.Substring(0, url.LastIndexOf("/", url.Length - 1, url.Length)) + "/" + crop_file_name;


                byte[] bytes = Convert.FromBase64String(imageData);
                using (var image = new MagickImage(bytes))
                {
                    imageFile.ImageWidth = image.Width;
                    imageFile.ImageHeight = image.Height;
                    image.Format = MagickFormat.Jpeg;
                    image.Write(path);
                }
                imageFile.Url = new_url;
                imageFile.ImagePath = path;
                imageFile.ImageName = crop_file_name;
                imageFile.returnCode = "OK";
            }
            catch (Exception ex)
            {
                imageFile.returnCode = ex.Message;
            }
            return imageFile;
        }

        #endregion
    }
}

