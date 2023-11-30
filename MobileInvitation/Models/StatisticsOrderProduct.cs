using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Models
{
    public class StatisticsOrderProduct
    {
        public string ProductBrandCode { set; get; }
        public string ProductCode { set; get; }
        public int PayCount { set; get; }
        public int FreeCount { set; get; }
        public int TotalCount { set; get; }
    }
}
