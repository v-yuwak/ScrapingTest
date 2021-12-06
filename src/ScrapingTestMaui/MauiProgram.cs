using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ScrapingTestCommon;
using ScrapingTestMaui.ViewModel;

namespace ScrapingTestMaui
{
	public static class MauiProgram
	{
		public static MauiApp App { get; private set; }

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddTransient<FreeBookScraping>();
			builder.Services.AddTransient<MainPageViewModel>();

			App = builder.Build();
			return App;
		}
	}
}