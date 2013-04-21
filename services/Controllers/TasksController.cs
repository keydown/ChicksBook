using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{
    public class TasksController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetTaskInfo(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                int taskId = obj["taskId"].Value<int>();
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                TasksModel task = new TasksModel(db.Tasks.SingleOrDefault(x => x.Id == taskId));
                if (task == null)
                {
                    throw BuildHttpResponseException("No such task ID", "ERR_ID");
                }
                return task;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage AddTaskForUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                DateTime date = obj["date"].Value<DateTime>();
                int taskId = obj["taskId"].Value<int>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                if (taskId <= 0)
                {
                    throw BuildHttpResponseException("Invalid task ID", "ERR_TSK_ID");
                }
                Task task = db.Tasks.SingleOrDefault(x => x.Id == taskId);
                if (task == null)
                {
                    throw BuildHttpResponseException("No task with this ID", "ERR_TSK_ID");
                }

                Calendar newTask = new Calendar()
                {
                    TaskId = taskId,
                    UserId = currentUser.Id,
                    Date = date,
                    TaskCompleted = false
                };
                db.Calendars.Add(newTask);
                db.SaveChanges();
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetDailyTasksByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                TaskListModel tasksList = GetTasksListByDate(DateTime.Today.Date, currentUser);

                return tasksList;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage MarkTaskCompleted(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                int taskId = obj["taskId"].Value<int>();

                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);
                Calendar task = db.Calendars.SingleOrDefault(x => x.TaskId == taskId && x.UserId == currentUser.Id && EntityFunctions.TruncateTime(x.Date) == DateTime.Today.Date);

                if (task == null)
                {
                    throw BuildHttpResponseException("No such task ID", "ERR_ID");
                }

                task.TaskCompleted = true;
                db.SaveChanges();
                return "Success";
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetTasksByDate(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                DateTime date = obj["date"].Value<DateTime>();
                if (date == null)
                {
                    throw BuildHttpResponseException("Invalid date", "ERR_DATE");
                }
                TaskListModel tasksList = GetTasksListByDate(date, currentUser);

                return tasksList;
            });
            return msg;
        }
         
        [HttpPost]
        public HttpResponseMessage AddCustomTask(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                string name = obj["name"].Value<string>();
                if (name == null)
                {
                    throw BuildHttpResponseException("Name can not be empty", "ERR_NAME_EMP");
                }
                else if (name.Length > 50)
                {
                    throw BuildHttpResponseException("Name too long (maximum 50 symbols)", "ERR_NAME");
                }
                else if (name.Length < 2)
                {
                    throw BuildHttpResponseException("Name too short (minimum 4 symbols)", "ERR_NAME");
                }

                string description = obj["description"].Value<string>();
                if (description != null)
                {
                    if (description.Length < 4)
                    {
                        throw BuildHttpResponseException("Description too short (minimum 4 symbols)", "ERR_DESC");
                    }
                    if (description.Length > 50)
                    {
                        throw BuildHttpResponseException("Description too long (maximum 500 symbols)", "ERR_DESC");
                    }
                }
                Task newTask = new Task()
                {
                    Name = name,
                    Description = description,
                    UserId = currentUser.Id,
                    DateAdded = DateTime.Now
                };
                db.Tasks.Add(newTask);
                db.SaveChanges();
            });
            return msg;
        }
  
        public static TaskListModel GetTasksListByDate(DateTime date, User currentUser)
        {
            BYEntities db = new BYEntities();
            List<Task> tempUncTasks = db.Tasks.Where(x => db.Calendars.Any(
                    y => EntityFunctions.TruncateTime(y.Date) == date
                    && x.Id == y.TaskId
                    && y.UserId == currentUser.Id
                    && y.TaskCompleted == false)).ToList();

            List<TasksModel> uncompletedTasks = new List<TasksModel>();
            foreach (Task tsk in tempUncTasks)
            {
                uncompletedTasks.Add(new TasksModel(tsk));
            }

            List<Task> tempCompTasks = db.Tasks.Where(x => db.Calendars.Any(
                y => EntityFunctions.TruncateTime(y.Date) == date
                && x.Id == y.TaskId
                && y.UserId == currentUser.Id
                && y.TaskCompleted == true)).ToList();

            List<TasksModel> completedTasks = new List<TasksModel>();
            foreach (Task tsk in tempCompTasks)
            {
                completedTasks.Add(new TasksModel(tsk));
            }
            return new TaskListModel(completedTasks, uncompletedTasks);
        }
    }
}