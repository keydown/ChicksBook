using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class TasksModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public DateTime DateAdded { get; set; }

        public TasksModel()
        {
            this.Name = "Invalid Name";
            this.Description = "No Description Found";
            this.TaskId = 0;
            this.DateAdded = DateTime.Now;
        }
        public TasksModel(Task task) : base()
        {
            this.Name = task.Name;
            this.TaskId = task.Id;
            this.Description = task.Description;
            this.DateAdded = task.DateAdded;
        }
    }

    public class TaskListModel
    {
        public List<TasksModel> CompletedTasks { get; set; }
        public List<TasksModel> UncompletedTasks { get; set; }

        public TaskListModel()
        {
            this.CompletedTasks = new List<TasksModel>();
            this.UncompletedTasks = new List<TasksModel>();
        }
        public TaskListModel(List<TasksModel> completedTasks, List<TasksModel> uncompletedTasks) : base()
        {
            this.CompletedTasks = completedTasks.ToList();
            this.UncompletedTasks = uncompletedTasks.ToList();
        }
    }
}