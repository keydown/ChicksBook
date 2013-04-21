using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GetVetsDoctorsInfo
{
    public partial class _Default : Page
    {
        public class ShittyDeadlocks
        {
            public string Zip { get; set; }
            public string City { get; set; }
            public string State { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int errorCounter = 0;

            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();
            //List<string> codes = db.TempZipCodes.Select(x => x.Zip).ToList();
            List<ShittyDeadlocks> items = db.TempZipCodes.Select(i =>
            new ShittyDeadlocks
            {
                Zip = i.Zip,
                City = i.City,
                State = i.State
            }).ToList<ShittyDeadlocks>();
            //var items = db.TempZipCodes.Select(i =>
            //new
            //{
            //    Zip = i.Zip,
            //    City = i.City,
            //    State = i.State
            //});

            foreach (var item in items)
            {
                string zip = item.Zip;
                int zipParsed;
                try
                {
                    zipParsed = int.Parse(zip);
                }
                catch (Exception)
                {
                    continue;
                }
                if (zipParsed < 97220)
                {
                    continue;
                }
                string source = PerformRequest(zip);
                try
                {
                    ParsePage(source, db, item.City, item.State);
                }
                catch (Exception)
                {
                    errorCounter++;
                }
            }

            TextBox1.Text = errorCounter.ToString();
        }

        public static string PerformRequest(string zip)
        {
            string requestString = "http://www.webvet.com/main/vetFinder?zc=" + zip + "&dst=5";
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(requestString);
            req.Method = "GET";

            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }

            return source;
        }

        public void ParsePage(string source, keydowno_backyard_farmerEntities db, string city, string state)
        {
            while (source.IndexOf("// Set up the InfoWindow for this marker.") != -1)
            {
                int start = source.IndexOf("// Set up the InfoWindow for this marker.");
                int end = source.IndexOf("markerArray[maNum] = curMarker;", start);

                string vetInfo = source.Substring(start, end - start);

                source = source.Substring(end);

                //vet phone
                start = vetInfo.IndexOf("<a href=\"tel://");
                end = vetInfo.IndexOf("\">", start);
                string vetPhone = vetInfo.Substring(start + "<a href=\"tel://".Length, end - start - "<a href=\"tel://".Length);
                vetInfo = vetInfo.Substring(end);

                //vet name
                start = vetInfo.IndexOf("curMarker = createMarker(\"");
                end = vetInfo.IndexOf("\",", start);
                string vetName = vetInfo.Substring(start + "curMarker = createMarker(\"".Length, end - start - "curMarker = createMarker(\"".Length);
                vetInfo = vetInfo.Substring(end);

                //vet address
                start = vetInfo.IndexOf("'', \"\", \"");
                end = vetInfo.IndexOf("\",", start + "'', \"\", \"".Length);
                string vetAddress = vetInfo.Substring(start + "'', \"\", \"".Length, end - start - "'', \"\", \"".Length);
                vetInfo = vetInfo.Substring(end);

                string vetZip = vetAddress.Substring(vetAddress.LastIndexOf(' ') + 1);
                try
                {
                    int testZip = int.Parse(vetZip);
                }
                catch (Exception)
                {
                    vetZip = "";
                }

                //vet longitude (strange, but longitude is first here)
                start = vetInfo.IndexOf("'");
                end = vetInfo.IndexOf("',", start);
                string vetLongitude = vetInfo.Substring(start + "'".Length, end - start - "'".Length);
                vetInfo = vetInfo.Substring(end + "',".Length);

                //vet latitude
                start = vetInfo.IndexOf("'");
                end = vetInfo.IndexOf("')", start);
                string vetLatitude = vetInfo.Substring(start + "'".Length, end - start - "'".Length);
                vetInfo = vetInfo.Substring(end);

                Vet doctor = new Vet();
                doctor.Name = vetName;
                doctor.Address = vetAddress;
                doctor.City = city;
                doctor.State = state;
                doctor.Zip = vetZip;
                doctor.Phone = vetPhone;

                doctor.Latitude = double.Parse(vetLatitude, CultureInfo.InvariantCulture);
                doctor.Longitude = double.Parse(vetLongitude, CultureInfo.InvariantCulture);
                db.Vets.Add(doctor);
                db.SaveChanges();
            }
        }

        //public string ParseWorkingHours(string input, string day)
        //{
        //    int subStart = input.IndexOf("<table");
        //    int subEnd = input.IndexOf("</table>");
        //    string subStr = input.Substring(subStart, subEnd - subStart);

        //    int temp = subStr.IndexOf(day);
        //    int dayStart = subStr.IndexOf("<td>", temp) + 4;
        //    int dayEnd = subStr.IndexOf("</td>",dayStart);

        //    string workHours = subStr.Substring(dayStart, dayEnd - dayStart);

        //    return workHours;
        //}

        //string[] ParseCoordinates(string input)
        //{
        //    int locationEnd = input.IndexOf(")");
        //    string locationsStr = input.Substring(1, locationEnd - 1);
        //    string[] coords = locationsStr.Split(',');
        //    coords[0] = coords[0].Trim('"');
        //    coords[1] = coords[1].Trim('"');           
        //    return coords;
        //}

        //int GetPagesNum(string input) // input result[len-1] as there are the number of pages string
        //{
        //    int temp = input.IndexOf("More Vets:");
        //    int star = temp + 10;
        //    int end = input.IndexOf("</div>", star);
        //    string tempStr = input.Substring(star, end - star);

        //    int pagesPos = tempStr.LastIndexOf("nofollow");
        //    int pagesStart = tempStr.IndexOf(">", pagesPos) + 1;
        //    int numPages = int.Parse(tempStr.Substring(pagesStart, 2));

        //    return numPages;
        //}

        //string GetImg(string input)
        //{
        //    int star = input.IndexOf("img src") + 9;
        //    int end = input.IndexOf("\"", star);
        //    string substr = input.Substring(star, end - star);
        //    return substr;
        //}

        //string ParseData(string input, string toSearch) //tosearch is v:address or something like that//start from index 1 of the array as 0 is not applicable
        //{
        //    int temp = input.IndexOf(toSearch);
        //    int star = input.IndexOf(">", temp) + 1;
        //    int ender = input.IndexOf("</", star);
        //    string subst = input.Substring(star, ender - star);

        //    return subst;
        //}

    }
}
