using System;


namespace CryptoApp.Module.CryptoLogic
{
    public class CryptoPoint
    {
        public TimeSpan TimeConvert => new TimeSpan(Time);
        public long Time { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }

}
