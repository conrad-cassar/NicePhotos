using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Exceptions;
using Common;
using System.Security.Cryptography;
using System.IO;

namespace BL
{
    public class Accounts : BusinessBase
    {
        private DL.DAAccount dlu;

        public Accounts(string username)
            : base(username)
        { dlu = new DL.DAAccount(this.Entities); }

        internal Accounts()
            : base(null)
        { dlu = new DL.DAAccount(this.Entities); }

        /// <summary>
        /// Runs all the necessary procceses for new users
        /// </summary>
        /// <param name="userEmail">Account's email (used as a username)</param>
        public void FirstTimer(string userEmail)
        {
            /*try
            {
                //create folder for users personal photos
                string path = HttpContext.Current.Server.MapPath("~/UserContent/" + userEmail.ToLower());
                System.IO.Directory.CreateDirectory(path);
                System.IO.Directory.CreateDirectory(path + "/S");
                System.IO.Directory.CreateDirectory(path + "/XS");

                //creates the default album
                new Photos(null).CreateAlbum(userEmail, "Default Album");
            }
            catch (Exception ex)
            {
                this.LogError(this.UserEmail, MethodInfo.GetCurrentMethod(), ex);
                throw new Exception("Unfortunately an error occured during the 'First Timer' processes");
            }*/
        }

        /// <summary>
        /// Get an Account object matching with the provided username
        /// </summary>
        /// <param name="email">Account's email (used as a username)</param>
        /// <returns>The matching UserAccount entity object</returns>
        public Common.Account GetUserAccount(string username)
        {
            try
            {
                return dlu.GetUserAccount(username);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve current user's information");
            }
        }

        /// <summary>
        /// Sets the matching account's verification code as NULL, which means that the account has been verified
        /// </summary>
        /// <param name="username">Account's username</param>
        /// <param name="verificationCode">Account's verification code</param>
        public void SetAccountAsVerified(string username, string verificationCode, string privateKey)
        {
            try
            {
                dlu.SetAccountVerified(username, verificationCode, privateKey);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to verify account");
            }

            try
            {
                this.FirstTimer(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks whether user/email and password are correct.
        /// If both are correct the matching user's entity is returned, else a NULL value is returned.
        /// </summary>
        /// <param name="email">Account's email (used as a username)</param>
        /// <param name="password">Account's password</param>
        /// <returns>The user account entity mathcing with the entered username/email and password</returns>
        public Common.Account LogIn(string username, string password)
        {
            try
            {
                IQueryable<Common.Account> tmp =
                    (dlu.GetUserAccount(username, Common.Utilities.HashPassword(password, username))).AsQueryable().Cast<Common.Account>();

                List<Common.Account> res = tmp.ToList();

                //
                /*List<CommonLayer.UserAccount> res = new List<CommonLayer.UserAccount>();
                res.Add(dlu.GetUserAccount(username, password));*/
                //

                if (res.Count() > 0)
                {
                    if (res.ElementAt(0).VerificationCode != null)
                    {
                        throw new AccountNotVerifiedException("Please check your email inbox to activate your account first");
                    }
                    else
                    {
                        return res.ElementAt(0);
                    }
                }
                else
                {
                    throw new InvalidCredentialsException();
                }

                /*Common.Account tmp = dlu.GetUserAccount(username, password);

                if (tmp == null)
                    throw new AccountNotVerifiedException("Please check your email inbox to activate your account first");
                else
                    return tmp;*/

            }
            catch (AccountNotVerifiedException ex)
            {
                throw ex;
            }
            catch (InvalidCredentialsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while logging in");
            }
        }

        /// <summary>
        /// Checks for the existance of the entered username and confirms weather the entered username is available or not
        /// </summary>
        /// <param name="username">Account's username</param>
        /// <returns>A boolean value confirming the availability of the username</returns>
        public bool IsUsernameAvailable(string username)
        {
            try
            {
                return dlu.IsUsernameAvailable(username);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to check username availability");
            }
        }

        /// <summary>
        /// Get a list of all user accounts
        /// </summary>
        /// <returns>Returns an IQueryable UserAccount list of all user accounts</returns>
        public IQueryable<Common.Account> GetUserAccounts()
        {
            try
            {
                return dlu.GetUserAccounts();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to retrieve accounts");
            }
        }

        /// <summary>
        /// A new user account is created. In case the entered username already exists,
        /// a UserAlreadyExistsException is thrown.
        /// </summary>
        /// <param name="account">The entity containing all new user's data</param>
        public void Register(Common.Account account)
        {
            try
            {
                if (this.Entities.Accounts.Where(p => p.Username == account.Username).Count() > 0)
                {
                    throw new UserAlreadyExistException();
                }

                account.Password = Utilities.HashPassword(account.Password, account.Username);
                dlu.CreateUserAccount(account);
            }
            catch (UserAlreadyExistException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to register you");
            }
        }

        /// <summary>
        /// Update existing user account's details
        /// </summary>
        /// /// <param name="username">Username</param>
        /// <param name="userEmail">User account's email address</param>
        /// <param name="Name">User's name</param>
        /// <param name="Surname">User's surname</param>
        /// <param name="aboutMe">Abount Me</param>
        public void UpdateUserData(string username, string userEmail, string name, string surname, string abontMe)
        {
            try
            {
                Common.Account account = GetUserAccount(username);

                if (account != null)
                {
                    account.Name = name;
                    account.Surname = surname;
                    account.Email = userEmail;
                    account.AboutMe = abontMe;

                    this.Entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="oldPassword">Old Password</param>
        /// <param name="newPassword">New Password</param>
        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                Common.Account account = LogIn(username, oldPassword);

                if (account != null)
                {
                    dlu.ChangePassword(username, newPassword);
                }
                else
                    throw new Common.Exceptions.InvalidCredentialsException("Old password is invalid");
            }
            catch (Common.Exceptions.InvalidCredentialsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unabel to change password.");
            }
        }

        /// <summary>
        /// Change user's password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">New Password</param>
        public void ChangePassword(string username, string password)
        {
            try
            {
                dlu.ChangePassword(username, password);
            }
            catch (Exception ex)
            {
                throw new Exception("Unabel to change password.");
            }
        }

        public void SendForgotPasswordRequest(string username)
        {
            try
            {
                dlu.SendForgotPasswordRequest(username);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to send request");
            }
        }

        public void AddCredit(string username, int credit)
        {
            try
            {
                dlu.AddCredit(username, credit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Common.Account GetAccountByApiKey(Guid key)
        {
            try
            {
                return dlu.GetAccountByApiKey(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
