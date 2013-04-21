using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class PetShopsModel
    {
        public string Zip { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string MonStart { get; set; }
        public string MonEnd { get; set; }
        public string TueStart { get; set; }
        public string TueEnd { get; set; }
        public string WedStart { get; set; }
        public string WedEnd { get; set; }
        public string ThuStart { get; set; }
        public string ThuEnd { get; set; }
        public string FriStart { get; set; }
        public string FriEnd { get; set; }
        public string SatStart { get; set; }
        public string SatEnd { get; set; }
        public string SunStart { get; set; }
        public string SunEnd { get; set; }
        public double? DistanceFromUser { get; set; }

        public PetShopsModel()
        {
            this.Zip = "Zip Not Found";
            this.Name = "Shop's Name Not Found";
            this.Address = "Address Not Found";
            this.Phone = "Phone Not Found";
            this.City = "City Not Found";
            this.Latitude = null;
            this.Longitude = null;
            this.MonStart = "Monday Work Time Not Found";
            this.MonEnd = "Monday Work Time Not Found";
            this.TueStart = "Tuesday Work Time Not Found";
            this.TueEnd = "Tuesday Work Time Not Found";
            this.WedStart = "Wednesday Work Time Not Found";
            this.WedEnd = "Wednesday Work Time Not Found";
            this.ThuStart = "Thursday Work Time Not Found";
            this.ThuEnd = "Thursday Work Time Not Found";
            this.FriStart = "Friday Work Time Not Found";
            this.FriEnd = "Friday Work Time Not Found";
            this.SatStart = "Saturday Work Time Not Found";
            this.SatEnd = "Saturday Work Time Not Found";
            this.SunStart = "Sunday Work Time Not Found";
            this.SunEnd = "Sunday Work Time Not Found";
            this.DistanceFromUser = null;
        }

        public PetShopsModel(PetShop ps) : base()
        {
            this.Zip = ps.Zip;
            this.Name = ps.Name;
            this.Address = ps.Address;
            this.Phone = ps.Phone;
            this.City = ps.City;
            this.Latitude = ps.Latitude;
            this.Longitude = ps.Longitude;
            this.MonStart = ps.MonStart;
            this.MonEnd = ps.MonEnd;
            this.TueStart = ps.TueStart;
            this.TueEnd = ps.TueEnd;
            this.WedStart = ps.WedStart;
            this.WedEnd = ps.WedEnd;
            this.ThuStart = ps.ThuStart;
            this.ThuEnd = ps.ThuEnd;
            this.FriStart = ps.FriStart;
            this.FriEnd = ps.FriEnd;
            this.SatStart = ps.SatStart;
            this.SatEnd = ps.SatEnd;
            this.SunStart = ps.SunStart;
            this.SunEnd = ps.SunEnd;
            this.DistanceFromUser = null;
        }

        public PetShopsModel(PetShop ps, double distanceFromUser)
            : base()
        {
            this.Zip = ps.Zip;
            this.Name = ps.Name;
            this.Address = ps.Address;
            this.Phone = ps.Phone;
            this.City = ps.City;
            this.Latitude = ps.Latitude;
            this.Longitude = ps.Longitude;
            this.MonStart = ps.MonStart;
            this.MonEnd = ps.MonEnd;
            this.TueStart = ps.TueStart;
            this.TueEnd = ps.TueEnd;
            this.WedStart = ps.WedStart;
            this.WedEnd = ps.WedEnd;
            this.ThuStart = ps.ThuStart;
            this.ThuEnd = ps.ThuEnd;
            this.FriStart = ps.FriStart;
            this.FriEnd = ps.FriEnd;
            this.SatStart = ps.SatStart;
            this.SatEnd = ps.SatEnd;
            this.SunStart = ps.SunStart;
            this.SunEnd = ps.SunEnd;
            this.DistanceFromUser = distanceFromUser;
        }
    }
}