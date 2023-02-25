using System;

namespace CryptoApp.Module.CryptoLogic
{

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

}
