using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Areas.User.Models
{
    /// <summary>
    /// 아기정보 모델
    /// </summary>
    public class BabyFirstBirthViewModel
    {
        public int idx { get; set; }
        /// <summary>
        /// 아기이름
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 탄생일
        /// </summary>
        public DateTime Birthday { get; set; }

        public string Image_URL { get; set; }
        public int Image_Width { get; set; }
        public int Image_Height { get; set; }

        /// <summary>
        /// 추가 정보
        /// </summary>
        public List<BabyFirstBirthExtraInfo> ExtraInfos { get; set; }
    }

    /// <summary>
    /// 추가정보
    /// </summary>
    public class BabyFirstBirthExtraInfo
    {
        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 값
        /// </summary>
        public string Value { get; set; }
    }
}
