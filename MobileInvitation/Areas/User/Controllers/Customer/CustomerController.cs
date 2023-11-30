using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using MobileInvitation.Config;
using MobileInvitation.Data.Member;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Product;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList;

namespace MobileInvitation.Areas.User.Controllers.Menu
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class CustomerController : Controller
    {
        private readonly IOperationRepository _operationrepository;
        private readonly PathController _PathController;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _ProductRepository;
        protected readonly BarunnConfig _barunnConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(BarunnConfig barunnConfig, PathController PathController, IMemberRepository memberRepository, IOperationRepository operationrepository
            , IProductRepository ProductRepository, IHttpClientFactory httpClientFactory )
        {
            _PathController = PathController;
            _operationrepository = operationrepository;
            _memberRepository = memberRepository;
            _ProductRepository = ProductRepository;
            _barunnConfig = barunnConfig;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// PC -> Customer/ -> Customer/Notice
        ///모바일 -> Customer/ -> Customer/index
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        //[Area("User")]
        [Route("Customer/")]
        [Route("Customer/Notice/{Page?}/{PageSize?}")]
        public IActionResult Notice(int? Page, int PageSize)
        {
            PageSize = 20;// (PageSize < 5) ? 5 : PageSize;
            var pageNum = Page ?? 1;

            ViewBag.Top_NoticeList = _operationrepository.User_Notice_List_Entity("Y").ToList();
            ViewBag.NoticeList = _operationrepository.User_Notice_List_Entity("N").ToPagedList(pageNum, PageSize);

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;

            string Path_Chk = Request.Path.ToString().ToLower();
            string Redirect_Url = _PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request);

            if (Path_Chk.Equals("/customer/") || Path_Chk.Equals("/customer"))
            {

                if (UrlHelper.IsMobile(Request)) // 모바일 -> customer/ 인덱스 
                {
                    return View(nameof(Index));
                }
                else // PC
                {
                    return View(Redirect_Url);
                }

            }
            else
            {
                return View(Redirect_Url);
            }

        }

      

        [AllowAnonymous]
        [Route("Customer/Notice_View/{id?}")]
        public IActionResult Notice_View(int id)
        {
        
            ViewBag.NoticeView = _operationrepository.Admin_Notice_View_Entity(id);
            _operationrepository.User_Notice_Faq_Click_Update_Entity(id, "N");

            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
    
        }
    


        [AllowAnonymous]
        [Route("Customer/Faq_Hit/{id?}")]
        public void Faq_Hit(int id)
        {
            _operationrepository.User_Notice_Faq_Click_Update_Entity(id, "F");

        }

        [Route("Customer/Ask/{Page?}/{PageSize?}")]
        public IActionResult Ask(int? Page, int PageSize)
        {
          
            PageSize = 20;// (PageSize < 5) ? 5 : PageSize;
            var pageNum = Page ?? 1;

            string User_ID = "";

            if (User.IsInRole("Users")) //회원
            {
                User_ID = User.FindFirst("Id").Value;

            }
            if (UrlHelper.IsMobile(Request))
            {
                ViewBag.Qna_List = _operationrepository.User_Qna_List_Entity(User_ID).ToList();
            }
            else
            {
                ViewBag.Qna_List = _operationrepository.User_Qna_List_Entity(User_ID).ToPagedList(pageNum, PageSize);
            }
               

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;

            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
            
        }

       

        /// <summary>
        /// 고객센터 - 1대1문의  뷰 페이지 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Customer/Ask_View/{id?}")]
        public IActionResult Ask_View(int Id)
        {          
            ViewBag.Qna_View = _operationrepository.User_Qna_Detail_Entity(Id);
            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
        }

        [Route("Customer/Ask/Download/{id?}/{num}")]
        public async Task<FileResult> Download(int Id,int num=1)
        {
            string fileName = "";
            string temp = _operationrepository.User_Qna_FileUpladName_Entity(Id,num).Replace(_barunnConfig.Sites.Url.ToString(), "");

            try
            {
                if (num == 3)
                {
                    fileName = temp;
                    var httpClient = _httpClientFactory.CreateClient();
                    var httpResponseMessage = await httpClient.GetAsync(_barunnConfig.Sites.UserFileUrl + fileName);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        byte[] fileBytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                        return File(fileBytes, "application/octet-stream", fileName);
                    }
                }
                else
                {
                    int aa = temp.LastIndexOf('/') + 1; //41
                    int aa2 = temp.Length;//73
                    fileName = temp.Substring(aa);
                    byte[] fileByteArray = System.IO.File.ReadAllBytes(Path.Combine(_PathController.Upload_Path() + "/Qna/" + fileName));
                    return File(fileByteArray, "application/octet-stream", fileName);
                }
            }
            catch
            {
                
            }

            return null;
        }


        /// <summary>
        ///  마이페이지 - 1대1문의 입력페이지 
        /// </summary>
        /// <returns></returns>
        [Route("Customer/Ask_Reg/{id?}")]
        public IActionResult Ask_Reg(int id)
        {
            ViewData["Qna_Id"] = id;
            ViewBag.Qna_Reg_View = _operationrepository.User_Qna_Detail_Entity(id); 

            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
        }

        /// <summary>
        /// 고객센터 - 1대1문의 게시판 - 주문완료리스트 조회
        /// </summary>
        /// <param name="Popup_Type"></param>
        /// <returns></returns>
        [Route("Customer/Order_List/{Order_Code?}")]

        public IActionResult Order_List(string Order_Code)
        {   
            string Gubun = "";

            TB_Order model = new TB_Order();

            model.Name = User.FindFirst("Name").Value;

            if (User.IsInRole("Users")) //회원
            {
                model.User_ID = User.FindFirst("Id").Value;
                Gubun = "U";
            }
            else //비회원
            {
                model.Email = User.FindFirst("Email").Value;
                Gubun = "G";
            }
            model.Order_Code = Order_Code;


            ViewBag.Order_List = _memberRepository.User_Seach_MyOrderList_Sql(Gubun, model); //
            return Json(new { order_list = ViewBag.Order_List });
        }


        /// <summary>
        ///  마이페이지 - 1대1문의 삭제
        /// </summary>
        /// <returns></returns>
        [Route("Customer/Ask_Del/{id?}")]
        public IActionResult Ask_Del(int id)
        {
            VW_User_QNA model = new VW_User_QNA();
            model.QNA_ID = id;
            _operationrepository.User_Qna_Save_Sql(model, "D");

            return LocalRedirect("/Customer/Ask");
        }

        [HttpPost]
        [Route("Customer/Ask_Save")]
        public IActionResult Ask_Save(ICollection<IFormFile> formFileCollection, VW_User_QNA model, string UPFILE_1, string UPFILE_2)
        {
            try
            {
                string Gubun = model.QNA_ID.Equals(0) ? "I" : "U";

                string uploadDirectoryPath = _PathController.Upload_Path() + "\\Qna\\";

                long totalSize = 0L;

                int idx = 1;
                bool bSameFile = false;

                if(formFileCollection.Count > 0)
                {                   
                    foreach (IFormFile formFile in formFileCollection)
                    {
                        if (!Directory.Exists(uploadDirectoryPath))
                        {
                            Directory.CreateDirectory(uploadDirectoryPath); // 웹 서비스 내 업로드폴더가 없을 경우 자동생성을 위한 처리 
                        }

                        string Name_Regex = Guid.NewGuid().ToString() + Regex.Replace(formFile.FileName.Substring(formFile.FileName.LastIndexOf(".")), @"[`~!@#$%^&()_+{}\-=\[\];',]", "", RegexOptions.Singleline).Trim();

                        string uploadFilePath = Path.Combine(uploadDirectoryPath, Name_Regex);

                        using (FileStream fileStream = System.IO.File.Create(uploadFilePath))
                        {
                            formFile.CopyTo(fileStream);
                            fileStream.Flush();
                        }

                        totalSize += formFile.Length;

                        //-- 같은 파일명인 경우 예외처리
                        if(formFileCollection.Count==2 && UPFILE_1.Equals(UPFILE_2) && idx==2)
                        {
                            bSameFile = true;
                        }

                        if (UPFILE_1 == formFile.FileName && !bSameFile)
                        {

                            model.UPFILE_1 = _barunnConfig.Sites.Url + "upload/Qna/" + Name_Regex;
                        }
                        else
                        {
                            model.UPFILE_2 = _barunnConfig.Sites.Url + "upload/Qna/" + Name_Regex;
                        }

                        idx++;
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(UPFILE_1))
                    {
                        model.UPFILE_1 = _barunnConfig.Sites.Url + "upload/Qna/" + UPFILE_1;
                    }

                    if (!string.IsNullOrEmpty(UPFILE_2))
                    {
                        model.UPFILE_2 = _barunnConfig.Sites.Url + "upload/Qna/" + UPFILE_2;
                    }
                }                

                model.NAME = User.FindFirst("Name").Value;
                if (User.IsInRole("Users")) //회원
                {
                    model.USERID = User.FindFirst("Id").Value;
                    model.EMAIL = User.FindFirst("Email").Value;

                }
                else //비회원
                {
                    model.EMAIL = User.FindFirst("Email").Value;

                }
                
                model.STAT = "S1";

                _operationrepository.User_Qna_Save_Sql(model, Gubun);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

           


            return LocalRedirect("/Customer/Ask");
        }

        /// <summary>
        /// 고객센터 - FAQ
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [Route("Customer/Faq/{Page?}/{PageSize?}/{SearchTxt?}")]
        public IActionResult Faq(int? Page, int PageSize, string SearchTxt)
        {
           
            PageSize = 10;// (PageSize < 5) ? 5 : PageSize;
            var pageNum = Page ?? 1;

            ViewBag.Faq_List = _operationrepository.User_Faq_List_Entity(SearchTxt).ToPagedList(pageNum, PageSize);

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;
            ViewData["SearchTxt"] = SearchTxt;

          
            return View(_PathController.Moblie_Redirect(HttpContext.Request.RouteValues, Request));
        }

        ///// <summary>
        ///// 고객센터 - FAQ
        ///// </summary>
        ///// <param name="Page"></param>
        ///// <param name="PageSize"></param>
        ///// <returns></returns>
        //[Route("Customer/m_FAQ/{Page?}/{PageSize?}/{SearchTxt?}")]
        //public IActionResult m_FAQ(int? Page, int PageSize, string SearchTxt)
        //{

        //    PageSize = 20;// (PageSize < 5) ? 5 : PageSize;
        //    var pageNum = Page ?? 1;

        //    ViewBag.Faq_List = _operationrepository.User_Faq_List_Entity(SearchTxt).ToPagedList(pageNum, PageSize);

        //    ViewData["Page"] = Page ?? 1;
        //    ViewData["PageSize"] = PageSize;
        //    ViewData["SearchTxt"] = SearchTxt;

        //    return View();
        //}




    }
}
