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
    
    public partial class User
    {
        public User()
        {
            this.Assets = new HashSet<Asset>();
            this.Calendars = new HashSet<Calendar>();
            this.Tasks = new HashSet<Task>();
            this.UserAssets = new HashSet<UserAsset>();
            this.UserAssetsHistories = new HashSet<UserAssetsHistory>();
            this.UserFarms = new HashSet<UserFarm>();
            this.UserFarmHistories = new HashSet<UserFarmHistory>();
            this.UserTrainings = new HashSet<UserTraining>();
        }
    
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
    
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<UserAsset> UserAssets { get; set; }
        public virtual ICollection<UserAssetsHistory> UserAssetsHistories { get; set; }
        public virtual ICollection<UserFarm> UserFarms { get; set; }
        public virtual ICollection<UserFarmHistory> UserFarmHistories { get; set; }
        public virtual ICollection<UserTraining> UserTrainings { get; set; }
    }
}
