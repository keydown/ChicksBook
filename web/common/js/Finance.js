(function () {
    var i;
    Finance = function (param) {
        this.user = param.user;
        this.$financeContainer = param.financeContainer;
    };
    Finance.prototype = {
        displayFinances: function () {

			$(document.documentElement).removeClass(SiteController.PAGE_CLASS_NAME);
			SiteController.PAGE_CLASS_NAME = "finances-page";
			$(document.documentElement).addClass(SiteController.PAGE_CLASS_NAME);

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
            var sb = [];
            sb.push( "<div class='totalBalanceContent'>" );
                sb.push( "<h3 class='sub'>Total</h3>");
                sb.push( "<table class='grid'>" );
                    sb.push( "<tbody>" );
                        sb.push( "<tr>" );
                            sb.push( "<th>Balance</th><td class='green'>" + r.TotalBalance + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>"+ r.TotalProfit + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>" + r.TotalLoss + "$</td>" );
                        sb.push( "</tr>" );
                    sb.push( "</tbody>" );
                sb.push( "</table>" );
            sb.push( "</div>" );
            sb.push( "<div class='totalBalanceContent'>" );
                sb.push( "<h3 class='sub'>Assets</h3>");
                sb.push( "<table class='grid'>" );
                    sb.push( "<tbody>" );
                        sb.push( "<tr>" );
                            sb.push( "<th>Balance</th><td class='green'>" + r.AssetsBalance + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>"+ r.AssetsProfit + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>" + r.AssetsLoss + "$</td>" );
                        sb.push( "</tr>" );
                    sb.push( "</tbody>" );
                sb.push( "</table>" );
            sb.push( "</div>" );
            sb.push( "<div class='totalBalanceContent'>" );
                sb.push( "<h3 class='sub'>Pets</h3>");
                sb.push( "<table class='grid'>" );
                    sb.push( "<tbody>" );
                        sb.push( "<tr>" );
                            sb.push( "<th>Balance</th><td class='green'>" + r.PetsBalance + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>"+ r.PetsProfit + "$</td>" );
                            sb.push( "<th>Profit</th><td class='green'>" + r.PetsLoss + "$</td>" );
                        sb.push( "</tr>" );
                    sb.push( "</tbody>" );
                sb.push( "</table>" );
            sb.push( "</div>" );
            return sb.join("");
        }
    }
})(jQuery);