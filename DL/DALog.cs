using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL
{
    public class DALog : ConnectionClass
    {
        public DALog()
            : base()
        { }

        public DALog(Common.NicePhotosEntities entities)
            : base(entities)
        { }

        public void AddLog(Common.Log log)
        {
            this.Entities.Logs.AddObject(log);
            this.Entities.SaveChanges();
        }
    }
}
