using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Order_Product
    {
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public string Product_Type_Code { get; set; }
        public int? Item_Price { get; set; }
        public int? Item_Count { get; set; }
        public int? Total_Price { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_User_ID { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }

        public virtual TB_Order Order { get; set; }
        public virtual TB_Product Product { get; set; }
    }
}
