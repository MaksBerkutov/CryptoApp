using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Module.ViewModel
{
    public class MainWindowViewModel:Base.ViewModel
    {
        public Base.Command GoToSite { get; }
        private ObservableCollection<string> _lang;
        private ObservableCollection<CryptoLogic.AssetsBase> _assetcItems;
        private CryptoLogic.AssetsBase _assetcSelectedItems;
        private CryptoLogic.AssetsBase _assetcSelectedItemsConvert;
        private CryptoLogic.AssetsFull _assetsFull;
        private int _defaultTop = 20;
        private double _convertToCount = 0;
        private double _convertResult = 0;
        private string _finder;
        private ScottPlot.WpfPlot _plot;
        public MainWindowViewModel(ScottPlot.WpfPlot plot)
        {
            _plot = plot;
            GoToSite = new Base.Command(GoToSiteHandler);
            _assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(_defaultTop));
            
        }
        private void GoToSiteHandler(object obj)
        {
            
            Process.Start($"https://www.coingecko.com/en/coins/{_assetcSelectedItems.Name.ToLower().Replace(" ","-")}");
        }
        //async
        public async Task LoadFullInfo()
        {
            if (_assetcSelectedItems != null)
                AssetsFull = await CryptoLogic.CryptingUp.CryptingUpApi.GetFullAssetsAsync(_assetcSelectedItems);
        }
        public async Task BuildGraphics()
        {
            if (_assetcSelectedItems == null) return;
            var points = await CryptoLogic.CryptingUp.CryptingUpApi.GetCryptoPoints(_assetcSelectedItems);
            List<double>x=new List<double>();   
            List<double>y=new List<double>();   
            List<double>y1=new List<double>();   

            points.ToList().ForEach(p =>
            {
                x.Add(p.time);
                y.Add(p.high);
                y1.Add(p.low);
            });
            _plot.Plot.Clear();
           
            _plot.Plot.AddScatter(x.ToArray(), y.ToArray(), System.Drawing.Color.Green, 1, 5, ScottPlot.MarkerShape.filledCircle, ScottPlot.LineStyle.Solid, "High");
            _plot.Plot.AddScatter(x.ToArray(), y1.ToArray(),System.Drawing.Color.Red,1,5,ScottPlot.MarkerShape.filledCircle,ScottPlot.LineStyle.Solid,"Low");
           
            _plot.Refresh();

        }
        //Object
        public CryptoLogic.AssetsFull AssetsFull
        {
            get => _assetsFull;
            set
            {
                _assetsFull = value;
                OnPropertyChanged(nameof(AssetsFull));
            }
        }
        public bool ItemsNoNull => _assetcSelectedItems != null;
        public string Finder
        {
            get => _finder;
            set
            {
                _finder = value;
                if (_finder.Replace(" ","").Any())
                {
                    _assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(_defaultTop).
                       ToList().FindAll(x => x.Name.Contains(_finder) || x.Symbol.Contains(_finder)).ToArray());
                    OnPropertyChanged(nameof(AssetsItems));

                }



                else DefaultTop = _defaultTop;
            }
        }
        public int DefaultTop
        {
            get=> _defaultTop; 
            set { 
                if (value > 0)
                {
                    _defaultTop = value;
                    _assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(_defaultTop));
                    OnPropertyChanged(nameof(AssetsItems));
                }  
            }
        }
        public ObservableCollection<string> Lang => _lang;
        public ObservableCollection<CryptoLogic.AssetsBase> AssetsItems
        {
            get => _assetcItems;
        }
        public double ConvertResult
        {
            get => _convertResult;
          
        }
        public double ConvertToCount
        {
            get => _convertToCount;
            set
            {
                if(value>0&& _assetcSelectedItems!=null&& AssetsSelectedItemsConvert != null)
                {
                    _convertToCount = value;
                    var course = _assetcItems.ToList().Find(x => x.Symbol == _assetcSelectedItems.Symbol).Current_Price / _assetcItems.ToList().Find(x => x.Symbol == AssetsSelectedItemsConvert.Symbol).Current_Price;
                    _convertResult = _convertToCount * course;
                    OnPropertyChanged(nameof(ConvertToCount));
                    OnPropertyChanged(nameof(ConvertResult));

                }
            }
        }
        public CryptoLogic.AssetsBase AassetcSelectedItems 
        {
            get => _assetcSelectedItems; 
            set
            {
                if (_assetcSelectedItems != value)
                {
                    _assetcSelectedItems = value;
                    ConvertToCount = _convertToCount;
                    LoadFullInfo();
                    BuildGraphics();
                    OnPropertyChanged(nameof(AassetcSelectedItems));
                    OnPropertyChanged(nameof(ItemsNoNull));
                }
            }
        }
        public CryptoLogic.AssetsBase AssetsSelectedItemsConvert
        {
            get => _assetcSelectedItemsConvert;
            set
            {
                if (_assetcSelectedItemsConvert != value)
                {
                    _assetcSelectedItemsConvert = value;
                    ConvertToCount = _convertToCount;
                    OnPropertyChanged(nameof(AssetsSelectedItemsConvert));
                }
            }
        }
    }
}
