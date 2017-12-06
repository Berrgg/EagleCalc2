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
		public ScanPage (EagleBatch item = null, ProductInfo productInfo = null)
		{
			InitializeComponent ();
            BindingContext = new ScanPageViewModel(item, productInfo);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            scanText.Focus();
        }

        private void ScanText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}