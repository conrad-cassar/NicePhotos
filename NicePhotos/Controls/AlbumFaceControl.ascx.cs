using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Common;

namespace NicePhotos.Controls
{
    public partial class AlbumFaceControl : System.Web.UI.UserControl
    {
        private string albumId = "";
        private string owner = "";
        private Common.Album thisAlbum;
        private string imageDirectory = "";

        public string AlbumId
        {
            get { return albumId; }
            set { albumId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                owner = new BL.Albums(Context.User.Identity.Name).GetAlbumOwner(AlbumId);
                imbDelete.Visible = owner == Context.User.Identity.Name;
                thisAlbum = new BL.Albums(Context.User.Identity.Name).GetAlbum(AlbumId);
                lblTitle.Text = TitleCut(thisAlbum.AlbumName);
                imbDelete.CommandArgument = AlbumId;

                imageDirectory = ConfigurationManager.AppSettings["userImagesCommonPath"]
                        + "/" + AlbumId;

                IQueryable<Common.Image> photos = new BL.Albums(
                    Context.User.Identity.Name).GetAlbumImages(thisAlbum.AlbumId).Take(3);

                if (photos != null)
                {
                    int count = 1;
                    foreach (Common.Image p in photos)
                    {
                        switch (count)
                        {
                            case 1:
                                imgOne.ImageUrl = imageDirectory + "/thumbs/" + p.ImageId.ToString() + ".jpg";
                                break;
                            case 2:
                                imgTwo.ImageUrl = imageDirectory + "/thumbs/" + p.ImageId.ToString() + ".jpg";
                                break;
                            case 3:
                                imgThree.ImageUrl = imageDirectory + "/thumbs/" + p.ImageId.ToString() + ".jpg";                                break;
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to load album"), false);
                this.Visible = false;
            }            
        }

        private string TitleCut(string title)
        {
            if (title.Count() <= 16)
            {
                return title;
            }
            else
            {
                title = title.Remove(16);
                title += "...";
                return title;
            }
        }

        protected void imbDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imb = (ImageButton)sender;

                if (AlbumId == "")
                {
                    AlbumId = imb.CommandArgument;
                }

                Refresh();
                if (AlbumId != "")
                {
                    string dir = Server.MapPath(imageDirectory);
                    new BL.Albums(Context.User.Identity.Name).DeleteAlbum(AlbumId);
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                     this.GetType(), Guid.NewGuid().ToString(),
                     Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                     "Unable to remove album!"), false);
            }
        }
    }
}