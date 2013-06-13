using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApiClient
{
    public partial class Default : System.Web.UI.Page
    {
        HttpResponseMessage responseAll;
        HttpResponseMessage responseFiltered;
        HttpClient client;

        protected void Page_Load(object sender, EventArgs e)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:2381/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("API-Key", "6be69d33-13f1-4508-80d3-cec47dc732e2");

            responseAll = client.GetAsync("api/Album").Result;

            if (responseAll.IsSuccessStatusCode)
            {
                try
                {
                    grvAll.DataSource = responseAll.Content.ReadAsAsync<IEnumerable<NicePhotos.Album>>().Result;
                    grvAll.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                grvFiltered.DataSource = null;
                grvFiltered.DataBind();
                Response.Write("Username text field cannot be left empty!");
                return;
            }

            responseFiltered = client.GetAsync("api/Album/" + txtUsername.Text).Result;

            try
            {
                if (responseFiltered.IsSuccessStatusCode)
                {
                    grvFiltered.DataSource = responseFiltered.Content.ReadAsAsync<IEnumerable<NicePhotos.Album>>().Result;
                    grvFiltered.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}