using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using CryptoApp.Module.Extension;


namespace CryptoApp.Module.CryptoLogic.CryptingUp
{
  
  
        static class CryptingUpApi
        {
        private static readonly string BaseUrl = "https://cryptingup.com/api";
        //Перевіряемо звязок
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var result = ping.Send("8.8.8.8", 2000); 
                    return result.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        
        //Отримання топу крипто монет
        public static async Task<ObservableCollection<AssetsBase>> GetAssetsBase(int maximum)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={maximum}&page=1&sparkline=false");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
                    return currencies;


                }
                else
                {
                    throw new Exception("Request failed with status code: " + response.StatusCode);
                }
            }
        }
        public static async Task<ObservableCollection<AssetsBase>> GetAssetsBaseAsync(int maximum)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={maximum}&page=1&sparkline=false");

                if (response.IsSuccessStatusCode)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(await response.Content.ReadAsStringAsync());
                else
                    throw new Exception("Request failed with status code: " + response.StatusCode);
            }
        }
        //+
        public static async Task<ObservableCollection<SellByItem>> GetPriceAcsessAsync(AssetsBase obj)
        {
            ObservableCollection<SellByItem> returns = new ObservableCollection<SellByItem>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{obj.Name.ToLower().Replace(new char[] { '[', ']' }, "").Replace(" ", "-")}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                    foreach (var item in data.tickers)

                        returns.Add(new SellByItem() { In = item["base"].ToString(), To = item.target.ToString(), URL = item.trade_url.ToString() });
                    return returns;
                    //ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
                    //return currencies;


                }
                else
                {
                    throw new Exception("Request failed with status code: " + response.StatusCode);
                }
            }
        }
        //Отримманя всіх бірж на яких можна купити крипто монету
        public static async void GetExchangesInAssets(AssetsBase obj)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}/markets");

                if (response.IsSuccessStatusCode)
                    await response.Content.ReadAsStringAsync();
                else
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                
            }
        }

        public static async Task<AssetsFull> GetFullAssets(AssetsBase obj)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}");
                if (response.IsSuccessStatusCode)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<AssetsFull>((Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).asset.ToString()));
                else
                   throw new Exception("Request failed with status code: " + response.StatusCode);

            }
          
        }
        public static async Task<AssetsFull> GetFullAssetsAsync(AssetsBase obj)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}");

                if (response.IsSuccessStatusCode)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<AssetsFull>((Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).asset.ToString()));
                else
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
            }

        }
        public static async Task<ObservableCollection<CryptoPoint>> GetCryptoPoints(AssetsBase obj, int coun_points = 720)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://min-api.cryptocompare.com/data/v2/histohour?fsym={obj.Symbol.ToUpper()}&tsym=USD&limit={coun_points}");
                if (response.IsSuccessStatusCode)
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CryptoPoint>>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync()).Data.Data.ToString());
                else
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }

    }
    
}
