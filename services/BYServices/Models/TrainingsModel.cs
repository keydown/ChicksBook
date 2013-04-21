using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class TrainingsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public TrainingsModel()
        {
            this.Text = "Training not found";
            this.Title = "No such training";
        }

        public TrainingsModel(Training training) : base()
        {
            this.Id = training.Id;
            this.Text = training.Text;
            this.Title = training.Title;
        }

        public TrainingsModel(UserTraining training) : base()
        {
            this.Id = training.Training.Id;
            this.Text = training.Training.Text;
            this.Title = training.Training.Title;
        }
    }

    public class TrainingsListModel
    {
        public List<TrainingsModel> UncompletedTrainings { get; set; }
        public List<TrainingsModel> CompletedTrainings { get; set; }

        public TrainingsListModel()
        {
            this.UncompletedTrainings = new List<TrainingsModel>();
            this.CompletedTrainings = new List<TrainingsModel>();
        }
        public TrainingsListModel(List<TrainingsModel> uncompletedTrainings, List<TrainingsModel> completedTrainings) : base()
        {
            this.UncompletedTrainings = uncompletedTrainings.ToList();
            this.CompletedTrainings = completedTrainings.ToList();
        }

    }
}