using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using ScrapingTestMaui.ViewModel;

namespace ScrapingTestMaui
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		public MainPage(MainPageViewModel model) : this()
		{
			BindingContext = model;
		}

		private MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			// await ViewModel?.UpdateData();
        }
    }
}
