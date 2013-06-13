using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace NicePhotos.User
{
    public partial class friends : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadFriends();
            }
        }

        private void LoadFriends()
        {
            try
            {
                repFriends.DataSource = new BL.Friends(Context.User.Identity.Name).GetUserFriends(Context.User.Identity.Name);
                repFriends.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to load friends list"), false);
            }
        }

        protected void btnAddFriend_Click(object sender, EventArgs e)
        {
            if (txtFriend.Text == "")
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Please enter a friend first"), false);
                return;
            }

            if (txtFriend.Text == Context.User.Identity.Name)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "You cannot add yourself as a friend"), false);
                return;
            }

            try
            {
                if (new BL.Friends(Context.User.Identity.Name).AddFriend(Context.User.Identity.Name, txtFriend.Text))
                {
                    LoadFriends();

                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                        "Friend added successfully"), false);
                }
                else
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Friend not added. Make sure the entered username is correct."), false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to add friend"), false);
            }
        }
    }
}