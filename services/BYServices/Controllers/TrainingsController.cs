using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{
    public class TrainingsController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetTrainingInfo(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                int trainingId = obj["trainingId"].Value<int>();
                ValidateSessionId(sessionId);

                Training tempTraining = db.Trainings.SingleOrDefault(x => x.Id == trainingId);
                if (tempTraining == null)
                {
                    throw BuildHttpResponseException("No such training ID", "ERR_ID");
                }
                return new TrainingsModel(tempTraining);
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetTrainingsByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
                
                List<Training> tempUncTrainings = db.Trainings.Where(x => !db.UserTrainings.Any(y => y.TrainingId == x.Id && y.UserId == currentUser.Id)).ToList();
                List<TrainingsModel> uncompletedTrainings = new List<TrainingsModel>();
                foreach (Training trn in tempUncTrainings)
                {
                    uncompletedTrainings.Add(new TrainingsModel(trn));
                }

                List<UserTraining> tempCompTrainings = db.UserTrainings.Where(x => x.UserId == currentUser.Id).ToList();
                List<TrainingsModel> completedTrainings = new List<TrainingsModel>();
                foreach (UserTraining trn in tempCompTrainings)
                {
                    completedTrainings.Add(new TrainingsModel(trn));
                }

                return new TrainingsListModel(uncompletedTrainings, completedTrainings);
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage MarkTrainingCompleted(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                int trainingId = obj["trainingId"].Value<int>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                Training tempTraining = db.Trainings.SingleOrDefault(x => x.Id == trainingId);
                if (tempTraining == null)
                {
                    throw BuildHttpResponseException("No such training ID", "ERR_ID");
                }
                UserTraining tempUserTraining = db.UserTrainings.SingleOrDefault(x => x.TrainingId == trainingId && x.UserId == currentUser.Id);
                if (tempUserTraining != null)
                {
                    throw BuildHttpResponseException("The selected training has already been marked as trained", "ERR_ID");
                }
                UserTraining ut = new UserTraining()
                {
                    UserId = currentUser.Id,
                    TrainingId = trainingId,
                    DateCompleted = DateTime.Now
                };
                db.UserTrainings.Add(ut);
                db.SaveChanges();
                return "Success";
            });
            return msg;
        }
    }
}