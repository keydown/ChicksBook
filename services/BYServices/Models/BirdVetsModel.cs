using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class BirdVetsModel
    {
        public string Zip { get; set; }
        public string Name { get; set; }
        public string ClinicName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public BirdVetsModel()
        {
            this.Zip = "Zip Not Found";
            this.Name = "Doctor's Name Not Found";
            this.ClinicName = "Clinic Name Not Found";
            this.City = "City Not Found";
            this.State = "State Not Found";
            this.Address = "Address Not Found";
            this.Phone = "Phone Not Found";
        }

        public BirdVetsModel(BirdVet bv) : base()
        {
            this.Zip = bv.Zip;
            this.Name = bv.Name;
            this.ClinicName = bv.ClinicName;
            this.City = bv.City;
            this.State = bv.State;
            this.Address = bv.Address;
            this.Phone = bv.Phone;            
        }
    }
}