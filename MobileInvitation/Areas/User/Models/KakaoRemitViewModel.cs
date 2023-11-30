using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileInvitation.Areas.User.Models
{
	/// <summary>
	/// 송금 내역 뷰 모델
	/// </summary>
	public class MyKakaoRemitViewModel : PageViewModel
	{
		/// <summary>
		/// 주문 ID
		/// </summary>
		public int OrderId { get; set; }

        /// <summary>
        /// 누적 입금수
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 누적 입금액
        /// </summary>
        public int TotalPrice { get; set; }

		/// <summary>
		/// 서비스 이용료
		/// </summary>
		public int TotalTax { get; set; }

		/// <summary>
		/// 정산 완료
		/// </summary>
		public int TotalRemitPrice { get; set; }

		/// <summary>
		/// 송금대상필터
		/// </summary>
		public string AccountTypeCode { get; set; } = "";
		/// <summary>
		/// 송금 대상 목록
		/// </summary>
		public SelectList AccountTypeList { get; set; }

		public List<MyKakaoRemitDataModel> DataModel { get; set; }
		/// <summary>
		/// 노출 배너 목록
		/// </summary>
		public List<BannerViewModel> Banners { get; set; }

		public override int PageSize { get; set; } = 5;

		public override Dictionary<string, string> RouteData
		{
			get
			{
				var routeall = new Dictionary<string, string>
				{
					{ nameof(OrderId), OrderId.ToString() },
					{ nameof(AccountTypeCode), AccountTypeCode ?? "" },
					{ nameof(PageSize), PageSize.ToString() },
				};
				return routeall;
			}
		}
	}

	/// <summary>
	/// 송금 내역 데이터 뷰 모델
	/// </summary>
	public class MyKakaoRemitDataModel
	{
		public int No { get; set; }

		/// <summary>
		/// 입금일
		/// </summary>
		public DateTime? CompleteDate { get; set; }
		/// <summary>
		/// 입금자
		/// </summary>
		public string RemitterName { get; set; }
		/// <summary>
		/// 예금주
		/// </summary>
		public string DepositorName { get; set; }

		/// <summary>
		/// 입금액
		/// </summary>
		public int Price { get; set; }

		/// <summary>
		/// 서비스 이용료
		/// </summary>
		public int Tax { get; set; }

		/// <summary>
		/// 정산 완료
		/// </summary>
		public int RemitPrice { get; set; }
		/// <summary>
		/// 미정산
		/// </summary>
		public int NonRemitPrice { get; set; }

		/// <summary>
		/// 정산일
		/// </summary>
		public DateTime? CalculateDate { get; set; }

		/// <summary>
		/// 송금 대상 코드
		/// </summary>
		public string AccountTypeCode { get; set; }

	}

	/// <summary>
	/// 계좌 등록 뷰 모델
	/// </summary>
	public class MyKakaoRemitAccountModel
	{
		/// <summary>
		/// 주문 ID
		/// </summary>
		public int OrderId { get; set; }

		/// <summary>
		/// 송금대상
		/// </summary>
		public string AccountTypeCode { get; set; }
		/// <summary>
		/// 은행코드
		/// </summary>
		public string BankCode { get; set; }
		/// <summary>
		/// 계좌번호
		/// </summary>
		public string AccountNumber { get; set; }
		/// <summary>
		/// 예금주
		/// </summary>
		public string DepositorName { get; set; }

		/// <summary>
		/// 등록된 계좌 목록
		/// </summary>
		public List<MyKakaoRemitAccountDataModel> DataModel { get; set; }

		/// <summary>
		/// 송금 대상 목록
		/// </summary>
		public SelectList AccountTypeList { get; set; }

		/// <summary>
		/// 은행 목록
		/// </summary>
		public SelectList BankList { get; set; }
	}

	/// <summary>
	/// 계좌 등록 데이터뷰 모델
	/// </summary>
	public class MyKakaoRemitAccountDataModel
	{
		/// <summary>
		/// 계좌 ID
		/// </summary>
		public int AccountId { get; set; }
		/// <summary>
		/// 송금대상
		/// </summary>
		public string AccountTypeCode { get; set; }
		public string AccountTypeName { get; set; }
		/// <summary>
		/// 은행코드
		/// </summary>
		public string BankCode { get; set; }
		public string BankName { get; set; }
		/// <summary>
		/// 계좌번호
		/// </summary>
		public string AccountNumber { get; set; }
		/// <summary>
		/// 예금주
		/// </summary>
		public string DepositorName { get; set; }
	}

	public class RemitAccountToViewModel
	{
        /// <summary>
        /// 계좌 ID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 송금대상
        /// </summary>
        public string AccountTypeName { get; set; }
        /// <summary>
        /// 송금자 명
        /// </summary>
        public string RemitterName { get; set; }

        /// <summary>
        /// 입금액
        /// </summary>
        public int Price { get; set; }
    }


}
