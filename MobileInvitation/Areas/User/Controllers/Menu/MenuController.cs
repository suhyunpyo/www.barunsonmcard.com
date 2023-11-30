using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MobileInvitation.Config;
using MobileInvitation.Data.Operation;
using MobileInvitation.Data.Product;
using MobileInvitation.FunctionHelper;
using MobileInvitation.Models;
using System.Linq;

namespace MobileInvitation.Areas.User.Controllers.Menu
{
    [Area("User")]
    public class MenuController : PathController
    {
        private readonly IOperationRepository _operationrepository;
        private readonly IProductRepository _ProductRepository;

        public MenuController(IOperationRepository operationrepository, IProductRepository ProductRepository, IWebHostEnvironment environment,
            IHttpContextAccessor accessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
            : base(environment, accessor, barunsonContext, barunnConfig)
        {
            _operationrepository = operationrepository;
            _ProductRepository = ProductRepository;
        }

        /// <summary>
        /// 공통 - GNB 상단 / 하단메뉴 리스트 
        /// </summary>
        /// <param name="Category_Type">CTC01: 메인 / CTC02: 카테고리</param>
        /// <returns></returns>
        [Route("User/Menu/{Category_Type}/")]
        public IActionResult Index(/*string Code*/ string Category_Type)
        {
            ViewBag.Menu_List = _operationrepository.Admin_Menu_List_Entity().ToList(); //GNB 상단 / 하단메뉴 리스트  
            ViewBag.Category_Menu_List = _operationrepository.User_Menu_List_Entity(Category_Type, 0).ToList(); //상단 대분류카테고리명
            return Json(new { menu_list = ViewBag.Menu_List, category_menu_list = ViewBag.Category_Menu_List });
        }


        /// <summary>
        /// 프런트 - 배너 클릭수 업데이트 
        /// </summary>
        /// <param name="id">배너 아이디</param>
        /// <returns></returns>
        [Route("/User/Banner/Click_Update/{id}")]
        public IActionResult Banner_Click_Update(int id)
        {
            string ReturnValue = _operationrepository.User_Banner_Click_Update_Entity(id);

            return Json(new { success = ReturnValue });

        }



        /// <summary>
        /// 메인 - 팝업 노출 
        /// </summary>
        /// <param name="Popup_Type"></param>
        /// <returns></returns>
        [Route("User/Popup/{Popup_Type}")]
        public IActionResult Popop_View(string Popup_Type)
        {
            ViewBag.PopopList = _operationrepository.User_Popup_List_Entity("", Popup_Type);
            //ViewBag.PopopList = _operationrepository.User_Popup_List_Entity("메인팝업", Popup_Type);
            return Json(new { Popup = ViewBag.PopopList });
        }

        [Route("UserInfoAgreement")]
        public IActionResult UserInfoAgreement()
        {
            ViewBag.Policy_Contents = _operationrepository.PolicyInfo_View_Entity("P", 0).Contents;
            return View();     
        }

        [Route("UseAgreement")]
        public IActionResult UseAgreement()
        {
            ViewBag.Policy_Contents = _operationrepository.PolicyInfo_View_Entity("U", 0).Contents;
            return View();
        }

        [Route("PolicyHistory/{policy_divname}/{id}")]
        public IActionResult PolicyHistory(string policy_divname, int id=0)
        {
            int seq = 0;
            string policy_div = "P";
            string policy_title = "개인정보 처리방침";


			switch (policy_divname.ToLower())
            {
                case "service":
                    {
                        policy_div = "U";
						policy_title = "이용 약관";
						break;
                    }
                case "privacy":
                    {
                        policy_div = "P";
						policy_title = "개인정보 처리방침";
						break;
                    }                
            }

            var policyHistory = _operationrepository.PolicyInfo_History_List_Entity(policy_div);

            if(id>0)
            {
                seq= id;
            }
			else if (policyHistory.Count>0)
            {
                seq = policyHistory[0].Seq;                
            }

            if (seq > 0)
            {
                ViewBag.Policy_Contents = _operationrepository.PolicyInfo_View_Entity(policy_div, seq).Contents;
            }
            else
            {
                ViewBag.Policy_Contents = "";
			}

            ViewBag.Policy_Seq = seq.ToString();
			ViewBag.Policy_List = policyHistory; 
			ViewBag.Policy_DivName = policy_divname;
			ViewBag.Policy_Title = policy_title;

			return View();
        }


        [Route("Service_Intro")]
        public IActionResult Service_Intro()
        {
            return View();

            //ProductController aa = new ProductController();
            //aa.Wish_Save()
        }

        [Route("Event_List")]
        public IActionResult Event_List()
        {
            ViewBag.CDNUrl = _barunnConfig.Sites.CDNUrl;
            ViewBag.Event_Banner = _operationrepository.Admin_Banner_Add_List_Entity2("이벤트 배너", Request).ToList();

            return View();
        }


        [Route("remittance")]
        public IActionResult remittance()
        {
          //  ViewBag.Event_Banner = _operationrepository.Admin_Banner_Add_List_Entity2("이벤트 배너", Request).ToList();

            return View();

            //ProductController aa = new ProductController();
            //aa.Wish_Save()
        }


        [Route("paper_invitation")]
        public IActionResult paper_invitation()
        {
            ViewBag.ProductList = _ProductRepository.Product_MainImage_List_Entity().ToList();

            return View();

            //ProductController aa = new ProductController();
            //aa.Wish_Save()
        }


        [Route("event/onsil")]
        public IActionResult onsil()
        {
            return View();

            //ProductController aa = new ProductController();
            //aa.Wish_Save()
        }

        

        [Route("event/minvitation")]
        public IActionResult minvitation()
        {
            return View();
        }

        [Route("event/GiftShop")]
        public IActionResult GiftShop()
        {
            return View();
        }

        [Route("event/IntroGift")]
        public IActionResult IntroGift()
        {
            return View();
        }

        [Route("event/SetCard")]
        public IActionResult SetCard()
        {
            return View();
        }
        [Route("event/EventPrice")]
        public IActionResult EventPrice()
        {
            return View();
        }

        [Route("/ProductBrand")]
        public IActionResult ProductBrand()
        {
            return View();
        }

        [Route("/intro/IntroThanks")]
        public IActionResult IntroThanks()
        {
            return View();
        }
        [Route("/allaboutmcard")]
        public IActionResult allaboutmcard()
        {
            return View();
        }

    }
}

