using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileInvitation.Config;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Product;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System.Linq;

namespace MobileInvitation.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(AuthenticationSchemes = "userAuth", Roles = "Users, Guest")]
    public class MainController : PathController
    {
        private readonly IOperationRepository _operationrepository;
        private readonly IProductRepository _productrepository;

        public MainController(IOperationRepository operationrepository, IProductRepository productrepository, IWebHostEnvironment environment,
            IHttpContextAccessor accessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _operationrepository = operationrepository;
            _productrepository = productrepository;
        }
        //[Route("")]
        //[Route("User/Home/Index/{id?}")]

        [AllowAnonymous]
        public IActionResult Index(int id, string temp)
        {
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl;
            ViewBag.Banner_Pc_List3 = _operationrepository.Admin_Banner_Add_List_Entity2("main banner 3", Request).ToList();
            ViewBag.Banner_Pc_List4 = _operationrepository.Admin_Banner_Add_List_Entity2("main banner 4", Request).ToList();

            string User_Id = "";

            if (User.Identity.IsAuthenticated && (User.IsInRole("Users")))
            {
                User_Id = User.FindFirst("Id").Value;
            }
           
            ViewBag.Product_List = _productrepository.Display_ProductList_Sql("CTC01", null, null, 0, null, null, "Y", User_Id).ToList();

            return View();
           
        }


        [AllowAnonymous]
        [Route("User/MainBanner/{BannerType}")]
        public IActionResult MainBanner(string BannerType)
        {
            BannerType = "main banner " + BannerType;

            ViewBag.MainBanner = _operationrepository.Admin_Banner_Add_List_Entity2(BannerType, Request).ToList();
            return Json(new { Banner = ViewBag.MainBanner });
        }

    }
}
