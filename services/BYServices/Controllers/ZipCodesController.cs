using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;

namespace BYServices.Controllers
{
    public class ZipCodesController : Operations
    {
        //BYEntities db = new BYEntities();
        //// GET api/zipcodes
        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        List<ZipCode> zipCodes = db.ZipCodes.ToList();
        //        List<ZipCodesModel> lzc = new List<ZipCodesModel>();
        //        foreach (var zc in zipCodes)
        //        {
        //            lzc.Add(new ZipCodesModel(zc));
        //        }
        //        return lzc;
        //    });
        //    return msg;
        //}

        //// GET api/zipcodes/5
        ////Returns ZipCode model, by given ID
        //[HttpGet]
        //public HttpResponseMessage Get(string id)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        ZipCode zc = db.ZipCodes.SingleOrDefault(x => x.Zip == id);
        //        return new ZipCodesModel(zc);
        //    });
        //    return msg;    
        //}

        //[HttpGet]
        //public HttpResponseMessage GetCity(string zip)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        string city = db.ZipCodes.SingleOrDefault(x => x.Zip == zip).City;
        //        return city;
        //    });
        //    return msg; 
        //}

        //[HttpGet]
        //public HttpResponseMessage GetState(string zip)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        string state = db.ZipCodes.SingleOrDefault(x => x.Zip == zip).State;
        //        return state;
        //    });
        //    return msg; 
        //}

        //[HttpGet]
        //public HttpResponseMessage GetCounty(string zip)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        string country = db.ZipCodes.SingleOrDefault(x => x.Zip == zip).Country;
        //        return country;
        //    });
        //    return msg;
        //}

        //[HttpGet]
        //public HttpResponseMessage GetCoords(string zip)
        //{
            
        //    var msg = PerformOperation(() =>
        //    {
        //        double?[] coords = new double?[2];
        //        coords[0] = db.ZipCodes.SingleOrDefault(x => x.Zip == zip).Latitude;//latitude
        //        coords[1] = db.ZipCodes.SingleOrDefault(x => x.Zip == zip).Longitude; //longitude
        //        return coords;
        //    });
        //    return msg;            
        //}
    }
}
