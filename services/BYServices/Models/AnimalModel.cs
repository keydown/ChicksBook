using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYServices.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }
        public string Breed { get; set; }
        public string Varieties { get; set; }
        public string Rarity { get; set; }
        public string Weight { get; set; }
        public string Class { get; set; }
        public string Origins { get; set; }
        public string EggsColor { get; set; }
        public string EggSize { get; set; }
        public string Comb { get; set; }
        public string SkinColor { get; set; }
        public string Ealobes { get; set; }
        public string Brooding { get; set; }
        public string Hardness { get; set; }
        public string Behavior { get; set; }
        public string Maturing { get; set; }
        public string Picture { get; set; }
        public string Links { get; set; }
        public double? EggCost { get; set; }
        public int? EggHatchingDays { get; set; }

        public AnimalModel(Animal animal)
        {
            this.Id = animal.Id;
            this.Breed = animal.Breed;
            this.Varieties = animal.Varieties;
            this.Rarity = animal.Rarity;
            this.Weight = animal.Weight;  
            this.Class = animal.Class;
            this.Origins = animal.Origins;
            this.EggsColor = animal.EggsColor;
            this.EggSize = animal.EggSize;
            this.Comb = animal.Comb; 
            this.SkinColor = animal.SkinColor;
            this.Ealobes = animal.Earlobes; 
            this.Brooding = animal.Brooding; 
            this.Hardness = animal.Hardiness; 
            this.Behavior = animal.Behavior; 
            this.Maturing = animal.Maturing; 
            this.Picture = animal.Picture;
            this.Links = animal.Links;
            this.EggCost = animal.EggCost;
            this.EggHatchingDays = animal.EggHatchingDays;
        }
    }
    public class FarmModel
    {
        public int Id { get; set; }
        public string PetName { get; set; }
        public int Count { get; set; }
        public int EggsInStock { get; set; }
        public int EggsHatching { get; set; }

        public FarmModel()
        {
        }

        public FarmModel(int id, string petName, int count, int eggsInStock, int eggsHatching)
        {
            this.Id = id;
            this.PetName = petName;
            this.Count = count;
            this.EggsInStock = eggsInStock;
            this.EggsHatching = eggsHatching;
        }
    }
}