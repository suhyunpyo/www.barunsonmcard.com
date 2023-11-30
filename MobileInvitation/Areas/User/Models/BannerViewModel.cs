using System;

namespace MobileInvitation.Areas.User.Models
{
	/// <summary>
	/// 배너뷰 모델
	/// </summary>
	public class BannerViewModel
	{

		public int BannerId { get; set; }
		public int BannerItemId { get; set; }
		public int Sort { get; set; }

		public Uri ImageUrl { get; set; }
		public string LinkUrl { get; set; }

		public string MainDescription { get; set; }
		public string AddDescription { get; set; }

		/// <summary>
		/// 새창 여부
		/// </summary>
		public bool IsNewWindow { get; set; }

	}
}
