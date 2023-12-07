using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 데이터 베이스 초대장 데이터 모델
    /// </summary>
    public class M2DBDataModel
    {
        public TB_Invitation_Detail InvitationDetail { get; set; }
        public TB_Invitation Invitation { get; set; }
        public TB_Order Order { get; set; }
        public TB_Product Product { get; set; }
        public TB_Template Template { get; set; }
    }
    /// <summary>
    /// 모바일 초대장 뷰 모델
    /// </summary>
    public class M2ViewModel
    {
        /// <summary>
        /// 표시 여부
        /// false일경우 하위 모델의 값은 채워지면 안되고 무시 되어야 함.
        /// </summary>
        public bool IsDisplay { get; set; } = false;


        #region 기본정보
        /// <summary>
        /// 초대장 번호
        /// </summary>
        public int InvitaionId { get; set; }

        /// <summary>
        /// 초대장 카테고리 코드
        /// </summary>
        public string ProductCategoryCode { get; set; }


        /// <summary>
        /// 화원여부
        /// </summary>
        public bool IsUser { get; set; }
        #endregion

        #region 타이틀 정보
        /// <summary>
        /// 초대장 Full URL
        /// </summary>
        public Uri FullUrl { get; set; }

        /// <summary>
        /// 초대장 제목
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 초대장 이벤트 날짜 및 시간
        /// </summary>
        public string EventDateTime { get; set; }
        public string EventDate { get; set; }
        /// <summary>
        /// 초대장 설명
        /// </summary>
        public Dictionary<string, string> Descriptions => new Dictionary<string, string>
        {
            { "PCC01", "바른손카드 모바일청첩장" },
            { "PCC02", "바른손카드 모바일감사장" },
            { "PCC03", "바른손카드 돌잔치초대장" }
        };


        /// <summary>
        /// SNS 공유 이미지 정보
        /// </summary>
        public M2ImageInfoViewModel SNSImageInfo { get; set; }

        /// <summary>
        /// 갤러리 사진 확대 방지 기능 여부
        /// </summary>
        public bool GalleryPreventPhotoYN { get; set; } = false;

        /// <summary>
        /// 사용자 정의 스타일 및 JS
        /// </summary>
        public Uri CustomCssUrl { get; set; } = null;
        public Uri CustomJsUrl { get; set; } = null;
        #endregion

        public PCC01ViewModel PCC01DataModel { get; set; }
        public PCC02ViewModel PCC02DataModel { get; set; }
        public PCC03ViewModel PCC03DataModel { get; set; }

    }

    /// <summary>
    /// 이미지 정보
    /// </summary>
    public class M2ImageInfoViewModel
    {
        /// <summary>
        /// Full URL
        /// </summary>
        public Uri ImageUrl { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public Uri SmallImageUrl { get; set; }
    }

    /// <summary>
    /// 초대장 영역 모델
    /// </summary>
    public class M2AreaViewModel
    {
        /// <summary>
        /// 영역 아이디
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>
        /// 영역 높이
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 영역 널비
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 영역 배경색
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 영역 아이템 목록
        /// </summary>
        public List<M2AreaItemViewModel> Items { get; set; }

        public M2AreaViewModel Copy()
        {
            var copy = new M2AreaViewModel();
            copy.AreaID = AreaID;
            copy.Height = Height;
            copy.Width = Width;
            copy.Color = Color;
            copy.Items = Items.Select(m => (M2AreaItemViewModel)m.Clone()).ToList();
            return copy;

        }
    }

    /// <summary>
    /// 초대장 영역 아이템 모델
    /// </summary>
    public class M2AreaItemViewModel: ICloneable
    {
        public int Sort { get; set; }
        /// <summary>
        /// Item ID
        /// </summary>
        public int itemId { get; set; }
        /// <summary>
        /// Resource ID
        /// </summary>
        public int ResourceId { get; set; }
        /// <summary>
        /// 아이템 타입
        /// CodeNameHelper.ItemTypeCodeToResourceTypeCode 참조 
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Location Top
        /// </summary>
        public double? Top { get; set; }
        /// <summary>
        /// Location Left
        /// </summary>
        public double? Left { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public double? Height { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// html tag: value
        /// 표시 데이터, #인사말# 등을 변환하여 실제 표시할 데이터 값
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// css: font-family
        /// </summary>
        public string Font { get; set; }
        /// <summary>
        /// 글자 크기, 문자일경우만
        /// css: font-size
        /// </summary>
        public double? FontSize { get; set; }
        /// <summary>
        /// 글자색
        /// css: color
        /// </summary>
        public string FontColor { get; set; }
        /// <summary>
        /// 배경색
        /// css: background-color
        /// </summary>
        public string BackgroundColor { get; set; }
        /// <summary>
        /// css: font-weight: bold
        /// </summary>
        public bool IsBold { get; set; }
        /// <summary>
        /// css: font-style: italic
        /// </summary>
        public bool IsItalic { get; set; }
        /// <summary>
        /// css: text-decoration: underline
        /// </summary>
        public bool IsUnderline { get; set; }
        /// <summary>
        /// css: letter-spacing: db 값 between_text / 100 + "em"
        /// </summary>
        public double? LetterSpacing { get; set; }
        /// <summary>
        /// css: line-height: db 값 between_line + "em"
        /// </summary>
        public double? LineHeight { get; set; }
        /// <summary>
        /// CSS: align-items
        /// T: flex-start, M: center, B:flex-end
        /// </summary>
        public string VerticalAlign { get; set; }
        /// <summary>
        /// CSS: text-align
        /// C: center, R: right, L: left
        /// </summary>
        public string HorizontalAlign { get; set; }
        /// <summary>
        /// Element z-index, 사용되지 않음.
        /// </summary>
        public int? ZIndex { get; set; }
        
        /// <summary>
        /// 상대 URL
        /// </summary>
        public string ResourceUrl { get; set; }
        /// <summary>
        /// 절대 URL
        /// </summary>
        public Uri? ResourceAbsoluteUrl { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    #region Area 구분된 데이터 모델
    
    /// <summary>
    /// 초대장 데이터 공통 모델
    /// </summary>
    public class M2DataModel
    {
        #region 표지 디자인
        public M2AreaViewModel CoverArea { get; set; } = null;
        #endregion

        #region 초대글(인사말)
        public M2AreaViewModel GreetingsArea { get; set; } = null;
        #endregion

        #region 갤러리
        public M2GalleryViewModel GalleryArea { get; set; } = null;
        #endregion

        #region 동영상
        public M2VideoViewModel VideoArea { get; set; } = null;
        #endregion

        #region 오시는길
        public M2LocationVIewModel LocationArea { get; set; } = null;
        #endregion

        #region 송금
        public M2AccountViewModel AccountArea { get; set; } = null;
        #endregion

        #region 방명록
        public M2GuestBookViewModel GuestBookArea { get; set; } = null;
        #endregion
    }

    /// <summary>
    /// 청첩장 모델
    /// </summary>
    public class PCC01ViewModel : M2DataModel
    {

        #region 신랑 & 신부 연락 처
        public M2ContectViewModel ContectArea { get; set; } = null;
        #endregion

        #region 기타 광고 등
        /// <summary>
        /// 화환 선물
        /// </summary>
        public Uri FlowerGiftUri { get; set; } = null;

        /// <summary>
        /// 기타정보
        /// </summary>
        public List<TB_Invitation_Detail_Etc> ETCs { get; set; }
        #endregion
    }

    /// <summary>
    /// 감사장 모델
    /// </summary>
    public class PCC02ViewModel: M2DataModel
    {
        #region 신랑 & 신부 연락 처
        public M2ContectViewModel ContectArea { get; set; } = null;
        #endregion
    }

    /// <summary>
    /// 돌잔치 모델
    /// </summary>
    public class PCC03ViewModel : M2DataModel
    {
        #region 부모님 연락 처
        public M2ContectViewModel ContectArea { get; set; } = null;
        #endregion

        #region 아기정보
        public List<M2BabyInfoModel> BabyInfosArea { get; set; } = null;
        #endregion

        #region 기타 광고 등
        /// <summary>
        /// 기타정보
        /// </summary>
        public List<TB_Invitation_Detail_Etc> ETCs { get; set; }
        #endregion
    }
    /// <summary>
    /// 아기정보 본문 모델
    /// </summary>
    public class M2BabyInfoModel
    {
        public int idx { get; set; }
        /// <summary>
        /// 상단 타이틀 : 17
        /// </summary>
        public M2AreaViewModel TitleArea { get; set; } = null;
        /// <summary>
        /// 본문: 18
        /// </summary>
        public M2AreaViewModel BodyArea { get; set; } = null;
        /// <summary>
        /// 아기 정보: 19
        /// </summary>
        public List<M2AreaViewModel> infoArea { get; set; } = null;
        /// <summary>
        /// 하단: 20
        /// </summary>
        public M2AreaViewModel FooterArea { get; set; } = null;
    }
    #endregion

    #region Gallery, Video Model

    /// <summary>
    /// 갤러리 영역 모델
    /// </summary>
    public class M2GalleryViewModel
    {
        public string GalleryTypeCode { get; set; } = null;
        public M2AreaViewModel GalleryTitleArea { get; set; } = null;
        public Dictionary<int, M2ImageInfoViewModel> GalleryItems { get; set; } = null;
    }

    /// <summary>
    /// 동영상 영역 모델
    /// </summary>
    public class M2VideoViewModel
    {
        public string VideoTypeCode { get; set; } = null;
        public M2AreaViewModel VideoTitleArea { get; set; } = null;
        public string VideoUri { get; set; }
    }

    #endregion

    #region Contect Data Model

    /// <summary>
    /// 연락처 표시 모델
    /// </summary>
    public class M2ContectViewModel
    {
        /// <summary>
        /// 연락처 표시 여부
        /// </summary>
        public bool IsShow
        {
            get
            {
                return MainContects.Count > 0 || Extra1Contects.Count > 0 || Extra2Contects.Count > 0;
            }
        }

        /// <summary>
        /// 확장 연락처 표시 여부
        /// </summary>
        public bool IsExtraShow
        {
            get
            {
                return Extra1Contects.Count > 0 || Extra2Contects.Count > 0;
            }
        }
        /// <summary>
        /// 메인 연락처: 타이틀, 이름, 전화번호
        /// </summary>
        public List<M2ContectViewItemModel> MainContects { get; set; } = new List<M2ContectViewItemModel>();

        /// <summary>
        /// 확장 연락처: 신랑측
        /// </summary>
        public List<M2ContectViewItemModel> Extra1Contects { get; set; } = new List<M2ContectViewItemModel>();
        /// <summary>
        /// 확장 연락처: 신부측
        /// </summary>
        public List<M2ContectViewItemModel> Extra2Contects { get; set; } = new List<M2ContectViewItemModel>();

    }
    /// <summary>
    ///  연락처정보: 타이틀, 이름, 전화번호
    /// </summary>
    public class M2ContectViewItemModel
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string TelNo { get; set; }
    }
    #endregion

    #region 오시는길 영역 모델
    /// <summary>
    /// 오시는길
    /// </summary>
    public class M2LocationVIewModel
    {
        /// <summary>
        /// OTC01: 네이버 맵
        /// 아니면 이미지
        /// </summary>
        public string OutlineTypeCode { get; set; } = null;
        public M2AreaViewModel LocationTitleArea { get; set; } = null;

        public string Name { get; set; }
        public string DetailName { get; set; }
        public bool IsDetailNewLine { get; set; }   
        public string Address { get; set; }
        public string TelNo { get; set; }

        public double? Lat { get; set; }
        public double? Lot { get; set; }

        /// <summary>
        /// 지도 경로
        /// </summary>
        public Uri LocationUrl { get; set; } = null;

    }
    /// <summary>
    /// NaverMap View Model
    /// </summary>
    public class NaverMapViewModel
    {
        public string ApiId { set; get; }
        public string ApiKey { set; get; }

        public double Lat { get; set; }
        public double Lot { get; set; }
        public string Name { get; set; }

    }
    #endregion

    #region RemmitAccount Model
    /// <summary>
    /// 계좌번호 모델
    /// </summary>
    public class M2AccountViewModel
    {
        /// <summary>
        /// 계좌번호 타이틀
        /// </summary>
        public M2AreaViewModel AccountTitleArea { get; set; } = null;

        /// <summary>
        /// 카카오 송금 사용여부
        /// </summary>
        public bool UseGiftRemit { get; set; } = false;
        public Uri GiftRemitUrl { get; set; }

        /// <summary>
        /// 기타 송금 사용여부, Category=-1
        /// </summary>
        public bool UseAccountRemit { get; set; } = false;
        /// <summary>
        /// 신랑,신부 송금 , Category=1,2
        /// </summary>
        public bool UseAccountDiv { get; set; } = false;
        public List<M2AccountListModel> Accounts { get; set; }

        
    }
    /// <summary>
    /// 계좌번호 목록
    /// </summary>
    public class M2AccountListModel
    {
        public int Category { get; set; }
        public int Sort { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
    }

    #endregion

    #region Guest book Model
    /// <summary>
    /// 방명록 영역 모델
    /// </summary>
    public class M2GuestBookViewModel
    {
        /// <summary>
        /// 초대장 번호
        /// </summary>
        public int InvitaionId { get; set; }
        /// <summary>
        /// 방명록 타이틀
        /// </summary>
        public M2AreaViewModel GuestBookTitleArea { get; set; } = null;

    }
    /// <summary>
    /// 방명록 내용 모델
    /// </summary>
    public class M2GuestBookListModel
    {
        /// <summary>
        /// 방명록 ID
        /// </summary>
        public int GuestBookID { get; set; }
        public string Name { get; set; }
        public string Message { get; set;}
        public DateTime RegistDatetime { get; set; }
    }
    #endregion
}
