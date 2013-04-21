using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class AssetsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Cost { get; set; }
        public int AddedByUser { get; set; }

        public AssetsModel(Asset asset)
        {
            this.Id = asset.Id;
            this.Name = asset.Name;
            this.Description = asset.Description;
            this.Cost = asset.Cost;
            this.AddedByUser = asset.AddedByUser;
        }
        public AssetsModel()
        {
            this.Name = "Invalid name";
            this.Description = "No Description Found";
            this.Cost = null;
            this.AddedByUser = 0;
        }
    }
}