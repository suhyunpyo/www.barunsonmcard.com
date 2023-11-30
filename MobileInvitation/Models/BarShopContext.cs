using Microsoft.EntityFrameworkCore;

namespace MobileInvitation.Models
{
	public partial class BarShopContext : DbContext
	{
		public BarShopContext()
		{
		}

		public BarShopContext(DbContextOptions<BarShopContext> options)
			: base(options)
		{
		}

		#region 프로퍼티
		public virtual DbSet<User_Certification_Log> User_Certification_Log { get; set; }
		#endregion


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User_Certification_Log>(entity =>
			{
				entity.Property(e => e.CertSeq).HasComment("시퀀스");
				entity.Property(e => e.CertData).HasComment("인증 데이터");
				entity.Property(e => e.CertID).HasComment("인증고유 ID (웹에서 db access용으로 사용)");
				entity.Property(e => e.CertType)
					.HasDefaultValueSql("('CPC')")
					.HasComment("인증방식 구분 ( CPClient:통합인증, IPIN:아이핀, NONE:인증없이 di전달용)");
				entity.Property(e => e.DupInfo).HasComment("고유 개인정보");
				entity.Property(e => e.RegDate)
					.HasDefaultValueSql("(getdate())")
					.HasComment("등록일시");
			});
		}
	}
}
