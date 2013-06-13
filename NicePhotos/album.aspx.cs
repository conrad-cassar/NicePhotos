using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Common;
using System.Drawing;
using System.IO;
using System.Net;

namespace NicePhotos
{
    public partial class album : System.Web.UI.Page
    {
        private Hashtable param = null;
        private Common.Album thisAlbum = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ParametersCheck();

            if (!Page.IsPostBack)
            {
                if (thisAlbum.Owner == Context.User.Identity.Name)
                {
                    UpdateFriendsList();
                }
                else
                {
                    divFriendsList.Visible = false;
                    divAddPhoto.Visible = false;
                }
                GetAlbumImages();
                LoadComments();

                if (thisAlbum != null)
                {
                    lblAlbumName.Text = thisAlbum.AlbumName;
                }
            }
        }

        public byte[] GetOriginalImage(Common.Image image)
        {
            try
            {
                if (image == null)
                    return null;

                Common.Account owner =
                    new BL.Accounts(Context.User.Identity.Name).GetUserAccount(image.Album1.Owner);

                //decrypt
                string encryptedImageText = File.ReadAllText(Server.MapPath("~/UserContent/" + 
                    image.Album.ToString() + "/orig/" + image.ImageId.ToString()));
                byte[] encryptedImageBytes = Convert.FromBase64String(encryptedImageText);
                byte[] decreptedImageBytes = Common.Utilities.HybridDecrypt(encryptedImageBytes, owner.PrivateKey);

                //verify + download
                string publicKey = File.OpenText(Server.MapPath("~/PublicKeys/" + owner.Username)).ReadToEnd();
                byte[] digitalSignature = Convert.FromBase64String(image.DigitalSignature);
                if (Common.Utilities.VerifyDigitalSignature(decreptedImageBytes, digitalSignature, publicKey))
                {
                    //return "data:image/" + image.FileExtension.Substring(1) +
                      //  ";base64," + Convert.ToBase64String(decreptedImageBytes);

                    return decreptedImageBytes;
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to download images file :["), false);
            }
            return null;
        }

        public string ConstructCommenter(object commenter)
        {
            if (commenter == null)
            {
                return "Guest";
            }
            return commenter.ToString();
        }

        private void UpdateFriendsList()
        {
            if (thisAlbum.Availability != 3)
            {
                divFriendsList.Visible = false;
                return;
            }

            lblFriendsError.Visible = false;
            cblFriendsToSee.Visible = true;
            try
            {
                cblFriendsToSee.DataSource = new BL.Friends(Context.User.Identity.Name).GetUserFriends(Context.User.Identity.Name);
                cblFriendsToSee.DataBind();

                foreach (ListItem item in cblFriendsToSee.Items)
                    item.Selected = new BL.Albums(Context.User.Identity.Name).UserCanSeeAlbum(item.Value, thisAlbum.AlbumId);
            }
            catch
            {
                cblFriendsToSee.Visible = false;
                lblFriendsError.Visible = true;
            }
        }

        private void LoadComments()
        {
            try
            {
                rptComments.DataSource = new BL.Comments(Context.User.Identity.Name).GetComments(thisAlbum.AlbumId);
                rptComments.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "I wasn't able to load the comments :[<br/>Please try again later!"), false);
            }
        }

        private void ParametersCheck()
        {
            if (thisAlbum == null)
            {
                string albumid = "";

                if (Request.QueryString["p"] != null)
                {
                    param = Common.Utilities.DecryptParameters(Request.QueryString["p"]);

                    if (param.ContainsKey("a"))
                    {
                        albumid = param["a"].ToString();
                        thisAlbum = new BL.Albums(Context.User.Identity.Name).GetAlbum(new Guid(albumid));

                        if (thisAlbum == null)
                        {
                            ScriptManager.RegisterStartupScript(this.Page,
                                this.GetType(), Guid.NewGuid().ToString(),
                                Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                                "Ooh.. I wasn't able to load this album :["), false);
                        }
                    }
                }
                else
                    Server.Transfer("~/default.aspx");
            }
        }

        protected void cblFriendsToSee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < cblFriendsToSee.Items.Count; i++)
                {
                    BL.Albums alb = new BL.Albums(Context.User.Identity.Name);

                    if (cblFriendsToSee.Items[i].Selected && !alb.UserCanSeeAlbum(cblFriendsToSee.Items[i].Value, thisAlbum.AlbumId))
                    {
                        //add to list
                        alb.AddToUserCanSeeAlbumList(cblFriendsToSee.Items[i].Value, thisAlbum.AlbumId);
                    }
                    else if (!cblFriendsToSee.Items[i].Selected && alb.UserCanSeeAlbum(cblFriendsToSee.Items[i].Value, thisAlbum.AlbumId))
                    {
                        //remove from list
                        alb.RemoveFromUserCanSeeAlbumList(cblFriendsToSee.Items[i].Value, thisAlbum.AlbumId);
                    }
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Ooh.. Something went wrong when I was trying to make the changes :["), false);
            }
        }

        private bool UploadImageChecks()
        {
            if (!fuImageUpload.HasFile)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "You must choose an image first!"), false);
                return false;
            }

            if (txtPhotoTitle.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "You must give a title to the image first!"), false);
                return false;
            }

            if (txtDesc.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "You must give a description to the image first!"), false);
                return false;
            }

            if (txtCredits.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "You must include the image value first!"), false);
                return false;
            }

            int credit = 0;
            if (!Int32.TryParse(txtCredits.Text, out credit))
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "The entered credit value is invalid!"), false);
                return false;
            }
            if (credit < 0)
            {
                ScriptManager.RegisterStartupScript(this.Page,
               this.GetType(), Guid.NewGuid().ToString(),
               Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
               "Credit field cannot be lower than zero!"), false);
                return false;
            }

            #region Image Validitity

            string ext = System.IO.Path.GetExtension(fuImageUpload.FileName).ToUpper();
            byte[] validHeader = Common.Utilities.ValidImageHeaders[ext.Substring(1)];
            byte[] headerToValidate = new byte[validHeader.Length];
            fuImageUpload.FileContent.Read(headerToValidate, 0, headerToValidate.Length);
            if (!Common.Utilities.ArraysEqual(headerToValidate, validHeader))
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "You are trying to upload an invalid file type!"), false);
                return false;
            }

            #endregion

            return true;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Common.Account me;
            try
            {
                 me = new BL.Accounts(Context.User.Identity.Name).GetUserAccount(Context.User.Identity.Name);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to work on your request :["), false);
                return;
            }
            if (me == null)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to work on your request :["), false);
                return;
            }

            #region Validation

            if (!UploadImageChecks())
                return;

            #endregion

            #region Image => Prepare, Manipulate + Encrypt, Save on HDD

            Common.Image imageRecord = new Common.Image();
            imageRecord.ImageId = Guid.NewGuid();

            System.Drawing.Image originalImage = 
                System.Drawing.Image.FromStream(fuImageUpload.PostedFile.InputStream);

            #region digital signing

            byte[] signitureBytes = Common.Utilities.GenerateDigitalSignature(fuImageUpload.FileBytes, me.PrivateKey);
            imageRecord.DigitalSignature = Convert.ToBase64String(signitureBytes);

            #endregion

            try
            {
                #region Determine water marked image size
                //original dimensions
                int width = originalImage.Width;
                int height = originalImage.Height;

                // Find the longest and shortest dimentions  
                int longestDimension = (width > height) ? width : height;
                int shortestDimension = (width < height) ? width : height;

                double factor = ((double)longestDimension) / (double)shortestDimension;

                // Set width as max  
                int wmImageWidth = 500;
                int wmImageHeight = Convert.ToInt32(500 / factor);

                //If height is actually greater, then we reset it to use height instead of width  
                if (width < height)
                {
                    wmImageWidth = Convert.ToInt32(500 / factor);
                    wmImageHeight = 500;
                }
                #endregion

                #region Determine thumbnail size
                // Set width as max  
                int thumbWidth = 150;
                int thumbHeight = Convert.ToInt32(150 / factor);

                //If height is actually greater, then we reset it to use height instead of width  
                if (width < height)
                {
                    thumbWidth = Convert.ToInt32(150 / factor);
                    thumbHeight = 150;
                }
                #endregion

                //create thumbnail
                Bitmap thumb = new Bitmap(originalImage);
                Graphics canvas = Graphics.FromImage(thumb);

                Bitmap tmp = new Bitmap(thumbWidth, thumbHeight);
                canvas = Graphics.FromImage(tmp);
                tmp.SetResolution(1, 1);
                canvas.DrawImage(thumb, new Rectangle(0, 0, tmp.Width, tmp.Height),
                    0, 0, thumb.Width, thumb.Height, GraphicsUnit.Pixel);
                thumb = tmp;

                thumb.Save(
                    System.Web.HttpContext.Current.Server.MapPath(
                        "~/UserContent/" + thisAlbum.AlbumId.ToString() + "/thumbs/") + imageRecord.ImageId.ToString()+".jpg",
                    System.Drawing.Imaging.ImageFormat.Jpeg);


                //create water marked image
                Bitmap wmImage = new Bitmap(originalImage);
                canvas = Graphics.FromImage(wmImage);

                tmp = new Bitmap(wmImageWidth, wmImageHeight);
                canvas = Graphics.FromImage(tmp);
                tmp.SetResolution(1, 1);
                canvas.DrawImage(wmImage, new Rectangle(0, 0, tmp.Width, tmp.Height),
                    0, 0, wmImage.Width, wmImage.Height, GraphicsUnit.Pixel);
                wmImage = tmp;

                canvas.DrawString("NicePhotos NicePhotos NicePhotos", new Font("Calibri", 50, FontStyle.Bold),
                    new SolidBrush(Color.WhiteSmoke), -100, wmImage.Height/2);

                wmImage.Save(
                    System.Web.HttpContext.Current.Server.MapPath(
                        "~/UserContent/" + thisAlbum.AlbumId.ToString() + "/") + imageRecord.ImageId.ToString() + ".jpg",
                    System.Drawing.Imaging.ImageFormat.Jpeg);


                //encrypt original image
                string userPublicKey = File.OpenText(Server.MapPath("~/PublicKeys/" + Context.User.Identity.Name)).ReadToEnd();
                byte[] encryptedImage = Common.Utilities.HypridEncrypt(fuImageUpload.FileBytes, userPublicKey);

                StreamWriter imageStreamWriter = 
                    File.CreateText(Server.MapPath("~/UserContent/" + 
                    "/" + this.thisAlbum.AlbumId.ToString() + 
                    "/orig/" + imageRecord.ImageId.ToString()));

                string output = Convert.ToBase64String(encryptedImage);
                imageStreamWriter.Write(output);
                imageStreamWriter.Close();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to save image :["), false);
                return;
            }


            #endregion

            #region Save image record to database

            try
            {
                imageRecord.Album = thisAlbum.AlbumId;
                imageRecord.ImageName = txtPhotoTitle.Text;
                imageRecord.ImageDescruption = txtDesc.Text;
                imageRecord.Cost = Int32.Parse(txtCredits.Text);
                imageRecord.Removed = false;
                imageRecord.FileExtension = System.IO.Path.GetExtension(fuImageUpload.FileName).ToUpper();
                new BL.Albums(Context.User.Identity.Name).CreateImageRecord(imageRecord);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to save image data :["), false);
                return;
            }

            #endregion

            //clear
            txtCredits.Text = "0";
            txtDesc.Text = "";
            txtPhotoTitle.Text = "";

            GetAlbumImages();
        }

        private void GetAlbumImages()
        {
            try
            {
                lsvImages.DataSource = new BL.Albums(Context.User.Identity.Name).GetAlbumImages(thisAlbum.AlbumId);
                lsvImages.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                   this.GetType(), Guid.NewGuid().ToString(),
                   Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                   "Unable to load images data :["), false);
            }
        }

        public string GetButtonNameByCost(string cost)
        {
            int cst = Int32.Parse(cost);
            if (cst <= 0)
                return "Download";
            else
                return "Buy Now";
        }

        public string GetPresentableCost(string cost)
        {
            int cst = Int32.Parse(cost);
            if (cst <= 0)
                return "Free";
            else
                return cost.ToString() + " credits";
        }

        protected void btnPostComment_Click(object sender, EventArgs e)
        {
            //validate
            if (txtComment.Text == "")
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "You cannot post an empty comment."), false);
                return;
            }
            else if (!CaptchaControl1.HumanityCheck(txtCaptcha.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Captcha code must match!"), false);
                return;
            }

            //save data
            Common.Comment comment = new Comment();
            comment.CommentId = Guid.NewGuid();

            if (Context.User.Identity.Name == "")
                comment.Username = null;
            else
                comment.Username = Context.User.Identity.Name;

            comment.Comment1 = txtComment.Text;
            comment.Time = DateTime.Now;
            comment.Album = thisAlbum.AlbumId;
            comment.Flagged = false;

            try
            {
                new BL.Comments(Context.User.Identity.Name).AddComment(comment);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "I wasn't able to add your comment :[<br/>Please try again later!"), false);
            }

            //clear fields
            txtComment.Text = "";
            txtCaptcha.Text = "";
            CaptchaControl1.Refresh();

            //load comments
            LoadComments();
        }

        protected void flagClick(object sender, EventArgs e)
        {
            try
            {
                new BL.Comments(Context.User.Identity.Name).FlagComment(new Guid(((ImageButton)sender).CommandArgument));
                LoadComments();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "I wasn't able to flag the comment :[<br/>Please try again later!"), false);
            }
        }

        protected void flagPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                Guid phId = new Guid(((ImageButton)sender).CommandArgument);
                new BL.Albums(Context.User.Identity.Name).AddImageReport(phId, Context.User.Identity.Name);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "I wasn't able to flag the image :[<br/>Please try again later!"), false);
            }
        }

        protected void buyPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                Guid phId = new Guid(((Button)sender).CommandArgument);

                if (string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    Response.Redirect("~/login.aspx");
                }

                string ipAddr = Request.ServerVariables["REMOTE_ADDR"].ToString();
                Common.Image img = new BL.Albums(Context.User.Identity.Name).BuyImage(phId, Context.User.Identity.Name, ipAddr);

                if (img != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                      this.GetType(), Guid.NewGuid().ToString(),
                      Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                      "Congratulations! You have bought an image, you can find it in you 'bought images' list."), false);

                    byte[] imageBytes = GetOriginalImage(img);
                    if (imageBytes == null)
                        return;
                    //Response.Write("<script type='text/javascript'>location.replace('" + image + "');</script>");
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=" + img.ImageName.Replace(' ', '_') + img.FileExtension.ToLower());
                    Response.ContentType = "image/" + img.FileExtension.Replace(".", "").ToUpper();
                    Response.BinaryWrite(imageBytes);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                      this.GetType(), Guid.NewGuid().ToString(),
                      Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                      "You have not enough credit to buy this image!<br/>Please buy more credits."), false);
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "Something went wrong :[<br/>Please try again later!"), false);
            }
        }
    }
}