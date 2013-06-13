using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace NicePhotos
{
    class MenuItemToDisp
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                lblCurrentUser.Text = Context.User.Identity.Name;
                btnLogInOrOut.Text = "Log Out";
                btnLogInOrOut.Click += btnLogOut_Click;
            }
            else
            {
                lblCurrentUser.Text = "";
                btnLogInOrOut.Text = "Log In or Register";
                btnLogInOrOut.Click += btnLogIn_Click;
            }

            try
            {
                List<MenuItemToDisp> lst = new List<MenuItemToDisp>();

                foreach (Common.MenuItem m in new BL.Roles(Context.User.Identity.Name).GetMenuItems(Context.User.Identity.Name))
                {
                    lst.Add(new MenuItemToDisp{ Name = m.ItemName, Path = m.ItemLink });
                }

                repMenu.DataSource = lst;
                repMenu.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Web page malfunction, Unable to load menu!"), false);
            }
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }
    }
}
