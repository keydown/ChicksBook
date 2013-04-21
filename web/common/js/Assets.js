(function () {
    var i;
    Assets = function (param) {
        this.user = param.user;
        this.$assetsContainer = param.assetsContainer;
        this.assetsList = new Array();

        this.$modal = param.modal;
    };
    Assets.prototype = {
        displayAssets: function () {

			$(document.documentElement).removeClass(SiteController.PAGE_CLASS_NAME);
			SiteController.PAGE_CLASS_NAME = "assets-page";
			$(document.documentElement).addClass(SiteController.PAGE_CLASS_NAME);

            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.loadAssets();
        },

        generateAssetsLayout: function (asset) {
            return "<tr><th class='assetName'>" + asset.Asset + "</th><td class='farmQty'>" + asset.CurrentQuantity + "</td> \
                            <td><button data-role='none' data-assetname='" + asset.Asset + "' data-curval='" + asset.CurrentQuantity + "' data-assetid='" + asset.AssetId + "'> \
                    Add</button></td></tr>";
        },
        attachAssetsEvents: function () {
            var that = this;

            this.$assetsContainer.on("click", "#btnAddNewAsset", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.loadAssetsNames();
            });

            this.$assetsContainer.on("click", "#btnSubmitNewAsset", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                that.generateSubmitAsset();
            });
            this.$assetsContainer.on("click", "button", function (evt) {
                evt.preventDefault();
                evt.stopImmediatePropagation();
                var curval = $(this).data('curval');
                var assetId = $(this).data('assetid');
                var assetName = $(this).data('assetname');
                $("#modalTitle").html("Asset Information");
                $("#modalContent").empty();
                $("#modalContent").html(that.generateUpdateAssetForm(curval, assetId, assetName));
                that.$modal.show();

            });

            this.$modal.on("click", "#addAssetToUser", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                var assetId = $("#selectAsset").val();
                var assetCount = $("#assetCount").val();
                var assetPrice = $("#assetPrice").val();
                that.addNewAsset(assetId, assetCount, assetPrice);
            });

            this.$modal.on("click", "#btnSubmitAsset", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                var assetName = $("#assetName").val();
                that.submitAsset(assetName);
            });
            that.$modal.on("click", "#btnUpdateAsset", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                var assetId = $("#selectUpdateAsset").data('assetid');
                var val = $("#assetUpdateCount").val();
                var price = $("#assetUpdatePrice").val();
                var assetStatus = $("#selectUpdateAsset").val();
                that.updateAsset(assetId, assetStatus, val, price);
            });

        },
        generateModalAddAsset: function () {
            $("#modalTitle").html("Add New Assets");
            $("#modalContent").empty();
            $("#modalContent").html(this.generateAssetsOptions());
            this.$modal.show();
        },
        generateAssetsOptions: function () {
            var i = 0;
            var options = "<form id='frmAddNewAsset'><label>Select Asset: </label><br /><div class='style-select'><select id='selectAsset'>";
            for (i = 0; i < this.assetsList.length; i++) {
                options += "<option value='" + this.assetsList[i].id + "'>" + this.assetsList[i].name + "</option>"
            }
            options += "</select></div><br /> \
                        <label>Count</label><br /> \
                        <input type='text' id='assetCount' value='0' /><br /> \
                        <label>Price</label><br /> \
                        <input type='text' id='assetPrice' value='0'><br /> \
                        <input type='submit' id='addAssetToUser' value='Add Asset' /> \
                       </form>";
            return options;
        },
        generateSubmitAsset: function () {
            $("#modalTitle").html("Submit New Assets");
            $("#modalContent").empty();
            $("#modalContent").html(this.generateSubmitAssetForm());
            this.$modal.show();
        },
        generateUpdateAssetForm: function(curval, assetId, assetName) {
            return "<form id='frmUpdateAsset'> \
                        <h2>"+assetName+"</h2> \
                        <label>Reason for Update</label> \
                        <div class='style-select'><select id='selectUpdateAsset' data-assetid='"+assetId +"'> \
                            <option value='ADD'>Add</option> \
                            <option value='SELL'>Sell</option> \
                            <option value='LOSE'>Lose</option> \
                        </select></div> \
                        <label>Count</label> \
                        <input type='text' id='assetUpdateCount' data-curval='"+curval+"' value='"+curval+"' /> \
                        <label>Price</label> \
                        <input type='text' id='assetUpdatePrice' /> \
                        <a href='#' id='btnUpdateAsset'>Update Asset</a> \
                       </form>";
        },
        generateSubmitAssetForm: function() {
            return "<form id='frmSubmitAsset'> \
                        <label>Asset Name</label><br /> \
                        <input type='text' id='assetName' /><br /> \
                        <input type='submit' id='btnSubmitAsset' value='Submit Asset' /> \
                       </form>";
        },
        addNewAsset: function (assetId, assetCount, assetPrice, action) {
            var act = action || "ADD";
            var that = this;
            var i = 0;
            var userData = {
                "sessionId": this.user.sessionId,
                "assetId": assetId,
                "assetStatus": act,
                "assetPrice": assetPrice,
                "assetCount": assetCount
            };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Assets/UpdateAssetHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.assets.displayAssets();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        loadAssetsNames: function () {
            var that = this;
            var i = 0;
            var userData = { "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Assets/GetAssetsList",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    that.assetsList = [];
                    for (i = 0; i < r.length; i++) {
                        that.assetsList.push({ name: r[i].Name, id: r[i].Id });
                    }
                    that.generateModalAddAsset();
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },


        loadAssets: function () {
            var that = this;
            var userData = { "sessionId": this.user.sessionId };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Assets/GetCurrentAssetsListByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.$assetsContainer.empty();

                    that.$assetsContainer.append("<table class='grid'><tbody></tbody></table>");

                    for (i = 0; i < r.length; i++) {
                        that.$assetsContainer.find("table tbody").append(that.generateAssetsLayout(r[i]));
                    }

                    that.$assetsContainer.append("<div class='buttons'><a href='#' class='button' id='btnAddNewAsset'>New Asset</a><a href='#' class='button' id='btnSubmitNewAsset'>Custom Asset</a></div>");

                    that.$assetsContainer.fadeIn('slow');

                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        submitAsset: function (assetName) {
            var that = this;
            var userData = {
                "sessionId": this.user.sessionId,
                "assetName": assetName
            };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Assets/SubmitAsset",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.assets.displayAssets();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        updateAsset: function (assetId, assetStatus, val, price) {
            var that = this;
            var userData = {
                "sessionId": this.user.sessionId,
                "assetId": assetId,
                "assetStatus": assetStatus,
                "assetPrice": price,
                "assetCount": val
            };
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Assets/UpdateAssetHistoryByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {
                    siteController.assets.displayAssets();
                    that.$modal.fadeOut('fast');
                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        }

    }
})(jQuery);