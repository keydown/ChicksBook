using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class UsersModel
    {
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string SessionId { get; set; }
        public DateTime DateRegistered { get; set; }
        public DateTime? LastLogin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public double LastLong { get; set; }
        public double LastLat { get; set; }

        public UsersModel()
        {
            this.Username = "Invalid Username";
            this.AuthCode = "Invalid AuthCode";
            this.SessionId = null;
            this.DateRegistered = DateTime.Now;
            this.LastLogin = null;
            this.FirstName = "No First Name";
            this.LastName = "No Last Name";
            this.Email = "No Email";
            this.LastLat = 0.0d;
            this.LastLong = 0.0d;

        }

        public UsersModel(string username, string firstName, string lastName, string email, string authCode, double lat, double lon) //used for registration
        {
            this.Username = username;
            this.AuthCode = authCode;
            this.SessionId = null;
            this.DateRegistered = DateTime.Now;
            this.LastLogin = DateTime.Now;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.LastLat = lat;
            this.LastLong = lon;
        }

        public UsersModel(string sessionId, string firstName, string lastName, string email) //used for edit user info
        {
            BYEntities db = new BYEntities();
            this.Username = db.Users.SingleOrDefault(x => x.SessionId == sessionId).Username;
            this.AuthCode = db.Users.SingleOrDefault(x => x.SessionId == sessionId).AuthCode;
            this.SessionId = sessionId;
            this.DateRegistered = db.Users.SingleOrDefault(x => x.SessionId == sessionId).DateRegistered;
            this.LastLogin = db.Users.SingleOrDefault(x => x.SessionId == sessionId).LastLogin;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }

        public UsersModel(User usr)
        {
            this.Username = usr.Username;
            this.AuthCode = usr.AuthCode;
            this.SessionId = usr.SessionId;
            this.DateRegistered = usr.DateRegistered;
            this.LastLogin = usr.LastLogin;
            this.FirstName = usr.FirstName;
            this.LastName = usr.LastName;
            this.Email = usr.Email;
            this.LastLat = usr.LastLat;
            this.LastLong = usr.LastLong;
        }
    }

    public class UserSession
    {
        public string SessionID { get; set; }
        public UserSession()
        {
            this.SessionID = null;
        }
        public UserSession(string sessionID) : base()
        {
            this.SessionID = sessionID;
        }
    }
}