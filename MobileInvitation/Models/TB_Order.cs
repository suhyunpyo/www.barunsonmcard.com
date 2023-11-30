using System;
using System.Collections.Generic;

#nullable disable

namespace MobileInvitation.Models
{
    public partial class TB_Order
    {
        public TB_Order()
        {
            InversePrevious_Order = new HashSet<TB_Order>();
            TB_Invitations = new HashSet<TB_Invitation>();
            TB_Order_Products = new HashSet<TB_Order_Product>();
            TB_Refund_Infos = new HashSet<TB_Refund_Info>();
        }

        public int Order_ID { get; set; }
        public int? Previous_Order_ID { get; set; }
        public string Order_Code { get; set; }
        public string User_ID { get; set; }
        public string Name { get; set; }
        public string CellPhone_Number { get; set; }
        public string Email { get; set; }
        public int Order_Price { get; set; }
        public string Payment_Method_Code { get; set; }
        public string PG_ID { get; set; }
        public int? Coupon_Price { get; set; }
        public int? Payment_Price { get; set; }
        public string Payment_Status_Code { get; set; }
        public string Order_Status_Code { get; set; }
        public string Regist_User_ID { get; set; }
        public DateTime? Regist_DateTime { get; set; }
        public string Regist_IP { get; set; }
        public string Update_User_ID { get; set; }
        public DateTime? Update_DateTime { get; set; }
        public string Update_IP { get; set; }
        public string Card_Installment { get; set; }
        public string CashReceipt_Publish_YN { get; set; }
        public string Noint_YN { get; set; }
        public string Finance_Auth_Number { get; set; }
        public string Finance_Name { get; set; }
        public string Payer_Name { get; set; }
        public string Escrow_YN { get; set; }
        public string Account_Number { get; set; }
        public string Trading_Number { get; set; }
        public DateTime? Order_DateTime { get; set; }
        public DateTime? Cancel_DateTime { get; set; }
        public string Cancel_Time { get; set; }
        public DateTime? Deposit_DeadLine_DateTime { get; set; }
        public string Order_Path { get; set; }
        public string Payment_Path { get; set; }
        public DateTime? Payment_DateTime { get; set; }

        public virtual TB_Order Previous_Order { get; set; }
        public virtual ICollection<TB_Order> InversePrevious_Order { get; set; }
        public virtual ICollection<TB_Invitation> TB_Invitations { get; set; }
        public virtual ICollection<TB_Order_Product> TB_Order_Products { get; set; }
        public virtual ICollection<TB_Refund_Info> TB_Refund_Infos { get; set; }
    }
}
