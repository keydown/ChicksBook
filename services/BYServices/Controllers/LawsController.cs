using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace BYServices.Controllers
{
    public class LawsController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetLocalLaws(JObject obj)
        {

            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
                double lati = currentUser.LastLat;
                double longi = currentUser.LastLong;

                string url = "http://dev.virtualearth.net/REST/v1/Locations/"+lati.ToString()+","+longi.ToString()+"?o=xml&key=Asd3or82owbl2UFZ1tu5H8sOzfhKPb-YKG_hbXY4T4Xq0uDSKlY2eewSDXuZZWAS";
                XmlDocument xml = new XmlDocument();
                xml.Load(url);

                if (xml == null)
                {
                    throw BuildHttpResponseException("Bing API Query Failed", "ERR_QRY");
                }

                XmlNodeList stateNodeLists = xml.SelectNodes("//AdminDistrict");
                string state ="";
                foreach (XmlNode node in stateNodeLists)
                {
                    state = node.InnerText;
                }

                if (string.IsNullOrEmpty(state))
                {
                    throw BuildHttpResponseException("No Location Data Was Found", "ERR_LOC");
                }

                ChickenLaw law = db.ChickenLaws.SingleOrDefault(x => x.Location == state);

                if (law == null)
                {
                    throw BuildHttpResponseException("No Laws Found For Your Location", "ERR_LWS_FND");
                }

                LawsModel lawMod = new LawsModel(law);

                return lawMod;                
            });
            return msg;
        }
    }
}
