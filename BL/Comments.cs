using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class Comments : BusinessBase
    {
        private DL.DAComment dlc;

        public Comments(string username)
            : base(username)
        { dlc = new DL.DAComment(this.Entities); }

        internal Comments()
            : base(null)
        { dlc = new DL.DAComment(this.Entities); }


        public void AddComment(Common.Comment comment)
        {
            try
            {
                dlc.AddComment(comment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Comment> GetComments(Guid albumId)
        {
            try
            {
                return dlc.GetComments(albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Common.Comment GetComment(Guid commentId)
        {
            try
            {
                return dlc.GetComment(commentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteComment(Guid commentId)
        {
            try
            {
                dlc.DeleteComment(GetComment(commentId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FlagComment(Guid commentId)
        {
            try
            {
                Common.Comment c = GetComment(commentId);
                if (c != null)
                {
                    c.Flagged = true;
                    this.Entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
