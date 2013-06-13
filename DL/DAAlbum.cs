using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace DL
{
    public class DAAlbum : ConnectionClass
    {
        public DAAlbum()
            : base()
        { }

        public DAAlbum(Common.NicePhotosEntities entities)
            : base(entities)
        { }

        public IQueryable<Common.AvailabilityType> GetAvailabilityTypes()
        {
            return this.Entities.AvailabilityTypes;
        }

        public Guid CreateAlbum(string name, string username, int availabilityId)
        {
            List<Guid?> res = this.Entities.CreateAlbum(name, username, availabilityId).ToList();

            if (res == null)
                return new Guid();

            if (res.Count() < 1)
                return new Guid();

            if (res.ElementAt(0) == null)
                return new Guid();

            return new Guid(res.ElementAt(0).GetValueOrDefault().ToString());
        }

        public Common.Album GetAlbum(Guid albumId)
        {
            return this.Entities.Albums.Where(p => p.AlbumId == albumId).SingleOrDefault();
        }

        public IQueryable<Common.Album> GetAlbums()
        {
            return this.Entities.Albums;
        }

        public List<Common.Album> GetAlbums(string username)
        {
            List<Common.Album> res = 
                    (from alb in this.Entities.Albums
                    where (alb.Availability == 1 || alb.Availability == 3 || alb.Owner == username)
                    && alb.Removed == false
                    select alb).Distinct().ToList();

            List<Common.Album> ret = new List<Common.Album>();
            foreach (Common.Album alb in res)
            {
                if (alb.Availability == 3)
                {
                    if (UserCanSeeAlbum(username, alb.AlbumId))
                        ret.Add(alb);
                }
                else
                {
                    ret.Add(alb);
                }
            }

            return ret;
        }

        public IQueryable<Common.Image> GetAlbumImages(Guid albumId)
        {
            return this.Entities.Images.Where(p => p.Album == albumId && p.Removed == false);
        }

        public bool UserCanSeeAlbum(string username, Guid albumId)
        {
            if (GetAlbum(albumId).Owner == username)
                return true;

            return this.Entities.Album_Fiend.Where(p => p.FriendAccount == username && p.AlbumId == albumId).Count() > 0;
        }

        public void AddToUserCanSeeAlbumList(string username, Guid albumId)
        {
            Common.Album_Fiend af = new Common.Album_Fiend();
            af.AlbumFriendId = Guid.NewGuid();
            af.AlbumId = albumId;
            af.FriendAccount = username;

            this.Entities.Album_Fiend.AddObject(af);
            this.Entities.SaveChanges();
        }

        public void RemoveFromUserCanSeeAlbumList(string username, Guid albumId)
        {
            Common.Album_Fiend af = this.Entities.Album_Fiend.Where(p => p.AlbumId == albumId && p.FriendAccount == username).SingleOrDefault();
            
            if (af == null)
                throw new Exception("Unable to find record to delete");

            this.Entities.DeleteObject(af);
            this.Entities.SaveChanges();
        }

        public void CreateImageRecord(Common.Image image)
        {
            this.Entities.Images.AddObject(image);
            this.Entities.SaveChanges();
        }

        public IQueryable<Common.Album> GetUserAlbums(string owner)
        {
            return this.Entities.Albums.Where(p => p.Owner == owner);
        }

        public IQueryable<Common.Image> GetBoughtImages(string username)
        {
            return (from imgs in this.Entities.Images
                from ai in imgs.Account_Image
                where ai.Username == username
                select imgs);
        }

        public int GetTotalSpentAmount(string username)
        {
            List<int> sum = (from imgs in this.Entities.Images
                    from ai in imgs.Account_Image
                    where ai.Username == username
                    select imgs.Cost).ToList();

            if (sum.Count() < 1)
                return 0;
            else
                return Convert.ToInt32(sum.Sum());
        }

        public Common.Image GetImage(Guid imageId)
        {
            return this.Entities.Images.Where(p => p.ImageId == imageId).SingleOrDefault();
        }

        public void AddAccount_Image(Common.Account_Image ai)
        {
            this.Entities.Account_Image.AddObject(ai);
        }

        public void DeleteImage(Guid imageId)
        {
            Common.Image img = GetImage(imageId);

            if (imageId != null)
            {
                img.Removed = true;
            }
        }

        public List<Common.ReportNotificationData> ClearImageReports(Guid imageId)
        {
            IQueryable<Common.ImageReport> reports =
                (from imr in this.Entities.ImageReports
                 where imr.Image == imageId
                 select imr);
            List<Common.ReportNotificationData> returnData = 
                new List<Common.ReportNotificationData>();

            foreach (Common.ImageReport r in reports)
            {
                returnData.Add(new Common.ReportNotificationData(){
                        Username = r.Username,
                        EmailAddress = r.Account.Email,
                        ImageTitle = r.Image1.ImageName
                    });

                this.Entities.ImageReports.DeleteObject(r);
            }

            return returnData;
        }

        public IQueryable<Common.Image> GetReportedImages()
        {
            return (from img in this.Entities.Images
                    from imr in img.ImageReports
                    where img.Removed == false
                    select img).Distinct();
        }

        public void AddNiceCommision(Common.NiceCommision commision)
        {
            this.Entities.NiceCommisions.AddObject(commision);
            this.Entities.SaveChanges();
        }

        public List<Common.EarningsView> GetEarnings(string username)
        {
            List<Common.EarningsView> ret = new List<Common.EarningsView>();

            List<Common.Image> lst = (from ai in this.Entities.Account_Image
                                      from im in this.Entities.Images
                                      where im.ImageId == ai.ImageId
                                      && im.Album1.Owner == username
                                      select im).ToList();

            foreach (Common.Image i in lst)
            {
                int cost = 0;
                foreach(Common.Account_Image ai in i.Account_Image){
                    cost += ai.Cost;
                    foreach(Common.NiceCommision c in ai.NiceCommisions){
                        cost -= c.Commision;
                    }
                }

                ret.Add(
                    new Common.EarningsView()
                    {
                        ImageUrl = "UserContent/" + i.Album + "/thumbs/" + i.ImageId.ToString() + ".jpg",
                        EarnedCredits = cost
                    }
                );
            }

            return ret;
        }

        public void AddImageReport(Common.ImageReport report)
        {
            this.Entities.ImageReports.AddObject(report);
            this.Entities.SaveChanges();
        }
    }
}
