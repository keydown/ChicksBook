//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetVetsDoctorsInfo
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAssetsHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public string AssetStatus { get; set; }
        public System.DateTime ActionDate { get; set; }
        public double AssetPrice { get; set; }
        public double AssetCount { get; set; }
    
        public virtual Asset Asset { get; set; }
        public virtual User User { get; set; }
    }
}
