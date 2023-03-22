using Newtonsoft.Json;

namespace CryptoApp.Module.CryptoLogic
{
    public class AssetsBase
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Symbol { get; set; }
        [JsonProperty("Current_Price")]
        public double CurrentPrice { get; set; }
        [JsonProperty("Price_Change_Percentage_24h")]
        public double Price24H { get; set; }
    }

}
