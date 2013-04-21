using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace GetVets
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetVets_Click(object sender, EventArgs e)
        {
            string source = PerformRequest("http://www.localvets.com/search?zip=33323");

            ParsePage(source);
            //int numPages = GetPagesNum(source);

            //for (int i = 1; i <= numPages; i++)
            //{
            //    string requestString = "http://www.localvets.com/search?start=" + i * 10 + "&zip=33323";
            //    string output = PerformRequest(requestString);
            //    ParsePage(output);
            //}


        }

        public static string PerformRequest(string requestString)
        {
            WebRequest req = HttpWebRequest.Create(requestString);
            req.Method = "GET";

            string source;
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }
            Thread.Sleep(100);

            return source;
        }

        public void ParsePage(string source)
        {
            int start = source.IndexOf("<div id=\"searchresults\">");
            int end = source.IndexOf("<script type=\"text/javascript\">", start);

            string substr = source.Substring(start, end - start);

            string[] stringSeparators = new string[] { "http://rdf.data-vocabulary.org/" }; // separate the different vetshops
            string[] htmlText;
            htmlText = substr.Split(stringSeparators, StringSplitOptions.None); //string with vetshops
            //Result.Text = parsePages(result[len - 1]).ToString();

            // coords parser start here

            int pointsTemp = source.IndexOf("var point");
            int pointsStart = source.IndexOf(";", pointsTemp);
            int pointsEnd = source.IndexOf("map.bestFit");
            string pointsSubstr = source.Substring(pointsStart, pointsEnd - pointsStart);

            string[] pointSeparator = new string[] { "MQA.LatLng" }; // separate all the coordinates
            string[] pointsResult;
            pointsResult = pointsSubstr.Split(pointSeparator, StringSplitOptions.None); // string with coordinates

            for (int i = 1; i < htmlText.Length; i++)
            {
                string name = ParseData(htmlText[i], "v:name");
                string address = ParseData(htmlText[i], "v:street-address");
                string city = ParseData(htmlText[i], "v:locality");
                string state = ParseData(htmlText[i], "v:region");
                string zip = ParseData(htmlText[i], "v:postal-code");
                string phone = ParseData(htmlText[i], "v:tel");
                string img = GetImg(htmlText[i]);
                double latitude = double.Parse(ParseCoordinates(pointsResult[i])[0]);
                double longitude = double.Parse(ParseCoordinates(pointsResult[i])[1]);
            }

            string res = ParseWorkingHours(htmlText[4], "Sat")[1];
            Result.Text = res;

        }

        public string[] ParseWorkingHours(string input, string day)
        {
            int subStart = input.IndexOf("<table");
            int subEnd = input.IndexOf("</table>");
            string subStr = input.Substring(subStart, subEnd - subStart);

            int temp = subStr.IndexOf(day);
            int dayStart = subStr.IndexOf("<td>", temp)+4;
            int dayEnd = dayStart + 6;
            string open = subStr.Substring(dayStart, dayEnd - dayStart);

            int dayCloseStart = dayEnd + 3;
            int dayCloseEnd = dayCloseStart + 6;
            string close = subStr.Substring(dayCloseStart, dayCloseEnd - dayCloseStart);

            string[] openCloseHours = new string[2];
            openCloseHours[0] = open;
            openCloseHours[1] = close;

            return openCloseHours;
        }

        string[] ParseCoordinates(string input)
        {
            int locationEnd = input.IndexOf(")");
            string locationsStr = input.Substring(1, locationEnd - 1);
            string[] coords = locationsStr.Split(',');
            //double latitude = double.Parse(coords[0]);
            //double longitude = double.Parse(coords[1]);            
            return coords;
        }

        int GetPagesNum(string input) // input result[len-1] as there are the number of pages string
        {
            int temp = input.IndexOf("More Vets:");
            int star = temp + 10;
            int end = input.IndexOf("</div>",star);
            string tempStr = input.Substring(star, end - star);

            int pagesPos = tempStr.LastIndexOf("nofollow");
            int pagesStart = tempStr.IndexOf(">", pagesPos)+1;
            int numPages = int.Parse(tempStr.Substring(pagesStart,2));

            return numPages;
        }

        string GetImg(string input)
        {
            int star = input.IndexOf("img src") + 9;
            int end = input.IndexOf("\"",star);
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

        //Get Chicken Ordinances
        protected void GetLaws_Click(object sender, EventArgs e)
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            string source = PerformRequest("http://www.backyardchickens.com/atype/3/Laws");

            //Get number of records.
            int start = source.IndexOf("<div class=\"actionbar shazam\">");
            int end = source.IndexOf("law / ordinance", start);

            string pagingBlock = source.Substring(start, end - start);

            start = pagingBlock.IndexOf(" of ");
            end = pagingBlock.IndexOf("\">", start);

            string recordsNumberBlock = pagingBlock.Substring(start + " of ".Length, end - start - " of ".Length);
            int recordsNumber = int.Parse(recordsNumberBlock);

            List<string> articleLinks = new List<string>(recordsNumber);

            //Get data for every page.
            for (int pageBlock = 0; pageBlock < recordsNumber; pageBlock += 10)
            {
                string innerUrl = "http://www.backyardchickens.com/atype/3/Laws/page/" + pageBlock.ToString();
                source = PerformRequest(innerUrl);

                GetLawArticleLinks(source, articleLinks);
            }

            foreach (var link in articleLinks)
            {
                try
                {
                    GetSpecificLawTable(link, db);
                }
                //That site is just awful.
                catch (Exception)
                {
                }
            }

            lblProgress.Text = String.Format("{0} of {1} records processed.", articleLinks.Count, recordsNumber);
            ResultLaws.Text = articleLinks[articleLinks.Count - 1];
        }

        //Get a url for every location.
        void GetLawArticleLinks(string source, List<string> articleLinks)
        {
            int start = source.IndexOf("<div class=\"articles\">");
            int end = source.IndexOf("<div class=\"actionbar shazam\">", start);

            string articlesBlock = source.Substring(start, end - start);

            start = 0;
            end = 0;
            string articleLink = "";

            while (articlesBlock.IndexOf("<a href=\"/a/", start) != -1)
            {
                start = articlesBlock.IndexOf("<a href=\"/a/", end);

                if (start < 0)
                    break;
                end = articlesBlock.IndexOf("\">", start);

                articleLink = "http://www.backyardchickens.com" + articlesBlock.Substring(start + "<a href=\"".Length, end - start - "<a href=\"".Length);
                articleLinks.Add(articleLink);
            }
        }

        //Get the law table for a specific location.
        void GetSpecificLawTable(string link, keydowno_backyard_farmerEntities db)
        {
            string source = PerformRequest(link);

            int start = source.IndexOf("<table");
            int end = source.IndexOf("</table>");

            if (start != -1)
            {
                string tableBlock = source.Substring(start, end - start);

                ChickenLaws law = new ChickenLaws();

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td") + 1));
                end = tableBlock.IndexOf("</td>", start);
                string areChickensAllowed = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                areChickensAllowed = StripHtmlTags(areChickensAllowed);


                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string maxChickensAllowed = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                maxChickensAllowed = StripHtmlTags(maxChickensAllowed);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string roostersAllowed = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                roostersAllowed = StripHtmlTags(roostersAllowed);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string permitRequired = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                permitRequired = StripHtmlTags(permitRequired);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string coopRestrictions = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                coopRestrictions = StripHtmlTags(coopRestrictions);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string contacts = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                contacts = StripHtmlTags(contacts);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string info = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                info = StripHtmlTags(info);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string contactLink = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                contactLink = StripHtmlTags(contactLink);

                start = tableBlock.IndexOf(">", tableBlock.IndexOf("<td", tableBlock.IndexOf("<td", start + 1) + 1));
                end = tableBlock.IndexOf("</td>", start);
                string lastUpdate = tableBlock.Substring(start + ">".Length, end - start - ">".Length);
                lastUpdate = StripHtmlTags(lastUpdate);

                int linkStart = link.LastIndexOf("/");
                string googleJsonParam = link.Substring(linkStart + 1, link.Length - (linkStart + 1));
                googleJsonParam = googleJsonParam.Replace('-', '+');
                googleJsonParam = googleJsonParam.Replace("+chicken+ordinance", "");
                string googleJson = PerformRequest(String.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=true", googleJsonParam));
                int locationStart = googleJson.IndexOf("formatted_address");
                if (locationStart != -1)
                {
                    int locationEnd = googleJson.IndexOf("\",", locationStart);
                    googleJson = googleJson.Substring(locationStart + "formatted_address\" : \"".Length, locationEnd - locationStart - "formatted_address\" : \"".Length);
                }
                else
                {
                    googleJson = googleJsonParam.Replace('+', ' ');
                }

                law.Location = googleJson;
                law.AreChickensAllowed = areChickensAllowed;
                law.ChickensAllowed = maxChickensAllowed;
                law.RoostersAllowed = roostersAllowed;
                law.PermitRequired = permitRequired;
                law.CoopRestrictions = coopRestrictions;
                law.Contacts = contacts;
                law.Info = info;
                law.Link = contactLink;
                law.LastUpdate = lastUpdate;

                db.ChickenLaws.Add(law);
                db.SaveChanges();
            }
            else
            {
                return;
            }

            //We have to somehow match the town/region/state with our existing ZipCodes table data.

            //Code to insert html table elements into Database.
        }

        string StripHtmlTags(string text)
        {
            int subStart = 0;
            int subEnd = 0;

            while (text.IndexOf("<") != -1)
            {
                subStart = text.IndexOf("<");
                subEnd = text.IndexOf(">", subStart);
                text = text.Remove(subStart, (subEnd - subStart) + 1);
            }

            return text;
        }
    }
}