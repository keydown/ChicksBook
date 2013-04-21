using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;

namespace BYServices.Controllers
{
    public class BirdVetsController : Operations
    {
        //BYEntities db = new BYEntities();

        //public HttpResponseMessage GetBirdVetsByZip(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //        {
        //            List<BirdVetsModel> birdVets = new List<BirdVetsModel>();
        //            List<BirdVet> matchedVets = db.BirdVets.Where(x => x.Zip == id).ToList();
        //            foreach (var vet in matchedVets)
        //            {
        //                birdVets.Add(new BirdVetsModel(vet));
        //            }
        //            return birdVets;
        //        });
        //    return msg;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetBirdVetsByState(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<BirdVetsModel> birdVets = new List<BirdVetsModel>();
        //        List<BirdVet> matchedVets = db.BirdVets.Where(x => x.State == id).ToList();
        //        foreach (var vet in matchedVets)
        //        {
        //            birdVets.Add(new BirdVetsModel(vet));
        //        }
        //        return birdVets;
        //    });
        //    return msg;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetBirdVetsByCity(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<BirdVetsModel> birdVets = new List<BirdVetsModel>();
        //        List<BirdVet> matchedVets = db.BirdVets.Where(x => x.City == id).ToList();
        //        foreach (var vet in matchedVets)
        //        {
        //            birdVets.Add(new BirdVetsModel(vet));
        //        }
        //        return birdVets;
        //    });
        //    return msg;
        //}
        //[HttpGet]
        //public HttpResponseMessage GetBirdVetsByLocation(double latitude, double longitude)
        //{
            
        //    var msg = PerformOperation(() =>
        //        {
        //            List<BirdVetsModel> matchedBirdVets = new List<BirdVetsModel>();
        //            List<ZipCode> zipCodesList = db.ZipCodes.Where(x => x.Latitude == latitude).Where(x => x.Longitude == longitude).ToList();
        //            foreach (var zipCode in zipCodesList)
        //            {
        //                matchedBirdVets.Add(new BirdVetsModel(db.BirdVets.FirstOrDefault(x => x.Zip.ToString() == zipCode.ToString())));
        //            }
        //            return matchedBirdVets;
        //        });
        //    return msg;
        //}

    }
}