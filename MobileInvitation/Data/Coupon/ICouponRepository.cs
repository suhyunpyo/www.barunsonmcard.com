using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Coupon
{
    public interface ICouponRepository
    {

        public List<Dictionary<string, object>> User_Coupon_List_Entity_New(string User_Id, string Product_Code);

        public int SetCouponProcess(TB_Serial_Coupon_Publish serial_coupon_publish);
    }
}
