using MobileInvitation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace MobileInvitation.Data.Mcard
{
    public class McardRepository : IMcardRepository
    {
        private readonly barunsonContext barunson;

        public McardRepository(barunsonContext ctx)
        {
            this.barunson = ctx;
        }

        List<TB_GuestBook> IMcardRepository.UserGuestBookList(int InvitationId, string UserId)
        {
            TB_Invitation invitation = barunson.TB_Invitations.Where(s => s.Invitation_ID == InvitationId && s.User_ID == UserId).FirstOrDefault();
            
            if(invitation == null)
            {
                return null;
            }

            IQueryable<TB_GuestBook> query = barunson.TB_GuestBooks.Where(s => s.Invitation_ID == InvitationId).OrderByDescending(s => s.GuestBook_ID);
            List<TB_GuestBook> GuestBooks = query.ToList();

            return GuestBooks;
        }

        public void UserGuestbookDisplay(int InvitationId, int GuestbookId, string DisplayYn, string UserId)
        {
            TB_Invitation invitation = barunson.TB_Invitations.Where(s => s.Invitation_ID == InvitationId && s.User_ID == UserId).FirstOrDefault();

            if (invitation == null)
            {
                return;
            }

            TB_GuestBook guestbook = barunson.TB_GuestBooks.Where(s => s.Invitation_ID == InvitationId && s.GuestBook_ID == GuestbookId).FirstOrDefault();
            if(guestbook != null)
            {
                guestbook.Display_YN = DisplayYn;

                barunson.TB_GuestBooks.Update(guestbook);
                barunson.SaveChanges();
            }
        }

        public bool RemoveUserGuestbook(int InvitationId, int GuestbookId, string UserId)
        {
            TB_Invitation invitation = barunson.TB_Invitations.Where(s => s.Invitation_ID == InvitationId && s.User_ID == UserId).FirstOrDefault();

            if (invitation == null)
            {
                return false;
            }

            TB_GuestBook guestbook = barunson.TB_GuestBooks.Where(s => s.Invitation_ID == InvitationId && s.GuestBook_ID == GuestbookId).FirstOrDefault();
            if (guestbook != null)
            {
                barunson.TB_GuestBooks.Remove(guestbook);
                barunson.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
