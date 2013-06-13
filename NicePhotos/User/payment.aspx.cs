using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PayPal;
using PayPal.Manager;
using PayPal.Api.Payments;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NicePhotos.User
{
    public partial class payment : System.Web.UI.Page
    {
        HttpContext CurrContext = HttpContext.Current;
        Payment pymnt = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            int credits = 0;
            if (!Int32.TryParse(txtCredits.Text, out credits))
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "The entered credit amount is invalid!<br/>Make the entered value is a numeric value which is greater than zero."), false);
                return;
            }
            if (credits < 1)
            {
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), Guid.NewGuid().ToString(),
                    Helpers.GetWebMessageScriptInTags(Helpers.MessageType.Error,
                    "The entered credit amount is invalid!<br/>Make the entered value is a numeric value which is greater than zero."), false);
                return;
            }

            /////////////////// Input Validation Done. /// Input = Valid /////////////////////////////////////

            // ## Creating Payment            
            {
                // ###Payer
                // A resource representing a Payer that funds a payment
                // Payment Method
                // as `paypal`
                Payer payr = new Payer();
                payr.payment_method = "paypal";
                Random rndm = new Random();
                var guid = Convert.ToString(rndm.Next(100000));

                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/user/default.aspx?p=" +
                    Common.Utilities.EncryptTextForParameters("am=" + credits) + "&";

                // # Redirect URLS
                RedirectUrls redirUrls = new RedirectUrls();
                redirUrls.cancel_url = baseURI + "cancel=ed&" + "guid=" + guid;
                redirUrls.return_url = baseURI + "guid=" + guid;

                // ###AmountDetails
                // Let's you specify details of a payment amount.
                AmountDetails amntDetails = new AmountDetails();
                amntDetails.subtotal = credits.ToString();

                // ###Amount
                // Let's you specify a payment amount.
                Amount amnt = new Amount();
                amnt.currency = "USD";
                // Total must be equal to sum of shipping, tax and subtotal.
                amnt.total = credits.ToString();
                amnt.details = amntDetails;

                // ###Transaction
                // A transaction defines the contract of a
                // payment - what is the payment for and who
                // is fulfilling it. Transaction is created with
                // a `Payee` and `Amount` types
                List<Transaction> transactionList = new List<Transaction>();
                Transaction tran = new Transaction();
                tran.description = "Nice Photos credit purchase.";
                tran.amount = amnt;
                // The Payment creation API requires a list of
                // Transaction; add the created `Transaction`
                // to a List
                transactionList.Add(tran);

                // ###Payment
                // A Payment Resource; create one using
                // the above types and intent as 'sale'
                pymnt = new Payment();
                pymnt.intent = "sale";
                pymnt.payer = payr;
                pymnt.transactions = transactionList;
                pymnt.redirect_urls = redirUrls;

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

                    // Create a payment by posting to the APIService
                    // using a valid AccessToken
                    // The return object contains the status;
                    Payment createdPayment = pymnt.Create(apiContext);

                    CurrContext.Items.Add("ResponseJson", JObject.Parse(createdPayment.ConvertToJson()).ToString(Formatting.Indented));

                    var links = createdPayment.links.GetEnumerator();

                    while (links.MoveNext())
                    {
                        Link lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            CurrContext.Items.Add("RedirectURL", lnk.href);
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                }
                catch (PayPal.Exception.PayPalException ex)
                {
                    CurrContext.Items.Add("Error", ex.Message);
                }
            }
            CurrContext.Items.Add("RequestJson", JObject.Parse(pymnt.ConvertToJson()).ToString(Formatting.Indented));
            //Server.Transfer("~/Response.aspx");

            Response.Redirect(CurrContext.Items["RedirectURL"].ToString());
        }
    }
}