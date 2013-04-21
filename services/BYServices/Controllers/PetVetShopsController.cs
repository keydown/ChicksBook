using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BYServices.Models;

namespace BYServices.Controllers
{
    public class PetVetShopsController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetNearbyPetShops(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                double defaultRadius = 10;

                List<PetShop> petShops = db.PetShops.ToList();
                List<PetShopsModel> modelPetShops = new List<PetShopsModel>();

                foreach (PetShop petShop in petShops)
                {
                    modelPetShops.Add(new PetShopsModel(petShop));
                }

                IOrderedEnumerable<PetShopsModel> nearbyPetShopsWithDistance = GeoLocationHelper.FindNearbyLocations(currentUser.LastLat, currentUser.LastLong, defaultRadius, modelPetShops).OrderBy(x => x.DistanceFromUser);


                //List<PetShopsModel> nearbyPetShops = new List<PetShopsModel>();

                //foreach (PetShop petShop in tmpNearbyPetShopsWithDistance)
                //{
                //    nearbyPetShops.Add(new PetShopsModel(petShop));
                //}

                return nearbyPetShopsWithDistance;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetNearbyVets(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                double defaultRadius = 10;

                List<Vet> vets = db.Vets.ToList();
                List<VetsModel> modelVets = new List<VetsModel>();

                foreach (Vet vet in vets)
                {
                    modelVets.Add(new VetsModel(vet));
                }

                IOrderedEnumerable<VetsModel> nearbyVetsWithDistance = GeoLocationHelper.FindNearbyLocations(currentUser.LastLat, currentUser.LastLong, defaultRadius, modelVets).OrderBy(x => x.DistanceFromUser);

                return nearbyVetsWithDistance;
            });
            return msg;
        }
    }
}
