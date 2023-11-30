using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Models
{
    public class Map
    {
        public string ApiId { set; get; }
        public string ApiKey { set; get; }

        public double DefaultMapLat { set; get; }
        public double DefaultMapLot { set; get; }
    }
}
