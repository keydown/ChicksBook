﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class keydowno_backyard_farmerEntities : DbContext
    {
        public keydowno_backyard_farmerEntities()
            : base("name=keydowno_backyard_farmerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<BirdVet> BirdVets { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<ChickenLaw> ChickenLaws { get; set; }
        public DbSet<ChickenLawsProper> ChickenLawsPropers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<PetShop> PetShops { get; set; }
        public DbSet<PetShopsWorkingHour> PetShopsWorkingHours { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TempZipCode> TempZipCodes { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<UserAsset> UserAssets { get; set; }
        public DbSet<UserAssetsHistory> UserAssetsHistories { get; set; }
        public DbSet<UserFarm> UserFarms { get; set; }
        public DbSet<UserFarmHistory> UserFarmHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTraining> UserTrainings { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<ZipCode> ZipCodes { get; set; }
    }
}
