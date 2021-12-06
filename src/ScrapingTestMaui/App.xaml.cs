using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using ScrapingTestMaui.ViewModel;
using Application = Microsoft.Maui.Controls.Application;

namespace ScrapingTestMaui
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var model = MauiProgram.App.Services.GetService(typeof(MainPageViewModel)) as MainPageViewModel;
			MainPage = new NavigationPage(new MainPage(model));
		}
	}
}
