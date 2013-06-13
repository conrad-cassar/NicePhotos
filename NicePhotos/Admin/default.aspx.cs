using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace NicePhotos.Admin
{
    public partial class _default : System.Web.UI.Page
    {
        string selectedUsername = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadRolesList();
                LoadMenuList();
            }

            if (Session["selectedUsername"] != null)
                selectedUsername = Session["selectedUsername"].ToString();
        }

        private void LoadRolesList()
        {
            try
            {
                ddlRoles.DataSource = new BL.Roles(Context.User.Identity.Name).GetRoles();
                ddlRoles.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "Unable to load roles list!"), false);
            }
        }

        private void LoadMenuList()
        {
            try
            {
                cblMenuItem.DataSource = new BL.Roles(Context.User.Identity.Name).GetMenuItems();
                cblMenuItem.DataBind();

                foreach (ListItem item in cblMenuItem.Items)
                    item.Selected = new BL.Roles(Context.User.Identity.Name).RoleHasMenu(
                        ddlRoles.SelectedValue, 
                        item.Value);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                  this.GetType(), Guid.NewGuid().ToString(),
                  Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                  "Unable to load menu list for role allocation!"), false);
            }
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMenuList();
        }

        protected void cblMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < cblMenuItem.Items.Count; i++)
                {
                    BL.Roles rl = new BL.Roles(Context.User.Identity.Name);

                    if (cblMenuItem.Items[i].Selected && !rl.RoleHasMenu(ddlRoles.SelectedValue, cblMenuItem.Items[i].Value))
                    {
                        //add to list
                        rl.AddToRoleMenuItem(ddlRoles.SelectedValue, cblMenuItem.Items[i].Value);
                    }
                    else if (!cblMenuItem.Items[i].Selected && rl.RoleHasMenu(ddlRoles.SelectedValue, cblMenuItem.Items[i].Value))
                    {
                        //remove from list
                        rl.RemoveRoleMenuItem(ddlRoles.SelectedValue, cblMenuItem.Items[i].Value);
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

        public void LoadUserRoles()
        {
            try
            {
                BL.Roles rl = new BL.Roles(Context.User.Identity.Name);
                cblRoles.DataSource = rl.GetRoles();
                cblRoles.DataBind();

                foreach (ListItem item in cblRoles.Items)
                    item.Selected = rl.IsUserInRole(selectedUsername, item.Value);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Ooh.. Something went wrong when I was trying to load user roles :["), false);
            }
        }

        protected void btnDoFind_Click(object sender, EventArgs e)
        {
            try
            {
                lblUsername.Text = "";
                cblRoles.Visible = false;
                Session["selectedUsername"] = new BL.Accounts(Context.User.Identity.Name).GetUserAccount(txtUserToMod.Text).Username;
                selectedUsername = Session["selectedUsername"].ToString();

                if (selectedUsername == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "The entered username does not exist"), false);
                    return;
                }

                cblRoles.Visible = true;
                lblUsername.Text = "Selected User: " + selectedUsername;
                LoadUserRoles();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Ooh.. Something went wrong when I was trying to make the changes :["), false);
            }
            txtUserToMod.Text = "";
        }

        protected void cblRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BL.Roles rl = new BL.Roles(Context.User.Identity.Name);
                for (int i = 0; i < cblRoles.Items.Count; i++)
                {
                    if (cblRoles.Items[i].Selected && !rl.IsUserInRole(selectedUsername, cblRoles.Items[i].Value))
                    {
                        //add to list
                        rl.AddUserRole(selectedUsername, cblRoles.Items[i].Value);
                    }
                    else if (!cblRoles.Items[i].Selected && rl.IsUserInRole(selectedUsername, cblRoles.Items[i].Value))
                    {
                        //remove from list
                        rl.RemoveUserRole(selectedUsername, cblRoles.Items[i].Value);
                    }
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Ooh.. Something went wrong when I was trying to make the changes :["), false);
            }
        }
    }
}