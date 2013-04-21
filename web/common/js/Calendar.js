(function () {
    var i;
    Calendar = function (param) {
        this.user = param.user;
        this.$calendarContainer = param.calendarContainer;
    };
    Calendar.prototype = {
        init: function () {
           
        },
        displayCalendar: function () {

			$(document.documentElement).removeClass(SiteController.PAGE_CLASS_NAME);
			SiteController.PAGE_CLASS_NAME = "calendar-page";
			$(document.documentElement).addClass(SiteController.PAGE_CLASS_NAME);

            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.$calendarContainer.fadeIn('slow');
        },
        attachCalendarHandler: function() {
            var that = this;
            $("#mydate").on("change", function () {
                that.displayDateInformation($('#mydate').val());
            });
        },
        displayDateInformation: function (date) {
            var userData = { "sessionId": this.user.sessionId, "date": date }
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Calendar/GetDateInfoByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.generateDayLayout(r);
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        generateDayLayout: function (r) {
            var i;
            var that = this;
            var result = "";
            result += "<dt>Events</dt>";
            if (r.Event.length == 0) {
                result += "<dd>No Events</dd>"
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Event.length; i++) {
                    result += "<li><h3>" + r.Event[i].Title + "</h3> \
                                   <p>" + e.Event[i].Description + "</p> \
                                    <a href='" + e.Event[i].Link + "'>More Info</a> \
                               </li>";
                }
                result += "</ul></dd>";
            }

            result += "<dt>Tasks Completed</dt>";
            if (r.Tasks.CompletedTasks.length == 0) {
                result += "<dd>No Completed Tasks</dd>";
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Tasks.CompletedTasks.length; i++) {
                    result += "<li><h3>" + r.Tasks.CompletedTasks[i].Name + "</h3></li>";
                }
                result += "</ul></dd>";
            }

            result += "<dt>Tasks Uncompleted</dt>";
            if (r.Tasks.UncompletedTasks.length == 0) {
                result += "<dd>No Uncompleted Tasks</dd>";
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Tasks.UncompletedTasks.length; i++) {
                    result += "<li><h3>" + r.Tasks.UncompletedTasks[i].Name + "</h3></li>";
                }
                result += "</ul></dd>";
            }

            result += "<dt>Trainings Completed</dt>";
            if (r.Trainings.length == 0) {
                result += "<dd>No Uncompleted Trainings</dd>";
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Trainings.length; i++) {
                    result += "<li><h3>" + r.Trainings[i].Title + "</h3></li>";
                }
                result += "</ul></dd>";
            }

            result += "<dt>Pets</dt>";
            if (r.Farm.length == 0) {
                result += "<dd>No Farm Actions</dd>";
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Farm.length; i++) {
                    result += "<li><h3>" + r.Farm[i].PetName + "</h3> \
                                   <p>Action: " + r.Farm[i].Action + "</p> \
                                   <p>Count: " + r.Farm[i].Count + "</p> \
                                   <p>Price: " + r.Farm[i].Price + "</p> \
                               </li>";
                }
                result += "</ul></dd>";
            }

            result += "<dt>Assets</dt>";
            if (r.Assets.length == 0) {
                result += "<dd>No Assets Actions</dd>";
            }
            else {
                result += "<dd><ul>";
                for (i = 0; i < r.Assets.length; i++) {
                    result += "<li><h3>" + r.Assets[i].AssetName + "</h3> \
                                   <p>Action: " + r.Assets[i].Action + "</p> \
                                   <p>Count: " + r.Assets[i].Count + "</p> \
                                   <p>Price: " + r.Assets[i].Price + "</p> \
                               </li>";
                }
                result += "</ul></dd>";
            }


            $("#dateResult").empty().append(result);
        }
    }
})(jQuery);