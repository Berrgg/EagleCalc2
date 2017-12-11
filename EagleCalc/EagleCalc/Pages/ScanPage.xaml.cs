using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EagleCalc.ViewModels;
using EagleCalc.Models;

namespace EagleCalc.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanPage : ContentPage
	{
        ScanPageViewModel vm;
		public ScanPage (EagleBatch item = null, ProductInfo productInfo = null)
		{
			InitializeComponent ();
            BindingContext = vm = new ScanPageViewModel(item, productInfo);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            scanText.Focus();
        }

        private void ScanText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(scanText.Text.Length == 31)
                vm.AddItemCommand.Execute(null);
        }

        private void MenuItem_DeleteClicked(object sender, EventArgs e)
        {
            var batchItem = (sender as MenuItem).CommandParameter as EagleBatch;
            vm.ScanList.Remove(batchItem);
            vm.CurrentBatch = batchItem;
            vm.DeleteItemCommand.Execute(null);
        }

        private void ButtonConfrm_Clicked(object sender, EventArgs e)
        {
            vm.SaveItemCommand.Execute(null);
            ResetEntryText();
            vm.ButtonCancelEnabled = false;
        }

        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            //RemoveLastItemOnListView();
            //ResetEntryText();
            //CalculateWeightedAverage();
            //ButtonCancelEnabled(false);
        }


        private void ResetEntryText()
        {
            scanText.Focus();
            scanText.Text = "";
        }

    }
}