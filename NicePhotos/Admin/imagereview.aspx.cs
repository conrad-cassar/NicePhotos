using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace NicePhotos.Admin
{
    public partial class _imagereview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadImagesToReview();
            }
        }

        private void LoadImagesToReview()
        {
            try
            {
                lsvReportedImages.DataSource = new BL.Albums(Context.User.Identity.Name).GetReportedImages();
                lsvReportedImages.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "I wasn't able to retrieve reported images :[<br/>Please try again later!"), false);
            }
        }

        protected void Approve_Click(object sender, EventArgs e)
        {
            try
            {
                Guid imgId = new Guid(((Button)sender).CommandArgument);
                new BL.Albums(Context.User.Identity.Name).ApproveImageReport(imgId);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "An error has occured :[<br/>Please try again later!"), false);
            }
            LoadImagesToReview();
        }

        protected void Disapprove_Click(object sender, EventArgs e)
        {
            try
            {
                Guid imgId = new Guid(((Button)sender).CommandArgument);
                new BL.Albums(Context.User.Identity.Name).DisapproveImageReport(imgId);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "An error has occured :[<br/>Please try again later!"), false);
            }
            LoadImagesToReview();
        }
    }
}