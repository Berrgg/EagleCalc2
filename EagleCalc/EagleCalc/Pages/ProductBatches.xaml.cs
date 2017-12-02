using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EagleCalc.ViewModels;
using EagleCalc.Models;

namespace EagleCalc.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductBatches : ContentPage
	{
		public ProductBatches (ProductInfo prodInfo = null)
		{
			InitializeComponent ();
            BindingContext = new ProductBatchesViewModel(prodInfo);
		}
	}
}