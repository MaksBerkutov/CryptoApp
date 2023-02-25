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

        private static readonly string BaseUrl = "https://cryptingup.com/api";
            //Отримуємо всі біржі  == Не знадобилося
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
            //Наступні елементиCount елементів ринку == Не знадобилося тому нема функції для повернення на сторінку назад
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
            //Отримання топу крипто монет
            public static  ObservableCollection<AssetsBase> GetAssetsBase(int def)
            {
                using (var httpClient = new HttpClient())
                {
                    var response =  httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={def}&page=1&sparkline=false").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result =  response.Content.ReadAsStringAsync().Result;
                        ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
                        return currencies;


                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                return null;
            }
            public static async Task<ObservableCollection<AssetsBase>> GetAssetsBaseAsync(int def)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={def}&page=1&sparkline=false");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
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
            public static async Task<ObservableCollection<SellByItem>> GetPriceAcsessAsync(AssetsBase obj)
            {
                ObservableCollection < SellByItem > returns = new ObservableCollection<SellByItem> ();
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{obj.Name.ToLower().Replace(new char[] { '[', ']' }, "").Replace(" ", "-")}");
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);  
                        foreach(var item in data.tickers)
                        
                            returns.Add( new SellByItem() { IN=item["base"].ToString(),TO=item.target.ToString(),URL=item.trade_url.ToString() });
                        return returns;
                        //ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
                        //return currencies;


                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                return null;
            }
            //Отримманя всіх бірж на яких можна купити крипто монету
            public static void GetExchangesInAssets(AssetsBase obj)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}/markets").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        //ObservableCollection<AssetsBase> currencies = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<AssetsBase>>(result);
                        //return currencies;


                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
            }

            public static AssetsFull GetFullAssets(AssetsBase obj)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                   
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<AssetsFull>((Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result).asset.ToString()));

                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }

                }
                return null;
            }
            public static async Task<AssetsFull> GetFullAssetsAsync(AssetsBase obj)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{BaseUrl}/assets/{obj.Symbol.ToUpper()}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        return Newtonsoft.Json.JsonConvert.DeserializeObject<AssetsFull>((Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result).asset.ToString()));

                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }

                }
                return null;
            }
            public static async Task<ObservableCollection<CryptoPoint>> GetCryptoPoints(AssetsBase obj, int counPoints = 720)
            {
                //
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://min-api.cryptocompare.com/data/v2/histohour?fsym={obj.Symbol.ToUpper()}&tsym=USD&limit={counPoints}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                      
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CryptoPoint>>(Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result).Data.Data.ToString());

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
