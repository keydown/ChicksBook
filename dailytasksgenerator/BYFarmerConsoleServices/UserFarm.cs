//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BYFarmerConsoleServices
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserFarm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public int PetCount { get; set; }
        public int EggsInStock { get; set; }
        public int EggsHatching { get; set; }
    
        public virtual Animal Animal { get; set; }
        public virtual User User { get; set; }
    }
}
