using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL
{
    public class DAFriend : ConnectionClass
    {
        public DAFriend()
            : base()
        { }

        public DAFriend(Common.NicePhotosEntities entities)
            : base(entities)
        { }

        public bool AddFriend(string username, string friend)
        {
            return Convert.ToBoolean(this.Entities.AddFriend(username, friend));
        }

        public IQueryable<Common.Friend> GetUserFriends(string username)
        {
            return this.Entities.Friends.Where(p => p.Username == username);
        }

        public bool IsFriend(string username, string friend)
        {
            try
            {
                return GetUserFriends(username).Where(p => p.Friend1 == friend).Count() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
