using EagleCalc.Abstractions;
using EagleCalc.Helpers;
using EagleCalc.Services;
using Xamarin.Forms;

namespace EagleCalc
{
	public partial class App : Application
	{
		public App ()
		{
            ServiceLocator.Add<ICloudService, AzureCloudService>();
			MainPage = new NavigationPage(new Pages.StartPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
