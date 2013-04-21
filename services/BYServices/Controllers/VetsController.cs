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
    public class VetsController : Operations
    {
        // GET api/zipcodes
        //BYEntities db = new BYEntities();

        //public HttpResponseMessage Get(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<VetsModel> vets = new List<VetsModel>();
        //        List<Vet> matchedVets = db.Vets.Where(x => x.Zip == id).ToList();
        //        foreach (var vet in matchedVets)
        //        {
        //            vets.Add(new VetsModel(vet));
        //        }
        //        return vets;
        //    });
        //    return msg;
            
        //}

        //[HttpGet]
        ////public HttpResponseMessage GetWorkingHours(Vet vs)
        ////{
            
        ////    var msg = PerformOperation(() =>
        ////    {
        ////        VetsWorkingHour workingHours = new VetsWorkingHour();
        ////        workingHours = db.VetsWorkingHours.SingleOrDefault(x => x.Id == vs.Id);
        ////        return workingHours;
        ////    });
        ////    return msg;
        ////}

        //[HttpGet]
        //public HttpResponseMessage GetVetsByCity(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<VetsModel> vets = new List<VetsModel>();
        //        List<Vet> matchedVets = db.Vets.Where(x => x.City == id).ToList();
        //        foreach (var vet in matchedVets)
        //        {
        //            vets.Add(new VetsModel(vet));
        //        }
        //        return vets;
        //    });
        //    return msg;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetPetShopByName(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        Vet vetShop = db.Vets.First(x => x.Name == id);
        //        VetsModel vs = new VetsModel(vetShop);
        //        return vs;
        //    });
        //    return msg;
            
        //}

        //[HttpGet]
        //public HttpResponseMessage GetShopsByLocation(double Latitude, double Longitude)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<VetsModel> vetShops = new List<VetsModel>();
        //        List<Vet> matchedShops = db.Vets.Where(x => x.Latitude == Latitude).Where(x => x.Longitude == Longitude).ToList();
        //        foreach (var shop in matchedShops)
        //        {
        //            vetShops.Add(new VetsModel(shop));
        //        }
        //        return vetShops;
        //    });
        //    return msg;   
        //}
    }

}