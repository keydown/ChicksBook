using System;
using System.Collections.Generic;
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();
            List<string> codes = db.TempZipCodes.Select(x => x.Zip).ToList();

            foreach (var code in codes)
            {
                try
                {
                Thread.Sleep(1000);
                string zip = code.ToString();
                string source = PerformRequest(zip);
                ParsePage(source, db);
                int numPages = GetPagesNum(source);
                Thread.Sleep(1000);
                

               
                var a = db.TempZipCodes.Where(x => x.Zip == code).First();
                a.Parsed = true;
                db.SaveChanges();
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }         

        public static string PerformRequest(string zip)
        {

            string requestString = "http://www.localvets.com/search?zip="+zip;
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(requestString);
            req.Method = "GET";

            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }

            return source;
        }

        public void ParsePage(string source, keydowno_backyard_farmerEntities db)
        {
            int start = source.IndexOf("<div id=\"searchresults\">");
            int end = source.IndexOf("<script type=\"text/javascript\">", start);

            string substr = source.Substring(start, end - start);

            string[] stringSeparators = new string[] { "http://rdf.data-vocabulary.org/" }; // separate the different vetshops
            string[] htmlText;
            htmlText = substr.Split(stringSeparators, StringSplitOptions.None); //string with vetshops

            // coords parser start here

            int pointsTemp = source.IndexOf("var point");
            int pointsStart = source.IndexOf(";", pointsTemp);
            int pointsEnd = source.IndexOf("map.bestFit");
            string pointsSubstr = source.Substring(pointsStart, pointsEnd - pointsStart);

            string[] pointSeparator = new string[] { "MQA.LatLng" }; // separate all the coordinates
            string[] pointsResult;
            pointsResult = pointsSubstr.Split(pointSeparator, StringSplitOptions.None); // string with coordinates

           // return htmlText[2];

            for (int i = 1; i < htmlText.Length - 1; i++)
            {
                Vet doctor = new Vet();
                VetsWorkingHour workHours = new VetsWorkingHour();

                doctor.Name = ParseData(htmlText[i], "v:name");
                doctor.Address = ParseData(htmlText[i], "v:street-address");
                doctor.City = ParseData(htmlText[i], "v:locality");
                doctor.State = ParseData(htmlText[i], "v:region");
                doctor.Zip = ParseData(htmlText[i], "v:postal-code");
                doctor.Phone = ParseData(htmlText[i], "v:tel");
                doctor.Image = GetImg(htmlText[i]);

                doctor.Latitude = double.Parse(ParseCoordinates(pointsResult[i])[0]);
                doctor.Longitude = double.Parse(ParseCoordinates(pointsResult[i])[1]);


                
                workHours.Monday = ParseWorkingHours(htmlText[i], "Mon");
                workHours.Tuesday = ParseWorkingHours(htmlText[i], "Tue");
                workHours.Wednesday = ParseWorkingHours(htmlText[i], "Wed");
                workHours.Thursday = ParseWorkingHours(htmlText[i], "Thu");
                workHours.Friday = ParseWorkingHours(htmlText[i], "Fri");
                workHours.Saturday = ParseWorkingHours(htmlText[i], "Sat");
                workHours.Sunday = ParseWorkingHours(htmlText[i], "Sun");
                
                db.Vets.Add(doctor);
                workHours.Id = doctor.Id;
                db.VetsWorkingHours.Add(workHours);
                db.SaveChanges();
            }

        }

        public string ParseWorkingHours(string input, string day)
        {
            int subStart = input.IndexOf("<table");
            int subEnd = input.IndexOf("</table>");
            string subStr = input.Substring(subStart, subEnd - subStart);

            int temp = subStr.IndexOf(day);
            int dayStart = subStr.IndexOf("<td>", temp) + 4;
            int dayEnd = subStr.IndexOf("</td>",dayStart);

            string workHours = subStr.Substring(dayStart, dayEnd - dayStart);

            return workHours;
        }

        string[] ParseCoordinates(string input)
        {
            int locationEnd = input.IndexOf(")");
            string locationsStr = input.Substring(1, locationEnd - 1);
            string[] coords = locationsStr.Split(',');
            coords[0] = coords[0].Trim('"');
            coords[1] = coords[1].Trim('"');           
            return coords;
        }

        int GetPagesNum(string input) // input result[len-1] as there are the number of pages string
        {
            int temp = input.IndexOf("More Vets:");
            int star = temp + 10;
            int end = input.IndexOf("</div>", star);
            string tempStr = input.Substring(star, end - star);

            int pagesPos = tempStr.LastIndexOf("nofollow");
            int pagesStart = tempStr.IndexOf(">", pagesPos) + 1;
            int numPages = int.Parse(tempStr.Substring(pagesStart, 2));

            return numPages;
        }

        string GetImg(string input)
        {
            int star = input.IndexOf("img src") + 9;
            int end = input.IndexOf("\"", star);
            string substr = input.Substring(star, end - star);
            return substr;
        }

        string ParseData(string input, string toSearch) //tosearch is v:address or something like that//start from index 1 of the array as 0 is not applicable
        {
            int temp = input.IndexOf(toSearch);
            int star = input.IndexOf(">", temp) + 1;
            int ender = input.IndexOf("</", star);
            string subst = input.Substring(star, ender - star);

            return subst;
        }

        }
    }
