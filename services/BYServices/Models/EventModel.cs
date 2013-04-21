using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYServices.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? Date { get; set; }

        public EventModel(Event evt)
        {
            this.Id = evt.Id;
            this.Title = evt.Title;
            this.Description = evt.Description;
            this.Link = evt.Link;
            this.Date = evt.Date;
        }
    }
}