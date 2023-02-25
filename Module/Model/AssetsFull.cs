using System;


namespace CryptoApp.Module.CryptoLogic
{
    public class AssetsFull
    {

        public string asset_id { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string website { get; set; }
        public string pegged { get; set; }
        public double volume_24h { get; set; }
        public double change_1h { get; set; }
        public double change_24h { get; set; }
        public double change_7d { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public double total_supply { get; set; }
        public double circulating_supply { get; set; }
        public double max_supply { get; set; }
        public long market_cap { get; set; }
        public long full_diluted_market_cap { get; set; }
    }

}
