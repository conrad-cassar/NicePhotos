using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL
{
    public class DAComment : ConnectionClass
    {
        public DAComment()
            : base()
        { }

        public DAComment(Common.NicePhotosEntities entities)
            : base(entities)
        { }

        public void AddComment(Common.Comment comment)
        {
            this.Entities.Comments.AddObject(comment);
            this.Entities.SaveChanges();
        }

        public IQueryable<Common.Comment> GetComments(Guid albumId)
        {
            return this.Entities.Comments.Where(p => p.Album == albumId && p.Flagged == false).OrderByDescending(p => p.Time);
        }

        public Common.Comment GetComment(Guid commentId)
        {
            return this.Entities.Comments.Where(p => p.CommentId == commentId).SingleOrDefault();
        }

        public void DeleteComment(Common.Comment comment)
        {
            this.Entities.Comments.DeleteObject(comment);
            this.Entities.SaveChanges();
        }
    }
}
