using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp.Module.ViewModel
{
    public class MainWindowViewModel:Base.ViewModel
    {
        private ObservableCollection<string> _lang;
        private ObservableCollection<CryptoLogic.AssetsFull> _assetcItems;
        private CryptoLogic.AssetsFull _assetcSelectedItems;
        private CryptoLogic.AssetsFull _assetcSelectedItemsConvert;
        private int _defaultTop = 20;
        private double _convertToCount = 0;
        private double _convertResult = 0;
        private string _finder;

        public MainWindowViewModel()
        {

            _assetcItems = new ObservableCollection<CryptoLogic.AssetsFull>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsFull(_defaultTop));
            
        }
        public string Finder
        {
            get => _finder;
            set
            {
                _finder = value;
                if (_finder.Replace(" ","").Any())
                {
                    _assetcItems = new ObservableCollection<CryptoLogic.AssetsFull>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsFull(_defaultTop).
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
                    _assetcItems = new ObservableCollection<CryptoLogic.AssetsFull>(CryptoLogic.CryptingUp.CryptingUpApi.GetAssetsFull(_defaultTop));
                    OnPropertyChanged(nameof(AssetsItems));
                }  
            }
        }
        //Object
        public ObservableCollection<string> Lang => _lang;
        public ObservableCollection<CryptoLogic.AssetsFull> AssetsItems
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
        public CryptoLogic.AssetsFull AassetcSelectedItems 
        {
            get => _assetcSelectedItems; 
            set
            {
                if (_assetcSelectedItems != value)
                {
                    _assetcSelectedItems = value;
                    ConvertToCount = _convertToCount;

                    OnPropertyChanged(nameof(AassetcSelectedItems));
                }
            }
        }
        public CryptoLogic.AssetsFull AssetsSelectedItemsConvert
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
