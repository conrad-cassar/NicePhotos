using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    public class Roles : BusinessBase
    {
        private DL.DARole dlr;

        public Roles(string username)
            : base(username)
        { dlr = new DL.DARole(this.Entities); }

        internal Roles()
            : base(null)
        { dlr = new DL.DARole(this.Entities); }


        /// <summary>
        /// Retrieve a list of roles possessed by a particular user account
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Queryable list of Roles</returns>
        public IQueryable<Common.Role> GetUserRoles(string username)
        {
            try
            {
                return dlr.GetUserRoles(username);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve user's roles");
            }
        }

        public bool IsUserInRole(string username, string roleId)
        {
            try
            {
                List<Common.Role> userRoles = GetUserRoles(username).ToList();
                return userRoles.Where(p => p.RoleId == roleId).Count() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retrieve a list of roles IDs possessed by a particular user account
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>List of al user roles' names</returns>
        public string[] GetUserRolesIds(string username)
        {
            try
            {
                IQueryable<Common.Role> roles = dlr.GetUserRoles(username);
                List<string> s = new List<string>();

                foreach (Common.Role r in roles)
                {
                    s.Add(r.RoleId);
                }

                return s.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve user's roles");
            }
        }

        /// <summary>
        /// Get a list of all roles
        /// </summary>
        /// <returns>Queryable list of Roles</returns>
        public IQueryable<Common.Role> GetRoles()
        {
            try
            {
                return dlr.GetRoles();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve roles");
            }
        }

        /// <summary>
        /// Get a Role by ID
        /// </summary>
        /// <param name="roleid">Role ID</param>
        /// <returns>Matching Role</returns>
        public Common.Role GetRole(string roleid)
        {
            try
            {
                return dlr.GetRole(roleid);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve role");
            }
        }

        public IQueryable<Common.MenuItem> GetMenuItems(string username)
        {
            try
            {
                return dlr.GetUserMenuItems(username);

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get menu items");
            }
        }

        public IQueryable<Common.MenuItem> GetMenuItems()
        {
            try
            {
                return dlr.GetMenuItems();

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get menu items");
            }
        }

        public bool RoleHasMenu(string roleId, string menuItemId)
        {
            try
            {
                return dlr.RoleHasMenu(roleId, Convert.ToInt32(menuItemId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddToRoleMenuItem(string roleId, string menuItemId)
        {
            try
            {
                dlr.AddToRoleMenuItem(roleId, Convert.ToInt32(menuItemId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveRoleMenuItem(string roleId, string menuItemId)
        {
            try
            {
                Common.Role_MenuItem rmi = dlr.GetRole_MenuItem(roleId, Convert.ToInt32(menuItemId));
                dlr.RemoveRoleMenuItem(rmi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddUserRole(string username, string roleId)
        {
            try
            {
                dlr.AddUserRole(username, roleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveUserRole(string username, string roleId)
        {
            try
            {
                dlr.RemoveUserRole(username, roleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
