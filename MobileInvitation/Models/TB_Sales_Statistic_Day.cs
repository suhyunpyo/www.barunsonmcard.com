using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Sales_Statistic_Day
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public int? Barunn_Sales_Price { get; set; }
        public int? Barunn_Free_Order_Count { get; set; }
        public int? Barunn_Charge_Order_Count { get; set; }
        public int? Bhands_Sales_Price { get; set; }
        public int? Bhands_Free_Order_Count { get; set; }
        public int? Bhands_Charge_Order_Count { get; set; }
        public int? Thecard_Sales_Price { get; set; }
        public int? Thecard_Free_Order_Count { get; set; }
        public int? Thecard_Charge_Order_Count { get; set; }
        public int? Premier_Sales_Price { get; set; }
        public int? Premier_Free_Order_Count { get; set; }
        public int? Premier_Charge_Order_Count { get; set; }
        public int? Total_Sales_Price { get; set; }
        public int? Total_Free_Order_Count { get; set; }
        public int? Total_Charge_Order_Count { get; set; }
    }
}
