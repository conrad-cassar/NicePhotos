using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace DL
{
    public class DAAccount : ConnectionClass
    {
        public DAAccount()
            : base()
        { }

        public DAAccount(Common.NicePhotosEntities entities)
            : base(entities)
        { }

        /// <summary>
        /// Get a Account object matching with the provided username
        /// </summary>
        /// <param name="username">Account's username</param>
        /// <returns>The matching Account entity object</returns>
        public Common.Account GetUserAccount(string username)
        {
            return this.Entities.Accounts.SingleOrDefault(p => p.Username == username);
        }

        /// <summary>
        /// Sets the matching account's verification code as NULL, which means that the account has been verified
        /// </summary>
        /// <param name="email">Account's username</param>
        /// <param name="verificationCode">Account's verification code</param>
        public void SetAccountVerified(string username, string verificationCode, string privateKey)
        {
            this.Entities.SetAccountVerified(verificationCode, username, privateKey);
        }

        /// <summary>
        /// If the entered username and password exacly match with an existing account such account is retrieved
        /// </summary>
        /// <param name="username">User account's username</param>
        /// <param name="password">User account's password</param>
        /// <returns>The matching user account</returns>
        public ObjectResult /*Common.Account*/ GetUserAccount(string username, string password)
        {
            return this.Entities.LogIn(username, password);
        }

        /// <summary>
        /// Checks for the existance of the entered username and confirms weather the entered username is available or not
        /// </summary>
        /// <param name="username">User account's username</param>
        /// <returns>A boolean value confirming the availability of the username</returns>
        public bool IsUsernameAvailable(string username)
        {
            return (this.Entities.Accounts.Where(a => a.Username == username).Count() < 1);
        }

        /// <summary>
        /// Get a list of all user accounts
        /// </summary>
        /// <returns>Returns an IQueryable Account list of all user accounts</returns>
        public IQueryable<Common.Account> GetUserAccounts()
        {
            return this.Entities.Accounts;
        }

        public Common.Account GetAccountByApiKey(Guid key)
        {
            return this.Entities.Accounts.Where(p => p.ApiKey == key).SingleOrDefault();
        }

        /// <summary>
        /// Create a new user account
        /// </summary>
        /// <param name="userAccount">An entity which specifies all data of the new user account</param>
        public bool CreateUserAccount(Common.Account userAccount)
        {
            return Convert.ToBoolean(this.Entities.CreateAccount(
                 userAccount.Email,
                 userAccount.Username,
                 userAccount.Password));
        }

        public void ChangePassword(string username, string password)
        {
            this.Entities.ChangePassword(username, Common.Utilities.HashPassword(password, username));
        }

        public void SendForgotPasswordRequest(string username)
        {
            this.Entities.SendForgotPasswordRequest(username);
        }

        public void AddCredit(string username, int credit)
        {
            Common.Account acc = GetUserAccount(username);

            if (acc == null)
                throw new Exception("User does not existes");

            acc.Credits += credit;
            this.Entities.SaveChanges();
        }
    }
}
