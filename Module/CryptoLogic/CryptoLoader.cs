using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Module.CryptoLogic
{
   
    public class AssetsFull
    {



        public string Name { get; set; }
        public string Image { get; set; }
        public string Symbol { get; set; }
        public double Current_Price { get; set; }
        public double Price_Change_Percentage_24h { get; set; }
    }
    public class ExchangeMarkets
    {
        //Для навігації
        public static string Next = null;
        public static string Pred = null;
        public static int itemsCount = 10;

        private string exchange_id;
        public string ExchangeId => exchange_id;

        private string base_asset;
        public string Base => base_asset;

        decimal spread;
        public decimal Spread=>spread;

        decimal volume_24h;
        public decimal Volume24h => volume_24h;

        string status;
        public string Status => status;

        private string quote_asset;
        public string Quote => quote_asset;

        private DateTime created_at;
        public DateTime CreatedAt => created_at;

        private DateTime updated_at;
        public DateTime UpdatedAt => updated_at;

        public string Symbol => $"{base_asset}-{quote_asset}";
    }

    public class CryptingExchanges 
    {
        private string name;
        public string Name => name;

        private string exchangesId;
        public string ExchangesId => exchangesId;

        private decimal circulationOfCrypto;
        public decimal CirculationOfCrypto => circulationOfCrypto;

        private string website;
        public string WebSite => website;
        public CryptingExchanges(string name, string exchangesId, decimal circulationOfCrypto, string website)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.exchangesId = exchangesId ?? throw new ArgumentNullException(nameof(exchangesId));
            this.circulationOfCrypto = circulationOfCrypto;
            this.website = website ?? throw new ArgumentNullException(nameof(website));
        }
        

    }
   
    namespace CryptingUp
    {
        static class CryptingUpApi
        {
           
            private static readonly string BaseUrl = "https://cryptingup.com/api";
            //Отримуємо всі біржі
            public static async Task<ObservableCollection<CryptingExchanges>> GetExchanges()
            {
               

                var results = new ObservableCollection<CryptingExchanges>();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{BaseUrl}/exchanges");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        dynamic Elements = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                        foreach (var item in Elements.exchanges)
                            results.Add(new CryptingExchanges(item.name.ToString(), item.exchange_id.ToString(), Convert.ToDecimal(item.volume_24h), item.website.ToString()));
                        

                        return results;
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                return null;

            }
            //Отримання обмінних пунктів
            //Наступні елементиCount елементів ринку
            public static async Task GetNextMarketsAsync(List<ExchangeMarkets> cryptoItems)
            {
                cryptoItems = null;
                using (var httpClient = new HttpClient())
                {
                  
                    List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("size", ExchangeMarkets.itemsCount.ToString()) };
                    if (ExchangeMarkets.Next != null) keyValuePairs.Add(new KeyValuePair<string, string>("start", ExchangeMarkets.Next));

                    var response = await httpClient.GetAsync($"{BaseUrl}/markets?{new FormUrlEncodedContent(keyValuePairs.ToArray()).ReadAsStringAsync().Result}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        dynamic Elements = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                        foreach (var item in Elements.markets)
                        {

                        }
                        ExchangeMarkets.Pred = ExchangeMarkets.Next;
                        ExchangeMarkets.Next = Elements.next.ToString();
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                    
                



                }
            }

            //Отримання обмінників конкретної біржі
            public static async Task GetExchangesMarketsAsync(CryptingExchanges obj)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{BaseUrl}/exchanges/{obj?.ExchangesId}/markets");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        dynamic Elements = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                        foreach (var item in Elements.exchanges)
                        {
                            //results.Add(new CryptingExchanges(item.name.ToString(), item.exchange_id.ToString(), Convert.ToDecimal(item.volume_24h), item.website.ToString()));
                        }

                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
            }

            public static  ObservableCollection<AssetsFull> GetAssetsFull(int def)
            {
                using (var httpClient = new HttpClient())
                {
                    var response =  httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={def}&page=1&sparkline=false").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result =  response.Content.ReadAsStringAsync().Result;
                        ObservableCollection<AssetsFull> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsFull>>(result);
                        return currencies;


                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                return null;
            }
            public static async Task<ObservableCollection<AssetsFull>> GetAssetsFullAsync(int def)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={def}&page=1&sparkline=false");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        ObservableCollection<AssetsFull> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsFull>>(result);
                        return currencies;


                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                return null;
            }

        }
    }
}
