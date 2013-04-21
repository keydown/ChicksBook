(function () {
    var i;
    Farm = function (param) {
        this.user = param.user;
        this.$farmContainer = param.farmContainer;
        this.petsList = new Array();

        this.$modal = param.modal;
    };
    Farm.prototype = {
        displayFarm: function () {

			$(document.documentElement).removeClass(SiteController.PAGE_CLASS_NAME);
			SiteController.PAGE_CLASS_NAME = "farm-page";
			$(document.documentElement).addClass(SiteController.PAGE_CLASS_NAME);

            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.loadFarm();
        },
        loadFarm: function () {
            var userData = { "sessionId": this.user.sessionId };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/GetFarmByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.$farmContainer.empty();

                    for (i = 0; i < r.length; i++) {
                        that.$farmContainer.append(that.generateFarmLayout(r[i]));
                    }

                    that.$farmContainer.append("<a href='#' id='btnAddNewPet'>Add New Pet</a>")

                    that.$farmContainer.fadeIn('slow');

                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        generateFarmLayout: function (farm) {
            return "<div> \
                        <h3 class='petName'>" + farm.PetName + "<button data-petid='" + farm.Id + "' class='btnMoreInfo'></button></h3> \
                        <span class='farmAsset'>Pet Count</span> \
                        <span class='farmQty'>" + farm.Count + "</span> \
                        <button data-role='none' data-asset='P' data-curval='" + farm.Count + "' data-petid='" + farm.Id + "'></button> \
                        <br /> \
                        <span class='farmAsset'>Eggs In Stock</span> \
                        <span class='farmQty'>" + farm.EggsInStock + "</span> \
                        <button data-role='none' data-asset='E' data-curval='" + farm.EggsInStock + "' data-petid='" + farm.Id + "'></button> \
                        <br /> \
                        <span class='farmAsset'>Eggs Hatching</span> \
                        <span class='farmQty'>" + farm.EggsHatching + "</span> \
                        <button data-role='none' data-asset='H' data-curval='" + farm.EggsHatching + "' data-petid='" + farm.Id + "'></button> \
                        <br /> \
                    </div>"
        },
        attachBlueEvents: function () {
            var that = this;

            this.$farmContainer.on("click", "#btnAddNewPet", function () {
                that.loadPetNames();
            });

            this.$farmContainer.on("click", ".btnMoreInfo", function () {
                var petId = $(this).data('petid');
                that.loadPetInfo(petId);
            });

            this.$farmContainer.on("click", "button", function () {
                var curval = $(this).data('curval');
                var petId = $(this).data('petid');
                var asset = $(this).data('asset');

                $("#modalTitle").html("Change Pet Information");
                $("#modalContent").empty();
                $("#modalContent").html(that.generateModalInfo(curval, petId, asset));
                that.$modal.show(); 
            });

            this.$modal.on("click", "#btnUpdate", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();

                // validate input

                var curVal = $("#farmQty").data('curval');
                var newVal = $("#farmQty").val();
                var count = newVal;
                var petId = $("#farmUpdate").data('petid');
                var action = $("#farmAction").val();
                var asset = $("#farmUpdate").data('asset');
                var cost = $("#farmCost").val();
                that.submitFarmChanges(petId, count, action, asset, cost);

                siteController.farm.displayFarm();

            });
            this.$modal.on("click", "#addPetToUser", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                var petId = $("#selectPet").val();
                that.addNewPetToUser(petId);
            });
        },
        generateModalInfo: function (curval, petId, asset) {
            return "<form id='farmUpdate' data-asset='" + asset + "' data-petId='" + petId + "'> \
                        <label for='farmQty'>Quantity</label><br /> \
                        <input id='farmQty' type='text' data-curval='" + curval + "' value='" + curval + "' /><br /> \
                        <label>Reason for Update</label><br /> \
                        <div class='style-select'><select id='farmAction'> \
                            <option value='ADD'>Add</option> \
                            <option value='SELL'>Sell</option> \
                            <option value='LOSE'>Lose</option> \
                        </select></div> <br /> \
                        <label for='farmCost'>How much the Update Cost/Profit You</label><br /> \
                        <input type='text' id='farmCost' value='0' /> <br />\
                        <button data-role='none' id='btnUpdate'>Update</button> \
                    </form>"
        },
        generateModalAddNewPet: function () {
            $("#modalTitle").html("Add New Pet");
            $("#modalContent").empty();
            $("#modalContent").html(this.generatePetOptions());
            this.$modal.show();
        },
        generateModalPetInfo: function (r) {
            $("#modalTitle").html("Pet Info");
            $("#modalContent").empty();
            $("#modalContent").html(this.generatePetInfo(r));
            this.$modal.show();
        },
        generatePetOptions: function () {
            var i = 0;
            var options = "<form id='frmAddNewPet'><label>Select Pet: </label><div class='style-select'><select id='selectPet'>";
            for (i = 0; i < this.petsList.length; i++) {
                options += "<option value='" + this.petsList[i].id + "'>" + this.petsList[i].name + "</option>"
            }
            options += "</select></div> <br /> \
                       <input type='submit' id='addPetToUser' value='Add Pet' /> \
                       </form>";
            return options;
        },
        generatePetInfo: function (r) {
            return "<img src='" + r.Picture + "' /> \
                    <h2>" + r.Breed + "</h2> \
                    <dl> \
                        <dt>Breed</dt> \
                        <dd>" + r.Breed + "</dd> \
                        <dt>Behavior</dt> \
                        <dd>" + r.Behavior + "</dd> \
                        <dt>Brooding</dt> \
                        <dd>" + r.Brooding + "</dd> \
                        <dt>Class</dt> \
                        <dd>" + r.Class + "</dd> \
                        <dt>Comb</dt> \
                        <dd>" + r.Comb + "</dd> \
                        <dt>Ealobes</dt> \
                        <dd>" + r.Ealobes + "</dd> \
                        <dt>EggCost</dt> \
                        <dd>" + r.EggCost + "</dd> \
                        <dt>EggHatchingDays</dt> \
                        <dd>" + r.EggHatchingDays + "</dd> \
                        <dt>EggSize</dt> \
                        <dd>" + r.EggSize + "</dd> \
                        <dt>EggsColor</dt> \
                        <dd>" + r.EggsColor + "</dd> \
                        <dt>Hardness</dt> \
                        <dd>" + r.Hardness + "</dd> \
                        <dt>Maturing</dt> \
                        <dd>" + r.Maturing + "</dd> \
                        <dt>Rarity</dt> \
                        <dd>" + r.Rarity + "</dd> \
                        <dt>SkinColor</dt> \
                        <dd>" + r.SkinColor + "</dd> \
                        <dt>Varieties</dt> \
                        <dd>" + r.Varieties + "</dd> \
                        <dt>Weight</dt> \
                        <dd>" + r.Weight + "</dd> \
                   </dl>";
        },
        loadPetInfo: function (petId) {
            var that = this;
            var i = 0;
            var userData = { "sessionId": this.user.sessionId , "petId" : petId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/GetPetInfo",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.generateModalPetInfo(r);
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        loadPetNames: function () {
            var that = this;
            var i = 0;
            var userData = { "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/GetPetsList",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.petsList = [];
                    for (i = 0; i < r.length; i++) {
                        that.petsList.push({ name: r[i].Breed, id: r[i].Id });
                    }
                    that.generateModalAddNewPet();
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        submitFarmChanges: function (petId, count, action, asset, price) {
            switch (asset) {
                case "P": {
                    this.updatePetHistory(petId, count, price, action + asset);
                    break;
                }
                case "H": {
                    this.updateHatchingHistory(petId, count, price, action + asset);
                    break;
                }
                case "E": {
                    this.updateEggsHistory(petId, count, price, action + asset);
                    break;
                }
                default:

            }
        },

        addNewPetToUser: function (petId) {
            var that = this;
            var userData = { "sessionId": this.user.sessionId, "petId": petId, "count": "0", "price": "0", "action": "ADDP" }
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/UpdateFarmPetHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.farm.displayFarm();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        updatePetHistory: function (petId, count, price, action) {
            var userData = {
                "sessionId": this.user.sessionId,
                "petId": petId,
                "count": count,
                "price": price,
                "action": action,
            };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/UpdateFarmPetHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.farm.displayFarm();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        updateHatchingHistory: function (petId, count, price, action) {
            var userData = {
                "sessionId": this.user.sessionId,
                "petId": petId,
                "count": count,
                "price": price,
                "action": action,
            };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/UpdateFarmHatchingHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.farm.displayFarm();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        updateEggsHistory: function (petId, count, price, action) {
            var userData = {
                "sessionId": this.user.sessionId,
                "petId": petId,
                "count": count,
                "price": price,
                "action": action,
            };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Farm/UpdateFarmEggsHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.farm.displayFarm();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        }
    }
})(jQuery);