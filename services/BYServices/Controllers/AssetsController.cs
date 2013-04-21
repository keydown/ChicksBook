using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BYServices.Models;
using Newtonsoft.Json.Linq;

namespace BYServices.Controllers
{
    public class AssetsController : Operations
    {
        BYEntities db = new BYEntities();

        [HttpPost]
        public HttpResponseMessage GetAssetInfo(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                int assetId = obj["assetId"].Value<int>();
                Asset aset = ValidateAssetId(assetId);
                return new AssetsModel(aset);
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage GetAssetsList(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                List<Asset> tempAssets = db.Assets.Where(x => x.AddedByUser == currentUser.Id || x.AddedByUser == 1).ToList();
                List<AssetsModel> assets = new List<AssetsModel>();
                foreach (Asset asse in tempAssets)
                {
                    assets.Add(new AssetsModel(asse));
                }
                return assets;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage SubmitAsset(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                string assetName = obj["assetName"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                Asset newAsset = new Asset() { 
                    AddedByUser = currentUser.Id,
                    Cost = 0,
                    Description = "no description",
                    Name = assetName
                };
                db.Assets.Add(newAsset);
                db.SaveChanges();
                return "success";
            });
            return msg;
        }


        [HttpPost]
        public HttpResponseMessage GetCurrentAssetsListByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                List<UserAsset> tempUserAssets = db.UserAssets.Where(x => x.UserId == currentUser.Id || x.UserId == 1).ToList();
                if (tempUserAssets == null)
                {
                    throw BuildHttpResponseException("No Assets by user", "ERR_NO_AS");
                }
                List<UserAssetsModel> userAssets = new List<UserAssetsModel>();
                foreach (UserAsset ua in tempUserAssets)
                {
                    userAssets.Add(new UserAssetsModel(ua));
                }
                return userAssets;
            });
            return msg;
        }

        [HttpPost]
        public HttpResponseMessage UpdateAssetHistoryByUser(JObject obj)
        {
            var msg = PerformOperation(() =>
            {
                string sessionId = obj["sessionId"].Value<string>();
                ValidateSessionId(sessionId);
                User currentUser = db.Users.SingleOrDefault(x => x.SessionId == sessionId);

                int assetId = obj["assetId"].Value<int>();
                Asset aset = ValidateAssetId(assetId);

                string assetStatus = obj["assetStatus"].Value<string>();
                if (assetStatus == null && assetStatus != "ADD" && assetStatus != "SELL" && assetStatus != "LOSE")
                {
                    throw BuildHttpResponseException("Invalid Asset Status", "ERR_AS_ST");
                }
                double assetPrice = obj["assetPrice"].Value<double>();
                if (assetPrice == 0.0D)
                {
                    assetPrice = 0;
                }
                double assetCount = obj["assetCount"].Value<double>();
                if (assetCount == 0.0D)
                {
                    assetCount = 1;
                }

                UserAsset ua = db.UserAssets.SingleOrDefault(x => x.UserId == currentUser.Id && x.AssetId == assetId);
                if (ua == null)
                {
                    UserAsset newAsset = new UserAsset()
                    {
                        UserId = currentUser.Id,
                        CurrentQuantity = 0,
                        AssetId = assetId
                    };
                    db.UserAssets.Add(newAsset);
                    db.SaveChanges();
                    ua = newAsset;
                }
                if (assetStatus == "ADD")
                {
                    ua.CurrentQuantity += assetCount;
                }
                else if (assetStatus == "SELL")
                {
                    ua.CurrentQuantity -= assetCount;
                }
                else if (assetStatus == "LOSE")
                {
                    ua.CurrentQuantity -= assetCount;
                }

                UserAssetsHistory assetHistory = new UserAssetsHistory()
                {
                    AssetId = assetId,
                    UserId = currentUser.Id,
                    AssetStatus = assetStatus,
                    AssetCount = assetCount,
                    AssetPrice = assetPrice,
                    ActionDate = DateTime.Now
                };
                db.UserAssetsHistories.Add(assetHistory);
                db.SaveChanges();
                return "success";
            });
            return msg;
        }

        private Asset ValidateAssetId(int assetId)
        {
            Asset aset = db.Assets.SingleOrDefault(x => x.Id == assetId);
            if (aset == null)
            {
                throw BuildHttpResponseException("Invalid Asset Id", "ERR_AS_ID");
            }
            return aset;
        }
    }
}