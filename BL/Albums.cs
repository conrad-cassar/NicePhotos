using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BL
{
    public class Albums : BusinessBase
    {
        private DL.DAAlbum dla;

        public Albums(string username)
            : base(username)
        { dla = new DL.DAAlbum(this.Entities); }

        internal Albums()
            : base(null)
        { dla = new DL.DAAlbum(this.Entities); }


        public IQueryable<Common.AvailabilityType> GetAvailabilityType()
        {
            return dla.GetAvailabilityTypes();
        }

        public Guid CreateAlbum(string albumName, string username, int availabilityId)
        {
            try
            {
                Guid id = dla.CreateAlbum(albumName, username, availabilityId);

                if (id != Guid.Empty)
                {
                    //create folder for users personal photos
                    string path = HttpContext.Current.Server.MapPath("~/UserContent/" + id.ToString());
                    System.IO.Directory.CreateDirectory(path);
                    System.IO.Directory.CreateDirectory(path + "/orig");
                    System.IO.Directory.CreateDirectory(path + "/thumbs");
                }

                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Common.Album GetAlbum(string albumId)
        {
            try
            {
                return GetAlbum(new Guid(albumId));
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve album");
            }
        }

        public  Common.Album GetAlbum(Guid albumId)
        {
            try
            {
                return dla.GetAlbum(albumId);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve album");
            }
        }

        public IQueryable<Common.Album> GetAlbums()
        {
            try
            {
                return dla.GetAlbums();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteAlbum(string albumId)
        {
            try
            {
                Common.Album alb = GetAlbum(albumId);

                if (alb == null)
                    throw new Exception();

                alb.Removed = true;
                this.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to remove album");
            }
        }

        public List<Common.Album> GetAlbums(string username)
        {
            try
            {
                return dla.GetAlbums(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAlbumOwner(string albumId)
        {
            try
            {
                Common.Album alb = GetAlbum(albumId);

                if (alb == null)
                    return null;

                return alb.Owner;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Image> GetAlbumImages(Guid albumId)
        {
            try
            {
                return dla.GetAlbumImages(albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UserCanSeeAlbum(string username, Guid albumId)
        {
            try
            {
                return dla.UserCanSeeAlbum(username, albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddToUserCanSeeAlbumList(string username, Guid albumId)
        {
            try
            {
                dla.AddToUserCanSeeAlbumList(username, albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveFromUserCanSeeAlbumList(string username, Guid albumId)
        {
            try
            {
                dla.RemoveFromUserCanSeeAlbumList(username, albumId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateImageRecord(Common.Image image)
        {
            try
            {
                dla.CreateImageRecord(image);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Album> GetUserAlbums(string owner)
        {
            try
            {
                return dla.GetUserAlbums(owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ApproveImageReport(Guid imageId)
        {
            try
            {
                dla.DeleteImage(imageId);
                dla.ClearImageReports(imageId);
                this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DisapproveImageReport(Guid imageId)
        {
            try
            {
                List<Common.ReportNotificationData> reports = dla.ClearImageReports(imageId);

                foreach (Common.ReportNotificationData r in reports)
                {
                    string body =
                        "Dear " + r.Username + ",<br/><br/>The report you submitted regarding the image entitled '";
                    body += r.ImageTitle + "' has not been accepted.<br/><br/>Best Regards,<br/>NicePhotos Team";
                    this.Entities.SendEmail(r.EmailAddress,
                        "NicePhotos Ltd. - Notification about a reported image",
                        body);
                }
                this.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Image> GetReportedImages()
        {
            try
            {
                return dla.GetReportedImages();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Common.Image> GetBoughtImages(string username)
        {
            try
            {
                return dla.GetBoughtImages(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTotalSpentAmount(string username)
        {
            try
            {
                return dla.GetTotalSpentAmount(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Common.Image GetImage(Guid imageId)
        {
            try
            {
                return dla.GetImage(imageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddImageReport(Guid imageId, string username)
        {
            try
            {
                Common.ImageReport report = new Common.ImageReport();
                report.ImageReportId = Guid.NewGuid();
                report.Username = username;
                report.Image = imageId;

                dla.AddImageReport(report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Common.Image BuyImage(Guid imageId, string username, string userIpAddress)
        {
            try
            {
                Common.Image img = GetImage(imageId);

                if (img == null)
                    throw new Exception("Image does not exist");

                if (img.Cost < 1)
                    return img;

                #region Log Purchase Request

                string logDetails = "[PURCASE REQUEST] || IpAddress:" + userIpAddress + " || Username:" + username + " || ImageId:" + imageId.ToString();
                new Logs().AddLog(logDetails, null);

                #endregion

                Common.Account usr = new DL.DAAccount(this.Entities).GetUserAccount(username);
                Common.Account owner = new DL.DAAccount(this.Entities).GetUserAccount(img.Album1.Owner);

                if (usr == null)
                    throw new Exception("User account does not exist");

                if(img.Cost > usr.Credits)
                    return null;


                //calculate commision
                int commission = img.Cost;
                commission = Convert.ToInt32(Math.Ceiling((double)img.Cost / 5));
                int earning = img.Cost - commission;

                owner.Credits += earning;

                usr.Credits = usr.Credits - img.Cost;
                Common.Account_Image ai = new Common.Account_Image();
                ai.AccountImageId = Guid.NewGuid();
                ai.Cost = img.Cost;
                ai.Username = usr.Username;
                ai.ImageId = img.ImageId;
                dla.AddAccount_Image(ai);

                Common.NiceCommision niceCommision = new Common.NiceCommision();
                niceCommision.NiceCommisionId = Guid.NewGuid();
                niceCommision.Commision = commission;
                niceCommision.BuyerRecord = ai.AccountImageId;
                dla.AddNiceCommision(niceCommision);

                this.Entities.SaveChanges();
                return img;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Common.EarningsView> GetEarnings(string username)
        {
            try
            {
                return dla.GetEarnings(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
