using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Order
{
    public interface IOrderRepository
    {

        public TB_Order TB_Order_EntityFromInvitation_ID(int Invitation_ID);

        public int TB_Order_Insert_Sql(TB_Order order);

        public int TB_Order_Update_Sql(TB_Order order);

        public int TB_Order_Product_Insert_Sql(TB_Order_Product order_product);


        /// <summary>
        /// 마이페이지 - 주문 삭제 , 일단 삭제보다는 Order_Status_Code값을 주문 취소로 변경
        /// </summary>
        /// <param name="Order_Id"></param>
        void User_Order_Del(int Order_Id);


        string User_Invitation_Mms_Send_Sql(int Order_Id, string Phone);

        string User_Order_Sms_Send_Sql(string Order_Code, string Order_Path);


    }
}
