using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class VetsModel
    {
        public string Zip { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? DistanceFromUser { get; set; }

        public VetsModel()
        {
            this.Zip = "Zip Not Found";
            this.Name = "Doctor's Name Not Found";
            this.Address = "Address Not Found";
            this.Phone = "Phone Not Found";
            this.State = "State Not Found";
            this.Latitude = null;
            this.Longitude = null;
            this.DistanceFromUser = null;
        }

        public VetsModel(Vet vet) : base()
        {
            this.Zip = vet.Zip;
            this.Name = vet.Name;
            this.Address = vet.Address;
            this.Phone = vet.Phone;
            this.State = vet.State;
            this.Latitude = vet.Latitude;
            this.Longitude = vet.Longitude;
            this.DistanceFromUser = null;
        }

        public VetsModel(Vet vet, double distanceFromUser)
            : base()
        {
            this.Zip = vet.Zip;
            this.Name = vet.Name;
            this.Address = vet.Address;
            this.Phone = vet.Phone;
            this.State = vet.State;
            this.Latitude = vet.Latitude;
            this.Longitude = vet.Longitude;
            this.DistanceFromUser = distanceFromUser;
        }
    }
}