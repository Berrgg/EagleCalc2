using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using EagleCalc.Abstractions;
using EagleCalc.Helpers;
using EagleCalc.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

namespace EagleCalc.ViewModels
{
    public class StartPageViewModel : BaseViewModel
    {
        public StartPageViewModel()
        {
            Title = "Start page";

            TakeListsCommand = new Command(async () => await TakeLists());
            TakeProducts = new Command(async () => await TakeCustomerProducts());

            TakeListsCommand.Execute(null);
         //   TakeProducts.Execute(null);
        }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();
        public ICommand TakeListsCommand { get; }
        public ICommand TakeProducts { get; }

        public string SelectedLine { get; set; }

        bool isPickerEnable = true;
        public bool IsPickerEnable
        {
            get { return isPickerEnable; }
            set
            {
                if(isPickerEnable != value)
                {
                    isPickerEnable = value;
                    OnPropertyChanged(nameof(IsPickerEnable));
                }
            }
        }

        string customerName;
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                if(customerName != value)
                {
                    customerName = value;
                    OnPropertyChanged(nameof(CustomerName));
                }
            }
        }

        ObservableCollection<string> lineNames = new ObservableCollection<string>();
        public ObservableCollection<string> LineNames
        {
            get { return lineNames; }
            set { SetProperty(ref lineNames, value, "LineNames"); }
        }

        ObservableCollection<string> customers = new ObservableCollection<string>();
        public ObservableCollection<string> Customers
        {
            get { return customers; }
            set { SetProperty(ref customers, value, "Customers"); }
        }

        ObservableCollection<string> products = new ObservableCollection<string>();
        public ObservableCollection<string> Products
        {
            get { return products; }
            set { SetProperty(ref products, value, "Products"); }
        }

        async Task TakeLists()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table = CloudService.GetTable<Line>();
                var list = await table.ReadAllItemsAsync();

                foreach (var item in list)
                    LineNames.Add(item.LineName);

                LineNames = new ObservableCollection<string>(LineNames.OrderBy(i => i));

                var tblCustomer = CloudService.GetTable<Customer>();
                var listCustomer = await tblCustomer.ReadAllItemsAsync();

                foreach (var itemCust in listCustomer)
                    Customers.Add(itemCust.CustomerName);

                Customers = new ObservableCollection<string>(Customers.OrderBy(i => i));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Lines not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task TakeCustomerProducts()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var table1 = CloudService.GetTable<Product>();
                var products = await table1.ReadProducts(CustomerName);

                Products.Clear();

                foreach (var prod in products)
                    Products.Add(prod.Description);

                Products = new ObservableCollection<string>(Products.OrderBy(i => i));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Products not loaded", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
