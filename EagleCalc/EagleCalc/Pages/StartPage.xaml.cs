﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EagleCalc.ViewModels;

namespace EagleCalc.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
        StartPageViewModel vm;
		public StartPage ()
		{
			InitializeComponent ();
            BindingContext = vm = new StartPageViewModel();
		}

        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(vm != null)
            {
                vm.SetFields();
                vm.TakeProductsCommand.Execute(null);
            }
        }

        private void PickerProduct_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(vm != null)
            {
                vm.ProductSelectedIndex = pickerProduct.SelectedIndex;
            }
        }
    }
}