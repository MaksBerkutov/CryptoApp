using Newtonsoft.Json;
using System;


namespace CryptoApp.Module.CryptoLogic
{
    public class AssetsFull
    {
        [JsonProperty("asset_id")]
        public string AssetId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("pegged")]
        public string Pegged { get; set; }
        [JsonProperty("volume_24h")]
        public double Volume24H { get; set; }
        [JsonProperty("change_1h")]
        public double Change1H { get; set; }
        [JsonProperty("change_24h")]
        public double Change24H { get; set; }
        [JsonProperty("change_7d")]
        public double Change7D { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("total_supply")]
        public double TotalSupply { get; set; }
        [JsonProperty("circulating_supply")]
        public double CirculatingSupply { get; set; }
        [JsonProperty("max_supply")]
        public double MaxSupply { get; set; }
        [JsonProperty("market_cap")]
        public long MarketCap { get; set; }
        [JsonProperty("FullDilutedMarketCap")]
        public long full_diluted_market_cap { get; set; }
    }

}
