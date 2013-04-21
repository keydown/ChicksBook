using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYServices.Models
{
    public class FinanceModel
    {
        public double? TotalBalance { get; set; }
        public double? TotalProfit { get; set; }
        public double? TotalLoss { get; set; }
        public double? PetsBalance { get; set; }
        public double? PetsProfit { get; set; }
        public double? PetsLoss { get; set; }
        public double? AssetsBalance { get; set; }
        public double? AssetsProfit { get; set; }
        public double? AssetsLoss { get; set; }

        public FinanceModel()
        {
        }

        public FinanceModel(double? totalBalance, double? totalProfit, double? totalLoss, double? petsBalance, double? petsProfit, double? petsLoss, double? assetsBalance, double? assetsProfit, double? assetsLoss)
        {
            this.TotalBalance = totalBalance;
            this.TotalProfit = totalProfit;
            this.TotalLoss = totalLoss;
            this.PetsBalance = petsBalance;
            this.PetsProfit = petsProfit;
            this.PetsLoss = petsLoss;
            this.AssetsBalance = assetsBalance;
            this.AssetsProfit = assetsProfit;
            this.AssetsLoss = assetsLoss;
        }
    }
}