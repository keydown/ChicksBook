using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class LawsModel
    {
        public string Location { get; set; }
        public string AreChickensAllowed { get; set; }
        public string ChickensAllowed { get; set; }
        public string RoostersAllowed { get; set; }
        public string PermitRequired { get; set; }
        public string CoopRestrictions { get; set; }
        public string Contacts { get; set; }
        public string Info { get; set; }
        public string Link { get; set; }
        public string LastUpdate { get; set; }


        public LawsModel(ChickenLaw law)
        {
            this.Location = law.Location;
            this.AreChickensAllowed = law.AreChickensAllowed;
            this.ChickensAllowed = law.ChickensAllowed;
            this.RoostersAllowed = law.RoostersAllowed;
            this.PermitRequired = law.PermitRequired;
            this.CoopRestrictions = law.CoopRestrictions;
            this.Contacts = law.Contacts;
            this.Info = law.Info;
            this.Link = law.Link;
            this.LastUpdate = law.LastUpdate;
        }
        public LawsModel()
        {
            this.Location = "No Information Found";
            this.AreChickensAllowed = "No Information Found";
            this.ChickensAllowed = "No Information Found";
            this.RoostersAllowed = "No Information Found";
            this.PermitRequired = "No Information Found";
            this.CoopRestrictions = "No Information Found";
            this.Contacts = "No Information Found";
            this.Info = "No Information Found";
            this.Link = "No Information Found";
            this.LastUpdate = "No Information Found";
        }
    }
}