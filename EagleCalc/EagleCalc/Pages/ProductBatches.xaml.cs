using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EagleCalc.ViewModels;

namespace EagleCalc.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductBatches : ContentPage
	{
		public ProductBatches ()
		{
			InitializeComponent ();
            BindingContext = new ProductBatchesViewModel(null, null, null, null);
		}
	}
}