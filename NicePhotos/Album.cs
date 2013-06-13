using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NicePhotos
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string Owner { get; set; }
        public string Availability { get; set; }
    }
}