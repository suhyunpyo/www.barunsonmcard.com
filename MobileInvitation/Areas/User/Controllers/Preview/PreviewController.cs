using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileInvitation.Config;
using MobileInvitation.Data.Invitation;
using MobileInvitation.Data.Product;
using MobileInvitation.Data.Template;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MobileInvitation.Areas.User.Controllers.Invitaion
{
    [Area("User")]
    public class PreviewController : PathController
    {
        private readonly ITemplateRepository _t_repository;
        private readonly IProductRepository _p_repository;
        private readonly IInvitationRepository _repository;

        public PreviewController(IInvitationRepository repository, ITemplateRepository t_repository, IProductRepository p_repository, 
            IWebHostEnvironment environment, 
            IHttpContextAccessor accessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _repository = repository;
            _t_repository = t_repository;
            _p_repository = p_repository;
            _environment = environment;
        }


        [Route("Preview/{Product_ID?}")]
        public IActionResult Index(int Product_ID)
        {
            ViewBag.Product = _p_repository.TB_Product_Entity(Product_ID);
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl.ToString().TrimEnd('/');

            if (ViewBag.Product.Count > 0)
            {
                int Template_ID = ViewBag.Product[0].Template_ID;
                string Product_Category_Code = ViewBag.Product[0].Product_Category_Code;

                ViewData["Product_Category_Code"] = Product_Category_Code;
                ViewBag.Template = _t_repository.TB_Template_Entity(Template_ID);

                if (ViewBag.Template.Count > 0)
                {
                    ViewData["Invitation_Title"] = ViewBag.Template[0].Template_Name;
                    ViewData["Preview_URL"] = ViewBag.Template[0].Preview_URL;
                    ViewData["Background_Color"] = ViewBag.Template[0].Background_Color;
                    ViewData["Photo_YN"] = ViewBag.Template[0].Photo_YN;
                    ViewData["Attached_File1_URL"] = ViewBag.Template[0].Attached_File1_URL;
                    ViewData["Attached_File2_URL"] = ViewBag.Template[0].Attached_File2_URL;
                }

                ViewBag.Template_Detail = _t_repository.TB_Template_Detail_Entity(Template_ID);

                if (ViewBag.Template_Detail.Count > 0)
                {
                    ViewData["Greetings"] = ViewBag.Template_Detail[0].Greetings;
                    ViewData["Groom_Name"] = ViewBag.Template_Detail[0].Groom_Name;
                    ViewData["Groom_Phone"] = ViewBag.Template_Detail[0].Groom_Phone;
                    ViewData["Bride_Name"] = ViewBag.Template_Detail[0].Bride_Name;
                    ViewData["Bride_Phone"] = ViewBag.Template_Detail[0].Bride_Phone;
                    ViewData["Groom_Parents1_Name"] = ViewBag.Template_Detail[0].Groom_Parents1_Name;
                    ViewData["Groom_Parents1_Phone"] = ViewBag.Template_Detail[0].Groom_Parents1_Phone;
                    ViewData["Groom_Parents2_Name"] = ViewBag.Template_Detail[0].Groom_Parents2_Name;
                    ViewData["Groom_Parents2_Phone"] = ViewBag.Template_Detail[0].Groom_Parents2_Phone;
                    ViewData["Bride_Parents1_Name"] = ViewBag.Template_Detail[0].Bride_Parents1_Name;
                    ViewData["Bride_Parents1_Phone"] = ViewBag.Template_Detail[0].Bride_Parents1_Phone;
                    ViewData["Bride_Parents2_Name"] = ViewBag.Template_Detail[0].Bride_Parents2_Name;
                    ViewData["Bride_Parents2_Phone"] = ViewBag.Template_Detail[0].Bride_Parents2_Phone;
                    ViewData["WeddingDate"] = ViewBag.Template_Detail[0].WeddingDate;
                    ViewData["WeddingHHmm"] = ViewBag.Template_Detail[0].WeddingHHmm;
                    ViewData["Time_Type_Code"] = ViewBag.Template_Detail[0].Time_Type_Code;
                    ViewData["WeddingYY"] = ViewBag.Template_Detail[0].WeddingYY;
                    ViewData["WeddingMM"] = ViewBag.Template_Detail[0].WeddingMM;
                    ViewData["WeddingDD"] = ViewBag.Template_Detail[0].WeddingDD;
                    ViewData["WeddingWeek"] = ViewBag.Template_Detail[0].WeddingWeek;
                    ViewData["WeddingHour"] = ViewBag.Template_Detail[0].WeddingHour;
                    ViewData["WeddingMin"] = ViewBag.Template_Detail[0].WeddingMin;
                    ViewData["Weddinghall_Name"] = ViewBag.Template_Detail[0].Weddinghall_Name;
                    ViewData["WeddingHallDetail"] = ViewBag.Template_Detail[0].WeddingHallDetail;
                    ViewData["Weddinghall_Address"] = ViewBag.Template_Detail[0].Weddinghall_Address;
                    ViewData["Weddinghall_PhoneNumber"] = ViewBag.Template_Detail[0].Weddinghall_PhoneNumber;
                    ViewData["WeddingWeek_Eng_YN"] = ViewBag.Template_Detail[0].WeddingWeek_Eng_YN;
                    ViewData["Time_Type_Eng_YN"] = ViewBag.Template_Detail[0].Time_Type_Eng_YN;

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

                    if (ViewData["WeddingWeek_Eng_YN"].ToString() == "Y")
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
                    else
                    {
                        //한글
                        ViewData["WeddingWeekName"] = ViewData["WeddingWeek"].ToString();
                    }
                    
                    ViewData["Baby_Name"] = ViewBag.Template_Detail[0].Baby_Name;
                    ViewData["Baby_Birthday"] = ((DateTime?)ViewBag.Template_Detail[0].Baby_Birthday);
                    ViewData["RepeatData"] = ViewBag.Template_Detail[0].RepeatData;
                    //if (string.IsNullOrEmpty(ViewBag.Template_Detail[0].RepeatData))
                    //    ViewData["RepeatData"] = new Dictionary<string, Dictionary<string, string>>();
                    //else
                    //{
                    //    var jsonVal = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(ViewBag.Template_Detail[0].RepeatData, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
                    //    ViewData["RepeatData"] = jsonVal;
                    //}
                }

                ViewData["TB_Area"] = JsonConvert.SerializeObject(_t_repository.TB_Template_Area_LIst(Template_ID, Product_Category_Code));
                ViewData["Objects"] = JsonConvert.SerializeObject(_t_repository.TB_Template_Item_Resource_LIst(Template_ID));

                var defaultInfo = _t_repository.TB_Template_Detail_Default_Info_New();
                ViewData["defaultInfos"] = defaultInfo;

                ViewData["D-DAY"] = defaultInfo.FirstOrDefault(m => m.ReserveWord == "D-DAY")?.DefaultValue;
            }

            return View();
        }

    }
}

