(function () {



    function setCurrent( item ) {
        var $item = $(item),
            $parent = $item.parent();

        $parent.siblings().removeClass("current");
        $parent.addClass("current");
    }

    SiteController = function (param) {
        this.$frmLogin = $(param.frmLogin);
        this.$frmRegister = $(param.frmRegister);
        this.$txtLogUsername = $(param.txtLogUsername);
        this.$txtLogPassword = $(param.txtLogPassword);
        this.$txtRegUsername = $(param.txtRegUsername);
        this.$txtRegPassword = $(param.txtRegPassword);
        this.$txtRegRepeatPassword = $(param.txtRegRepeatPassword);
        this.$btnRegister = $(param.btnRegister);
        this.$btnSubmitLogin = $(param.btnSubmitLogin);
        this.$btnSubmitRegister = $(param.btnSubmitRegister);

        this.$tasksList = $(param.tasksList);
        this.uncompTaskClass = param.uncompTaskClass;
        this.compTaskClass = param.compTaskClass;

        this.$navTasks = $(param.navTasks);
        this.$navTrainings = $(param.navTrainings);
        this.$navFinances = $(param.navFinances);
        this.$navFarm = $(param.navFarm);
        this.$navAssets = $(param.navAssets);
        this.$navCalendar = $(param.navCalendar);
        this.$navDictionary = $(param.navDictionary);

        this.$trainingsList = $(param.trainingsList);
        this.compTrainingClass = param.compTrainingClass;
        this.uncompTrainingClass = param.uncompTrainingClass;

        this.$financeContainer = $(param.financeContainer);

        this.$farmContainer = $(param.farmContainer);

        this.$assetsContainer = $(param.assetsContainer);

        this.$calendarContainer = $(param.calendarContainer);

        this.$modal = $(param.modal);

        this.$menu = $(param.menu);
        this.menuStatus = false;
        this.isMobile = false;
        this.$hdr = $(param.hdr);
    };

    SiteController.prototype = {
        init: function () {
            var that = this;

            this.initClasses();
            this.initNavigation();

            if (this.user.isUserLogged)
            {
                this.tasks.displayDailyTasks();
            }
            else
            {
                this.user.displayLoginPage();

                this.$btnRegister.on("click", function (evt) {
                    evt.preventDefault();
                    that.user.displayRegisterPage();
                });
            }
        },
        initClasses: function () {

            this.user = new User({
                frmLogin: this.$frmLogin,
                frmRegister: this.$frmRegister,
                txtUsername: this.$txtLogUsername,
                txtPassword: this.$txtLogPassword,
                txtRegUsername: this.$txtRegUsername,
                txtRegPassword: this.$txtRegPassword,
                txtRegRepeatPassword: this.$txtRegRepeatPassword,
                btnRegister: this.$btnRegister,
                btnSubmitLogin: this.$btnSubmitLogin,
                btnSubmitRegister: this.$btnSubmitRegister
            });
            this.tasks = new Tasks({
                user: this.user,
                tasksList: this.$tasksList,
                uncompTaskClass: this.uncompTaskClass,
                compTaskClass: this.compTaskClass,
                modal: this.$modal
            });
            this.tasks.attachTasksClicks();

            this.trainings = new Trainings({
                user: this.user,
                trainingsList: this.$trainingsList,
                compTrainingClass: this.compTrainingClass,
                uncompTrainingClass: this.uncompTrainingClass,
                modal: this.$modal
            });
            this.trainings.attachTrainingsClicks();

            this.finances = new Finance({
                user: this.user,
                financeContainer: this.$financeContainer
            });
            this.farm = new Farm({
                user: this.user,
                farmContainer: this.$farmContainer,
                modal: this.$modal
            });
            this.farm.attachBlueEvents();

            this.assets = new Assets({
                user: this.user,
                assetsContainer: this.$assetsContainer,
                modal: this.$modal
            });
            this.assets.attachAssetsEvents();

            this.calendar = new Calendar({
                user: this.user,
                calendarContainer: this.$calendarContainer
            });
            this.calendar.attachCalendarHandler();
        },
        initNavigation: function () {
            var that = this;

            this.$modal.on("click", "#modalClose", function (evt) {
                evt.preventDefault();
                that.$modal.fadeOut('fast');
            });
            this.$modal.on('click', "#cancelModal", function (evt) {
                evt.preventDefault();
                that.$modal.fadeOut('fast');
            });
            this.$navTasks.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                that.tasks.displayDailyTasks();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            this.$navTrainings.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            this.$navFinances.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                that.finances.displayFinances();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            this.$navFarm.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                that.farm.displayFarm();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            this.$navAssets.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                that.assets.displayAssets();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            this.$navCalendar.on("click", function (evt) {
                setCurrent( this );
                evt.preventDefault();
                that.calendar.displayCalendar();
                if (that.isMobile == true) {
                    that.menuHandler();
                }
            });
            if (this.isMobile) {
                this.$menu.on("click", this.menuHandler.bind(that))
            }
        },
        menuHandler: function menuHandler(){
            var that = this;
            if(that.menuStatus)
            {
                SiteController.$MAIN_CONTAINER.animate({ 'left': '0px' }, 500);
                that.$hdr.animate({'left':'0px'},500);
                that.menuStatus = false;
            }
            else
            {
                SiteController.$MAIN_CONTAINER.animate({ 'left': '272px' }, 500);
                that.$hdr.animate({'left':'272px'},500);
                that.menuStatus = true;
            }
        },
        closeModal: function () {
            this.$modal.fadeOut('slow');
        }
    }
    SiteController.SERVICE_URL = "http://chicksbook.keydown.org/api/";
    SiteController.$MAIN_CONTAINER = $("#content");
    SiteController.$MAIN_WRAPPER = $("#site-wrap");
	SiteController.PAGE_CLASS_NAME = "login-page";

    $(document).ready(
        function () {
            siteController = new SiteController({
                modal: "#modal",
                frmLogin: "#frmLogin",
                frmRegister: "#frmRegister",
                txtLogUsername: "#txtLogUsername",
                txtLogPassword: "#txtLogPassword",
                btnSubmitLogin: "#btnSubmitLogin",
                txtRegUsername: "#txtRegUsername",
                txtRegPassword: "#txtRegPassword",
                txtRegRepeatPassword: "#txtRepeatPassword",
                btnRegister: "#btnRegister",
                btnSubmitRegister: "#btnSubmitRegister",
                tasksList: "#tasksList",
                uncompTaskClass: "listNotDone",
                compTaskClass: "listDone",
                trainingsList: "#trainingsList",
                compTrainingClass: "listDone",
                uncompTrainingClass: "listNotDone",
                financeContainer: "#financeContainer",
                farmContainer: "#farmContainer",
                assetsContainer: "#assetsContainer",
                calendarContainer: "#calendarContainer",
                navTasks: "#nav-tasks",
                navTrainings: "#nav-trainings",
                navFinances: "#nav-finances",
                navFarm: "#nav-farm",
                navAssets: "#nav-assets",
                navCalendar: "#nav-calendar",
                navDictionary: "#nav-dictionary",
                hdr: "#hdr",
                menu: "#menu"
            });
            siteController.init();
        }
    );

})(jQuery);