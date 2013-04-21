(function () {
    var i;
    var _round = Math.round;
    Math.round = function (number, decimals /* optional, default 0 */) {
        if (arguments.length == 1)
            return _round(number);

        var multiplier = Math.pow(10, decimals);
        return _round(number * multiplier) / multiplier;
    }
    Tasks = function (param) {
        this.user = param.user;
        this.$tasksList = param.tasksList;
        this.compTaskClass = param.compTaskClass;
        this.uncompTaskClass = param.uncompTaskClass;

        this.$modal = param.modal;
    };
    Tasks.prototype = {
        init: function () {
        },
        displayDailyTasks: function () {
            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.loadTasksList();
        },

        loadTasksList: function () {
            var userData = { "sessionId": this.user.sessionId };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Tasks/GetDailyTasksByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.$tasksList.empty();

                    for (i = 0; i < r.UncompletedTasks.length; i++) {
                        that.$tasksList.append(that.generateTaskCode(r.UncompletedTasks[i], that.uncompTaskClass));
                    }
                    for (i = 0; i < r.CompletedTasks.length; i++) {
                        that.$tasksList.append(that.generateTaskCode(r.CompletedTasks[i], that.compTaskClass));
                    }
                    that.$tasksList.fadeIn('slow');

                },
                error: function (responseData, textStatus, errorThrown) {
                }
            });
        },

        generateTaskCode: function (task, className) {
            return "<li class='" + className + "'><h2>" + task.Name + "</h2><p>" +
                task.Description + "</p><span hidden>" + task.TaskId + "</span></li>";
        },
        attachTasksClicks: function () {
            var that = this;

            this.$tasksList.on("click", "li", function () {
                $("#modalContent").empty();
                $("#modalContent").html($(this).html());
                $("#modalTitle").html("Task Information");
                //logic for task ID - show nearby shops... show nearby vets...
                var taskID = parseInt($($(this).children("span")[0]).text());
                if (taskID == 9) {
                    if ($(this).hasClass(that.compTaskClass)) {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a class='nearby' href='#' id='showNearbyVets'>Show Nearby Vets</a><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                    else {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a class='nearby' href='#' id='showNearbyVets'>Show Nearby Vets</a><a href='#' class='tsk' data-taskid='" + taskID + "'>Mark As Done</a><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                }
                else if (taskID == 11) {
                    if ($(this).hasClass(that.compTaskClass)) {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a class='nearby' href='#' id='showNearbyPets'>Show Nearby Pet Shops</a><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                    else {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a class='nearby' href='#' id='showNearbyPets'>Show Nearby Pet Shops</a><a href='#' class='tsk' data-taskid='" + taskID + "'>Mark As Done</a><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                }
                else {
                    if ($(this).hasClass(that.compTaskClass)) {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                    else {
                        $("#modalContent").append("<div class='modalButtonsHolder'><a href='#' class='tsk' data-taskid='" + taskID + "'>Mark As Done</a><a href='#' id='cancelModal'>Cancel</a></div>");
                    }
                }
                that.$modal.show(); 
            });
            this.$modal.on("click", "a.tsk", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.markTaskAsCompleted($(this).data('taskid'));
            });

            that.$modal.on("click", "#showNearbyVets", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.displayNearbyVets();
            });

            that.$modal.on("click", "#showNearbyPets", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.displayNearbyPets();
            });
        },
        markTaskAsCompleted: function (taskId) {
            var that = this;
            var userData = { "taskId": taskId, "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Tasks/MarkTaskCompleted",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.loadTasksList();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        displayNearbyVets: function () {
            var that = this;
            var userData = { "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "PetVetShops/GetNearbyVets",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.generateVetsList(r);
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        generateVetsList: function (r) {
            var list = "<ul class='list'>";
            for (i = 0; i < r.length; i++) {
                list += "<li><h2 class='sub'>" + r[i].Name + "</h2><c>Distance: " + Math.round(r[i].DistanceFromUser, 2) +" miles</c><k>Address: " + r[i].Address + "</k><k>Phone: " + r[i].Phone + "</k></li>";
            }
            list += "</li>";

            $("#modalContent").empty().append(list);
        },
        displayNearbyPets: function () {
            var that = this;
            var userData = { "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "PetVetShops/GetNearbyPetShops",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.generatePetsList(r);
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        generatePetsList: function (r) {
            var list = "<ul class='list'>";
            for (i = 0; i < r.length; i++) {
                list += "<li><h2 class='sub'>" + r[i].Name + "</h2><c>Distance: " + Math.round(r[i].DistanceFromUser, 2) +" miles</c><k>Address: " + r[i].Address + "</k><k id='phone'>Phone: " + r[i].Phone + "</k> \
                        <table class='grid'> \
                            <tr> \
                                <td>Monday</td><td>" + r[i].MonStart + " - " + r[i].MonEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Tuesday</td><td>" + r[i].TueStart + " - " + r[i].TueEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Wednesday</td><td>" + r[i].WedStart + " - " + r[i].WedEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Thursday</td><td>" + r[i].ThuStart + " - " + r[i].ThuEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Friday</td><td>" + r[i].FriStart + " - " + r[i].FriEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Saturday</td><td>" + r[i].SatStart + " - " + r[i].SatEnd + "</td> \
                            </tr> \
                            <tr> \
                                <td>Sunday</td><td>" + r[i].SunStart + " - " + r[i].SunEnd + "</td> \
                            </tr> \
                        </table></li>";
            }
            list += "</li>";

            $("#modalContent").empty().append(list);
        }
    }
})(jQuery);