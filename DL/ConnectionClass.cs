using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DL
{
    public class ConnectionClass
    {
        public NicePhotosEntities Entities { get; set; }
        public System.Data.IDbTransaction Transaction { get; set; }

        public ConnectionClass()
        {
            this.Entities = new NicePhotosEntities();
        }

        public ConnectionClass(NicePhotosEntities _Entities)
        {
            this.Entities = _Entities;
        }
    }
}
