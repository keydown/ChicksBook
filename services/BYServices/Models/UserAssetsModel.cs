using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BYServices.Models
{
    public class UserAssetsModel
    {
        BYEntities db = new BYEntities();
        public int UsersId { get; set; }
        public int AssetId { get; set; }
        public string Asset { get; set; }
        public double CurrentQuantity { get; set; }

        public UserAssetsModel(UserAsset asset)
        {
            this.UsersId = asset.UserId;
            this.AssetId = asset.AssetId;
            this.Asset = db.Assets.SingleOrDefault(x => x.Id == this.AssetId).Name.ToString();
            this.CurrentQuantity = asset.CurrentQuantity;
        }
        public UserAssetsModel()
        {
            this.UsersId = 1;
            this.AssetId = 1;
            this.Asset = " No asset";
            this.CurrentQuantity = 0;
        }
    }

    public class UserAssetHistoryModel
    {
        BYEntities db = new BYEntities();
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public double Count { get; set; }
        public double Price { get; set; }
        public string Action { get; set; }

        public UserAssetHistoryModel(UserAssetsHistory asset)
        {
            this.Id = asset.Id;
            this.AssetId = asset.AssetId;
            this.AssetName = db.Assets.SingleOrDefault(x => x.Id == this.AssetId).Name.ToString();
            this.Price = asset.AssetPrice;
            this.Count = asset.AssetCount;
            if (asset.AssetStatus.Trim().Contains("ADD"))
            {
                this.Action = "Added";
            }
            else if (asset.AssetStatus.Trim().Contains("SELL"))
            {
                this.Action = "Sold";
            }
            else if (asset.AssetStatus.Trim().Contains("LOSE"))
            {
                this.Action = "Lost";
            }
            else this.Action = "No Info";
        }
    }
}