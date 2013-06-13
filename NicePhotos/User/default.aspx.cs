using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal;
using PayPal.Api.Payments;
using PayPal.Manager;

namespace NicePhotos.User
{
    public partial class _default : System.Web.UI.Page
    {
        Common.Account acc;

        //PayPal related
        HttpContext CurrContext = HttpContext.Current;
        Payment pymnt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UpdateUserInfo();
                DisplayUserInfo();
                LoadMyAlbums();
                LoadBoughtImages();
                LoadEarnings();


                #region PayPal Related Code

                // ## ExecutePayment
                if (Request.Params["PayerID"] != null)
                {
                    if (Request.Params["cancel"] != null)
                    {
                        ScriptManager.RegisterStartupScript(this,
                            this.GetType(), Guid.NewGuid().ToString(),
                            Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                            "Process has been canceled"), false);
                        return;
                    }

                    pymnt = new Payment();
                    if (Request.Params["guid"] != null)
                    {
                        pymnt.id = (string)Session[Request.Params["guid"]];

                    }
                    try
                    {
                        // ###AccessToken
                        // Retrieve the access token from
                        // OAuthTokenCredential by passing in
                        // ClientID and ClientSecret
                        // It is not mandatory to generate Access Token on a per call basis.
                        // Typically the access token can be generated once and
                        // reused within the expiry window
                        string accessToken = new OAuthTokenCredential(ConfigManager.Instance.GetProperties()["ClientID"], ConfigManager.Instance.GetProperties()["ClientSecret"]).GetAccessToken();

                        // ### Api Context
                        // Pass in a `ApiContext` object to authenticate 
                        // the call and to send a unique request id 
                        // (that ensures idempotency). The SDK generates
                        // a request id if you do not pass one explicitly. 
                        APIContext apiContext = new APIContext(accessToken);
                        // Use this variant if you want to pass in a request id  
                        // that is meaningful in your application, ideally 
                        // a order id.
                        // String requestId = Long.toString(System.nanoTime();
                        // APIContext apiContext = new APIContext(accessToken, requestId ));
                        PaymentExecution pymntExecution = new PaymentExecution();
                        pymntExecution.payer_id = Request.Params["PayerID"];

                        Hashtable param = new Hashtable();
                        int credit = 0;
                        if (Request.QueryString["p"] != null)
                        {
                            param = Common.Utilities.DecryptParameters(Request.QueryString["p"].ToString());

                            if (param.ContainsKey("am"))
                            {
                                string s = param["am"].ToString();
                                if (!Int32.TryParse(s, out credit))
                                {
                                    ScriptManager.RegisterStartupScript(this,
                                        this.GetType(), Guid.NewGuid().ToString(),
                                        Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                                        "An error has occured while processing the credit purchase :["), false);
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                        
                        Payment executedPayment = pymnt.Execute(apiContext, pymntExecution);
                        
                        
                        CurrContext.Items.Add("ResponseJson", JObject.Parse(executedPayment.ConvertToJson()).ToString(Formatting.Indented));

                        if (executedPayment.state == "approved")
                        {
                            new BL.Accounts(Context.User.Identity.Name).AddCredit(Context.User.Identity.Name, credit);
                        }

                        UpdateUserInfo();
                        DisplayUserInfo();
                    }
                    catch (PayPal.Exception.PayPalException ex)
                    {
                        CurrContext.Items.Add("Error", ex.Message);
                        ScriptManager.RegisterStartupScript(this,
                            this.GetType(), Guid.NewGuid().ToString(),
                            Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                            "An error has occured while processing the credit purchase :["), false);
                    }
                }

                #endregion
            }
        }

        private void LoadEarnings()
        {
            try
            {
                lsvEarnings.DataSource = new BL.Albums(Context.User.Identity.Name).GetEarnings(Context.User.Identity.Name);
                lsvEarnings.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Unable to load earnings list :["), false);
            }
        }

        private void LoadBoughtImages()
        {
            try
            {
                lsvBought.DataSource = new BL.Albums(Context.User.Identity.Name).GetBoughtImages(Context.User.Identity.Name);
                lsvBought.DataBind();

                int tot = 0;
                tot = new BL.Albums(Context.User.Identity.Name).GetTotalSpentAmount(Context.User.Identity.Name);
                lblTotalSpent.Text = tot.ToString();

                if (tot < 1)
                    divTotalBought.Visible = false;
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Unable to load your bought images :["), false);
            }
        }

        private void LoadMyAlbums()
        {
            try
            {
                lsvAlbums.DataSource = new BL.Albums(Context.User.Identity.Name).GetUserAlbums(Context.User.Identity.Name);
                lsvAlbums.DataBind();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Unable to load all your data :["), false);
            }
        }

        private void UpdateUserInfo()
        {
            lblUsername.Text = Context.User.Identity.Name;
            acc = new BL.Accounts(Context.User.Identity.Name).GetUserAccount(Context.User.Identity.Name);
        }

        private void DisplayUserInfo()
        {
            if (acc == null)
                return;

            txtName.Text = acc.Name;
            txtSurname.Text = acc.Surname;
            txtMe.Text = acc.AboutMe;
            lblEmail.Text = acc.Email;

            lblCurrentBalance.Text = acc.Credits.ToString();
        }

        protected void btnSaveDetails_Click(object sender, EventArgs e)
        {
            try
            {
                new BL.Accounts(Context.User.Identity.Name).UpdateUserData(
                    Context.User.Identity.Name, 
                    lblEmail.Text, 
                    txtName.Text, 
                    txtSurname.Text, 
                    txtMe.Text);

                UpdateUserInfo();
                DisplayUserInfo();

                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Your data has been updated."), false);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "An error has occured while updating your profile."), false);
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text == "" || txtOldPass.Text == "" || txtConfPass.Text == "")
                    throw new Exception("Some fields are empty");

                if (txtNewPass.Text != txtConfPass.Text)
                    throw new Exception("New Password and Confirm Password fields does not match");

                new BL.Accounts(Context.User.Identity.Name).ChangePassword(Context.User.Identity.Name, txtOldPass.Text, txtNewPass.Text);

                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Information,
                    "Password has been changed successfully!"), false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    ex.Message), false);
            }
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            try
            {
                Guid id = new Guid(((Button)sender).CommandArgument);
                Common.Image img = new BL.Albums(Context.User.Identity.Name).GetImage(id);
                byte[] imageBytes = new album().GetOriginalImage(img);
                if (imageBytes == null)
                    return;
                //Response.Write("<script type='text/javascript'>location.replace('" + image + "');</script>");
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + img.ImageName.Replace(' ', '_') + img.FileExtension.ToLower());
                Response.ContentType = "image/" + img.FileExtension.Replace(".", "").ToUpper();
                Response.BinaryWrite(imageBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "Unable to download image :["), false);
            }
        }
    }
}