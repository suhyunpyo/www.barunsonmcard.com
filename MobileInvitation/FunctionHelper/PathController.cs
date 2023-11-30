using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobileInvitation.Areas.User.Models;
using MobileInvitation.Config;
using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.FunctionHelper
{
    public class PathController : Controller
    {
		protected readonly IHttpContextAccessor _httpContextAccessor;
		protected IWebHostEnvironment _environment; // 환경 변수
        protected readonly barunsonContext _barunsonDb;

        protected readonly BarunnConfig _barunnConfig;
		protected readonly Uri CdnUri;
		protected readonly Uri SiteUri;

		public PathController(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, barunsonContext barunsonContext, BarunnConfig barunnConfig)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _barunsonDb = barunsonContext;

            _barunnConfig = barunnConfig;
			CdnUri = _barunnConfig.Sites.CDNUrl;
			SiteUri = _barunnConfig.Sites.Url;
		}


        /// <summary>
        /// 호스트네임 가져오기
        /// </summary>
        /// <returns></returns>
        public string Search_HostName()
        {
            return _httpContextAccessor.HttpContext.Request.Host.ToString();
        }


        /// <summary>
        /// 서비스 환경에 따른 업로드 경로 가져오기 
        /// </summary>
        /// <returns></returns>
        public string Upload_Path()
        {
            return System.IO.Path.Combine(_barunnConfig.FileConfig.UploadPath, _barunnConfig.FileConfig.UploadContainer);
        }

        public string Moblie_Redirect(Microsoft.AspNetCore.Routing.RouteValueDictionary routeValues, HttpRequest request)
        {
            var route = routeValues;
            var action = route["action"];
            var controller = route["controller"];
            var url = action.ToString();

            if (UrlHelper.IsMobile(request))
            {
                url = "m_" + action.ToString();
            }
            return url;
        }

		/// <summary>
		/// 리소스 절대 URL 생성 -
		/// 이미지등 정적 콘텐츠
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public Uri GetResourceAbsoluteUrl(string url)
		{
            Uri result = null;
            if (!string.IsNullOrEmpty(url))
            {
                var rurl = url.Replace("\\", "/");

                if (rurl.StartsWith("/upload/", StringComparison.InvariantCultureIgnoreCase))
                    result = new Uri(CdnUri, rurl);
                else if (rurl.StartsWith("/img/", StringComparison.InvariantCultureIgnoreCase))
                    result = new Uri(SiteUri, rurl);
                else if (rurl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    result = new Uri(rurl);
            }
			return result;
		}

		/// <summary>
		/// DB상의 결혼일및 시간을 날짜형식으로 변환
		/// </summary>
		/// <param name="weddingdate"></param>
		/// <param name="whoure"></param>
		/// <param name="wmin"></param>
		/// <param name="typecode"></param>
		/// <returns></returns>
		protected DateTime? GetWeddingDate(string weddingdate, string whoure, string wmin, string typecode)
		{
			if (string.IsNullOrEmpty(weddingdate) || weddingdate == "1900-01-01")
				return null;

			try
			{
				var wdate = DateTime.Parse(weddingdate);
				if (!string.IsNullOrEmpty(whoure))
				{
					var h = int.Parse(whoure.Trim());
					if (typecode == "오후" && h < 12)
						h += 12;

					wdate = wdate.AddHours(h);
				}
				if (!string.IsNullOrEmpty(wmin))
					wdate = wdate.AddMinutes(int.Parse(wmin.Trim()));

				return wdate;
			}
			catch
			{
				return null;
			}
		}

        /// <summary>
        /// 배너 목록 
        /// </summary>
        /// <param name="cateName"></param>
        /// <returns></returns>
        protected async Task<List<BannerViewModel>> GetBanners(string cateName)
        {
            var result = new List<BannerViewModel>();
            var bannerType = "BTC01"; //PC
            if (UrlHelper.IsMobile(Request))
                bannerType = "BTC02";

            var nowDate = DateTime.Now;

            var bquery = from bc in _barunsonDb.TB_Banner_Categories
                         join b in _barunsonDb.TB_Banners on bc.Banner_Category_ID equals b.Banner_Category_ID
                         join bi in _barunsonDb.TB_Banner_Items on b.Banner_ID equals bi.Banner_ID
                         where bc.Banner_Category_Name == cateName && bi.Banner_Type_Code == bannerType
                         orderby b.Banner_ID, bi.Sort
                         select new
                         {
                             b.Banner_ID,
                             bi.Banner_Item_ID,
                             bi.Sort,
                             bi.Image_URL,
                             bi.Link_URL,
                             bi.Banner_Main_Description,
                             bi.Banner_Add_Description,
                             bi.Deadline_Type_Code,
                             bi.Start_Date,
                             bi.Start_Time,
                             bi.End_Date,
                             bi.End_Time,
                             bi.NewPage_YN
                         };
            var bItems = await bquery.ToListAsync();
            if (bItems.Count > 0)
            {
                foreach (var item in bItems)
                {
                    //유효기간 있을 경우
                    if (item.Deadline_Type_Code != "PTC02")
                    {
                        //기본 날짜 없으면 표시 하지 않음
                        if (string.IsNullOrEmpty(item.Start_Date) || string.IsNullOrEmpty(item.Start_Time) || string.IsNullOrEmpty(item.End_Date) || string.IsNullOrEmpty(item.End_Time))
                        {
                            continue;
                        }

                        var sdt = DateTime.Parse(item.Start_Date).AddHours(int.Parse(item.Start_Time));
                        var edt = DateTime.Parse(item.End_Date).AddHours(int.Parse(item.End_Time));

                        //날짜 범위 검사
                        if (!(sdt < nowDate && edt > nowDate))
                        {
                            continue;
                        }
                    }
                    result.Add(new BannerViewModel
                    {
                        BannerId = item.Banner_ID,
                        BannerItemId = item.Banner_Item_ID,
                        Sort = item.Sort ?? 0,
                        ImageUrl = GetResourceAbsoluteUrl(item.Image_URL),
                        LinkUrl = item.Link_URL,
                        MainDescription = item.Banner_Main_Description,
                        AddDescription = item.Banner_Add_Description,
                        IsNewWindow = item.NewPage_YN == "Y"
                    });

                }
            }
            return result;
        }


		/// <summary>
		/// 공통 코드 목록 - 코드 그룹
		/// </summary>
		/// <param name="codeGroup"></param>
		/// <returns></returns>
		protected async Task<List<TB_Common_Code>> GetCommonCodeAsync(string codeGroup)
		{
			var query = from r in _barunsonDb.TB_Common_Codes
						where r.Code_Group == codeGroup
						orderby r.Sort
						select r;

			return await query.ToListAsync();
		}
		/// <summary>
		/// 공통 코드 SelectList 출력
		/// </summary>
		/// <param name="codeGroup"></param>
		/// <param name="addAll">All 추가 여부</param>
		/// <param name="allValue"></param>
		/// <param name="allText"></param>
		/// <returns></returns>
		protected async Task<IEnumerable<SelectListItem>> GetSelectListsCommonCodesAsync(string codeGroup,
			bool addAll = false, string allValue = "", string allText = "전체")
		{
			var codes = await GetCommonCodeAsync(codeGroup);
			var items = codes.Select(m => new SelectListItem { Text = m.Code_Name, Value = m.Code }).ToList();
			if (addAll)
				items.Insert(0, new SelectListItem { Text = allText, Value = allValue });

			return new SelectList(items, "Value", "Text");
		}

		/// <summary>
		/// 일자별 유니크 숫자 생성
		/// </summary>
		/// <param name="date"></param>
		/// <param name="context"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		protected async Task<int> GetTelegramNoAsync(string date)
		{
			var uniquNum = -1;

			using (var tran = await _barunsonDb.Database.BeginTransactionAsync())
			{
				var q = from a in _barunsonDb.TB_Daily_Uniques
						where a.Request_Date == date
						select a.Unique_Number;
				var item = await q.MaxAsync(m => (int?)m);
				if (!item.HasValue)
					item = 0;

				uniquNum = item.Value + 1;

				var addItem = new TB_Daily_Unique { Request_Date = date, Unique_Number = uniquNum };
				_barunsonDb.TB_Daily_Uniques.Add(addItem);
				await _barunsonDb.SaveChangesAsync();

				await tran.CommitAsync();
			}
			return uniquNum;
		}
	}
}
