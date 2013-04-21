using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYFarmerConsoleServices
{
    class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string AuthCode { get; set; }
        public string SessionId { get; set; }
        public System.DateTime DateRegistered { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public double LastLat { get; set; }
        public double LastLong { get; set; }
    }
}
