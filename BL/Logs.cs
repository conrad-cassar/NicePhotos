using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class Logs : BusinessBase
    {
        private DL.DALog dll;

        public Logs(string username)
            : base(username)
        { dll = new DL.DALog(this.Entities); }

        internal Logs()
            : base(null)
        { dll = new DL.DALog(this.Entities); }

        public void AddLog(string details, string previousDetails)
        {
            try
            {
                Common.Log log = new Common.Log();
                log.LogId = Guid.NewGuid();
                log.Time = DateTime.Now;
                log.Details = details;
                log.DetailsBeforeModification = previousDetails;

                dll.AddLog(log);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
