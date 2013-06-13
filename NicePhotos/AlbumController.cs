using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NicePhotos
{
    public class AlbumController : ApiController
    {
        
        // GET api/<controller>
        public IEnumerable<Album> GetAllAlbums()
        {
            string username = "";
            string password = "";
            Guid apikey = new Guid();
            System.Net.Http.Headers.HttpRequestHeaders headers = base.Request.Headers;
            BL.Accounts acc = new BL.Accounts("");
            foreach (var item in headers)
            {
                if (item.Key == "API-Key")
                {
                    foreach (string key in item.Value)
                    {
                        if (acc.GetAccountByApiKey(new Guid(key)) != null)
                        {
                            var ret = GetAlbums();
                            if (ret == null)
                                throw new HttpResponseException(HttpStatusCode.NotFound);

                            return ret;
                        }
                    }
                }
                else if (item.Key == "Username")
                {
                    foreach (string un in item.Value)
                        username = un;
                }
                else if (item.Key == "Password")
                {
                    foreach (string ps in item.Value)
                        password = ps;
                }

            }
            return null;
        }
        

        // GET api/<controller>/5
        public IEnumerable<Album> GetUserAvailableAlbums(string username)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = base.Request.Headers;
            BL.Accounts acc = new BL.Accounts("");
            foreach (var item in headers)
            {
                if (item.Key == "API-Key")
                {
                    foreach (string key in item.Value)
                    {
                        if (acc.GetAccountByApiKey(new Guid(key)) != null)
                        {
                            var ret = GetAvailableAlbums(username);
                            if (ret == null)
                                throw new HttpResponseException(HttpStatusCode.NotFound);

                            return ret;
                        }
                    }
                }
            }
            return null;
        }


        private IEnumerable<Album> GetAlbums()
        {
            IQueryable<Common.Album> originalList = new BL.Albums("").GetAlbums();
            if (originalList == null)
                return null;

            List<Album> lst = new List<Album>();

            foreach (Common.Album a in originalList)
            {
                lst.Add(
                    new Album
                    {
                        AlbumId = a.AlbumId,
                        AlbumName = a.AlbumName,
                        Owner = a.Owner,
                        Availability = a.AvailabilityType.TypeName
                    });
            }

            return lst;
        }
        private IEnumerable<Album> GetAvailableAlbums(string username)
        {
            List<Common.Album> albums = new BL.Albums("").GetAlbums(username);
            if (username == null)
                return null;

            List<Album> lst = new List<Album>();

            foreach (Common.Album a in albums)
            {
                lst.Add(
                    new Album
                    {
                        AlbumId = a.AlbumId,
                        AlbumName = a.AlbumName,
                        Owner = a.Owner,
                        Availability = a.AvailabilityType.TypeName
                    });
            }

            return lst;
        }

        
    }
}