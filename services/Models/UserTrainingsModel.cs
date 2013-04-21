using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class UserTrainingsModel
    {
        public int UserId { get; set; }
        public int TrainingId { get; set; }
        public DateTime DateCompleted { get; set; }

        public UserTrainingsModel(UserTraining train)
            : base()
        {
            this.UserId = train.UserId;
            this.TrainingId = train.TrainingId;
            this.DateCompleted = train.DateCompleted;
        }

        public UserTrainingsModel()
        {
            this.UserId = -1;
            this.TrainingId = -1;
            this.DateCompleted = DateTime.Now;
        }
    }
}