using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class barunsonContext : DbContext
    {
        public barunsonContext()
        {
        }

        public barunsonContext(DbContextOptions<barunsonContext> options)
            : base(options)
        {
        }

        #region 프로퍼티
        public virtual DbSet<Origin_Invitation_Detail> Origin_Invitation_Details { get; set; }
        public virtual DbSet<SMS_Log> SMS_Logs { get; set; }
        public virtual DbSet<TB_Account> TB_Accounts { get; set; }
        public virtual DbSet<TB_Account_Extra> TB_Account_Extras { get; set; }
        public virtual DbSet<TB_Account_Setting> TB_Account_Settings { get; set; }
        public virtual DbSet<TB_Admin_Memo> TB_Admin_Memos { get; set; }
        public virtual DbSet<TB_Apply_Product> TB_Apply_Products { get; set; }
        public virtual DbSet<TB_Area> TB_Areas { get; set; }
        public virtual DbSet<TB_Bank> TB_Banks { get; set; }
        public virtual DbSet<TB_Banner> TB_Banners { get; set; }
        public virtual DbSet<TB_Banner_Category> TB_Banner_Categories { get; set; }
        public virtual DbSet<TB_Banner_Item> TB_Banner_Items { get; set; }
        public virtual DbSet<TB_Board> TB_Boards { get; set; }
        public virtual DbSet<TB_CASEA_COUPON_PUBLISHED> TB_CASEA_COUPON_PUBLISHEDs { get; set; }
        public virtual DbSet<TB_CASEB_COUPON_PUBLISHED> TB_CASEB_COUPON_PUBLISHEDs { get; set; }
        public virtual DbSet<TB_Calculate> TB_Calculates { get; set; }
        public virtual DbSet<TB_Category> TB_Categories { get; set; }
        public virtual DbSet<TB_Common_Code> TB_Common_Codes { get; set; }
        public virtual DbSet<TB_Common_Code_Group> TB_Common_Code_Groups { get; set; }
        public virtual DbSet<TB_Common_Menu> TB_Common_Menus { get; set; }
        public virtual DbSet<TB_Company_Tax> TB_Company_Taxes { get; set; }
        public virtual DbSet<TB_Coupon> TB_Coupons { get; set; }
        public virtual DbSet<TB_Coupon_Apply_Product> TB_Coupon_Apply_Products { get; set; }
        public virtual DbSet<TB_Coupon_Exception_Product> TB_Coupon_Exception_Products { get; set; }
        public virtual DbSet<TB_Coupon_Order> TB_Coupon_Orders { get; set; }
        public virtual DbSet<TB_Coupon_Product> TB_Coupon_Products { get; set; }
        public virtual DbSet<TB_Coupon_Product_Option> TB_Coupon_Product_Options { get; set; }
        public virtual DbSet<TB_Coupon_Publish> TB_Coupon_Publishes { get; set; }
        public virtual DbSet<TB_Coupon_Publish_TEST> TB_Coupon_Publish_TESTs { get; set; }
        public virtual DbSet<TB_Daily_Unique> TB_Daily_Uniques { get; set; }
        public virtual DbSet<TB_Depositor_Hit> TB_Depositor_Hits { get; set; }
        public virtual DbSet<TB_Error_Content> TB_Error_Contents { get; set; }
        public virtual DbSet<TB_Gallery> TB_Galleries { get; set; }
        public virtual DbSet<TB_GuestBook> TB_GuestBooks { get; set; }
        public virtual DbSet<TB_Icon> TB_Icons { get; set; }
        public virtual DbSet<TB_Invitation> TB_Invitations { get; set; }
        public virtual DbSet<TB_Invitation_Account> TB_Invitation_Accounts { get; set; }
        public virtual DbSet<TB_Invitation_Admin> TB_Invitation_Admins { get; set; }
        public virtual DbSet<TB_Invitation_Area> TB_Invitation_Areas { get; set; }
        public virtual DbSet<TB_Invitation_Detail> TB_Invitation_Details { get; set; }
        public virtual DbSet<TB_Invitation_Detail_Etc> TB_Invitation_Detail_Etcs { get; set; }
        public virtual DbSet<TB_Invitation_Item> TB_Invitation_Items { get; set; }
        public virtual DbSet<TB_Invitation_Tax> TB_Invitation_Taxes { get; set; }
        public virtual DbSet<TB_Item_Resource> TB_Item_Resources { get; set; }
        public virtual DbSet<TB_Order> TB_Orders { get; set; }
        public virtual DbSet<TB_Order_Copy> TB_Order_Copies { get; set; }
        public virtual DbSet<TB_Order_Coupon_Use> TB_Order_Coupon_Uses { get; set; }
        public virtual DbSet<TB_Order_PartnerShip> TB_Order_PartnerShip { get; set; } = null!;
        public virtual DbSet<TB_Order_Product> TB_Order_Products { get; set; }
        public virtual DbSet<TB_Order_Serial_Coupon_Use> TB_Order_Serial_Coupon_Uses { get; set; }
        public virtual DbSet<TB_Payment_Status_Day> TB_Payment_Status_Days { get; set; }
        public virtual DbSet<TB_Payment_Status_Month> TB_Payment_Status_Months { get; set; }
        public virtual DbSet<TB_Popup> TB_Popups { get; set; }
        public virtual DbSet<TB_Popup_Item> TB_Popup_Items { get; set; }
        public virtual DbSet<TB_Product> TB_Products { get; set; }
        public virtual DbSet<TB_Product_Category> TB_Product_Categories { get; set; }
        public virtual DbSet<TB_Product_Icon> TB_Product_Icons { get; set; }
        public virtual DbSet<TB_Product_Image> TB_Product_Images { get; set; }
        public virtual DbSet<TB_Refund_Info> TB_Refund_Infos { get; set; }
        public virtual DbSet<TB_Remit> TB_Remits { get; set; }
        public virtual DbSet<TB_Remit_Statistics_Daily> TB_Remit_Statistics_Dailies { get; set; }
        public virtual DbSet<TB_Remit_Statistics_Monthly> TB_Remit_Statistics_Monthlies { get; set; }
        public virtual DbSet<TB_ReservationWord> TB_ReservationWords { get; set; }
        public virtual DbSet<TB_SCHEDULER_COUPON_PUBLISHED> TB_SCHEDULER_COUPON_PUBLISHEDs { get; set; }
        public virtual DbSet<TB_Sales_Statistic_Day> TB_Sales_Statistic_Days { get; set; }
        public virtual DbSet<TB_Sales_Statistic_Month> TB_Sales_Statistic_Months { get; set; }
        public virtual DbSet<TB_Serial_Apply_Product> TB_Serial_Apply_Products { get; set; }
        public virtual DbSet<TB_Serial_Coupon> TB_Serial_Coupons { get; set; }
        public virtual DbSet<TB_Serial_Coupon_Apply_Product> TB_Serial_Coupon_Apply_Products { get; set; }
        public virtual DbSet<TB_Serial_Coupon_Publish> TB_Serial_Coupon_Publishes { get; set; }
        public virtual DbSet<TB_Standard_Date> TB_Standard_Dates { get; set; }
        public virtual DbSet<TB_Tax> TB_Taxes { get; set; }
        public virtual DbSet<TB_Temp_Order> TB_Temp_Orders { get; set; }
        public virtual DbSet<TB_Template> TB_Templates { get; set; }
        public virtual DbSet<TB_Template_Area> TB_Template_Areas { get; set; }
        public virtual DbSet<TB_Template_Detail> TB_Template_Details { get; set; }
        public virtual DbSet<TB_Template_Item> TB_Template_Items { get; set; }
        public virtual DbSet<TB_Total_Statistic_Day> TB_Total_Statistic_Days { get; set; }
        public virtual DbSet<TB_Total_Statistic_Month> TB_Total_Statistic_Months { get; set; }
        public virtual DbSet<TB_Wish_List> TB_Wish_Lists { get; set; }
        public virtual DbSet<VW_Admin> VW_Admins { get; set; }
        public virtual DbSet<VW_User> VW_Users { get; set; }
        public virtual DbSet<VW_User_QNA> VW_User_QNAs { get; set; }
        public virtual DbSet<TB_Kakao_Cache> TB_Kakao_Cache { get; set; }
        public virtual DbSet<TB_PolicyInfo> TB_PolicyInfos { get; set; }
        public virtual DbSet<TB_FlaBannerManage> TB_FlaBannerManage { get; set; }
        #endregion


        #region 내부 함수
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Korean_Wansung_CI_AS");

            modelBuilder.Entity<Origin_Invitation_Detail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Origin_Invitation_Detail");

                entity.Property(e => e.Origin_Invitation_URL).HasMaxLength(200);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<SMS_Log>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SMS_Log");

                entity.Property(e => e.CONTENT).HasColumnType("text");

                entity.Property(e => e.ORDER_CDOE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ORDER_HPHONE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.regdate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TB_Account>(entity =>
            {
                entity.HasKey(e => e.Account_ID);

                entity.ToTable("TB_Account");

                entity.HasComment("계좌_정보");

                entity.Property(e => e.Account_ID).HasComment("모바일초대장에 매핑할 키\r\n");

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("계좌_번호");

                entity.Property(e => e.Account_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("신랑\r\n신부\r\n신랑혼주\r\n신부혼주\r\n");

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("금융관리원에서 표준으로 잡는 은행 코드\r\n");

                entity.Property(e => e.Depositor_Name)
                    .HasMaxLength(100)
                    .HasComment("예금주_명");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Accounts)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TB_Account");
            });

            modelBuilder.Entity<TB_Account_Extra>(entity =>
            {
                entity.HasKey(e => new { e.Invitation_ID, e.Sort });

                entity.ToTable("TB_Account_Extra");

                entity.Property(e => e.Account_Holder)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Catetory).HasDefaultValueSql("((1))");

                entity.Property(e => e.Send_Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Send_Target_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Account_Extras)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TB_Account_Extra");
            });

            modelBuilder.Entity<TB_Account_Setting>(entity =>
            {
                entity.HasKey(e => e.Account_Setting_ID);

                entity.ToTable("TB_Account_Setting");

                entity.HasComment("계좌_설정");

                entity.Property(e => e.Account_Setting_ID).HasComment("계좌_설정_ID");

                entity.Property(e => e.Barunn_Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("바른_계좌_번호");

                entity.Property(e => e.Barunn_Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("바른_은행_코드");

                entity.Property(e => e.Kakao_Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("카카오_계좌_번호");

                entity.Property(e => e.Kakao_Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("카카오_은행_코드");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");
            });

            modelBuilder.Entity<TB_Admin_Memo>(entity =>
            {
                entity.HasKey(e => e.Memo_ID);

                entity.ToTable("TB_Admin_Memo");

                entity.HasComment("관리자_메모");

                entity.Property(e => e.Memo_ID).HasComment("메모_ID");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("내용");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");
            });

            modelBuilder.Entity<TB_Apply_Product>(entity =>
            {
                entity.HasKey(e => new { e.Product_Apply_ID, e.Product_Code });

                entity.ToTable("TB_Apply_Product");

                entity.HasComment("쿠폰_적용_상품_리스트");

                entity.Property(e => e.Product_Apply_ID).HasComment("상품_적용_ID");

                entity.Property(e => e.Product_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_코드");

                entity.HasOne(d => d.Product_Apply)
                    .WithMany()
                    .HasForeignKey(d => d.Product_Apply_ID)
                    .HasConstraintName("FK_TB_Coupon_Apply_Product_TO_TB_Apply_Product");
            });

            modelBuilder.Entity<TB_Area>(entity =>
            {
                entity.HasKey(e => e.Area_ID);

                entity.ToTable("TB_Area");

                entity.HasComment("영억");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Area_Name)
                    .HasMaxLength(100)
                    .HasComment("영역_명");

                entity.Property(e => e.Edit_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("편집_여부");

                entity.Property(e => e.ThanksCard_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("감사장_여부");

                entity.Property(e => e.WeddingCard_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("청첩장_여부");
            });

            modelBuilder.Entity<TB_Bank>(entity =>
            {
                entity.HasKey(e => e.Bank_Code);

                entity.ToTable("TB_Bank");

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bank_Name).HasMaxLength(100);

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<TB_Banner>(entity =>
            {
                entity.HasKey(e => e.Banner_ID);

                entity.ToTable("TB_Banner");

                entity.HasComment("배너");

                entity.Property(e => e.Banner_ID).HasComment("배너_ID");

                entity.Property(e => e.Banner_Category_ID).HasComment("배너_분류_ID");

                entity.Property(e => e.Banner_Mobile_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("배너_모바일_여부");

                entity.Property(e => e.Banner_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("배너_명");

                entity.Property(e => e.Banner_PC_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("배너_PC_여부");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Banner_Category)
                    .WithMany(p => p.TB_Banners)
                    .HasForeignKey(d => d.Banner_Category_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Banner_Category_TO_TB_Banner");
            });

            modelBuilder.Entity<TB_Banner_Category>(entity =>
            {
                entity.HasKey(e => e.Banner_Category_ID);

                entity.ToTable("TB_Banner_Category");

                entity.HasComment("배너_분류");

                entity.Property(e => e.Banner_Category_ID).HasComment("배너_분류_ID");

                entity.Property(e => e.Banner_Category_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("배너_분류_명");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");
            });

            modelBuilder.Entity<TB_Banner_Item>(entity =>
            {
                entity.HasKey(e => e.Banner_Item_ID);

                entity.ToTable("TB_Banner_Item");

                entity.HasComment("배너_아이템");

                entity.Property(e => e.Banner_Item_ID).HasComment("배너_아이템_ID");

                entity.Property(e => e.Banner_Add_Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Banner_ID).HasComment("배너_ID");

                entity.Property(e => e.Banner_Main_Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Banner_Type_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("PC\r\n모바");

                entity.Property(e => e.Click_Count).HasComment("클릭_수");

                entity.Property(e => e.Deadline_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("날짜지정\r\n무제");

                entity.Property(e => e.End_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("종료_날짜");

                entity.Property(e => e.End_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("종료_시간");

                entity.Property(e => e.Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("이미지_URL");

                entity.Property(e => e.Image_URL2)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Link_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("링크_URL");

                entity.Property(e => e.NewPage_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("새창_여부");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Start_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("시작_날짜");

                entity.Property(e => e.Start_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("시작_시간");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Banner)
                    .WithMany(p => p.TB_Banner_Items)
                    .HasForeignKey(d => d.Banner_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Banner_TO_TB_Banner_Item");
            });

            modelBuilder.Entity<TB_Board>(entity =>
            {
                entity.HasKey(e => e.Board_ID);

                entity.ToTable("TB_Board");

                entity.HasComment("게시판");

                entity.Property(e => e.Board_ID).HasComment("게시판_ID");

                entity.Property(e => e.Board_Category)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("N - 공지사항\r\n\r\nF - FAQ");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("내용");

                entity.Property(e => e.Display_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("노출_여부");

                entity.Property(e => e.Hits).HasComment("조회수");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("제목");

                entity.Property(e => e.Top_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("상단고정_여부");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");
            });

            modelBuilder.Entity<TB_CASEA_COUPON_PUBLISHED>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_CASEA_COUPON_PUBLISHED");

                entity.Property(e => e.Member_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<TB_CASEB_COUPON_PUBLISHED>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_CASEB_COUPON_PUBLISHED");

                entity.Property(e => e.Member_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<TB_Calculate>(entity =>
            {
                entity.HasKey(e => e.Calculate_ID);

                entity.ToTable("TB_Calculate");

                entity.HasComment("정산");

                entity.Property(e => e.Calculate_ID).HasComment("정산_ID");

                entity.Property(e => e.Calculate_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("정산_일시");

                entity.Property(e => e.Calculate_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Error_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("오류_코드");

                entity.Property(e => e.Error_Message)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Remit_Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("송금_계좌_번호");

                entity.Property(e => e.Remit_Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("송금_은행_코드");

                entity.Property(e => e.Remit_Content)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("송금_내용");

                entity.Property(e => e.Remit_ID).HasComment("송금_ID");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Request_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("요청_일자");

                entity.Property(e => e.Request_DateTime)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasComment("요청_일시");

                entity.Property(e => e.Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상태_코드");

                entity.Property(e => e.Trading_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("거래_번호");

                entity.Property(e => e.Unique_Number).HasComment("고유_번호");

                entity.HasOne(d => d.Remit)
                    .WithMany(p => p.TB_Calculates)
                    .HasForeignKey(d => d.Remit_ID)
                    .HasConstraintName("FK_TB_Remit_TO_TB_Calculate");
            });

            modelBuilder.Entity<TB_Category>(entity =>
            {
                entity.HasKey(e => e.Category_ID);

                entity.ToTable("TB_Category");

                entity.HasComment("분류");

                entity.Property(e => e.Category_ID).HasComment("분류_ID");

                entity.Property(e => e.Category_Name)
                    .HasMaxLength(100)
                    .HasComment("분류_명");

                entity.Property(e => e.Category_Name_Mobile)
                    .HasMaxLength(100)
                    .HasComment("분류_명_모바일");

                entity.Property(e => e.Category_Name_Mobile_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("분류_명_모바일_URL");

                entity.Property(e => e.Category_Name_PC)
                    .HasMaxLength(100)
                    .HasComment("분류_명_PC");

                entity.Property(e => e.Category_Name_PC_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("분류_명_PC_URL");

                entity.Property(e => e.Category_Name_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("분류_명_구분_코드");

                entity.Property(e => e.Category_Step).HasComment("분류_단계");

                entity.Property(e => e.Category_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("분류_구분_코드");

                entity.Property(e => e.Display_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("진열_여부");

                entity.Property(e => e.Icon_ID).HasComment("아이콘_ID");

                entity.Property(e => e.Parent_Category_ID).HasComment("상위_분류_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Common_Code>(entity =>
            {
                entity.HasKey(e => new { e.Code_Group, e.Code });

                entity.ToTable("TB_Common_Code");

                entity.HasComment("공통_코드");

                entity.Property(e => e.Code_Group)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("코드_그룹");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("코드");

                entity.Property(e => e.Code_Name)
                    .HasMaxLength(100)
                    .HasComment("코드_명");

                entity.Property(e => e.Extra_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Code_GroupNavigation)
                    .WithMany(p => p.TB_Common_Codes)
                    .HasForeignKey(d => d.Code_Group)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Common_Code_Group_TO_TB_Common_Code");
            });

            modelBuilder.Entity<TB_Common_Code_Group>(entity =>
            {
                entity.HasKey(e => e.Code_Group);

                entity.ToTable("TB_Common_Code_Group");

                entity.HasComment("공통_코드_그룹");

                entity.Property(e => e.Code_Group)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("코드_그룹");

                entity.Property(e => e.Group_Name)
                    .HasMaxLength(100)
                    .HasComment("그룹_명");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Common_Menu>(entity =>
            {
                entity.HasKey(e => e.Menu_ID)
                    .IsClustered(false);

                entity.ToTable("TB_Common_Menu");

                entity.HasComment("공통_메뉴");

                entity.Property(e => e.Menu_ID).HasComment("메뉴_ID");

                entity.Property(e => e.Display_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("진열_여부");

                entity.Property(e => e.Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Menu_Name)
                    .HasMaxLength(100)
                    .HasComment("메뉴_명");

                entity.Property(e => e.Menu_Step).HasComment("메뉴_단계");

                entity.Property(e => e.Menu_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("메뉴_구분_코드");

                entity.Property(e => e.Menu_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("메뉴_URL");

                entity.Property(e => e.Parent_Menu_ID).HasComment("상위_메뉴_아이디");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Parent_Menu)
                    .WithMany(p => p.InverseParent_Menu)
                    .HasForeignKey(d => d.Parent_Menu_ID)
                    .HasConstraintName("FK_TB_Common_Menu_TO_TB_Common_Menu");
            });

            modelBuilder.Entity<TB_Company_Tax>(entity =>
            {
                entity.HasKey(e => e.Company_Tax_ID);

                entity.ToTable("TB_Company_Tax");

                entity.HasComment("업체_수수료");

                entity.Property(e => e.Company_Tax_ID).HasComment("업체_수수료_ID");

                entity.Property(e => e.Apply_Start_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("적용_시작_날짜");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Remit_Tax).HasComment("수수료_비율");
            });

            modelBuilder.Entity<TB_Coupon>(entity =>
            {
                entity.HasKey(e => e.Coupon_ID);

                entity.ToTable("TB_Coupon");

                entity.HasComment("쿠폰");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Coupon_Apply_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("쿠폰_적용_코드");

                entity.Property(e => e.Coupon_Apply_Product_ID).HasComment("쿠폰_적용_상품_ID");

                entity.Property(e => e.Coupon_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("쿠폰_이미지_URL");

                entity.Property(e => e.Coupon_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("쿠폰_명");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("설명");

                entity.Property(e => e.Discount_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("할인_방식_코드");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

                entity.Property(e => e.Discount_Rate).HasComment("할인_율");

                entity.Property(e => e.Period_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("기간_방식_코드");

                entity.Property(e => e.Publish_End_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("발행_종료_일자");

                entity.Property(e => e.Publish_Method_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("발급_방식_코드");

                entity.Property(e => e.Publish_Period_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("발행_기간_코드");

                entity.Property(e => e.Publish_Start_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("발행_시작_일자");

                entity.Property(e => e.Publish_Target_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("발급_대상_코드");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Standard_Purchase_Price).HasComment("기준_구매_금액");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Use_Available_Standard_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용_가능_기준_코드");

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("사용_여부");
            });

            modelBuilder.Entity<TB_Coupon_Apply_Product>(entity =>
            {
                entity.HasKey(e => e.Product_Apply_ID)
                    .IsClustered(false);

                entity.ToTable("TB_Coupon_Apply_Product");

                entity.HasComment("쿠폰_적용_상품군");

                entity.Property(e => e.Product_Apply_ID).HasComment("상품_적용_ID");
            });

            modelBuilder.Entity<TB_Coupon_Exception_Product>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_Coupon_Exception_Product");

                entity.Property(e => e.Product_Code)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<TB_Coupon_Order>(entity =>
            {
                entity.HasKey(e => e.Coupon_Order_ID);

                entity.ToTable("TB_Coupon_Order");

                entity.HasComment("쿠폰_주문");

                entity.Property(e => e.Coupon_Order_ID)
                    .ValueGeneratedNever()
                    .HasComment("쿠폰_주문_ID");

                entity.Property(e => e.Callback_PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("콜백_전화번호");

                entity.Property(e => e.Coupon_Expiration_DateTime)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasComment("쿠폰_만료_일시");

                entity.Property(e => e.Coupon_OrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("쿠폰_주문번호");

                entity.Property(e => e.Coupon_Product_ID).HasComment("쿠폰_상품_ID");

                entity.Property(e => e.Futures_Trading_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("선물_거래_번호");

                entity.Property(e => e.Option_Code)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("옵션_코드");

                entity.Property(e => e.PIN_Number)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasComment("핀_번호");

                entity.Property(e => e.PIN_Option_Information)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasComment("핀_부가_정보");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComment("전화번호");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Request_Count)
                    .HasDefaultValueSql("((1))")
                    .HasComment("요청_수");

                entity.Property(e => e.Result_Code)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("결과_코드");

                entity.Property(e => e.Result_Content)
                    .HasMaxLength(100)
                    .HasComment("결과_내용");

                entity.Property(e => e.Result_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("결과_일시");

                entity.Property(e => e.StampOffice)
                    .HasMaxLength(20)
                    .HasComment("인지국");

                entity.Property(e => e.Stamp_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("인지세_코드");

                entity.Property(e => e.Stamp_Type)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("인지세_구분");

                entity.HasOne(d => d.Coupon_Product)
                    .WithMany(p => p.TB_Coupon_Orders)
                    .HasForeignKey(d => d.Coupon_Product_ID)
                    .HasConstraintName("FK_TB_Coupon_Product_TO_TB_Coupon_Order");
            });

            modelBuilder.Entity<TB_Coupon_Product>(entity =>
            {
                entity.HasKey(e => e.Coupon_Product_ID);

                entity.ToTable("TB_Coupon_Product");

                entity.HasComment("쿠폰_상품");

                entity.Property(e => e.Coupon_Product_ID).HasComment("쿠폰_상품_ID");

                entity.Property(e => e.Affiliate)
                    .HasMaxLength(100)
                    .HasComment("교환처");

                entity.Property(e => e.Affiliate_Category)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("교환처_분류");

                entity.Property(e => e.Delegate_Affiliate_Code)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("대표_교환처_코드");

                entity.Property(e => e.Destination_URL)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("배송지_URL");

                entity.Property(e => e.Image_Path)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("이미지_경로");

                entity.Property(e => e.Product_Category)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_분류");

                entity.Property(e => e.Product_Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("상품_설명");

                entity.Property(e => e.Product_ID)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("상품_ID");

                entity.Property(e => e.Product_Name)
                    .HasMaxLength(100)
                    .HasComment("상품_명");

                entity.Property(e => e.Product_Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_구분");

                entity.Property(e => e.Retail_Price).HasComment("소비자_가격");

                entity.Property(e => e.Retail_Price_Tax).HasComment("소비자_가격_부가세");

                entity.Property(e => e.Sale_End_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("판매_종료_날짜");

                entity.Property(e => e.Sale_Price).HasComment("판매_가격");

                entity.Property(e => e.Sale_Price_Tax).HasComment("판매_가격_부가세");

                entity.Property(e => e.Total_Price).HasComment("전체_가격");

                entity.Property(e => e.Valid_Period)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("유효_기간");
            });

            modelBuilder.Entity<TB_Coupon_Product_Option>(entity =>
            {
                entity.HasKey(e => e.Coupon_Product_Option_ID);

                entity.ToTable("TB_Coupon_Product_Option");

                entity.HasComment("쿠폰_상품_옵션");

                entity.Property(e => e.Coupon_Product_Option_ID).HasComment("쿠폰_상품_옵션_ID");

                entity.Property(e => e.Coupon_Product_ID).HasComment("쿠폰_상품_ID");

                entity.Property(e => e.Option_Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("옵션_명");

                entity.Property(e => e.Option_Value)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("옵션_값");

                entity.HasOne(d => d.Coupon_Product)
                    .WithMany(p => p.TB_Coupon_Product_Options)
                    .HasForeignKey(d => d.Coupon_Product_ID)
                    .HasConstraintName("FK_TB_Coupon_Product_TO_TB_Coupon_Product_Option");
            });

            modelBuilder.Entity<TB_Coupon_Publish>(entity =>
            {
                entity.HasKey(e => e.Coupon_Publish_ID);

                entity.ToTable("TB_Coupon_Publish");

                entity.HasComment("쿠폰_발행");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Expiration_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("만료_일자");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Retrieve_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("회수_일시");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Use_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("사용_일시");

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("사용_여부");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.TB_Coupon_Publishes)
                    .HasForeignKey(d => d.Coupon_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Coupon_TO_TB_Coupon_Publish");
            });

            modelBuilder.Entity<TB_Coupon_Publish_TEST>(entity =>
            {
                entity.HasKey(e => e.Coupon_Publish_ID);

                entity.ToTable("TB_Coupon_Publish_TEST");

                entity.HasComment("쿠폰_발행");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Expiration_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("만료_일자");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Retrieve_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("회수_일시");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Use_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("사용_일시");

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("사용_여부");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.TB_Coupon_Publish_TESTs)
                    .HasForeignKey(d => d.Coupon_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Coupon_TO_TB_Coupon_Publish_TEST");
            });

            modelBuilder.Entity<TB_Daily_Unique>(entity =>
            {
                entity.HasKey(e => new { e.Request_Date, e.Unique_Number });

                entity.ToTable("TB_Daily_Unique");

                entity.HasComment("날짜_고유_번호");

                entity.Property(e => e.Request_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("요청_일자");

                entity.Property(e => e.Unique_Number)
                    .HasDefaultValueSql("((1))")
                    .HasComment("고유_번호");
            });

            modelBuilder.Entity<TB_Depositor_Hit>(entity =>
            {
                entity.HasKey(e => e.Depositor_Hits_ID);

                entity.HasComment("예금주_조회");

                entity.HasIndex(e => e.Request_Date, "NonClusteredIndex_TB_Depositor_Hits_RegistDate");

                entity.Property(e => e.Depositor_Hits_ID).HasComment("예금주_조회_ID");

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("계좌_번호");

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("은행_코드");

                entity.Property(e => e.Depositor)
                    .HasMaxLength(50)
                    .HasComment("예금주");

                entity.Property(e => e.Error_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("오류_코드");

                entity.Property(e => e.Error_Message)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("오류_메세지");

                entity.Property(e => e.Hits_Depositor).HasMaxLength(50);

                entity.Property(e => e.Request_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("요청_일자");

                entity.Property(e => e.Request_DateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("요청_일시");

                entity.Property(e => e.Request_Result_DateTime)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasComment("요청_결과_일시");

                entity.Property(e => e.Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상태_코드");

                entity.Property(e => e.Trading_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("거래_번호");

                entity.Property(e => e.Unique_Number).HasComment("고유_번호");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");
            });

            modelBuilder.Entity<TB_Error_Content>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_Error_Content");

                entity.Property(e => e.Error_Content).HasColumnType("text");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Method_Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");

                entity.Property(e => e.User_Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TB_Gallery>(entity =>
            {
                entity.HasKey(e => e.Gallery_ID);

                entity.ToTable("TB_Gallery");

                entity.HasComment("갤러리");

                entity.Property(e => e.Gallery_ID).HasComment("갤러리_ID");

                entity.Property(e => e.Image_Height).HasComment("이미지_높이");

                entity.Property(e => e.Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("이미지_URL");

                entity.Property(e => e.Image_Width).HasComment("이미지_너비");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Galleries)
                    .HasForeignKey(d => d.Invitation_ID)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_Gallery");
            });

            modelBuilder.Entity<TB_GuestBook>(entity =>
            {
                entity.HasKey(e => e.GuestBook_ID);

                entity.ToTable("TB_GuestBook");

                entity.HasComment("방명록");

                entity.Property(e => e.GuestBook_ID).HasComment("방명록_ID");

                entity.Property(e => e.Display_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("제목");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Message)
                    .HasMaxLength(1000)
                    .HasComment("내용");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasComment("이름");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("비밀번호");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_GuestBooks)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_GuestBook");
            });

            modelBuilder.Entity<TB_Icon>(entity =>
            {
                entity.HasKey(e => e.Icon_ID);

                entity.ToTable("TB_Icon");

                entity.HasComment("아이콘");

                entity.Property(e => e.Icon_ID).HasComment("아이콘_ID");

                entity.Property(e => e.Icon_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("아이콘_URL");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Invitation>(entity =>
            {
                entity.HasKey(e => e.Invitation_ID);

                entity.ToTable("TB_Invitation");

                entity.HasComment("초대장");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Invitation_Display_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TB_Invitations)
                    .HasForeignKey(d => d.Order_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Order_TO_TB_Invitation");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TB_Invitations)
                    .HasForeignKey(d => d.Template_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Template_TO_TB_Invitation");
            });

            modelBuilder.Entity<TB_Invitation_Account>(entity =>
            {
                entity.HasKey(e => new { e.Invitation_ID, e.Sort, e.Category });

                entity.ToTable("TB_Invitation_Account");

                entity.Property(e => e.Category).HasDefaultValueSql("((1))");

                entity.Property(e => e.Account_Holder)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Send_Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Send_Target_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Invitation_Accounts)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TB_Invitation_Account");
            });

            modelBuilder.Entity<TB_Invitation_Admin>(entity =>
            {
                entity.HasKey(e => e.seq)
                    .HasName("PK_CIDX_TB_Invitation_Admin__seq");

                entity.ToTable("TB_Invitation_Admin");

                entity.HasComment("관리자 아이디");

                entity.Property(e => e.JOB_NAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.access_flag).HasDefaultValueSql("((0))");

                entity.Property(e => e.admin_hphone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.admin_id)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.admin_mail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.admin_name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.admin_photo).HasMaxLength(500);

                entity.Property(e => e.admin_pwd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.is_errorMail)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.is_reviewMail)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.is_reviewSMS)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.reg_date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TB_Invitation_Area>(entity =>
            {
                entity.HasKey(e => new { e.Invitation_ID, e.Area_ID });

                entity.ToTable("TB_Invitation_Area");

                entity.HasComment("초대장_영역");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Size_Height).HasComment("사이즈_높이");

                entity.Property(e => e.Size_Width).HasComment("사이즈_너비");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.TB_Invitation_Areas)
                    .HasForeignKey(d => d.Area_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Area_TO_TB_Invitation_Area");

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Invitation_Areas)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_Invitation_Area");
            });

            modelBuilder.Entity<TB_Invitation_Detail>(entity =>
            {
                entity.HasKey(e => e.Invitation_ID);

                entity.ToTable("TB_Invitation_Detail");

                entity.HasComment("초대장_상세");

                entity.HasIndex(e => e.Invitation_URL, "NCIDX_Invitation_Detail_URL")
                    .IsUnique();

                entity.Property(e => e.Invitation_ID)
                    .ValueGeneratedNever()
                    .HasComment("초대장_ID");

                entity.Property(e => e.Bride_EngName)
                    .HasMaxLength(100)
                    .HasComment("신부_영문명");

                entity.Property(e => e.Bride_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_국제_전화_번호");

                entity.Property(e => e.Bride_Global_Phone_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신부_국제_전화_여부");

                entity.Property(e => e.Bride_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("신부_명");

                entity.Property(e => e.Bride_Parents1_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주1_국제_전화_번호");

                entity.Property(e => e.Bride_Parents1_Global_Phone_Number_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신부_혼주1_국제_전화_번호_여부");

                entity.Property(e => e.Bride_Parents1_Name)
                    .HasMaxLength(100)
                    .HasComment("신부_혼주1_명칭");

                entity.Property(e => e.Bride_Parents1_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주1_전화");

                entity.Property(e => e.Bride_Parents1_Title)
                    .HasMaxLength(50)
                    .HasComment("신부_혼주1_호칭");

                entity.Property(e => e.Bride_Parents2_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주2_국제_전화_번호");

                entity.Property(e => e.Bride_Parents2_Global_Phone_Number_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신부_혼주2_국제_전화_번호_여부");

                entity.Property(e => e.Bride_Parents2_Name)
                    .HasMaxLength(100)
                    .HasComment("신부_혼주2_명칭");

                entity.Property(e => e.Bride_Parents2_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주2_전화");

                entity.Property(e => e.Bride_Parents2_Title)
                    .HasMaxLength(50)
                    .HasComment("신부_혼주2_호칭");

                entity.Property(e => e.Bride_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_전화");

                entity.Property(e => e.Delegate_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("대표_이미지_URL");

                entity.Property(e => e.Etc_Information_Use_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("기타_정보_사용_여부");

                entity.Property(e => e.Gallery_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("갤러리_유형_코드");

                entity.Property(e => e.Gallery_Use_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("갤러리_사용_여부");

                entity.Property(e => e.Greetings)
                    .HasMaxLength(1000)
                    .HasComment("인사말");

                entity.Property(e => e.Groom_EngName)
                    .HasMaxLength(100)
                    .HasComment("신랑_영문명");

                entity.Property(e => e.Groom_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_국제_전화");

                entity.Property(e => e.Groom_Global_Phone_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신랑_국제_전화_여부");

                entity.Property(e => e.Groom_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("신랑_명");

                entity.Property(e => e.Groom_Parents1_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주1_국제_전화_번호");

                entity.Property(e => e.Groom_Parents1_Global_Phone_Number_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신랑_혼주1_국제_전화_번호_여부");

                entity.Property(e => e.Groom_Parents1_Name)
                    .HasMaxLength(100)
                    .HasComment("신랑_혼주1_명칭");

                entity.Property(e => e.Groom_Parents1_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주1_전화");

                entity.Property(e => e.Groom_Parents1_Title)
                    .HasMaxLength(50)
                    .HasComment("신랑_혼주1_호칭");

                entity.Property(e => e.Groom_Parents2_Global_Phone_Number)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주2_국제_전화_번호");

                entity.Property(e => e.Groom_Parents2_Global_Phone_Number_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("신랑_혼주2_국제_전화_번호_여부");

                entity.Property(e => e.Groom_Parents2_Name)
                    .HasMaxLength(100)
                    .HasComment("신랑_혼주2_명칭");

                entity.Property(e => e.Groom_Parents2_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주2_전화");

                entity.Property(e => e.Groom_Parents2_Title)
                    .HasMaxLength(50)
                    .HasComment("신랑_혼주2_호칭");

                entity.Property(e => e.Groom_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_전화");

                entity.Property(e => e.GuestBook_Use_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("방명록_사용_여부");

                entity.Property(e => e.Invitation_Display_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true);

                entity.Property(e => e.Invitation_Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("초대장_제목");

                entity.Property(e => e.Invitation_URL)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("초대장_URL");

                entity.Property(e => e.Invitation_Video_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("초대_영상_유형_코드");

                entity.Property(e => e.Invitation_Video_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("초대_영상_URL");

                entity.Property(e => e.Invitation_Video_Use_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("초대_영상_사용_여부");

                entity.Property(e => e.Location_LAT).HasComment("좌표_LAT");

                entity.Property(e => e.Location_LOT).HasComment("좌표_LOT");

                entity.Property(e => e.MMS_Send_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.MoneyAccount_Div_Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MoneyAccount_Remit_Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MoneyGift_Remit_Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("축의금_송금_사용_여부");

                entity.Property(e => e.Outline_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("약도_이미지_URL");

                entity.Property(e => e.Outline_Type_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("약도_구분_코드");

                entity.Property(e => e.Parents_Information_Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')")
                    .IsFixedLength(true)
                    .HasComment("혼주_정보_사용_여부");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.SNS_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("SNS_이미지_URL");

                entity.Property(e => e.Sender)
                    .HasMaxLength(100)
                    .HasComment("보내는이");

                entity.Property(e => e.Time_Type_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("시간_구분_코드");

                entity.Property(e => e.Time_Type_Eng_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("시간_구분_영문_여부");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.Property(e => e.WeddingDD)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("예식일");

                entity.Property(e => e.WeddingDate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("예식일자");

                entity.Property(e => e.WeddingHHmm)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("예식시분");

                entity.Property(e => e.WeddingHallDetail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("층홀실");

                entity.Property(e => e.WeddingHour)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.WeddingMM)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("예식월");

                entity.Property(e => e.WeddingMin)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasComment("예식분");

                entity.Property(e => e.WeddingWeek)
                    .HasMaxLength(100)
                    .HasComment("예식요일");

                entity.Property(e => e.WeddingWeek_Eng_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("예식요일_영어_여부");

                entity.Property(e => e.WeddingYY)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("예식년");

                entity.Property(e => e.Weddinghall_Address)
                    .HasMaxLength(500)
                    .HasComment("예식장주소");

                entity.Property(e => e.Weddinghall_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("예식장_명");

                entity.Property(e => e.Weddinghall_PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("예식장_연락처");

                entity.Property(e => e.Conf_KaKaoPay_YN)
                   .IsRequired()
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('N')")
                   .IsFixedLength(true)
                   .HasComment("카카오페이설정");

                entity.Property(e => e.Conf_Remit_YN)
                   .IsRequired()
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('N')")
                   .IsFixedLength(true)
                   .HasComment("일반송금설정");

                entity.Property(e => e.Flower_gift_YN)
                   .IsRequired()
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('N')")
                   .IsFixedLength(true);

                entity.HasOne(d => d.Invitation)
                    .WithOne(p => p.TB_Invitation_Detail)
                    .HasForeignKey<TB_Invitation_Detail>(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_Invitation_Detail");

                entity.Property(e => e.GalleryPreventPhoto_YN)
                   .IsRequired()
                   .HasMaxLength(1)
                   .IsUnicode(false)
                   .HasDefaultValueSql("('N')")
                   .IsFixedLength(true);
            });

            modelBuilder.Entity<TB_Invitation_Detail_Etc>(entity =>
            {
                entity.HasKey(e => new { e.Invitation_ID, e.Sort });

                entity.ToTable("TB_Invitation_Detail_Etc");

                entity.HasComment("초대장_상세_기타");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Etc_Title)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("기타_제목");

                entity.Property(e => e.Information_Content)
                    .HasMaxLength(1000)
                    .HasComment("정보_내용");

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Invitation_Detail_Etcs)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_Invitation_Detail_Etc");
            });

            modelBuilder.Entity<TB_Invitation_Item>(entity =>
            {
                entity.HasKey(e => e.Item_ID);

                entity.ToTable("TB_Invitation_Item");

                entity.HasComment("초대장_아이템");

                entity.Property(e => e.Item_ID).HasComment("아이템_ID");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Item_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("아이템_유형_코드");

                entity.Property(e => e.Location_Left).HasComment("위치_LEFT");

                entity.Property(e => e.Location_Top).HasComment("위치_TOP");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Resource_ID).HasComment("리소스_ID");

                entity.Property(e => e.Size_Height).HasComment("사이즈_높이");

                entity.Property(e => e.Size_Width).HasComment("사이즈_너비");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Invitation_Items)
                    .HasForeignKey(d => d.Invitation_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Invitation_TO_TB_Invitation_Item");

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.TB_Invitation_Items)
                    .HasForeignKey(d => d.Resource_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Item_Resource_TO_TB_Invitation_Item");
            });

            modelBuilder.Entity<TB_Invitation_Tax>(entity =>
            {
                entity.HasKey(e => e.Invitation_ID);

                entity.ToTable("TB_Invitation_Tax");

                entity.HasComment("초대장_수수료");

                entity.Property(e => e.Invitation_ID)
                    .ValueGeneratedNever()
                    .HasComment("초대장_ID");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Tax_ID).HasComment("수수료_ID");

                entity.HasOne(d => d.Tax)
                    .WithMany(p => p.TB_Invitation_Taxes)
                    .HasForeignKey(d => d.Tax_ID)
                    .HasConstraintName("FK_TB_Tax_TO_TB_Invitation_Tax");
            });

            modelBuilder.Entity<TB_Item_Resource>(entity =>
            {
                entity.HasKey(e => e.Resource_ID);

                entity.ToTable("TB_Item_Resource");

                entity.HasComment("아이템_리소스");

                entity.Property(e => e.Resource_ID).HasComment("리소스_ID");

                entity.Property(e => e.Background_Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("배경_색상");

                entity.Property(e => e.BetweenLine).HasComment("행간");

                entity.Property(e => e.BetweenText).HasComment("자간");

                entity.Property(e => e.Bold_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("굵게_여부");

                entity.Property(e => e.CharacterSet)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("신랑명 & 신부명\r\n");

                entity.Property(e => e.Character_Size).HasComment("문자_크기");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("색상");

                entity.Property(e => e.Font)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("폰트");

                entity.Property(e => e.Horizontal_Alignment)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("수평_정렬");

                entity.Property(e => e.Italic_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("이탤릭체_여부");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Resource_Height).HasComment("리소스_높이");

                entity.Property(e => e.Resource_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("I : 이미지\r\nM : 동영상\r\nT : 텍스트");

                entity.Property(e => e.Resource_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("리소스_URL");

                entity.Property(e => e.Resource_Width).HasComment("리소스_너비");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Underline_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("밑줄_여부");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.Property(e => e.Vertical_Alignment)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("수직_정렬");
            });

            modelBuilder.Entity<TB_Order>(entity =>
            {
                entity.HasKey(e => e.Order_ID);

                entity.ToTable("TB_Order");

                entity.HasComment("주문");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cancel_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Cancel_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')");

                entity.Property(e => e.Card_Installment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CashReceipt_Publish_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.CellPhone_Number)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("휴대전화_번호");

                entity.Property(e => e.Coupon_Price).HasComment("쿠폰_금액");

                entity.Property(e => e.Deposit_DeadLine_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("이메일");

                entity.Property(e => e.Escrow_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Finance_Auth_Number).HasMaxLength(100);

                entity.Property(e => e.Finance_Name).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("이름");

                entity.Property(e => e.Noint_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Order_Code)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("주문_코드");

                entity.Property(e => e.Order_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Order_Path).HasMaxLength(50);

                entity.Property(e => e.Order_Price).HasComment("주문_금액");

                entity.Property(e => e.Order_Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("주문_상태_코드");

                entity.Property(e => e.PG_ID)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("PG_ID");

                entity.Property(e => e.Payer_Name).HasMaxLength(50);

                entity.Property(e => e.Payment_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Payment_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("결제_방법_코드");

                entity.Property(e => e.Payment_Path).HasMaxLength(50);

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Payment_Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("결제_상태_코드");

                entity.Property(e => e.Previous_Order_ID).HasComment("이전_주문_ID");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Trading_Number)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.Property(e => e.User_ID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Previous_Order)
                    .WithMany(p => p.InversePrevious_Order)
                    .HasForeignKey(d => d.Previous_Order_ID)
                    .HasConstraintName("FK_TB_Order_TO_TB_Order");
            });

            modelBuilder.Entity<TB_Order_Copy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_Order_Copy");

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cancel_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Cancel_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Card_Installment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CashReceipt_Publish_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CellPhone_Number)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Deposit_DeadLine_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Escrow_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Finance_Auth_Number).HasMaxLength(100);

                entity.Property(e => e.Finance_Name).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Noint_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Order_Code)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Order_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Order_Path).HasMaxLength(50);

                entity.Property(e => e.Order_Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PG_ID)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Payer_Name).HasMaxLength(50);

                entity.Property(e => e.Payment_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Payment_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Payment_Path).HasMaxLength(50);

                entity.Property(e => e.Payment_Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Trading_Number)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.User_ID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Order_Coupon_Use>(entity =>
            {
                entity.HasKey(e => e.Order_ID);

                entity.ToTable("TB_Order_Coupon_Use");

                entity.HasComment("주문_쿠폰_사용");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Coupon_Publish)
                    .WithMany()
                    .HasForeignKey(d => d.Coupon_Publish_ID)
                    .HasConstraintName("FK_TB_Coupon_Publish_TO_TB_Order_Coupon_Use");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.Order_ID)
                    .HasConstraintName("FK_TB_Order_TO_TB_Order_Coupon_Use");
            });

            modelBuilder.Entity<TB_Order_Product>(entity =>
            {
                entity.HasKey(e => new { e.Order_ID, e.Product_ID });

                entity.ToTable("TB_Order_Product");

                entity.HasComment("주문_상품");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Item_Count).HasComment("아이템_수량");

                entity.Property(e => e.Item_Price).HasComment("아이템_가격");

                entity.Property(e => e.Product_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_구분_코드");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Total_Price).HasComment("전체_가격");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TB_Order_Products)
                    .HasForeignKey(d => d.Order_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Order_TO_TB_Order_Product");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TB_Order_Products)
                    .HasForeignKey(d => d.Product_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Product_TO_TB_Order_Product");
            });

            modelBuilder.Entity<TB_Order_Serial_Coupon_Use>(entity =>
            {
                entity.HasKey(e => e.Order_ID);

                entity.ToTable("TB_Order_Serial_Coupon_Use");

                entity.HasComment("주문_쿠폰_사용");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Coupon_Publish)
                    .WithMany()
                    .HasForeignKey(d => d.Coupon_Publish_ID)
                    .HasConstraintName("FK_TB_Serial_Coupon_Publish_TO_TB_Order_Serial_Coupon_Use");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.Order_ID)
                    .HasConstraintName("FK_TB_Order_TO_TB_Order_Serial_Coupon_Use");
            });

            modelBuilder.Entity<TB_Payment_Status_Day>(entity =>
            {
                entity.ToTable("TB_Payment_Status_Day");

                entity.HasComment("결제_수단_일별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Account_Transfer_Price).HasComment("계좌_이체_금액");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Card_Payment_Price).HasComment("카드_결제_금액");

                entity.Property(e => e.Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Etc_Price).HasComment("기타_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");

                entity.Property(e => e.Total_Price).HasComment("합계_금액");

                entity.Property(e => e.Virtual_Account_Price).HasComment("가상_계좌_금액");
            });

            modelBuilder.Entity<TB_Payment_Status_Month>(entity =>
            {
                entity.ToTable("TB_Payment_Status_Month");

                entity.HasComment("결제_수단_월별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Account_Transfer_Price).HasComment("계좌_이체_금액");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Card_Payment_Price).HasComment("카드_결제_금액");

                entity.Property(e => e.Date)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Etc_Price).HasComment("기타_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");

                entity.Property(e => e.Total_Price).HasComment("합계_금액");

                entity.Property(e => e.Virtual_Account_Price).HasComment("가상_계좌_금액");
            });

            modelBuilder.Entity<TB_Popup>(entity =>
            {
                entity.HasKey(e => e.Popup_ID);

                entity.ToTable("TB_Popup");

                entity.HasComment("팝업");

                entity.Property(e => e.Popup_ID).HasComment("팝업_ID");

                entity.Property(e => e.Popup_Height).HasComment("팝업_높이");

                entity.Property(e => e.Popup_Location_Left).HasComment("팝업_위치_LEFT");

                entity.Property(e => e.Popup_Location_Top).HasComment("팝업_위치_TOP");

                entity.Property(e => e.Popup_Mobile_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("팝업_모바일_여부");

                entity.Property(e => e.Popup_PC_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("팝업_PC_여부");

                entity.Property(e => e.Popup_Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("팝업_제목");

                entity.Property(e => e.Popup_Width).HasComment("팝업_너비");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Popup_Item>(entity =>
            {
                entity.HasKey(e => e.Popup_Item_ID);

                entity.ToTable("TB_Popup_Item");

                entity.HasComment("팝업_아이템");

                entity.Property(e => e.Popup_Item_ID).HasComment("팝업_아이템_ID");

                entity.Property(e => e.End_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("종료_날짜");

                entity.Property(e => e.End_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("종료_시간");

                entity.Property(e => e.Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("이미지_URL");

                entity.Property(e => e.Link_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("링크_URL");

                entity.Property(e => e.Period_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("날짜지정\r\n무제");

                entity.Property(e => e.Popup_ID).HasComment("팝업_ID");

                entity.Property(e => e.Popup_Type_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("PC\r\n모바일\r\n");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Start_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("시작_날짜");

                entity.Property(e => e.Start_Time)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("시작_시간");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Popup)
                    .WithMany(p => p.TB_Popup_Items)
                    .HasForeignKey(d => d.Popup_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Popup_TO_TB_Popup_Item");
            });

            modelBuilder.Entity<TB_Product>(entity =>
            {
                entity.HasKey(e => e.Product_ID);

                entity.ToTable("TB_Product");

                entity.HasComment("상품");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Display_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("진열_여부");

                entity.Property(e => e.Main_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("대표_이미지_URL");

                entity.Property(e => e.Original_Product_Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Preview_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SetCard_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("세트카드_URL");

                entity.Property(e => e.SetCard_Mobile_URL)
                  .HasMaxLength(1000)
                  .IsUnicode(false)
                  .HasComment("세트카드_모바일_URL");

                entity.Property(e => e.SetCard_Display_YN)
                  .IsRequired()
                  .HasMaxLength(1)
                  .IsUnicode(false)
                  .HasDefaultValueSql("('N')")
                  .IsFixedLength(true)
                  .HasComment("세트카드_진열_여부");


                entity.Property(e => e.Price).HasComment("가격");

                entity.Property(e => e.Product_Brand_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_브랜드_코드");

                entity.Property(e => e.Product_Category_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("청첩장\r\n감사장\r\n포토형\r\n\r\n답례품\r\n");

                entity.Property(e => e.Product_Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("상품_코드");

                entity.Property(e => e.Product_Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("상품_설명");

                entity.Property(e => e.Product_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("상품_명");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TB_Products)
                    .HasForeignKey(d => d.Template_ID)
                    .HasConstraintName("FK_TB_Template_TO_TB_Product");
            });

            modelBuilder.Entity<TB_Product_Category>(entity =>
            {
                entity.HasKey(e => new { e.Category_ID, e.Product_ID });

                entity.ToTable("TB_Product_Category");

                entity.HasComment("상품_분류");

                entity.Property(e => e.Category_ID).HasComment("분류_ID");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TB_Product_Categories)
                    .HasForeignKey(d => d.Product_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Product_TO_TB_Product_Category");
            });

            modelBuilder.Entity<TB_Product_Icon>(entity =>
            {
                entity.HasKey(e => new { e.Product_ID, e.Icon_ID });

                entity.ToTable("TB_Product_Icon");

                entity.HasComment("상품_아이콘");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Icon_ID).HasComment("아이콘_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Icon)
                    .WithMany(p => p.TB_Product_Icons)
                    .HasForeignKey(d => d.Icon_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Icon_TO_TB_Product_Icon");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TB_Product_Icons)
                    .HasForeignKey(d => d.Product_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Product_TO_TB_Product_Icon");
            });

            modelBuilder.Entity<TB_Product_Image>(entity =>
            {
                entity.HasKey(e => e.Image_ID);

                entity.ToTable("TB_Product_Image");

                entity.HasComment("상품_이미지");

                entity.Property(e => e.Image_ID).HasComment("이미지_ID");

                entity.Property(e => e.Image_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("이미지_유형_코드");

                entity.Property(e => e.Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("이미지_URL");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TB_Product_Images)
                    .HasForeignKey(d => d.Product_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Product_TO_TB_Product_Image");
            });

            modelBuilder.Entity<TB_Refund_Info>(entity =>
            {
                entity.HasKey(e => e.Refund_ID);

                entity.ToTable("TB_Refund_Info");

                entity.HasComment("환불_정보");

                entity.Property(e => e.Refund_ID).HasComment("환불_ID");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("계좌번호");

                entity.Property(e => e.Bank_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("은행_구분_코드");

                entity.Property(e => e.Depositor_Name)
                    .HasMaxLength(50)
                    .HasComment("예금주_명");

                entity.Property(e => e.Order_ID).HasComment("주문_ID");

                entity.Property(e => e.Refund_Content)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("환불_내용");

                entity.Property(e => e.Refund_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("환불_일시");

                entity.Property(e => e.Refund_Price).HasComment("환불_금액");

                entity.Property(e => e.Refund_Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("환불_상태_코드");

                entity.Property(e => e.Refund_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("환불_유형_코드");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TB_Refund_Infos)
                    .HasForeignKey(d => d.Order_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Order_TO_TB_Refund_Info");
            });

            modelBuilder.Entity<TB_Remit>(entity =>
            {
                entity.HasKey(e => e.Remit_ID);

                entity.ToTable("TB_Remit");

                entity.HasComment("송금_정보");

                entity.Property(e => e.Remit_ID).HasComment("송금_ID");

                entity.Property(e => e.Account_ID).HasComment("계좌_ID");

                entity.Property(e => e.Account_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("더즌에서 할당받은 카카오페이 계좌번호\r\n");

                entity.Property(e => e.Bank_Code)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasComment("더즌에서 할당 받은 카카오페이 결제 은행코드\r\n");

                entity.Property(e => e.Complete_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Complete_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("완료_일시");

                entity.Property(e => e.Coupon_Order_ID).HasComment("쿠폰_주문_ID");

                entity.Property(e => e.Error_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Error_Message)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Invitation_ID).HasComment("초대장_ID");

                entity.Property(e => e.Item_Name)
                    .HasMaxLength(50)
                    .HasComment("혼주 예금주로 대응\r\n");

                entity.Property(e => e.Partner_Order_ID)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("유니크인덱스 설정 필요\r\n\r\n[년월일] + [제로필 일련번호5자리]\r\n2021123100000");

                entity.Property(e => e.Payment_Token)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("결제_토큰");

                entity.Property(e => e.Ready_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("준비_일시");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Remitter_Name)
                    .HasMaxLength(50)
                    .HasComment("송금자_명");

                entity.Property(e => e.Request_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("요청_일시");

                entity.Property(e => e.Result_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("R0 : 준비요청\r\nR1 : 준비완료\r\nP2 : 승인요청\r\nP3 : 승인완료\r\n\r\nRC : 준비취소\r\nRF : 준비실패\r\nPF : 승인실패\r\n\r\nC0 : 정산 완료");

                entity.Property(e => e.Send_Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("전송_상태");

                entity.Property(e => e.Status_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total_Price).HasComment("전체_금액");

                entity.Property(e => e.Transaction_Detail_ID)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("더즌에서 받는 정보\r\n");

                entity.Property(e => e.Transaction_ID)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("더즌에서 받는 정보\r\n");

                entity.HasOne(d => d.Coupon_Order)
                    .WithMany(p => p.TB_Remits)
                    .HasForeignKey(d => d.Coupon_Order_ID)
                    .HasConstraintName("FK_TB_Coupon_Order_TO_TB_Remit");

                entity.HasOne(d => d.Invitation)
                    .WithMany(p => p.TB_Remits)
                    .HasForeignKey(d => d.Invitation_ID)
                    .HasConstraintName("FK_TB_Invitation_Tax_TO_TB_Remit");
            });

            modelBuilder.Entity<TB_Remit_Statistics_Daily>(entity =>
            {
                entity.HasKey(e => e.Date);

                entity.ToTable("TB_Remit_Statistics_Daily");

                entity.HasComment("송금_통계");

                entity.Property(e => e.Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Account_Count).HasComment("계좌_수");

                entity.Property(e => e.Calculate_Tax).HasComment("업체_수수료");

                entity.Property(e => e.Hits_Tax).HasComment("조회_수수료");

                entity.Property(e => e.Remit_Count).HasComment("송금_수");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Tax).HasComment("수수료");

                entity.Property(e => e.User_Count).HasComment("사용자_수");
            });

            modelBuilder.Entity<TB_Remit_Statistics_Monthly>(entity =>
            {
                entity.HasKey(e => e.Date);

                entity.ToTable("TB_Remit_Statistics_Monthly");

                entity.HasComment("송금_통계");

                entity.Property(e => e.Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Account_Count).HasComment("계좌_수");

                entity.Property(e => e.Calculate_Tax).HasComment("업체_수수료");

                entity.Property(e => e.Hits_Tax).HasComment("조회_수수료");

                entity.Property(e => e.Remit_Count).HasComment("송금_수");

                entity.Property(e => e.Remit_Price).HasComment("송금_금액");

                entity.Property(e => e.Tax).HasComment("수수료");

                entity.Property(e => e.User_Count).HasComment("사용자_수");
            });

            modelBuilder.Entity<TB_ReservationWord>(entity =>
            {
                entity.HasKey(e => e.ReserveWord_ID);

                entity.ToTable("TB_ReservationWord");

                entity.HasComment("예약어");

                entity.Property(e => e.DefaultValue)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("기본값");

                entity.Property(e => e.MappingField)
                    .HasMaxLength(100)
                    .HasComment("맵핑필드");

                entity.Property(e => e.Mapping_YN)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReserveWord)
                    .HasMaxLength(100)
                    .HasComment("예약어");

                entity.Property(e => e.ReserveWord_ID)
                    .ValueGeneratedOnAdd()
                    .HasComment("예약어_ID");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_SCHEDULER_COUPON_PUBLISHED>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TB_SCHEDULER_COUPON_PUBLISHED");

                entity.Property(e => e.Member_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reg_Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<TB_Sales_Statistic_Day>(entity =>
            {
                entity.ToTable("TB_Sales_Statistic_Day");

                entity.HasComment("매출_통계_일별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Barunn_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_유료_주문_수");

                entity.Property(e => e.Barunn_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_무료_주문_수");

                entity.Property(e => e.Barunn_Sales_Price).HasComment("바른손_매출_금액");

                entity.Property(e => e.Bhands_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_유료_주문 _수");

                entity.Property(e => e.Bhands_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_무료_주문 _수");

                entity.Property(e => e.Bhands_Sales_Price).HasComment("비핸즈_매출_금액");

                entity.Property(e => e.Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Premier_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_유료_주문_수");

                entity.Property(e => e.Premier_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_무료_주문_수");

                entity.Property(e => e.Premier_Sales_Price).HasComment("프리미어_매출_금액 ");

                entity.Property(e => e.Thecard_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_유료_주문_수 ");

                entity.Property(e => e.Thecard_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_무료_주문_수 ");

                entity.Property(e => e.Thecard_Sales_Price).HasComment("더카드_매출_금액");

                entity.Property(e => e.Total_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_유료_주문_수");

                entity.Property(e => e.Total_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_무료_주문_수");

                entity.Property(e => e.Total_Sales_Price).HasComment("합계_매출_금액");
            });

            modelBuilder.Entity<TB_Serial_Apply_Product>(entity =>
            {
                entity.HasKey(e => new { e.Product_Apply_ID, e.Product_Code });

                entity.ToTable("TB_Serial_Apply_Product");

                entity.HasComment("쿠폰_적용_상품_리스트");

                entity.Property(e => e.Product_Apply_ID).HasComment("상품_적용_ID");

                entity.Property(e => e.Product_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("상품_코드");

                entity.HasOne(d => d.Product_Apply)
                    .WithMany()
                    .HasForeignKey(d => d.Product_Apply_ID)
                    .HasConstraintName("FK_TB_Serial_Coupon_Apply_Product_TO_TB_Serial_Apply_Product");
            });

            modelBuilder.Entity<TB_Serial_Coupon>(entity =>
            {
                entity.HasKey(e => e.Coupon_ID);

                entity.ToTable("TB_Serial_Coupon");

                entity.HasComment("쿠폰");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Coupon_Apply_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("쿠폰_적용_코드");

                entity.Property(e => e.Coupon_Apply_Product_ID).HasComment("쿠폰_적용_상품_ID");

                entity.Property(e => e.Coupon_Image_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("쿠폰_이미지_URL");

                entity.Property(e => e.Coupon_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("쿠폰_명");

                entity.Property(e => e.Coupon_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("쿠폰_유형_코드");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("설명");

                entity.Property(e => e.Discount_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("할인_방식_코드");

                entity.Property(e => e.Discount_Price).HasComment("할인_금액");

                entity.Property(e => e.Discount_Rate).HasComment("할인_율");

                entity.Property(e => e.Period_Method_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("기간_방식_코드");

                entity.Property(e => e.Publish_End_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("발행_종료_일자");

                entity.Property(e => e.Publish_Period_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("발행_기간_코드");

                entity.Property(e => e.Publish_Start_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("발행_시작_일자");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Serial_Coupon_Number)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Standard_Purchase_Price).HasComment("기준_구매_금액");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Use_Available_Standard_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용_가능_기준_코드");

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("사용_여부");
            });

            modelBuilder.Entity<TB_Serial_Coupon_Apply_Product>(entity =>
            {
                entity.HasKey(e => e.Product_Apply_ID)
                    .IsClustered(false);

                entity.ToTable("TB_Serial_Coupon_Apply_Product");

                entity.Property(e => e.Product_Apply_ID).HasComment("상품_적용_ID");
            });

            modelBuilder.Entity<TB_Serial_Coupon_Publish>(entity =>
            {
                entity.HasKey(e => e.Coupon_Publish_ID);

                entity.ToTable("TB_Serial_Coupon_Publish");

                entity.HasIndex(e => e.Coupon_Number, "NIDX_Coupon_Number");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰_발행_ID");

                entity.Property(e => e.Assign_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Coupon_ID).HasComment("쿠폰_ID");

                entity.Property(e => e.Coupon_Number)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expiration_Date)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("만료_일자");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Retrieve_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("회수_일시");

                entity.Property(e => e.Update_DateTime).HasColumnType("datetime");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Use_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("사용_일시");

                entity.Property(e => e.Use_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("사용_여부");

                entity.Property(e => e.User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.TB_Serial_Coupon_Publishes)
                    .HasForeignKey(d => d.Coupon_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Serial_Coupon_TO_TB_Serial_Coupon_Publish");
            });

            modelBuilder.Entity<TB_Sales_Statistic_Month>(entity =>
            {
                entity.ToTable("TB_Sales_Statistic_Month");

                entity.HasComment("매출_통계_월별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Barunn_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_유료_주문_수");

                entity.Property(e => e.Barunn_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("바른손_무료_주문_수");

                entity.Property(e => e.Barunn_Sales_Price).HasComment("바른손_매출_금액");

                entity.Property(e => e.Bhands_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_유료_주문 _수");

                entity.Property(e => e.Bhands_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("비핸즈_무료_주문 _수");

                entity.Property(e => e.Bhands_Sales_Price).HasComment("비핸즈_매출_금액");

                entity.Property(e => e.Date)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Premier_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_유료_주문_수");

                entity.Property(e => e.Premier_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("프리미어_무료_주문_수");

                entity.Property(e => e.Premier_Sales_Price).HasComment("프리미어_매출_금액 ");

                entity.Property(e => e.Thecard_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_유료_주문_수 ");

                entity.Property(e => e.Thecard_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("더카드_무료_주문_수 ");

                entity.Property(e => e.Thecard_Sales_Price).HasComment("더카드_매출_금액");

                entity.Property(e => e.Total_Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_유료_주문_수");

                entity.Property(e => e.Total_Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("합계_무료_주문_수");

                entity.Property(e => e.Total_Sales_Price).HasComment("합계_매출_금액");
            });

            modelBuilder.Entity<TB_Standard_Date>(entity =>
            {
                entity.HasKey(e => e.Standard_Date);

                entity.ToTable("TB_Standard_Date");

                entity.HasComment("기준_날짜");

                entity.Property(e => e.Standard_Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("기준_날짜");

                entity.Property(e => e.Standard_Month)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Standard_Year)
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TB_Tax>(entity =>
            {
                entity.HasKey(e => e.Tax_ID);

                entity.ToTable("TB_Tax");

                entity.HasComment("수수료");

                entity.Property(e => e.Tax_ID).HasComment("수수료_ID");

                entity.Property(e => e.Previous_Tax).HasComment("이전_수수료");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Tax).HasComment("수수료");
            });

            modelBuilder.Entity<TB_Temp_Order>(entity =>
            {
                entity.HasKey(e => e.Order_Code)
                    .IsClustered(false);

                entity.ToTable("TB_Temp_Order");

                entity.HasComment("주문_임시");

                entity.Property(e => e.Order_Code)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasComment("주문코드");

                entity.Property(e => e.Coupon_Price).HasComment("쿠폰가격");

                entity.Property(e => e.Coupon_Publish_ID).HasComment("쿠폰발급아이디");
            });

            modelBuilder.Entity<TB_Template>(entity =>
            {
                entity.HasKey(e => e.Template_ID);

                entity.ToTable("TB_Template");

                entity.HasComment("템플릿");

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Attached_File1_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Attached_File2_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Background_Color)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Photo_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("포토_여부");

                entity.Property(e => e.Preview_URL)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("미리보기_URL");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Template_Name)
                    .HasMaxLength(100)
                    .HasComment("템플릿_명");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");
            });

            modelBuilder.Entity<TB_Template_Area>(entity =>
            {
                entity.HasKey(e => new { e.Template_ID, e.Area_ID });

                entity.ToTable("TB_Template_Area");

                entity.HasComment("템플릿_영역");

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Size_Height).HasComment("사이즈_높이");

                entity.Property(e => e.Size_Width).HasComment("사이즈_너비");

                entity.Property(e => e.Sort).HasComment("순서");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.TB_Template_Areas)
                    .HasForeignKey(d => d.Area_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Area_TO_TB_Template_Area");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TB_Template_Areas)
                    .HasForeignKey(d => d.Template_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Template_TO_TB_Template_Area");
            });

            modelBuilder.Entity<TB_Template_Detail>(entity =>
            {
                entity.HasKey(e => e.Template_ID);

                entity.ToTable("TB_Template_Detail");

                entity.HasComment("템플릿 상세");

                entity.Property(e => e.Template_ID)
                    .ValueGeneratedNever()
                    .HasComment("템플릿_ID");

                entity.Property(e => e.Bride_EngName)
                    .HasMaxLength(100)
                    .HasComment("신부_영문명");

                entity.Property(e => e.Bride_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("신부_명");

                entity.Property(e => e.Bride_Parents1_Name)
                    .HasMaxLength(100)
                    .HasComment("신부_혼주1_명칭");

                entity.Property(e => e.Bride_Parents1_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주1_전화");

                entity.Property(e => e.Bride_Parents2_Name)
                    .HasMaxLength(100)
                    .HasComment("신부_혼주2_명칭");

                entity.Property(e => e.Bride_Parents2_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_혼주2_전화");

                entity.Property(e => e.Bride_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신부_전화");

                entity.Property(e => e.Etc_Bus_Information)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("기타_버스_안내");

                entity.Property(e => e.Etc_Car_Information)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("기타_자가용_안내");

                entity.Property(e => e.Greetings)
                    .HasMaxLength(1000)
                    .HasComment("인사말");

                entity.Property(e => e.Groom_EngName)
                    .HasMaxLength(100)
                    .HasComment("신랑_영문명");

                entity.Property(e => e.Groom_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("신랑_명");

                entity.Property(e => e.Groom_Parents1_Name)
                    .HasMaxLength(100)
                    .HasComment("신랑_혼주1_명칭");

                entity.Property(e => e.Groom_Parents1_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주1_전화");

                entity.Property(e => e.Groom_Parents2_Name)
                    .HasMaxLength(100)
                    .HasComment("신랑_혼주2_명칭");

                entity.Property(e => e.Groom_Parents2_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_혼주2_전화");

                entity.Property(e => e.Groom_Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("신랑_전화");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Time_Type_Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("시간_구분_코드");

                entity.Property(e => e.Time_Type_Eng_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("시간_구분_영문_여부");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.Property(e => e.WeddingDD)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("예식일");

                entity.Property(e => e.WeddingDate)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("예식일자");

                entity.Property(e => e.WeddingHHmm)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0000')")
                    .HasComment("예식시분");

                entity.Property(e => e.WeddingHallDetail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("층홀실");

                entity.Property(e => e.WeddingHour)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("예식시간");

                entity.Property(e => e.WeddingMM)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("예식월");

                entity.Property(e => e.WeddingMin)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('00')")
                    .HasComment("예식분");

                entity.Property(e => e.WeddingWeek)
                    .HasMaxLength(100)
                    .HasComment("예식요일");

                entity.Property(e => e.WeddingWeek_Eng_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true)
                    .HasComment("예식요일_영어_여부");

                entity.Property(e => e.WeddingYY)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasComment("예식년");

                entity.Property(e => e.Weddinghall_Address)
                    .HasMaxLength(500)
                    .HasComment("예식장_주소1");

                entity.Property(e => e.Weddinghall_Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("예식장_명");

                entity.Property(e => e.Weddinghall_PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("예식장_연락처");

                entity.HasOne(d => d.Template)
                    .WithOne(p => p.TB_Template_Detail)
                    .HasForeignKey<TB_Template_Detail>(d => d.Template_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Template_TO_TB_Template_Detail");
            });

            modelBuilder.Entity<TB_Template_Item>(entity =>
            {
                entity.HasKey(e => e.Item_ID);

                entity.ToTable("TB_Template_Item");

                entity.HasComment("템플릿_아이템");

                entity.Property(e => e.Item_ID).HasComment("아이템_ID");

                entity.Property(e => e.Area_ID).HasComment("영역_ID");

                entity.Property(e => e.Item_Type_Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("텍스트\r\n이미지\r\n");

                entity.Property(e => e.Location_Left).HasComment("위치_LEFT");

                entity.Property(e => e.Location_Top).HasComment("위치_TOP");

                entity.Property(e => e.Regist_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.Regist_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("등록_IP");

                entity.Property(e => e.Regist_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("등록_사용자_ID");

                entity.Property(e => e.Resource_ID).HasComment("리소스_ID");

                entity.Property(e => e.Size_Height).HasComment("사이즈_높이");

                entity.Property(e => e.Size_Width).HasComment("사이즈_너비");

                entity.Property(e => e.Template_ID).HasComment("템플릿_ID");

                entity.Property(e => e.Update_DateTime)
                    .HasColumnType("datetime")
                    .HasComment("수정_일시");

                entity.Property(e => e.Update_IP)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("수정_IP");

                entity.Property(e => e.Update_User_ID)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("수정_사용자_ID");

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.TB_Template_Items)
                    .HasForeignKey(d => d.Resource_ID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_Item_Resource_TO_TB_Template_Item");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TB_Template_Items)
                    .HasForeignKey(d => d.Template_ID)
                    .HasConstraintName("FK_TB_Template_TO_TB_Template_Item");
            });

            modelBuilder.Entity<TB_Total_Statistic_Day>(entity =>
            {
                entity.ToTable("TB_Total_Statistic_Day");

                entity.HasComment("전체_현황_일별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Cancel_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("취소_수");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유료_주문_수");

                entity.Property(e => e.Date)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("무료_주문_수");

                entity.Property(e => e.Memberjoin_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("회원가입_수");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");
            });

            modelBuilder.Entity<TB_Total_Statistic_Month>(entity =>
            {
                entity.ToTable("TB_Total_Statistic_Month");

                entity.HasComment("전체_현황_월별");

                entity.Property(e => e.ID).HasComment("ID");

                entity.Property(e => e.Cancel_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("취소_수");

                entity.Property(e => e.Cancel_Refund_Price).HasComment("취소_환불_금액");

                entity.Property(e => e.Charge_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("유료_주문_수");

                entity.Property(e => e.Date)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasComment("날짜");

                entity.Property(e => e.Free_Order_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("무료_주문_수");

                entity.Property(e => e.Memberjoin_Count)
                    .HasDefaultValueSql("((0))")
                    .HasComment("회원가입_수");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Profit_Price).HasComment("순매출_금액");
            });

            modelBuilder.Entity<TB_Wish_List>(entity =>
            {
                entity.HasKey(e => e.Wish_ID);

                entity.ToTable("TB_Wish_List");

                entity.HasComment("위시리스트");

                entity.Property(e => e.Wish_ID).HasComment("찜_ID");

                entity.Property(e => e.Product_ID).HasComment("상품_ID");

                entity.Property(e => e.Regist_DateTime).HasColumnType("datetime");

                entity.Property(e => e.User_ID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("사용자_ID");
            });

            modelBuilder.Entity<VW_Admin>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_Admin");

                entity.Property(e => e.ADMIN_ID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADMIN_NAME)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADMIN_PASSWORD)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.REGIST_DATETIME).HasColumnType("datetime");
            });

            modelBuilder.Entity<VW_User>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_User");

                entity.Property(e => e.CARD_CODE)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CELLPHONE_NUMBER)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.DELETE_IP)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DELETE_USER_ID)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DUPINFO)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.EMAIL)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.INTEGRATION_MEMBER_YORN)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.JOIN_DATETIME).HasColumnType("datetime");

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE_NUMBER)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.PWD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.REFERER_SALES_GUBUN)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.REGIST_DATETIME).HasColumnType("datetime");

                entity.Property(e => e.REGIST_IP)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.REGIST_USER_ID)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATE_USER_ID)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.USER_ID)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WEDDINGCARD_ORDER_YN)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WEDDING_DATE)
                    .HasMaxLength(18)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VW_User_QNA>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VW_User_QNA");

                entity.Property(e => e.ADMIN_NAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ADMIN_UPFILE1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ANSWER_CONTENT).HasColumnType("text");

                entity.Property(e => e.ANSWER_DATETIME).HasColumnType("datetime");

                entity.Property(e => e.CONTENT)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.EMAIL)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NAME)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PHONE_NUMBER)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.QNA_ID).ValueGeneratedOnAdd();

                entity.Property(e => e.REGIST_DATETIME).HasColumnType("datetime");

                entity.Property(e => e.SALES_GUBUN)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.STAT)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.TITLE)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UPFILE_1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.UPFILE_2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.USERID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Q_KIND)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TB_Order_PartnerShip>(entity =>
            {
                entity.HasKey(e => new { e.P_OrderCode, e.P_Id });

                entity.HasComment("파트너 연동 주문 정보");

                entity.Property(e => e.P_OrderCode).HasComment("파트너사 주문번호");

                entity.Property(e => e.P_Id).HasComment("파트너사 고유ID");

                entity.Property(e => e.Is_Refund).HasComment("최소 여부");

                entity.Property(e => e.Order_ID).HasComment("바른손 주문_ID");

                entity.Property(e => e.P_ExtendData).HasComment("파트너사 확장 데이터");

                entity.Property(e => e.P_OrderDate).HasComment("주문일");

                entity.Property(e => e.P_Order_Name).HasComment("주문자명");

                entity.Property(e => e.P_Order_Phone).HasComment("주문자연락처");

                entity.Property(e => e.P_ProductCode).HasComment("상품코드");

                entity.Property(e => e.P_ProductName).HasComment("상품명");

                entity.Property(e => e.Payment_Method_Code).HasComment("결제_방법_코드");

                entity.Property(e => e.Payment_Price).HasComment("결제_금액");

                entity.Property(e => e.Payment_Status_Code).HasComment("결제_상태_코드");
            });

            modelBuilder.Entity<TB_Kakao_Cache>(entity =>
            {
                entity.HasComment("카카오_케시");

                entity.Property(e => e.Cache_ID).HasComment("케시_ID");

                entity.Property(e => e.Cache_URL).HasComment("케시_URL");

                entity.Property(e => e.Progress_DateTime).HasComment("처리_일시");

                entity.Property(e => e.Progress_YN)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength()
                    .HasComment("처리_여부");

                entity.Property(e => e.Regist_DateTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("등록_일시");
            });

            modelBuilder.Entity<TB_PolicyInfo>(entity =>
            {
                entity.HasKey(e => e.Seq);

                entity.ToTable("TB_PolicyInfo");

                entity.HasComment("약관관리");

                entity.Property(e => e.Seq).HasComment("약관_ID");

                entity.Property(e => e.PolicyDiv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('P')")
                    .IsFixedLength(true)
                    .HasComment("약관구분");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("제목");

                entity.Property(e => e.Contents)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasComment("내용");

                entity.Property(e => e.AdminName).HasComment("작성자명");

                entity.Property(e => e.RegDate)
                    .HasColumnType("datetime")
                    .HasComment("등록_일시");

                entity.Property(e => e.StartDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("시작_일자");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("종료_일자");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #endregion
    }
}
