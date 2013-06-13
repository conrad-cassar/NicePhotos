using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class Friends : BusinessBase
    {
        private DL.DAFriend dlf;

        public Friends(string username)
            : base(username)
        { dlf = new DL.DAFriend(this.Entities); }

        internal Friends()
            : base(null)
        { dlf = new DL.DAFriend(this.Entities); }

        public bool AddFriend(string username, string friend)
        {
            try
            {
                return dlf.AddFriend(username, friend);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Friend> GetUserFriends(string username)
        {
            try
            {
                return dlf.GetUserFriends(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
