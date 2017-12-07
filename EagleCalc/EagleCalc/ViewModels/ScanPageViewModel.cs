using System;
using System.Collections.Generic;
using System.Text;
using EagleCalc.Abstractions;
using EagleCalc.Models;
using EagleCalc.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EagleCalc.ViewModels
{
    public class ScanPageViewModel :BaseViewModel
    {
        public ScanPageViewModel(EagleBatch item = null, ProductInfo productInfo = null)
        {
            Title = productInfo.ProdCode + "/" + DateTime.Now.ToString("ddMMyyHHmmss");
            ProductInfo = productInfo;
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ProductInfo ProductInfo { get; set; }

        ObservableCollection<EagleBatch> scanList = new ObservableCollection<EagleBatch>();
        public ObservableCollection<EagleBatch> ScanList
        {
            get { return scanList; }
            set { SetProperty(ref scanList, value, "ScanList"); }
        }

        async Task RefreshBatchList()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = CloudService.GetTable<EagleBatch>();
                var list = await table.ReadListOfBatches(DateTime.Today, ProductInfo.ProductionLine, ProductInfo.ProdCode);

                ScanList.Clear();

                foreach (var item in list)
                    ScanList.Add(item);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("List not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
