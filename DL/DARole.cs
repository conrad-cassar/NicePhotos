using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL
{
    public class DARole : ConnectionClass
    {
        public DARole()
            : base()
        { }

        public DARole(Common.NicePhotosEntities entities)
            : base(entities)
        { }


        /// <summary>
        /// Get a list of all roles
        /// </summary>
        /// <returns>Queryable list of Roles</returns>
        public IQueryable<Common.Role> GetRoles()
        {
            return this.Entities.Roles;
        }

        /// <summary>
        /// Get a Role by ID
        /// </summary>
        /// <param name="roleid">Role ID</param>
        /// <returns>Matching Role</returns>
        public Common.Role GetRole(string roleid)
        {
            return this.Entities.Roles.SingleOrDefault(p => p.RoleId == roleid);
        }

        /// <summary>
        /// Retrieve a list of roles possessed by a particular user account
        /// </summary>
        /// <param name="email">User account's email</param>
        /// <returns>Queryable list of Roles</returns>
        public IQueryable<Common.Role> GetUserRoles(string username)
        {
            return (from account_roles
                    in this.Entities.Account_Role
                    from roles
                    in this.Entities.Roles
                    where account_roles.Username == username &&
                    account_roles.RoleId == roles.RoleId
                    select roles);
        }

        public IQueryable<Common.MenuItem> GetUserMenuItems(string username)
        {
            List<Common.Role> rl = GetUserRoles(username).ToList();
            rl.Add(new Common.Role { RoleId = "GUEST", RoleName = "Guest" });

            return (from ur in
                    rl
                    from rmi
                    in this.Entities.Role_MenuItem
                    from mi
                    in this.Entities.MenuItems
                    where mi.ItemId == rmi.Item && ur.RoleId == rmi.Role
                    select mi).OrderByDescending(x => x.ItemName).Distinct().AsQueryable();
        }

        public bool RoleHasMenu(string roleId, int menuId)
        {
            return (from rl in this.Entities.Roles
                    from rmi in this.Entities.Role_MenuItem
                    from mi in this.Entities.MenuItems
                    where mi.ItemId == rmi.Item 
                    && rl.RoleId == rmi.Role 
                    && rmi.Role == roleId
                    && rmi.Item == menuId
                    select mi).Count() > 0;
        }

        public IQueryable<Common.MenuItem> GetMenuItems()
        {
            return this.Entities.MenuItems;
        }

        public void AddToRoleMenuItem(string roleId, int menuItemId)
        {
            this.Entities.AddRole_MenuItem(roleId, menuItemId);
        }

        public void RemoveRoleMenuItem(Common.Role_MenuItem rmi)
        {
            this.Entities.Role_MenuItem.DeleteObject(rmi);
            this.Entities.SaveChanges();
        }

        public Common.Role_MenuItem GetRole_MenuItem(string roleId, int menuItemId)
        {
            return this.Entities.Role_MenuItem.Where(p => p.Role == roleId && p.Item == menuItemId).SingleOrDefault();
        }

        public void AddUserRole(string username, string roleId)
        {
            Common.Account_Role ar = new Common.Account_Role();
            ar.AccountRoleId = Guid.NewGuid();
            ar.RoleId = roleId;
            ar.Username = username;

            this.Entities.Account_Role.AddObject(ar);
            this.Entities.SaveChanges();
        }

        public void RemoveUserRole(string username, string roleId)
        {
            Common.Account_Role ar = 
                this.Entities.Account_Role.Where(p => p.Username == username && p.RoleId == roleId).SingleOrDefault();

            this.Entities.Account_Role.DeleteObject(ar);
            this.Entities.SaveChanges();
        }
    }
}
