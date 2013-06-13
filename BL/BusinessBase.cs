using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class BusinessBase
    {
        public Common.NicePhotosEntities Entities { get; set; }
        private string username;

        public void SaveChanges()
        {
            this.Entities.SaveChanges();
        }

        protected string Username
        {
            get { return username; }
        }

        public BusinessBase(string username)
        {
            this.username = username;
            DL.ConnectionClass cl = new DL.ConnectionClass();

            this.Entities = cl.Entities;
            /*this.Entities.Connection.ConnectionString =
                ConfigurationManager.ConnectionStrings["CS_Outsider"].ConnectionString;

            if (userEmail != null)
            {
                List<CommonLayer.Role> userRoles =
                    new DataLayer.DARole(this.Entities).GetUserRoles(userEmail).ToList();

                if (userRoles != null)
                {
                    List<string> tmpIDs = new List<string>();

                    foreach (CommonLayer.Role r in userRoles)
                    {
                        tmpIDs.Add(r.RoleId);
                    }

                    if (tmpIDs.Count < 1)
                    {
                        cl = new DataLayer.ConnectionClass();
                        this.Entities = cl.Entities;
                        this.Entities.Connection.ConnectionString =
                            ConfigurationManager.ConnectionStrings["CS_Outsider"].ConnectionString;
                    }
                    else if (tmpIDs.Contains("CLOU"))
                    {
                        cl = new DataLayer.ConnectionClass();
                        this.Entities = cl.Entities;
                        this.Entities.Connection.ConnectionString =
                            ConfigurationManager.ConnectionStrings["CS_Clouder"].ConnectionString;
                    }
                    else if (tmpIDs.Contains("ZEUS"))
                    {
                        cl = new DataLayer.ConnectionClass();
                        this.Entities = cl.Entities;
                        this.Entities.Connection.ConnectionString =
                            ConfigurationManager.ConnectionStrings["CS_Zeus"].ConnectionString;
                    }
                    else
                    {
                        cl = new DataLayer.ConnectionClass();
                        this.Entities = cl.Entities;
                        this.Entities.Connection.ConnectionString =
                            ConfigurationManager.ConnectionStrings["CS_Clouder"].ConnectionString;
                    }
                }
        }*/
        }

        /*public void LogError(string userEmail, MethodBase method, Exception exception)
        {
            string exceptionMsg = null;
            string innerExceptionMsg = null;

            if (exception != null)
            {
                exceptionMsg = exception.Message;

                if (exception.InnerException != null)
                {
                    innerExceptionMsg = exception.InnerException.Message;
                }
            }


            try
            {
                new DataLayer.Maintainability().LogError(
                    EmptyToNull(userEmail),
                    method.ToString(),
                    EmptyToNull(exceptionMsg),
                    EmptyToNull(innerExceptionMsg));
            }
            catch (Exception ex)
            { }
        }

        private string EmptyToNull(string str)
        {
            string s = null;

            if (str != null)
            {
                if (str != "")
                {
                    s = str;
                }
            }
            return s;
        }*/
    }
}
