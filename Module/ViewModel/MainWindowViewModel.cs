using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CryptoApp.Module.Extension;
using System.Windows;

namespace CryptoApp.Module.ViewModel
{

    public partial class MainWindowViewModel:Base.ViewModel
    {
        //Commands
        public Base.Command GoToSite { get; }
        public Base.Command GoToTrade { get; }
        //private 
        private ObservableCollection<string> _lang;
        private ObservableCollection<CryptoLogic.AssetsBase> _assetcItems;
        private CryptoLogic.AssetsBase _assetcSelectedItems;
        private CryptoLogic.AssetsBase _assetcSelectedItemsConvert;
        private ObservableCollection<CryptoLogic.SellByItem> _sellitems;
        private CryptoLogic.SellByItem _sellitemsselceted;
        private CryptoLogic.AssetsFull _assetsFull;
        private int _defaultTop = 20; // Default load top 20 assests
        private double _convertToCount = 0;
        private double _convertResult = 0;
        private string _finder;
        private int _countOfPoint = 720;// Default count points chart 720 (1 points = 1 hours)
        private string _selectedLang = "En";//Default Lang En
        private ScottPlot.WpfPlot _plot;
        private string _selectedTheme = "White";//Default theme White

        public MainWindowViewModel(ScottPlot.WpfPlot plot)
        {
            //Check internet status 
            if (!CryptoLogic.CryptingUp.CryptingUpApi.IsInternetAvailable())
            {
                MessageBox.Show(ERROR_VALUE[_selectedLang][0], ERROR_VALUE[_selectedLang][1], MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.MainWindow.Close();
                Application.Current.Shutdown();
                return;
            }
             //Init Default Lang
            _lang = new ObservableCollection<string> (LANG_VALUE[_selectedLang]);
            //Save variable for building charts
            _plot = plot;
            //init commands
            GoToSite = new Base.Command(GoToSiteHandler);
            GoToTrade = new Base.Command(GoToTradeHandler);
            //load acsess
            _assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(_defaultTop));
            
        }
        //Handlers
        private void GoToTradeHandler(object obj)
        {
            if(_sellitemsselceted!=null)
                Process.Start(_sellitemsselceted.URL);

        }
        private void GoToSiteHandler(object obj)
        {
            Process.Start($"https://www.coingecko.com/en/coins/{_assetcSelectedItems.Name.ToLower().Replace(new char[] {'[',']' },"").Replace(" ","-")}");
        }
        //async function
        public async Task LoadSellItems()
        {
            if (_assetcSelectedItems != null)
               SellItems = await CryptoLogic.CryptingUp.CryptingUpApi.GetPriceAcsessAsync(_assetcSelectedItems);
        }
        public async Task LoadFullInfo()
        {
            if (_assetcSelectedItems != null)
                AssetsFull = await CryptoLogic.CryptingUp.CryptingUpApi.GetFullAssetsAsync(_assetcSelectedItems);
        }
        public async Task BuildGraphics()
        {
            if (_assetcSelectedItems == null) return;
            var points = await CryptoLogic.CryptingUp.CryptingUpApi.GetCryptoPoints(_assetcSelectedItems,_countOfPoint);
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
        //Object for View binding
        public ObservableCollection<CryptoLogic.SellByItem> SellItems
        {
            get => _sellitems;
            set
            {
                _sellitems = value;
                OnPropertyChanged(nameof(SellItems));
            }
        }
        public CryptoLogic.SellByItem SellItemsSelected
        {
            get => _sellitemsselceted;
            set
            {
                _sellitemsselceted = value;
                OnPropertyChanged(nameof(SellItemsSelected));
            }
        }

        public string[] AllTheme => System.Enum.GetNames(typeof(Theme));
        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme));
                }
            }
        }
        public string[] AllLang => LANG_VALUE.Keys.ToArray();
        public string SelectedLang
        {
            get=> _selectedLang;
            set
            {

                Lang = new ObservableCollection<string>(LANG_VALUE[value]);
                _selectedLang = value;
            }
        }
        public string Title => "CryptoApp";
        public int CountOfPoint
        {
            get => _countOfPoint;
            set
            {
                if (value >= 2 && value <= 2000)
                {
                    _countOfPoint = value;
                    BuildGraphics();
                    

                    OnPropertyChanged(nameof(CountOfPoint));
                }
                else CountOfPoint = 720;
            }
        }
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
                if (value >= 2 && value <= 225) 
                {
                    _defaultTop = value;
                    _assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(_defaultTop));
                    OnPropertyChanged(nameof(AssetsItems));
                }
                OnPropertyChanged(nameof(AssetsItems));


            }
        }
        public ObservableCollection<string> Lang
        {
            get => _lang;
            set
            {

                _lang = value;
                OnPropertyChanged(nameof(Lang));
                
            }
        }
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
                    LoadSellItems();
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
