using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using BYServices.Models;
using System.Data.Objects;
using System.Web.Http;

namespace BYServices.Controllers
{
    public class CalendarController : Operations
    {

        BYEntities db = new BYEntities();

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetDateInfoByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                DateTime date = obj["date"].Value<DateTime>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                TaskListModel tasks = TasksController.GetTasksListByDate(date, currentUser);

                List<UserAssetsHistory> tempAssets = db.UserAssetsHistories.Where(x => EntityFunctions.TruncateTime(x.ActionDate) == date && x.UserId == currentUser.Id).ToList();
                List<UserAssetHistoryModel> assets = new List<UserAssetHistoryModel>(); 
                foreach (UserAssetsHistory asset in tempAssets)
                {
                    assets.Add(new UserAssetHistoryModel(asset));
                }

                List<UserTraining> tempTrainings = db.UserTrainings.Where(x => EntityFunctions.TruncateTime(x.DateCompleted) == date && x.UserId == currentUser.Id).ToList();
                List<TrainingsModel> trainings = new List<TrainingsModel>();
                foreach (UserTraining training in tempTrainings)
                {
                    trainings.Add(new TrainingsModel(training));
                }

                List<UserFarmHistory> tempFarm = db.UserFarmHistories.Where(x => EntityFunctions.TruncateTime(x.ActionDate) == date && x.UserId == currentUser.Id).ToList();
                List<UserFarmHistoryModel> farm = new List<UserFarmHistoryModel>();
                foreach (UserFarmHistory farmHistory in tempFarm)
                {
                    farm.Add(new UserFarmHistoryModel(farmHistory));
                }

                List<Event> tempEvent = db.Events.Where(x => EntityFunctions.TruncateTime(x.Date) == date).ToList();
                List<EventModel> events = new List<EventModel>();
                foreach (Event evt in tempEvent)
                {
                    events.Add(new EventModel(evt));
                }

                return new CalendarModel(date, tasks, assets, trainings, farm, events);
            });

            return msg;
        }
    }
}
