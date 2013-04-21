using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Globalization;
using System.Data.Objects;

namespace BYFarmerConsoleServices
{

    class Program
    {

        static void Main(string[] args)
        {
            // Get all the upcoming events from wht apppa.org website
            string url = "http://www.apppa.org/dynamic_content/xml/rss.xml";
            XmlDocument xml = new XmlDocument();
            xml.Load(url);

            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            XmlNodeList xnList = xml.SelectNodes("//item");
            foreach (XmlNode xn in xnList)
            {
                Event evt = new Event();

                XmlNode titleNode = xn.FirstChild;
                string cont = titleNode.InnerText;
                evt.Title = cont;

                XmlNode descNode = titleNode.NextSibling;
                string desc = descNode.InnerText;
                string descDecoded = WebUtility.HtmlDecode(Regex.Replace(desc, "<[^>]*(>|$)", string.Empty));
                evt.Description = descDecoded;

                XmlNode linkNode = descNode.NextSibling;
                string link = linkNode.InnerText;
                evt.Link = link;

                XmlNode dateNode = linkNode.NextSibling;
                string date = dateNode.InnerText;
                DateTime scheduledDate = DateTime.Parse(date);
                evt.Date = scheduledDate;

                Event evtFound = db.Events.SingleOrDefault(x => x.Link == evt.Link);

                if (evtFound == null)
                {
                    db.Events.Add(evt);
                    db.SaveChanges();
                }

                //Gets weather information for today
            }
            DateTime dateToday = DateTime.Today;
            double unixTime = (dateToday - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;

            List<User> users = db.Users.Where(x => x.Id >= 0).ToList();

            foreach (var user in users)
            {
                string request = "https://api.forecast.io/forecast/0d7b11fc893390123355ce88a9dd887d/" + user.LastLat.ToString() + "," + user.LastLong.ToString() + "," + unixTime.ToString();
                WebRequest req = HttpWebRequest.Create(request);
                req.Method = "GET";

                WeatherForecast wf = new WeatherForecast();
                wf.Time = dateToday;
                wf.UserId = user.Id;

                string source;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }
                int start = source.IndexOf("currently");
                int end = source.IndexOf("hourly");
                string subst = source.Substring(start, end - start);

                int summaryStart = subst.IndexOf("summary") + 10;
                int SummaryEnd = subst.IndexOf(",", summaryStart) - 1;
                string summary = subst.Substring(summaryStart, SummaryEnd - summaryStart);
                wf.Summary = summary;

                int temperatureStart = subst.IndexOf("temperature") + 13;
                int temperatureEnd = subst.IndexOf(",", temperatureStart) - 1;
                string temperature = subst.Substring(temperatureStart, temperatureEnd - temperatureStart);
                wf.Temperature = double.Parse(temperature, CultureInfo.InvariantCulture);

                int windSpeedStart = subst.IndexOf("windSpeed") + 11;
                int windSpeedEnd = subst.IndexOf(",", windSpeedStart);
                string windSpeed = subst.Substring(windSpeedStart, windSpeedEnd - windSpeedStart);
                wf.WindSpeed = double.Parse(windSpeed, CultureInfo.InvariantCulture);

                int windBearingStart = subst.IndexOf("windBearing") + 13;
                int windBearingEnd = subst.IndexOf(",", windBearingStart);
                string windBearing = subst.Substring(windBearingStart, windBearingEnd - windBearingStart);
                wf.WindBearing = double.Parse(windBearing);

                int cloudCoverStart = subst.IndexOf("cloudCover") + 12;
                int cloudCoverEnd = subst.IndexOf(",", cloudCoverStart);
                string cloudCover = subst.Substring(cloudCoverStart, cloudCoverEnd - cloudCoverStart);
                wf.CloudCover = double.Parse(cloudCover, CultureInfo.InvariantCulture);

                int humidityStart = subst.IndexOf("humidity") + 10;
                int humidityEnd = subst.IndexOf(",", humidityStart);
                string humidity = subst.Substring(humidityStart, humidityEnd - humidityStart);
                wf.Humidity = double.Parse(humidity, CultureInfo.InvariantCulture);

                int pressureStart = subst.IndexOf("pressure") + 10;
                int pressureEnd = subst.IndexOf(",", pressureStart);
                string pressure = subst.Substring(pressureStart, pressureEnd - pressureStart);
                wf.Pressure = double.Parse(pressure, CultureInfo.InvariantCulture);

                db.WeatherForecasts.Add(wf);
                db.SaveChanges();
            }

            GenerateUserTasks();

            Environment.Exit(0);
        }

        private static void GenerateUserTasks()
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            List<UserDTO> users = db.Users.Select(x =>
                                                    new UserDTO
                                                    {
                                                        Id = x.Id,
                                                        Username = x.Username,
                                                        AuthCode = x.AuthCode,
                                                        SessionId = x.SessionId,
                                                        DateRegistered = x.DateRegistered,
                                                        LastLogin = x.LastLogin,
                                                        FirstName = x.FirstName,
                                                        LastName = x.LastName,
                                                        Email = x.Email,
                                                        LastLat = x.LastLat,
                                                        LastLong = x.LastLong
                                                    }).ToList<UserDTO>();

            foreach (UserDTO user in users)
            {
                //Gets the number of pets for the current user.
                int userPetsCount = db.UserFarms.Where(x => x.UserId == user.Id).Select(x => (int?)x.PetCount).Sum() ?? 0;

                AddTaskFeedChicks(user.Id, db, userPetsCount);
                AddTaskCheckWater(user.Id, db, userPetsCount);
                AddTaskCleanCoop(user.Id, db, userPetsCount);
                AddTaskReleaseChicks(user.Id, db, userPetsCount);
                AddTaskBuyFood(user.Id, db, userPetsCount);
                AddTaskTurnEggs(user.Id, db);
                AddTaskCheckTemperature(user.Id, db);
                AddTaskCollectEggs(user.Id, db, userPetsCount);
            }
        }

        private static void GenerateNewUserTasks(int userId)
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            Calendar newCalendarEntry = new Calendar();

            Task updateFarmTask = db.Tasks.SingleOrDefault(x => x.Name == "Update Farm Information");

            newCalendarEntry.TaskId = updateFarmTask.Id;
            newCalendarEntry.TaskCompleted = false;
            newCalendarEntry.UserId = userId;

            db.Calendars.Add(newCalendarEntry);
            db.SaveChanges();
        }

        private static void AddTaskFeedChicks(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            int feedChicksHour = 9;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                Calendar newCalendarEntry = new Calendar();

                Task feedChicksTask = db.Tasks.SingleOrDefault(x => x.Name == "Feed Chicks");

                newCalendarEntry.TaskId = feedChicksTask.Id;
                newCalendarEntry.TaskCompleted = false;
                newCalendarEntry.Date = DateTime.Today.AddHours(feedChicksHour);
                newCalendarEntry.UserId = userId;

                db.Calendars.Add(newCalendarEntry);
                db.SaveChanges();
            }
        }

        private static void AddTaskCheckWater(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            int waterFirstCheckHour = 9;
            int waterSecondCheckHour = 16;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                Calendar newCalendarEntryFirst = new Calendar();
                Calendar newCalendarEntrySecond = new Calendar();

                Task checkWaterTask = db.Tasks.SingleOrDefault(x => x.Name == "Check Water");

                newCalendarEntryFirst.TaskId = checkWaterTask.Id;
                newCalendarEntryFirst.TaskCompleted = false;
                newCalendarEntryFirst.Date = DateTime.Today.AddHours(waterFirstCheckHour);
                newCalendarEntryFirst.UserId = userId;

                db.Calendars.Add(newCalendarEntryFirst);

                newCalendarEntrySecond.TaskId = checkWaterTask.Id;
                newCalendarEntrySecond.TaskCompleted = false;
                newCalendarEntrySecond.Date = DateTime.Today.AddHours(waterSecondCheckHour);
                newCalendarEntrySecond.UserId = userId;

                db.Calendars.Add(newCalendarEntrySecond);

                db.SaveChanges();
            }
        }

        private static void AddTaskCleanCoop(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            int cleanHour = 9;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                Calendar newCalendarEntry = new Calendar();

                Task cleanCoopTask = db.Tasks.SingleOrDefault(x => x.Name == "Clean the Coop");

                newCalendarEntry.TaskId = cleanCoopTask.Id;
                newCalendarEntry.TaskCompleted = false;
                newCalendarEntry.Date = DateTime.Today.AddHours(cleanHour);
                newCalendarEntry.UserId = userId;

                db.Calendars.Add(newCalendarEntry);
                db.SaveChanges();
            }
        }

        private static void AddTaskReleaseChicks(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            double releaseHour = 9.5;
            double windSpeedThreshold = 40;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                //Do not release chicks if the weather is bad and the wind is too strong.
                List<WeatherForecast> userWeatherForecast = db.WeatherForecasts.Where(x => x.UserId == userId && x.Time >= DateTime.Today)
                                                                               .OrderByDescending(x => x.Time)
                                                                               .ToList<WeatherForecast>();
                if (userWeatherForecast.Count > 0)
                {
                    if (userWeatherForecast[0].WindSpeed < windSpeedThreshold)
                    {
                        Calendar newCalendarEntry = new Calendar();

                        Task releaseChicksTask = db.Tasks.SingleOrDefault(x => x.Name == "Release the Chicks");

                        newCalendarEntry.TaskId = releaseChicksTask.Id;
                        newCalendarEntry.TaskCompleted = false;
                        newCalendarEntry.Date = DateTime.Today.AddHours(releaseHour);
                        newCalendarEntry.UserId = userId;

                        db.Calendars.Add(newCalendarEntry);
                        db.SaveChanges();
                    }
                }
                else
                {
                    Calendar newCalendarEntry = new Calendar();

                    Task releaseChicksTask = db.Tasks.SingleOrDefault(x => x.Name == "Release the Chicks");

                    newCalendarEntry.TaskId = releaseChicksTask.Id;
                    newCalendarEntry.TaskCompleted = false;
                    newCalendarEntry.Date = DateTime.Today.AddHours(releaseHour);
                    newCalendarEntry.UserId = userId;

                    db.Calendars.Add(newCalendarEntry);
                    db.SaveChanges();
                }
            }
        }

        //Should be triggered after successful completion of "Release the Chicks" task.
        private static void AddTaskCollectChicks(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            double releaseHour = 18;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                Calendar newCalendarEntry = new Calendar();

                Task collectChicksTask = db.Tasks.SingleOrDefault(x => x.Name == "Collect the Chicks");

                newCalendarEntry.TaskId = collectChicksTask.Id;
                newCalendarEntry.TaskCompleted = false;
                newCalendarEntry.Date = DateTime.Today.AddHours(releaseHour);
                newCalendarEntry.UserId = userId;

                db.Calendars.Add(newCalendarEntry);
                db.SaveChanges();
            }
        }

        //Should be triggered after adding new chickens to the farm.
        private static void AddTaskVaccination(int userId, keydowno_backyard_farmerEntities db, int utcTimeDifference)
        {
            Calendar newCalendarEntry = new Calendar();

            Task vaccinationTask = db.Tasks.SingleOrDefault(x => x.Name == "Vaccination");

            newCalendarEntry.TaskId = vaccinationTask.Id;
            newCalendarEntry.TaskCompleted = false;
            newCalendarEntry.Date = DateTime.UtcNow.AddHours(utcTimeDifference);
            newCalendarEntry.UserId = userId;

            db.Calendars.Add(newCalendarEntry);
            db.SaveChanges();
        }

        private static void AddTaskBuyFood(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            double taskHour = 9;
            double foodThreshold;
            double currentFoodAmount = 0;
            bool addTaskFlag = false;
            //Gets the number of pets for the current user.
            //int petsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => x.PetCount).Sum();

            //Checks if the user has pets.
            if (petsCount > 0)
            {
                //Get the food asset.
                Asset foodAsset = db.Assets.SingleOrDefault(x => x.Name == "Food");

                //Find the current amount of food the user has.
                currentFoodAmount = db.UserAssets.Where(x => x.UserId == userId &&
                                                             x.AssetId == foodAsset.Id)
                                                 .Select(x => x.CurrentQuantity)
                                                 .FirstOrDefault();

                //Find the last 10 records of removing food from the assets table and calculate the total food removed for these records.
                List<double> foodCountHistory = db.UserAssetsHistories.Where(x => x.UserId == userId && 
                                                                                  x.AssetStatus == "LOSE" && 
                                                                                  x.AssetId == foodAsset.Id)
                                                                      .OrderByDescending(x => x.ActionDate)
                                                                      .Select(x => x.AssetCount)
                                                                      .Take(10)
                                                                      .ToList();
                
                //Calculates the average food dosage for the last 10 feedings and uses it as a threshold.
                if (foodCountHistory.Count > 0)
                {
                    foodThreshold = foodCountHistory.Sum() / foodCountHistory.Count;

                    if (currentFoodAmount < foodThreshold)
                    {
                        addTaskFlag = true;
                    }
                }
                else
                {
                    double lastAddedFoodCount = db.UserAssetsHistories.Where(x => x.UserId == userId &&
                                                                                  x.AssetStatus == "ADD" &&
                                                                                  x.AssetId == foodAsset.Id)
                                                                      .OrderByDescending(x => x.ActionDate)
                                                                      .Select(x => x.AssetCount)
                                                                      .FirstOrDefault();

                    //Sets the threshold to 20% of the last bought amount of food.
                    if (lastAddedFoodCount > 0)
                    {
                        double thresholdPercentage = 0.2;

                        foodThreshold = lastAddedFoodCount * thresholdPercentage;

                        if (currentFoodAmount < foodThreshold)
                        {
                            addTaskFlag = true;
                        }
                    }
                    //User never bought food but has chickens, so the user needs to buy food.
                    else
                    {
                        addTaskFlag = true;
                    }
                }

                if (addTaskFlag)
                {
                    Calendar newCalendarEntry = new Calendar();

                    Task buyFoodTask = db.Tasks.SingleOrDefault(x => x.Name == "Buy Food");

                    newCalendarEntry.TaskId = buyFoodTask.Id;
                    newCalendarEntry.TaskCompleted = false;
                    newCalendarEntry.Date = DateTime.Today.AddHours(taskHour);
                    newCalendarEntry.UserId = userId;

                    db.Calendars.Add(newCalendarEntry);
                    db.SaveChanges();
                }
            }
        }

        private static void AddTaskTurnEggs(int userId, keydowno_backyard_farmerEntities db)
        {
            int turnEggsPeriod = 17;
            int turnEggsFirstHour = 8;
            int turnEggsSecondHour = 14;
            int turnEggsThirdHour = 20;

            //Gets the number of hatching eggs for the current user.
            int userHatchingEggsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => (int?)x.EggsHatching).Sum() ?? 0;
            DateTime turnEggsStartDate = DateTime.Now.AddDays(-turnEggsPeriod);

            //Get the last date of adding hatching eggs to the farm.
            List<UserFarmHistory> hatchingEggsHistory = db.UserFarmHistories.Where(x => x.UserId == userId &&
                                                                                        x.PetStatus == "ADDP" &&
                                                                                        x.ActionDate > turnEggsStartDate && 
                                                                                        x.EggsHatching > 0)
                                                                      .OrderByDescending(x => x.ActionDate)
                                                                      .ToList<UserFarmHistory>();
            //If the user added hatching eggs to the farm in the last 17 days, then he needs to turn them.
            //He must not turn the eggs during the last 3 days of the hatching period.
            if (hatchingEggsHistory.Count > 0)
            {
                //Checks if the user currently has hatching eggs.
                if (userHatchingEggsCount > 0)
                {
                    Calendar newCalendarEntryFirst = new Calendar();
                    Calendar newCalendarEntrySecond = new Calendar();
                    Calendar newCalendarEntryThird = new Calendar();

                    Task turnEggsTask = db.Tasks.SingleOrDefault(x => x.Name == "Turn Eggs");

                    newCalendarEntryFirst.TaskId = turnEggsTask.Id;
                    newCalendarEntryFirst.TaskCompleted = false;
                    newCalendarEntryFirst.Date = DateTime.Today.AddHours(turnEggsFirstHour);
                    newCalendarEntryFirst.UserId = userId;

                    db.Calendars.Add(newCalendarEntryFirst);

                    newCalendarEntrySecond.TaskId = turnEggsTask.Id;
                    newCalendarEntrySecond.TaskCompleted = false;
                    newCalendarEntrySecond.Date = DateTime.Today.AddHours(turnEggsSecondHour);
                    newCalendarEntrySecond.UserId = userId;

                    db.Calendars.Add(newCalendarEntrySecond);

                    newCalendarEntryThird.TaskId = turnEggsTask.Id;
                    newCalendarEntryThird.TaskCompleted = false;
                    newCalendarEntryThird.Date = DateTime.Today.AddHours(turnEggsThirdHour);
                    newCalendarEntryThird.UserId = userId;

                    db.Calendars.Add(newCalendarEntryThird);

                    db.SaveChanges();
                }
            }
        }

        private static void AddTaskCheckTemperature(int userId, keydowno_backyard_farmerEntities db)
        {
            int checkTemperatureHour = 9;

            //Gets the number of hatching eggs for the current user.
            int userHatchingEggsCount = db.UserFarms.Where(x => x.UserId == userId).Select(x => (int?)x.EggsHatching).Sum() ?? 0;

            //Checks if the user currently has hatching eggs.
            if (userHatchingEggsCount > 0)
            {
                Calendar newCalendarEntry = new Calendar();

                Task checkTemperatureTask = db.Tasks.SingleOrDefault(x => x.Name == "Check Temperature");

                newCalendarEntry.TaskId = checkTemperatureTask.Id;
                newCalendarEntry.TaskCompleted = false;
                newCalendarEntry.Date = DateTime.Today.AddHours(checkTemperatureHour);
                newCalendarEntry.UserId = userId;

                db.Calendars.Add(newCalendarEntry);

                db.SaveChanges();
            }
        }

        private static void AddTaskCollectEggs(int userId, keydowno_backyard_farmerEntities db, int petsCount)
        {
            double collectEggsHour = 9.75;

            //Checks if the user currently has pets.
            if (petsCount > 0)
            {
                Calendar newCalendarEntry = new Calendar();

                Task collectEggsTask = db.Tasks.SingleOrDefault(x => x.Name == "Collect Eggs");

                newCalendarEntry.TaskId = collectEggsTask.Id;
                newCalendarEntry.TaskCompleted = false;
                newCalendarEntry.Date = DateTime.Today.AddHours(collectEggsHour);
                newCalendarEntry.UserId = userId;

                db.Calendars.Add(newCalendarEntry);

                db.SaveChanges();
            }
        }
    }
}