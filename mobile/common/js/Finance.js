(function () {
    var i;
    Finance = function (param) {
        this.user = param.user;
        this.$financeContainer = param.financeContainer;
    };
    Finance.prototype = {
        displayFinances: function () {
            SiteController.$MAIN_CONTAINER.children().fadeOut('fast');
            this.loadFinances();
        },
        loadFinances: function () {
            var userData = { "sessionId": this.user.sessionId };
            var that = this;
            $.ajax({
                type: 'POST',
                url: SiteController.SERVICE_URL + "Finance/GetTotalByUser",
                crossDomain: true,
                data: userData,
                dataType: 'json',
                success: function (r, textStatus, jqXHR) {

                    that.$financeContainer.empty();
                    //{"TotalBalance":-10.0,"TotalProfit":10.0,"TotalLoss":20.0,"PetsBalance":-10.0,"PetsProfit":10.0,"PetsLoss":20.0,"AssetsBalance":0.0,"AssetsProfit":0.0,"AssetsLoss":0.0}
                    // set color depending on + / - 
                    that.$financeContainer.append(that.generateFinanceLayout(r));

                    that.$financeContainer.fadeIn('slow');

                },
                error: function (responseData, textStatus, errorThrown) {
                    //error function
                }
            });
        },
        generateFinanceLayout: function (r) {
            return "<div class='totalBalanceContent'><h3>Total</h3><ul><li>Balance: <span class='green'>" + r.TotalBalance + "$</span></li> \
                    <li>Profit: <span class='green'>"+ r.TotalProfit + "$</span></li> \
                    <li>Loss: <span class='green'>" + r.TotalLoss + "$</span></li> \
                </ul></div> \
                <div class='totalBalanceContent'><h3>Assets</h3><ul><li>Balance: <span class='green'>" + r.AssetsBalance + "$</span></li> \
                    <li>Profit: <span class='green'>"+ r.AssetsProfit + "$</span></li> \
                    <li>Loss: <span class='green'>" + r.AssetsLoss + "$</span></li> \
                </ul></div> \
                <div class='totalBalanceContent'><h3>Pets</h3><ul><li>Balance: <span class='green'>" + r.PetsBalance + "$</span></li> \
                    <li>Profit: <span class='green'>"+ r.PetsProfit + "$</span></li> \
                    <li>Loss: <span class='green'>" + r.PetsLoss + "$</span></li> \
                </ul></div>"
        }
    }
})(jQuery);