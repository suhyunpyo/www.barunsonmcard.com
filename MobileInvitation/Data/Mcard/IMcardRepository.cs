using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileInvitation.Data.Mcard
{
    public interface IMcardRepository
    {

        List<TB_GuestBook> UserGuestBookList(int InvitationId, string UserId);
        void UserGuestbookDisplay(int InvitationId, int GuestbookId, string DisplayYn, string UserId);
        Boolean RemoveUserGuestbook(int InvitationId, int GuestbookId, string UserId);
    }
}
