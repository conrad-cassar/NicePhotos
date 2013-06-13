using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace NicePhotos.Controls
{
    public partial class Login : System.Web.UI.UserControl
    {
        private Hashtable param = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CaptchaControl1 == null)
                CaptchaControl1 = new CaptchaControl();

            if (!Page.IsPostBack)
            {
                CaptchaControl1.ComplexityLevel = Complexity.High;
                CaptchaControl1.CharAmount = 3;
            }
            else
            {
                //txtNewPassword.Attributes.Add("value", txtNewPassword.Text);
                //txtConfPassword.Attributes.Add("value", txtConfPassword.Text);
            }

            ParametersCheck();
        }

        private void ParametersCheck()
        {
            string username = "";
            string verificationCode = "";

            if (Request.QueryString["p"] != null)
            {
                param = Common.Utilities.DecryptParameters(Request.QueryString["p"]);

                if (param.ContainsKey("u"))
                {
                    username = param["u"].ToString();
                }

                if (param.ContainsKey("c"))
                {
                    verificationCode = param["c"].ToString();
                }

                if (param.ContainsKey("resetp"))
                {
                    if(!Page.IsPostBack){
                        ResetPassMode();
                        txtUser.Text = username;

                        ScriptManager.RegisterStartupScript(this.Page,
                            this.GetType(), Guid.NewGuid().ToString(),
                            Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                            "You can now enter a new password."), false);
                    }
                }
                else if (username != "" && verificationCode != "" && !Page.IsPostBack)
                {
                    try
                    {
                        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                        string privateKey = RSA.ToXmlString(true);
                        string publicKey = RSA.ToXmlString(false);

                        if (!Directory.Exists(Server.MapPath("~/PublicKeys")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/PublicKeys"));
                        }

                        StreamWriter publicKeyFile = File.CreateText(Server.MapPath("~/PublicKeys/" + username));
                        publicKeyFile.Write(publicKey);
                        publicKeyFile.Close();

                        new BL.Accounts(Context.User.Identity.Name).SetAccountAsVerified(username, verificationCode, privateKey);

                        ScriptManager.RegisterStartupScript(this.Page,
                            this.GetType(), Guid.NewGuid().ToString(),
                            Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                            "You can now log in!"), false);

                        txtUser.Text = username;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.Page,
                            this.GetType(), Guid.NewGuid().ToString(),
                            Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                            ex.Message), false);
                    }
                }
            }

            if (param == null || !param.ContainsKey("resetp"))
            {
                LoginMode();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewUsername.Text == "" || txtNewEmail.Text == "" || txtNewPassword.Text == "" || txtConfirmPassword.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Make sure you fill all fields"), false);
                    return;
                }

                if (!Regex.IsMatch(txtNewPassword.Text, @"^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?!.*s).*$"))
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Invalid Password.<br/>"
                        + "Make sure the entered password is 6 characters or longer, and "
                        + "it containts numerinc digitas, upper case and lower case characters."
                        + "<br/>This is necessary to ensure you a higher level of security!"), false);
                    return;
                }

                if (txtNewPassword.Text.Equals(txtConfirmPassword.Text))
                {
                    //create user account object
                    Common.Account ua = new Common.Account();
                    ua.Username = txtNewUsername.Text;
                    ua.Email = txtNewEmail.Text;
                    ua.Password = txtNewPassword.Text;
                    ua.Name = "";
                    ua.Surname = "";

                    //proceed
                    new BL.Accounts(Context.User.Identity.Name).Register(ua);

                    ClearAll();

                    //message
                    ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                        "Congratulations Buddy! <br/> Please check your inbox for instructions to activate your account!"), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Password must match with the Confirm Password field"), false);
                }

            }
            catch (FormatException ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Unable to process registration, Make sure you filled the registration form correctly!"), false);
            }
            catch (Common.Exceptions.UserAlreadyExistException ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        ex.Message), false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Unable to register you! :["), false);
            }
        }

        private void ClearAll()
        {
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtNewEmail.Text = "";
            txtNewUsername.Text = "";
            txtUser.Text = "";
            txtPassword.Text = "";
            txtCaptchaInput.Text = "";
        }

        private void RefreshCaptcha()
        {
            txtCaptchaInput.Text = "";
            CaptchaControl1.Refresh();
        }

        protected void btnResetPass_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtPassword.Text, @"^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?!.*s).*$"))
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Invalid Password.<br/>"
                    + "Make sure the entered password is 6 characters or longer, and "
                    + "it containts numerinc digitas, upper case and lower case characters."
                    + "<br/>This is necessary to ensure you a higher level of security!"), false);
                return;
            }

            if (param.ContainsKey("resetp") && (param.ContainsKey("u")))
            {
                new BL.Accounts(Context.User.Identity.Name).ChangePassword(param["u"].ToString(), txtPassword.Text);
                LoginMode();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUser.Text == "" || txtPassword.Text == "" || txtCaptchaInput.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        "Make sure you fill all fields"), false);
                    return;
                }

                if (CaptchaControl1.HumanityCheck(txtCaptchaInput.Text))
                {
                    new BL.Accounts(Context.User.Identity.Name).LogIn(txtUser.Text, txtPassword.Text);
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(txtUser.Text, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "The entered captch code is invalid!"), false);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                        this.GetType(), Guid.NewGuid().ToString(),
                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                        ex.Message), false);
            }
        }

        protected void lnkForgot_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "The username field is empty!<br/>In order to reset your password we need to know your username."), false);
                return;
            }

            if (new BL.Accounts(Context.User.Identity.Name).IsUsernameAvailable(txtUser.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "The entered username does not exist."), false);
                return;
            }

            try
            {
                new BL.Accounts(Context.User.Identity.Name).SendForgotPasswordRequest(txtUser.Text);

                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Please check your email inbox and follow the provided link."), false);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this.Page,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Uable to send request!"), false);
            }
        }

        private void ResetPassMode()
        {
            CaptchaControl1.Visible = false;
            txtUser.Enabled = false;
            txtPassword.Visible = true;
            registerBox.Visible = false;
            lnkForgot.Visible = false;
            txtCaptchaInput.Visible = false;

            btnResetPass.Visible = true;
            btnLogin.Visible = false;

            lblModeHeader.Text = "Enter New Password";
        }
        private void LoginMode()
        {
            CaptchaControl1.Visible = true;
            txtUser.Enabled = true;
            txtPassword.Visible = true;
            registerBox.Visible = true;
            lnkForgot.Visible = true;
            txtCaptchaInput.Visible = true;

            btnResetPass.Visible = false;
            btnLogin.Visible = true;

            lblModeHeader.Text = "Log In";
        }
    }
}