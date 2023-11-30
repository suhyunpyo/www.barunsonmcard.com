using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MobileInvitation.Models
{
    /// <summary>
    /// 카카오_케시
    /// </summary>
    public partial class TB_Kakao_Cache
    {
        /// <summary>
        /// 케시_ID
        /// </summary>
        [Key]
        public int Cache_ID { get; set; }
        /// <summary>
        /// 케시_URL
        /// </summary>
        [StringLength(1000)]
        public string Cache_URL { get; set; } = null!;
        /// <summary>
        /// 처리_여부
        /// </summary>
        [StringLength(1)]
        public string? Progress_YN { get; set; }
        /// <summary>
        /// 등록_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Regist_DateTime { get; set; }
        /// <summary>
        /// 처리_일시
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? Progress_DateTime { get; set; }
    }
}
