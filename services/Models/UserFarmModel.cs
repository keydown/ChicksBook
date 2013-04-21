using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYServices.Models
{
    public class UserFarmModel
    {
        public int UserId { get; set; }
        public int PetId { get; set; }
        public int PetCount { get; set; }
    }

    public class UserFarmHistoryModel
    {
        public string PetName { get; set; }
        public string Object { get; set; }
        public string Action { get; set; }
        public double? Price { get; set; } 
        public int? Count { get; set; }

        public UserFarmHistoryModel()
        {
        }

        public UserFarmHistoryModel(UserFarmHistory userFarm)
        {
            this.PetName = userFarm.Animal.Breed;
            if (userFarm.PetStatus.Trim().EndsWith("P"))
            {
                this.Object = "Pet";
            }
            else if (userFarm.PetStatus.Trim().EndsWith("H"))
            {
                this.Object = "Hatching Egg";
            }
            else if (userFarm.PetStatus.Trim().EndsWith("E"))
            {
                this.Object = "Eggs In Stock";
            }
            else
            {
                this.Object = "No Info";
            }
            if (userFarm.PetStatus.Trim().Contains("ADD"))
            {
                this.Action = "Added";
            }
            else if (userFarm.PetStatus.Trim().Contains("SELL"))
            {
                this.Action = "Sold";
            }
            else if (userFarm.PetStatus.Trim().Contains("LOSE"))
            {
                this.Action = "Lost";
            }
            else this.Action = "No Info";

            this.Price = userFarm.PetPrice;
            this.Count = userFarm.PetCount;
        }
    }
}

//TODO Add constructors when DB is ready