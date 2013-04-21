using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{ 
    public class FinanceController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetTotalByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                List<UserAssetsHistory> assetHistory = db.UserAssetsHistories.Where(x => x.UserId == currentUser.Id).ToList();
                double? totalBalance = 0.0d;
                double? totalLoss = 0.0d;
                double? totalProfit = 0.0d;
                foreach (UserAssetsHistory ah in assetHistory)
                {
                    if (ah.AssetStatus.Trim() == "ADD")
                    {
                        totalLoss += ah.AssetPrice;
                    }
                    else if (ah.AssetStatus.Trim() == "SELL")
                    {
                        totalProfit += ah.AssetPrice;
                    }
                    else if (ah.AssetStatus.Trim() == "LOSS")
                    {
                        totalLoss += ah.AssetPrice;
                    }
                }

                totalBalance = totalProfit - totalLoss;

                List<UserFarmHistory> uf = db.UserFarmHistories.Where(x => x.UserId == currentUser.Id).ToList();
                double? assetsBalance = totalBalance;
                double? assetsProfit = totalProfit;
                double? assetsLoss = totalLoss;

                double? petsBalance = 0.0d;
                double? petsProfit = 0.0d;
                double? petsLoss = 0.0d;
                foreach (UserFarmHistory ufh in uf)
                {
                    if (ufh.PetStatus.Trim() == "ADDP" || ufh.PetStatus.Trim() == "ADDH" || ufh.PetStatus.Trim() == "ADDE")
                    {
                        petsLoss += ufh.PetPrice;
                    }
                    else if (ufh.PetStatus.Trim() == "SELLP" || ufh.PetStatus.Trim() == "SELLH" || ufh.PetStatus.Trim() == "SELLE")
                    {
                        petsProfit += ufh.PetPrice;
                    }
                    else if (ufh.PetStatus.Trim() == "LOSSP" || ufh.PetStatus.Trim() == "LOSSH" || ufh.PetStatus.Trim() == "LOSSE")
                    {
                        petsLoss += ufh.PetPrice;
                    }
                }

                petsBalance = petsProfit - petsLoss;
                totalBalance = assetsBalance + petsBalance;
                totalProfit += petsProfit;
                totalLoss += petsLoss;

                return new FinanceModel(totalBalance, totalProfit, totalLoss, petsBalance, petsProfit, petsLoss, assetsBalance, assetsProfit, assetsLoss);
            });
            return msg;
        }
    }
}
