using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.Entity;
using System.Web.Script.Serialization;
using System.Threading;

namespace NASAGetData
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetVets_Click(object sender, EventArgs e)
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();
            List<string> codes = db.ZipCodes.Select(x => x.Zip).ToList();
            
            foreach (var code in codes)
            {
                try
                {
                    Response.Write(code + " ");
                    string zip = code.ToString();
                    string source = PerformRequest(zip);
                    AddPetShop(source, db);
                    var a = db.ZipCodes.Where(x => x.Zip == code).First();
                    a.Parsed = true;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    continue;
                }
                
            }
        }

        public void AddPetShop(string source, keydowno_backyard_farmerEntities db)
        {
            int firstPosition = 0;
            firstPosition = source.IndexOf("StoreId");
            int jsonStart = firstPosition - 2;
            int jsonEnd = source.IndexOf("]", firstPosition);

            string jsonString = source.Substring(jsonStart, jsonEnd - jsonStart);

            string[] stringSeparators = new string[] { "},{" };

            string[] stores = jsonString.Split(stringSeparators, StringSplitOptions.None);


            foreach (var store in stores)
            {
                NASAGetData.PetShop petShop = new NASAGetData.PetShop();
                NASAGetData.PetShopsWorkingHour WorkingHours = new NASAGetData.PetShopsWorkingHour();

                petShop.Zip = ParseJSON("PostalCode", store);
                petShop.Name = ParseJSON("Name", store);
                petShop.Address = ParseJSON("Address", store);
                petShop.Phone = ParseJSON("PhoneNumber", store);
                petShop.City = ParseJSON("City", store);
                petShop.Latitude = double.Parse(ParseJSON("Latitude", store));
                petShop.Longitude = double.Parse(ParseJSON("Longitude", store).TrimEnd('}'));

                WorkingHours.MondayStartTime = ParseJSON("OpenTimeMonday",store);
                WorkingHours.MondayEndTime = ParseJSON("CloseTimeMonday", store);
                WorkingHours.TuesdayStartTime = ParseJSON("CloseTimeTuesday", store);
                WorkingHours.TuesdayEndTime = ParseJSON("CloseTimeTuesday", store);
                WorkingHours.WednesdayStartTime = ParseJSON("CloseTimeWednesday", store);
                WorkingHours.WednesdayEndTime = ParseJSON("CloseTimeWednesday", store);
                WorkingHours.ThursdayStartTime = ParseJSON("CloseTimeThursday", store);
                WorkingHours.ThursdayEndTime = ParseJSON("CloseTimeThursday", store);
                WorkingHours.FridayStartTime = ParseJSON("CloseTimeFriday", store);
                WorkingHours.FridayEndTime = ParseJSON("CloseTimeFriday", store);
                WorkingHours.SaturdayStartTime = ParseJSON("CloseTimeSaturday", store);
                WorkingHours.SaturdayEndTime = ParseJSON("CloseTimeSaturday", store);
                WorkingHours.SundayStartTime = ParseJSON("CloseTimeSunday", store);
                WorkingHours.SundayEndTime = ParseJSON("CloseTimeSunday", store);
                
                db.PetShops.Add(petShop);
                //db.SaveChanges();

                WorkingHours.Id = petShop.Id;
                
                db.PetShopsWorkingHours.Add(WorkingHours);
                db.SaveChanges();
                
            }
            
        }

        public static string PerformRequest(string zip)
        {
            string request = "http://www.petco.com/content/locator/SearchResult.aspx?city=&state=&zip="+zip+"&origin=searchResult&veterinary=0&fullGrooming=0&selfGrooming=0&education=0&photography=0&aquatics=0&unleashedbypetco=0&petcostore=0&Nav=2";
            WebRequest req = HttpWebRequest.Create(request);
            req.Method = "GET";

            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }
            return source;
        }
        

        string ParseJSON(string field, string store)
        {
            int tempField = store.IndexOf(field);
            int fieldStart = store.IndexOf(":", tempField) +1;
            int fieldEnd = store.IndexOf(",", tempField);
            string output = store.Substring(fieldStart, fieldEnd - fieldStart);
            output = output.Trim('"');

            return output;
        }

        protected void Result_TextChanged(object sender, EventArgs e)
        {

        }
    }
}