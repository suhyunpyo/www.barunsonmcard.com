using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobileInvitation.Config;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Product;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using X.PagedList;

namespace MobileInvitation.Areas.User.Controllers.Product
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class ProductController : PathController
    {
        private readonly IProductRepository _product_repository;
        private readonly IOperationRepository _operation_repository;


        public ProductController(IProductRepository product_repository, IOperationRepository operation_repository, IWebHostEnvironment environment,
            IHttpContextAccessor accessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _product_repository = product_repository;
            _operation_repository = operation_repository;
        }

        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 상품 상세 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Product/Detail/{id?}")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Detail(int id, string Image_Type_Code)
        {
            Image_Type_Code = string.IsNullOrEmpty(Image_Type_Code) ? "ICC01" : Image_Type_Code;
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl;

            string UserId = "";

            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                UserId = User.FindFirst("Id").Value;

            }

            ViewBag.Product_Detail_List = _product_repository.User_Product_Detail_Entity(id, UserId);
            ViewBag.Banner_List = _operation_repository.Admin_Banner_Add_List_Entity2("상품상세배너", Request).ToList();

            ViewBag.Detail_Img_Url = _product_repository.User_Product_Detail_Img(id);
            //ViewData["Detail_Img_Url"] = _product_repository.User_Product_Detail_Img(id, Image_Type_Code);

            if (ViewBag.Product_Detail_List.Count == 0)
            {
                ViewBag.AlertMessage = "존재하지 않는 상품입니다";
            }

            return View();
        }


        [Route("Product/Search/{Page?}/{PageSize?}/{Sort_Gubun?}/{SearchKeyword?}")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Search(int? Page, int PageSize, int Sort_Gubun, string SearchKeyword) 
        { 
        
            string UserId = "";

            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                UserId = User.FindFirst("Id").Value;

            }

            PageSize = 20;
            var pageNum = Page ?? 1;
            Sort_Gubun = (Sort_Gubun.Equals(0)) ? 0 : Sort_Gubun;

            ViewBag.Brandlist = _product_repository.Common_CodeList_Entity("Product_Brand_Code").ToList();
            ViewBag.SearchProduct = _product_repository.User_Search_ProductList_Sql(UserId, SearchKeyword, Sort_Gubun).ToPagedList(pageNum, PageSize);
            ViewBag.Icon_List = _product_repository.User_Product_Icon_List_Entity();

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;
            ViewData["Category_Id"] = "";
            ViewData["SearchCategoryList"] ="";
            ViewData["SearchBrandList"] = "";
            ViewData["Sort_Gubun"] = Sort_Gubun;

            ViewData["SearchKeyword"] = !string.IsNullOrEmpty(SearchKeyword) ?  SearchKeyword.Trim() : SearchKeyword;
            return View();
        }


        /// <summary>
        /// 프런트 - 상품 리스트
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Product/List/{Page?}/{PageSize?}/{Category_Id?}/{Sort_Gubun?}/{SearchCategoryList?}/{SearchBrandList?}/")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult List(int? Page, int PageSize, int Category_Id, int Sort_Gubun, string SearchCategoryList, string SearchBrandList)
        {
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl;
            ViewBag.Category_Menu_List = _operation_repository.User_Menu_List_Entity("CTC02", Category_Id).ToList(); //상단 대분류카테고리명
            ViewBag.Brandlist = _product_repository.Common_CodeList_Entity("Product_Brand_Code").ToList();

            PageSize = (PageSize < 1) ? 18 : PageSize;
            var pageNum = Page ?? 1;
            Sort_Gubun = (Sort_Gubun.Equals(0)) ? 0 : Sort_Gubun;
            SearchCategoryList = SearchCategoryList.Equals("0") ? "" : SearchCategoryList;
            SearchBrandList = SearchBrandList.Equals("0") ? "" : SearchBrandList;

            string UserId = "";
            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                UserId = User.FindFirst("Id").Value;
            }
            
            ViewBag.Product_List = _product_repository.User_Product_List_Sql(UserId, Category_Id, Sort_Gubun, SearchCategoryList, SearchBrandList).ToPagedList(pageNum, PageSize);
            ViewBag.Icon_List = _product_repository.User_Product_Icon_List_Entity();

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;
            ViewData["Category_Id"] = Category_Id;
            ViewData["SearchCategoryList"] = SearchCategoryList.Trim();
            ViewData["SearchBrandList"] = SearchBrandList.Trim();
            ViewData["Sort_Gubun"] = Sort_Gubun;

            return View();
        }


        /// <summary>
        /// 특정 상품의 총 위시리스트 개수 구하기 
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        [Route("Product/Total_Wish_Cnt/{Product_ID}/")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public int Product_Detail_Total_WishCnt(int Product_ID)
        {
            return _product_repository.Product_Detail_Total_Wish_Cnt(Product_ID);
        }


        /// <summary>
        /// 특정 회원당 특정 상품의  위시리스트 개수 구하기 
        /// </summary>
        /// <param name="Product_ID"></param>
        /// <returns></returns>
        [Route("Product/Mem_Wish_Cnt/{Product_ID}/")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public int Product_Detail_Member_WishCnt(int Product_ID)
        {
            string User_ID = "";
            if (User.Identity.IsAuthenticated && (User.IsInRole("Users")))
            {
                User_ID = User.FindFirst("Id").Value;
            }

            return _product_repository.Product_Detail_Member_Wish_Cnt(Product_ID, User_ID);
        }


        [Route("Product/Main_Category_List/{Page?}/{PageSize?}/{Category_Id?}/{Sort_Gubun?}/{SearchCategoryList?}/{SearchBrandList?}/")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult Main_Category_List(int? Page, int PageSize, int Category_Id, int Sort_Gubun, string SearchCategoryList, string SearchBrandList)
        {

            ViewBag.Category_Menu_List = _operation_repository.User_Menu_List_Entity("CTC01", Category_Id).ToList(); //상단 대분류카테고리명(메인)
            ViewBag.Brandlist = _product_repository.Common_CodeList_Entity("Product_Brand_Code").ToList();

            PageSize = (PageSize < 1) ? 20 : PageSize;
            var pageNum = Page ?? 1;
            //Sort_Gubun = (Sort_Gubun.Equals(0)) ? 1 : Sort_Gubun;
            SearchCategoryList = SearchCategoryList.Equals("0") ? "" : SearchCategoryList;
            SearchBrandList = SearchBrandList.Equals("0") ? "" : SearchBrandList;

            string UserId = "";
            if (User.Identity.IsAuthenticated && (User.IsInRole("Users") || User.IsInRole("Guest")))
            {
                UserId = User.FindFirst("Id").Value;
            }

            ViewBag.Product_List = _product_repository.User_Product_List_Sql(UserId, Category_Id, Sort_Gubun, SearchCategoryList, SearchBrandList).ToPagedList(pageNum, PageSize);
            ViewBag.Icon_List = _product_repository.User_Product_Icon_List_Entity();

            ViewData["Page"] = Page ?? 1;
            ViewData["PageSize"] = PageSize;
            ViewData["Category_Id"] = Category_Id;
            ViewData["SearchCategoryList"] = SearchCategoryList.Trim();
            ViewData["SearchBrandList"] = SearchBrandList.Trim();
            ViewData["Sort_Gubun"] = Sort_Gubun;
            return View();
        }
    


        //[Route("Product/Category_Product_List/{Page?}/{PageSize?}/{Category_Id?}/{Sort_Gubun?}/{SearchCategoryList?}/{SearchBrandList?}/")]
        //[AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        //public IActionResult Category_Product_List(int? Page, int PageSize, int Category_Id, int Sort_Gubun, string SearchCategoryList, string SearchBrandList)
        //{
        //    //ViewBag.Category_Menu_List = _operationrepository.User_Menu_List_Entity("CTC02", Category_Id).ToList(); //상단 대분류카테고리명
        //    //ViewBag.Brandlist = _repository.Common_CodeList_Entity("Product_Brand_Code").ToList();

        //    //PageSize = (PageSize < 18) ? 18 : PageSize;
        //    //var pageNum = Page ?? 1;
        //    //Sort_Gubun = (Sort_Gubun.Equals(0)) ? 1 : Sort_Gubun;
        //    //SearchCategoryList = SearchCategoryList.Equals("0") ? "" : SearchCategoryList;
        //    //SearchBrandList = SearchBrandList.Equals("0") ? "" : SearchBrandList;

        //    //ViewBag.Product_List = _repository.User_Product_List_Sql(User.FindFirst("Id").Value, Category_Id, Sort_Gubun, SearchCategoryList, SearchBrandList).ToPagedList(pageNum, PageSize);

        //    //ViewData["Page"] = Page ?? 1;
        //    //ViewData["PageSize"] = PageSize;

        //    return View();
        //}

        // <summary>
        /// 리스트 -> 클릭한 상품의 기본 정보 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Product/PreView_Url/{id?}")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public IActionResult PreView_Url(int id)
        {
            return Json(new { Product_Info = _product_repository.User_Product_PreViewImg_Entity(id) });
        }




        [Route("Product/Wish_Cnt")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public int Wish_Cnt()
        {
            
            string UserId = "";

            if (User.Identity.IsAuthenticated && (User.IsInRole("Users")))
            {
                UserId = User.FindFirst("Id").Value;
            }
            if(string.IsNullOrEmpty(UserId))
            {
                return 0;
            }
            else
            {
                return _operation_repository.User_Wish_Total_Cnt_Entity(UserId);
            }
           
        }




        [Route("Product/Wish_Save/{id?}/{Gubun}")]
        [AllowAnonymous] // 인증되지 않은 사용자도 접근 가능
        public string Wish_Save(int id, string Gubun)
        {
            TB_Wish_List Wish_List = new TB_Wish_List();
            Wish_List.Product_ID = id;
            try
            {
                Wish_List.User_ID = User.FindFirst("Id").Value;
            }
            catch { Wish_List.User_ID = ""; }

            Wish_List.Regist_DateTime = DateTime.Now;
            return _operation_repository.User_Wish_Save_Entity(Wish_List, Gubun);


        }


        [AllowAnonymous]

        [Route("Product/Product_Detail_View/{Product_ID?}")]
        public IActionResult Product_Detail_View(int Product_ID)
        {
            ViewData["PreView_Url"] =  _product_repository.Get_Product_PreView_Url(Product_ID);

            return View();


        }
    }

}
