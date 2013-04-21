(function () {
    var i;
    Trainings = function (param) {
        this.user = param.user;
        this.$trainingsList = param.trainingsList;
        this.compTrainingClass = param.compTrainingClass;
        this.uncompTrainingClass = param.uncompTrainingClass;

        this.$modal = param.modal;
    };
    Trainings.prototype = {
        init: function () {

        },
        displayTrainings: function(){
            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.loadTrainingsList();
        },

        loadTrainingsList: function() {
            var userData = { "sessionId": this.user.sessionId };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Trainings/GetTrainingsByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.$trainingsList.empty();

                    for (i = 0; i < r.UncompletedTrainings.length; i++) {
                        that.$trainingsList.append(that.generateTrainingCode(r.UncompletedTrainings[i], that.uncompTrainingClass));
                    }
                    for (i = 0; i < r.CompletedTrainings.length; i++) {
                        that.$trainingsList.append(that.generateTrainingCode(r.CompletedTrainings[i], that.compTrainingClass));
                    }
                    that.$trainingsList.fadeIn('slow');

                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },

        generateTrainingCode: function(training, className) {
            return "<li class='item training " + className + "'><h6 class='title'>" + training.Title + "</h6><div class='body'>" +
                training.Text + "<span hidden>" + training.Id + "</span></div></li>";
        },

        attachTrainingsClicks: function () {
            var that = this;

            this.$trainingsList.on("click", "li", function () {
                $("#modalContent").empty();
                $("#modalContent").html($(this).html());
                $("#modalTitle").html("Training Information");
                //logic for task ID - show nearby shops... show nearby vets...

                if ($(this).hasClass(that.compTrainingClass)) {
                    $("#modalContent").append("<div class='modalButtonsHolder'><a href='#' id='cancelModal'>Cancel</a></div>");
                }
                else {
                    var trainingId = $($(this).children("span")[0]).text();
                    $("#modalContent").append("<div class='modalButtonsHolder'><a href='#' id='markAsDone' class='tsk' data-trainingid='"
                            + trainingId + "'>Mark As Done</a><a href='#' id='cancelModal'>Cancel</a></div>");
                }
                that.$modal.show();
            });
            this.$modal.on("click", "#markAsDone", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.markTrainingAsCompleted($(this).data('trainingid'));
            });
        },
        markTrainingAsCompleted: function (trainingId) {
            var that = this;
            var userData = { "trainingId": trainingId, "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Trainings/MarkTrainingCompleted",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.loadTrainingsList();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        }
    }
})(jQuery);