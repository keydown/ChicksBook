using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class ZipCodesModel
    {
        public string Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Country { get; set; }

        //public ZipCodesModel(ZipCode zc)
        //{
        //    this.Zip = zc.Zip;
        //    this.City = zc.City;
        //    this.State = zc.State;
        //    this.Latitude = zc.Latitude;
        //    this.Longitude = zc.Longitude;
        //    this.County = zc.County;
        //    this.Country = zc.Country;
        //}
    }
}