//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NASAGetData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vet
    {
        public int Id { get; set; }
        public string Zip { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> Phone { get; set; }
        public string Description { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public string Image { get; set; }
    
        public virtual VetsWorkingHour VetsWorkingHour { get; set; }
        public virtual ZipCode ZipCode { get; set; }
    }
}
