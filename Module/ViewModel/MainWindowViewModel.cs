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
     
       

        public MainWindowViewModel(ScottPlot.WpfPlot inputPlot)
        {
            //Check internet status 
            if (!CryptoLogic.CryptingUp.CryptingUpApi.IsInternetAvailable())
            {
                MessageBox.Show(ErrorValue[selectedLang][0], ErrorValue[selectedLang][1], MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.MainWindow.Close();
                Application.Current.Shutdown();
                return;
            }
             //Init Default Lang
            _lang = new ObservableCollection<string> (LangValue[selectedLang]);
            //Save variable for building charts
            plot = inputPlot;
            //init commands
            GoToSite = new Base.Command(GoToSiteHandler);
            GoToTrade = new Base.Command(GoToTradeHandler);
            //load acsess
            var task = Task.Run(() => CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(defaultTop));
            task.Wait();
            assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(task.Result);
            
        }
        //Handlers
        private void GoToTradeHandler(object obj)
        {
            if(sellitemsselceted!=null)
                Process.Start(sellitemsselceted.URL);

        }
        private void GoToSiteHandler(object obj)
        {
            Process.Start($"https://www.coingecko.com/en/coins/{assetcSelectedItems.Name.ToLower().Replace(new char[] {'[',']' },"").Replace(" ","-")}");
        }
        //async function
        public async Task LoadSellItems()
        {
            if (assetcSelectedItems != null)
               SellItems = await CryptoLogic.CryptingUp.CryptingUpApi.GetPriceAcsessAsync(assetcSelectedItems);
        }
        public async Task LoadFullInfo()
        {
            if (assetcSelectedItems != null)
                AssetsFull = await CryptoLogic.CryptingUp.CryptingUpApi.GetFullAssetsAsync(assetcSelectedItems);
        }
        public async Task BuildGraphics()
        {
            if (assetcSelectedItems == null) return;
            var points = await CryptoLogic.CryptingUp.CryptingUpApi.GetCryptoPoints(assetcSelectedItems,countOfPoint);
            List<double>x=new List<double>();   
            List<double>y=new List<double>();   
            List<double>y1=new List<double>();   
           

            points.ToList().ForEach(p =>
            {
                x.Add(p.Time);
                y.Add(p.High);
                y1.Add(p.Low);
            });
            plot.Plot.Clear();
            plot.Plot.AddScatter(x.ToArray(), y.ToArray(), System.Drawing.Color.Green, 1, 5, ScottPlot.MarkerShape.filledCircle, ScottPlot.LineStyle.Solid, "High");
            plot.Plot.AddScatter(x.ToArray(), y1.ToArray(),System.Drawing.Color.Red,1,5,ScottPlot.MarkerShape.filledCircle,ScottPlot.LineStyle.Solid,"Low");
            plot.Refresh();

        }
      
        //Object for View binding
        public ObservableCollection<CryptoLogic.SellByItem> SellItems
        {
            get => sellitems;
            set
            {
                sellitems = value;
                OnPropertyChanged(nameof(SellItems));
            }
        }
        public CryptoLogic.SellByItem SellItemsSelected
        {
            get => sellitemsselceted;
            set
            {
                sellitemsselceted = value;
                OnPropertyChanged(nameof(SellItemsSelected));
            }
        }

        public string[] AllTheme => System.Enum.GetNames(typeof(Theme));
        public string SelectedTheme
        {
            get { return selectedTheme; }
            set
            {
                if (selectedTheme != value)
                {
                    selectedTheme = value;
                    OnPropertyChanged(nameof(SelectedTheme));
                }
            }
        }
        public string[] AllLang => LangValue.Keys.ToArray();
        public string SelectedLang
        {
            get=> selectedLang;
            set
            {

                Lang = new ObservableCollection<string>(LangValue[value]);
                selectedLang = value;
            }
        }
        public string Title => "CryptoApp";
        public int CountOfPoint
        {
            get => countOfPoint;
            set
            {
                if (value >= 2 && value <= 2000)
                {
                    countOfPoint = value;
                    BuildGraphics();
                    

                    OnPropertyChanged(nameof(CountOfPoint));
                }
                else CountOfPoint = 720;
            }
        }
        public CryptoLogic.AssetsFull AssetsFull
        {
            get => assetsFull;
            set
            {
                assetsFull = value;
                OnPropertyChanged(nameof(AssetsFull));
            }
        }
        public bool ItemsNoNull => assetcSelectedItems != null;
        public string Finder
        {
            get => finder;
            set
            {
                finder = value;
                if (finder.Replace(" ","").Any())
                {
                    var task = Task.Run(() => CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(defaultTop));
                    task.Wait();
                    assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(task.Result.
                       ToList().FindAll(x => x.Name.Contains(finder) || x.Symbol.Contains(finder)).ToArray());
                    OnPropertyChanged(nameof(AssetsItems));

                }



                else DefaultTop = defaultTop;
            }
        }
        public int DefaultTop
        {
            get=> defaultTop; 
            set {
                if (value >= 2 && value <= 225) 
                {
                    defaultTop = value;
                    var task = Task.Run(() => CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsBase(defaultTop));
                    task.Wait();
                    assetcItems = new ObservableCollection<CryptoLogic.AssetsBase>(task.Result);
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
            get => assetcItems;
        }
        public double ConvertResult
        {
            get => convertResult;
          
        }
        public double ConvertToCount
        {
            get => convertToCount;
            set
            {
                if(value>0&& assetcSelectedItems!=null&& AssetsSelectedItemsConvert != null)
                {
                    convertToCount = value;
                    var course = assetcItems.ToList().Find(x => x.Symbol == assetcSelectedItems.Symbol).CurrentPrice / assetcItems.ToList().Find(x => x.Symbol == AssetsSelectedItemsConvert.Symbol).CurrentPrice;
                    convertResult = convertToCount * course;
                    OnPropertyChanged(nameof(ConvertToCount));
                    OnPropertyChanged(nameof(ConvertResult));

                }
            }
        }
        public CryptoLogic.AssetsBase AassetcSelectedItems 
        {
            get => assetcSelectedItems; 
            set
            {
                if (assetcSelectedItems != value)
                {
                    assetcSelectedItems = value;
                    ConvertToCount = convertToCount;
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
            get => assetcSelectedItemsConvert;
            set
            {
                if (assetcSelectedItemsConvert != value)
                {
                    assetcSelectedItemsConvert = value;
                    ConvertToCount = convertToCount;
                    OnPropertyChanged(nameof(AssetsSelectedItemsConvert));
                }
            }
        }

        private ObservableCollection<string> _lang;
        private ObservableCollection<CryptoLogic.AssetsBase> assetcItems;
        private CryptoLogic.AssetsBase assetcSelectedItems;
        private CryptoLogic.AssetsBase assetcSelectedItemsConvert;
        private ObservableCollection<CryptoLogic.SellByItem> sellitems;
        private CryptoLogic.SellByItem sellitemsselceted;
        private CryptoLogic.AssetsFull assetsFull;
        private int defaultTop = 20; // Default load top 20 assests
        private double convertToCount = 0;
        private double convertResult = 0;
        private string finder;
        private int countOfPoint = 720;// Default count points chart 720 (1 points = 1 hours)
        private string selectedLang = "En";//Default Lang En
        private ScottPlot.WpfPlot plot;
        private string selectedTheme = "White";//Default theme White
    }
}
