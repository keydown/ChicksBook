using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class CalendarModel
    {
        public DateTime Date { get; set; }
        public TaskListModel Tasks { get; set; }
        public List<UserAssetHistoryModel> Assets { get; set; }
        public List<TrainingsModel> Trainings { get; set; }
        public List<UserFarmHistoryModel> Farm { get; set; }
        public List<EventModel> Event { get; set; }

        public CalendarModel()
        {
            this.Date = DateTime.Now;
            this.Tasks = new TaskListModel();
            this.Assets = new List<UserAssetHistoryModel>();
            this.Trainings = new List<TrainingsModel>();
            this.Farm = new List<UserFarmHistoryModel>();
        }

        public CalendarModel(DateTime date, TaskListModel tasks, List<UserAssetHistoryModel> assets, 
                            List<TrainingsModel> trainings, List<UserFarmHistoryModel> farm, List<EventModel> evt) : base()
        {
            this.Date = date;
            this.Tasks = tasks;
            this.Assets = assets.ToList();
            this.Trainings = trainings.ToList();
            this.Farm = farm.ToList();
            this.Event = evt.ToList();
        }
        
    }
}