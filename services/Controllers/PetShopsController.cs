using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BYServices;
using BYServices.Models;

namespace BYServices.Controllers
{
    public class PetShopsController : Operations
    {
        // GET api/zipcodes
        BYEntities db = new BYEntities();

        public HttpResponseMessage GetShopsByZip(string id)
        {
            
            var msg = PerformOperation(() =>
            {
                List<PetShopsModel> petShops = new List<PetShopsModel>();
                List<PetShop> matchedShops = db.PetShops.Where(x => x.Zip == id).ToList();
                foreach (var vet in matchedShops)
                {
                    petShops.Add(new PetShopsModel(vet));
                }

                return petShops;
            });
            return msg;

        }

        [HttpGet]
        public HttpResponseMessage GetWorkingHours(PetShop ps)
        {
            
            var msg = PerformOperation(() =>
            {
                PetShopsWorkingHour workingHours = new PetShopsWorkingHour();
                workingHours = db.PetShopsWorkingHours.SingleOrDefault(x => x.Id == ps.Id);
                return workingHours;
            });
            return msg;
        }

        [HttpGet]
        public HttpResponseMessage GetPetShopsByCity(string id)
        {
            
            var msg = PerformOperation(() =>
            {
                List<PetShopsModel> pets = new List<PetShopsModel>();
                List<PetShop> matchedPets = db.PetShops.Where(x => x.City == id).ToList();
                foreach (var pet in matchedPets)
                {
                    pets.Add(new PetShopsModel(pet));
                }
                return pets;
            });
            return msg;
        }

        [HttpGet]
        public HttpResponseMessage GetPetShopByName(string id)
        {
            
            var msg = PerformOperation(() =>
            {
                PetShop petShop = db.PetShops.First(x => x.Name == id);
                PetShopsModel ps = new PetShopsModel(petShop);
                return ps;
            });
            return msg;
        }

        [HttpGet]
        public HttpResponseMessage GetShopsByLocation(double Latitude, double Longitude)
        {
            
            var msg = PerformOperation(() =>
            {
                List<PetShopsModel> petShops = new List<PetShopsModel>();
                List<PetShop> matchedShops = db.PetShops.Where(x => x.Latitude == Latitude).Where(x => x.Longitude == Longitude).ToList();
                foreach (var shop in matchedShops)
                {
                    petShops.Add(new PetShopsModel(shop));
                }
                return petShops;
            });
            return msg;

        }
    }
}