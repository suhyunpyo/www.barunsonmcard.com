using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileInvitation.Areas.User.Models
{
    #region Nice 인증 모델

    /// <summary>
    /// Nice 인증 결과 암호화 데이터 모델
    /// </summary>
    public class FindIdViewModel
    {
        public string tokenVersionId { get; set; }
        public string encData { get; set; }
        public string integrityValue { get; set; }
    }
    /// <summary>
    /// ID/Pwd 찾기 결과 모델
    /// </summary>
    public class FindIdResponseViewModel
    {
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 검색 타입 id or pwd
        /// </summary>
        public string SearchType { get; set; } = "id";
        public string UserID { get; set; }

        public string AuthDupInfo { get; set; }
    }
    /// <summary>
    /// 응답 데이터
    /// </summary>
    public class NiceApiResponseData
    {
        /// <summary>
        /// 결과코드 result_code가 성공(0000)일 때만 전달
        /// </summary>
        [JsonPropertyName("resultcode")]
        public string ResultCode { get; set; }

        /// <summary>
        /// [필수] 서비스 요청 고유 번호
        /// </summary>
        [JsonPropertyName("requestno")]
        public string RequestNo { get; set; }

        /// <summary>
        /// 암호화 일시(YYYYMMDDHH24MISS)
        /// </summary>
        [JsonPropertyName("enctime")]
        public string EncTime { get; set; }

        /// <summary>
        /// [필수] 암호화토큰요청 API에 응답받은 site_code
        /// </summary>
        [JsonPropertyName("sitecode")]
        public string SiteCode { get; set; }

        /// <summary>
        /// 응답고유번호
        /// </summary>
        [JsonPropertyName("responseno")]
        public string ResponseNo { get; set; }

        /// <summary>
        /// 인증수단
        /// M	휴대폰인증		
        /// C 카드본인확인
        /// X 공동인증서
        /// F 금융인증서
        /// S PASS인증서
        /// </summary>
        [JsonPropertyName("authtype")]
        public string AuthType { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// UTF8로 URLEncoding된 이름 값
        /// </summary>
        [JsonPropertyName("utf8_name")]
        public string utf8_name { get; set; }

        /// <summary>
        /// 생년월일 8자리
        /// </summary>
        [JsonPropertyName("birthdate")]
        public string BirthDate { get; set; }

        /// <summary>
        /// 성별 0:여성, 1:남성
        /// </summary>
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// 내외국인 0:내국인, 1:외국인
        /// </summary>
        [JsonPropertyName("nationalinfo")]
        public string NationalInfo { get; set; }

        /// <summary>
        /// 이통사 구분(휴대폰 인증 시)
        /// 1	SK텔레콤		
        /// 2	KT		
        /// 3	LGU+		
        /// 5	SK텔레콤 알뜰폰		
        /// 6	KT 알뜰폰		
        /// 7	LGU+ 알뜰폰
        /// </summary>
        [JsonPropertyName("mobileco")]
        public string MobileCo { get; set; }

        /// <summary>
        /// 휴대폰 번호(휴대폰 인증 시)
        /// </summary>
        [JsonPropertyName("mobileno")]
        public string MobileNo { get; set; }

        /// <summary>
        /// 개인 식별 코드(CI)
        /// </summary>
        [JsonPropertyName("ci")]
        public string ci { get; set; }

        /// <summary>
        /// 개인 식별 코드(di)
        /// </summary>
        [JsonPropertyName("di")]
        public string di { get; set; }

        /// <summary>
        /// 사업자번호(법인인증서 인증시)
        /// </summary>
        [JsonPropertyName("businessno")]
        public string BusinessNo { get; set; }

        /// <summary>
        /// 인증 후 전달받을 데이터 세팅 (요청값 그대로 리턴)
        /// </summary>
        [JsonPropertyName("receivedata")]
        public string ReceiveData { get; set; } = "";
    }
    #endregion

    #region MyPage 모델, 주문 목록 뷰
    /// <summary>
    /// MyPage Main
    /// </summary>
    public class MyPageViewModel
    {
        /// <summary>
        /// Tab 메뉴 ID
        /// 0=제작중, 1=제작완료, 2=취소/환불
        /// </summary>
        public int TabId { get; set; }
        /// <summary>
        /// 노출 배너 목록
        /// </summary>
        public List<BannerViewModel> Banners { get; set; }
    }
    /// <summary>
    /// 마이페이지 주문 목록
    /// </summary>
    public class MyPageOrderViewModel
    {
        public bool IsMobileView { get; set; }
        /// <summary>
        /// Tab 메뉴 ID
        /// 0=제작중, 1=제작완료, 2=취소/환불
        /// </summary>
        public int TabId { get; set; }

        /// <summary>
        /// 제작중 수
        /// </summary>
        public int MakingCount { get; set; }
        /// <summary>
        /// 제작완료 수
        /// </summary>
        public int CompleteCount { get; set; }

        /// <summary>
        /// 취소 수
        /// </summary>
        public int CancelCount { get; set; }

        /// <summary>
        /// 주문 목록
        /// </summary>
        public List<MyOrderItemViewModel> DataModels { get; set; }

        
    }

    /// <summary>
    /// 내 주문 내역
    /// </summary>
    public class MyOrderItemViewModel
    {
        /// <summary>
        /// 주문 ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 주문 번호
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 주문 금액
        /// </summary>
        public int OrderPrice { get; set; }

        /// <summary>
        /// 초대장 ID
        /// </summary>
        public int InvitationId { get; set; }

        /// <summary>
        /// 주문일
        /// </summary>
        public DateTime OrderDateTime { get; set; }
        

        /// <summary>
        /// 상품 유형
        /// </summary>
        public string ProductCategoryCode { get; set; }
        /// <summary>
        /// 상품 유형 명
        /// </summary>
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// 상품코드
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 상품 명
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 상품 썸네일 이미지
        /// </summary>
        public string MainImageUrl { get; set; }
        /// <summary>
        /// 상품 썸네일 이미지 Full url
        /// </summary>
        public Uri MainImageUrlFull { get; set; }

        /// <summary>
        /// 상품 금액
        /// </summary>
        public int ProductPrice { get; set; }

        /// <summary>
        /// 지불 상태 코드
        /// </summary>
        public string PaymentStatusCode { get; set; }
        /// <summary>
        /// 지물 상태 명
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 지불 수단 코드
        /// </summary>
        public string PaymentMethodCode { get; set; }

        /// <summary>
        /// 지불 수단 명
        /// </summary>
        public string PaymentMethodName { get; set; }

        public string FinanceName { get; set; }
        public string AccountNumber { get; set; }

        /// <summary>
        /// 실제 결제 금액
        /// </summary>
        public int? PaymentPrice { get; set; }

        /// <summary>
        /// 결제일
        /// </summary>
        public DateTime? PaymentDateTime { get; set; }

        public string RefundTypeCode { get; set; }
        /// <summary>
        /// 지불 명
        /// </summary>
        public string RefundStatusCode { get; set; }
        public string PayName { get; set; }
        /// <summary>
        /// 취소 환불시 상태 값
        /// </summary>
        public string RefundStatusName { get; set; }
        /// <summary>
        /// 취소 환불시 날짜
        /// </summary>
        public DateTime? RefundDateTime { get; set; }
        /// <summary>
        /// 환불 금액
        /// </summary>
        public int? RefundPrice { get; set; }
        /// <summary>
        /// 결혼 일자
        /// </summary>
        public DateTime? WeddingDate { get; set; }
        public string WDate { get; set; }
        public string WHH { get; set; }
        public string WMM { get; set; }
        public string WTC { get; set; }

        /// <summary>
        /// 노출 여부
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 상태 뱃지
        /// 1. class type01 : 노출
        /// 2. class type02 : 기간만료
        /// 3. class type03 : 노출중지
        /// </summary>
        public string BadgeStatus { get; set; }
        public string BadgeMent { get; set; }

        /// <summary>
        /// 축의금 관리 버튼 여부
        /// </summary>
        public bool IsMoneyGift { get; set; } = false;
        /// <summary>
        /// 축의금 수
        /// </summary>
        public int MoneyGiftCount { get; set; }
        public Uri? MoneyGiftUrl { get; set; }

        /// <summary>
        /// 화환 선물 관리 버튼 여부
        /// </summary>
        public bool isFlower { get; set; } = false;
        public Uri? FlowerUrl { get; set; }
    }
    #endregion

    #region MyPage 모델, 주문 상세
    /// <summary>
    /// 내 주문 상세, 완료된 주문
    /// </summary>
    public class MyOrderDetailViewModel
    {
        /// <summary>
        /// 주문 ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 주문 번호
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 주문 금액
        /// </summary>
        public int OrderPrice { get; set; }

        /// <summary>
        /// 초대장 ID
        /// </summary>
        public int InvitationId { get; set; }

        /// <summary>
        /// 주문일
        /// </summary>
        public DateTime OrderDateTime { get; set; }


        /// <summary>
        /// 상품 유형
        /// </summary>
        public string ProductCategoryCode { get; set; }
        /// <summary>
        /// 상품 유형 명
        /// </summary>
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// 상품코드
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 상품 명
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 상품 금액
        /// </summary>
        public int ProductPrice { get; set; }

        /// <summary>
        /// 쿠폰 사용 금액
        /// </summary>
        public int? CouponPrice { get; set; }

        /// <summary>
        /// 지불 상태 코드
        /// </summary>
        public string PaymentStatusCode { get; set; }
        /// <summary>
        /// 지물 상태 명
        /// </summary>
        public string PaymentStatusName { get; set; }

        /// <summary>
        /// 지불 수단 코드
        /// </summary>
        public string PaymentMethodCode { get; set; }

        /// <summary>
        /// 지불 수단 명
        /// </summary>
        public string PaymentMethodName { get; set; }

        public string FinanceName { get; set; }
        public string AccountNumber { get; set; }

        /// <summary>
        /// 실제 결제 금액
        /// </summary>
        public int? PaymentPrice { get; set; }

        /// <summary>
        /// 결제일
        /// </summary>
        public DateTime? PaymentDateTime { get; set; }

        /// <summary>
        /// 초대장 URL
        /// </summary>
        public string InvitationUrl { get; set; }
        /// <summary>
        /// 초대장 Full url
        /// </summary>
        public Uri InvitationUrlFull { get; set; }

        /// <summary>
        /// 노출 여부
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 입금 만료 일
        /// </summary>
        public DateTime? DeadLineDate { get; set; }
        /// <summary>
        /// 취소 가능 여부
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 취소/환불 불가 사유
        /// </summary>
        public string NotCancelText { get; set; }

    }
    #endregion

    #region MyPage 모델, Wish
    /// <summary>
    /// Wish view model
    /// </summary>
    public class MyWishViewModel : PageViewModel
    {
        public List<MyWishDataModel> DataModel { set; get; }

        public override Dictionary<string, string> RouteData
        {
            get
            {
                var routeall = new Dictionary<string, string>
                {
                    { nameof(PageSize), PageSize.ToString() },
                };
                return routeall;
            }
        }
         
    }
    public class MyWishDataModel
    {

        public int WishID { get; set; }

        /// <summary>
        /// 상품 번호
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 상품코드
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 상품 썸네일 이미지
        /// </summary>
        public string MainImageUrl { get; set; }
        /// <summary>
        /// 상품 썸네일 이미지 Full url
        /// </summary>
        public Uri MainImageUrlFull { get; set; }

        /// <summary>
        /// 상품 명
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 상품 금액
        /// </summary>
        public int ProductPrice { get; set; }

    }
    #endregion

    #region 쿠폰
    public class MyCouponViewModel : PageViewModel
    {
        public List<MyCouponDataModel> DataModel { set; get; }
        public int UseCouponCount { get; set; }
        public override Dictionary<string, string> RouteData
        {
            get
            {
                var routeall = new Dictionary<string, string>
                {
                    { nameof(PageSize), PageSize.ToString() },
                };
                return routeall;
            }
        }

    }
    public class MyCouponDataModel
    {
        public int CouponID { get; set; }
        public string CouponName { get; set; }

        /// <summary>
        /// 사요가능 여부
        /// </summary>
        public bool IsCopuponUsing { get; set; }

        public string DiscountMethodCode { get; set; }
        public double? DiscountRate { get; set; }
        public int? DiscountPrice { get; set; }

        /// <summary>
        /// 할인방식
        /// </summary>
        public string DiscountViewText
        {
            get
            {
                var result = string.Empty;
                if (DiscountMethodCode == "DMC01") //금액
                    result = $"{DiscountPrice:#,##0}원";
                else if (DiscountMethodCode == "DMC02") //%
                    result = $"{DiscountRate}%";
                else if (DiscountMethodCode == "DMC03") //전액
                    result = $"전액";

                if (!IsCopuponUsing) result = "기간만료";
                return result;
            }
        }
        public string PeriodMethodCode { get; set; }
        public DateTime? PublishStartDate { get; set; }
        public DateTime? PublishEndDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? RegistDateTime { get; set; }

        /// <summary>
        /// 기간방식
        /// </summary>
        public string DateViewText
        {
            get
            {
                var result = string.Empty;
                if (PeriodMethodCode == "PMC01") //기간입력
                    result = $"{PublishStartDate:yyyy-MM-dd}~{PublishEndDate:yyyy-MM-dd}";
                else if (PeriodMethodCode == "PMC02") //발행일로부터 X일
                    result = $"{RegistDateTime:yyyy-MM-dd}~{ExpirationDate:yyyy-MM-dd}";
                else if (PeriodMethodCode == "PMC03") //무제한
                    result = $"사용기간 제한 없음";
                return result;
            }
        }

        public string Description { get; set; }
    }
    #endregion
}
