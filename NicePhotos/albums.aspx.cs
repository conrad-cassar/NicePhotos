using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace NicePhotos
{
    public partial class albums : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    ddlAvailabilityType.DataSource = new BL.Albums(Context.User.Identity.Name).GetAvailabilityType();
                    ddlAvailabilityType.DataBind();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to load some necessary data!<br/>This page may not function well."), false);
                }

                addAlbum.Visible = (Context.User.Identity.IsAuthenticated);                
            }
            LoadAlbums();
        }

        private void Clear()
        {
            try
            {
                txtAlbumName.Text = "";
                ddlAvailabilityType.SelectedIndex = 0;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Some functions on this page may not work well."), false);
            }
        }

        private void LoadAlbums()
        {
            try
            {
                lsvAlbums.DataSource = new BL.Albums(Context.User.Identity.Name).GetAlbums(Context.User.Identity.Name);
                lsvAlbums.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to load albums."), false);
            }
        }

        protected void btnCreateAlbum_Click(object sender, EventArgs e)
        {
            if (txtAlbumName.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Please give a name to the album first"), false);
                return;
            }

            if (ddlAvailabilityType.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Please choose the an Availability option for your album"), false);
                return;
            }

            try
            {
                new BL.Albums(Context.User.Identity.Name).CreateAlbum(txtAlbumName.Text, Context.User.Identity.Name, Int32.Parse(ddlAvailabilityType.SelectedValue));
                Clear();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to create album"), false);
            }
        }
    }
}