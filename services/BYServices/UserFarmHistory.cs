//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BYServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserFarmHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public Nullable<int> PetCount { get; set; }
        public System.DateTime ActionDate { get; set; }
        public string PetStatus { get; set; }
        public Nullable<double> PetPrice { get; set; }
        public Nullable<int> EggsStocked { get; set; }
        public Nullable<int> EggsHatching { get; set; }
    
        public virtual Animal Animal { get; set; }
        public virtual User User { get; set; }
    }
}
