using System;


namespace CryptoApp.Module.CryptoLogic
{
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
        public decimal Spread => spread;

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

        public ExchangeMarkets(string exchange_id, string base_asset, decimal spread, decimal volume_24h, string status, string quote_asset, DateTime created_at, DateTime updated_at)
        {
            this.exchange_id = exchange_id ?? throw new ArgumentNullException(nameof(exchange_id));
            this.base_asset = base_asset ?? throw new ArgumentNullException(nameof(base_asset));
            this.spread = spread;
            this.volume_24h = volume_24h;
            this.status = status ?? throw new ArgumentNullException(nameof(status));
            this.quote_asset = quote_asset ?? throw new ArgumentNullException(nameof(quote_asset));
            this.created_at = created_at;
            this.updated_at = updated_at;
        }

    }

}
