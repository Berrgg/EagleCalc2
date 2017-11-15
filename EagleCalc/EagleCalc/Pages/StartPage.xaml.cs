using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EagleCalc.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
		public StartPage ()
		{
			InitializeComponent ();
            BindingContext = new ViewModels.StartPageViewModel();
		}
	}
}