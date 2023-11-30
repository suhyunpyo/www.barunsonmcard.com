using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Member
{
    public interface IMemberRepository
    {

        List<Dictionary<string, object>> User_Seach_MyOrderList_Sql(string Gubun, TB_Order model);

    }
}
