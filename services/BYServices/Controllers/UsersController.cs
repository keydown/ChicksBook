using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{
    public class UsersController : Operations
    {
        private Regex usernameRegex = new Regex(@"^(?:[aA-zZ]+[\s\.]?[aA-zZ]+)+$");
        private Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage RegisterUser(User usr)
        {
            var msg = PerformOperation(() =>
            {
                UsernameValidation(usr.Username);
                AuthCodeValidation(usr.AuthCode);
                FirstNameValidation(usr.FirstName);
                LastNameValidation(usr.LastName);
                EmailValidation(usr.Email);

                var dbUser = db.Users.FirstOrDefault(u => u.Username == usr.Username || u.Email == usr.Email);
                if (dbUser != null)
                {
                    if (dbUser.Username == usr.Username)
                    {
                        throw BuildHttpResponseException("Username is already taken", "ERR_DUP_USR");
                    }
                    else if (dbUser.Email == usr.Email)
                    {
                        throw BuildHttpResponseException("Username is already registered with the system", "ERR_DUP_EML");
                    }
                }
                string sessionId = SessionIdGenerator();
                dbUser = new User()
                {
                    Username = usr.Username,
                    AuthCode = usr.AuthCode,
                    SessionId = sessionId,
                    DateRegistered = DateTime.Now,
                    LastLogin = DateTime.Now,
                    FirstName = usr.FirstName,
                    LastName = usr.LastName,
                    Email = usr.Email,
                    LastLong = usr.LastLong,
                    LastLat = usr.LastLat
                };
                db.Users.Add(dbUser);
                db.SaveChanges();
                return new UserSession(sessionId);

            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage LoginUser(JObject usr)
        {
            var msg = PerformOperation(() =>
            {
                string username = usr["Username"].Value<string>();
                string authcode = usr["AuthCode"].Value<string>();
                double lat = usr["lastLat"].Value<double>();
                double lon = usr["lastLong"].Value<double>();

                UsernameValidation(username);
                AuthCodeValidation(authcode);

                var dbUser = db.Users.FirstOrDefault(u => u.Username == username);
                if (dbUser == null || dbUser.AuthCode != authcode)
                {
                    throw BuildHttpResponseException("Invalid username or password", "ERR_WRONG_USR");
                }

                User currentUser = db.Users.SingleOrDefault(x => x.Username == username && x.AuthCode == authcode);

                string sessionID = SessionIdGenerator();

                currentUser.SessionId = sessionID;
                currentUser.LastLogin = DateTime.Now;
                currentUser.LastLat = lat;
                currentUser.LastLong = lon;
                db.SaveChanges();
                return new UserSession(sessionID);

            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage LogoutUser(JObject id)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = id["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                var usr = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
                if (usr == null)
                {
                    throw BuildHttpResponseException("Invalid session ID", "ERR_WRONG_SES");
                }
                usr.SessionId = null;
                db.SaveChanges();
                JObject output = new JObject();
                output["status"] = "Successfully logged out";
                return output;
            });
            return msg;
        }
        [HttpPost]
        public HttpResponseMessage EditUserInfo(JObject usr)
        {
            var msg = PerformOperation(() =>
            {
                BYEntities db = new BYEntities();
                string sessionId = usr["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                var user = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
                FirstNameValidation(usr["firstName"].Value<string>());
                LastNameValidation(usr["lastName"].Value<string>());
                EmailValidation(usr["email"].Value<string>());
                user.FirstName = usr["firstName"].Value<string>();
                user.LastName = usr["lastName"].Value<string>();
                user.Email = usr["email"].Value<string>();
                db.SaveChanges();
                return new UserSession(usr["sessionId"].Value<string>());
            });
            return msg;
        }


        private void UsernameValidation(string username)
        {
            if (string.IsNullOrEmpty(username) || username.Length < 4)
            {
                throw BuildHttpResponseException("Username must be at least 4 characters long", "ERR_INV_USR");
            }

            if (username.Length > 15)
            {
                throw BuildHttpResponseException("Username must be shroter than 15 characters", "ERR_INV_USR");
            }
            if (!usernameRegex.IsMatch(username))
            {
                throw BuildHttpResponseException("Username contains invalid characters", "ERR_INV_USR");
            }
        }

        private void AuthCodeValidation(string authCode)
        {
            if (string.IsNullOrEmpty(authCode))
            {
                throw BuildHttpResponseException("Invalid Authorization Code", "ERR_INV_AUTH");
            }
        }

        private void FirstNameValidation(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw BuildHttpResponseException("First Name Cannot Be Empty", "ERR_INV_FNM");
            }

            if (firstName.Length < 2)
            {
                throw BuildHttpResponseException("First Name Cannot Be Less than 2 symbols long", "ERR_INV_FNM");
            }
        }

        private void LastNameValidation(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw BuildHttpResponseException("Last Name Cannot Be Empty", "ERR_INV_LNM");
            }
            if (lastName.Length < 2)
            {
                throw BuildHttpResponseException("Last Name Cannot Be Less than 2 symbols long", "ERR_INV_FNM");
            }
        }

        private void EmailValidation(string email)
        {
            if (!emailRegex.IsMatch(email))
            {
                throw BuildHttpResponseException("Invalid Email Used", "ERR_INV_EML");
            }
        }

        private string SessionIdGenerator()
        {
            int passwordLength = 50;
            int alphaNumericalCharsAllowed = 2;
            string sessionId = Membership.GeneratePassword(passwordLength, alphaNumericalCharsAllowed);
            return sessionId;

        }
    }
}