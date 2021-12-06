using Microsoft.Maui.Essentials;
using Reactive.Bindings;
using ScrapingTestCommon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapingTestMaui.ViewModel
{
    public class MainPageViewModel
    {
        public FreeBookScraping Scraping { get; }

        public ReactiveCollection<TitleInfo> Titles { get; } = new ReactiveCollection<TitleInfo>();
        public ReactiveProperty<bool> IsIdle { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<int> Page { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<FreeBookSearchMode> Mode { get; } = new ReactiveProperty<FreeBookSearchMode>(FreeBookSearchMode.Media);
        public ReactiveProperty<string> ModeLabel { get; } = new ReactiveProperty<string>("Free Books");
        public ReactiveProperty<string> DebugDescription { get; } = new ReactiveProperty<string>("test");

        public MainPageViewModel(FreeBookScraping scraping)
        {
            Scraping = scraping;

            Mode.Subscribe(async _ =>
            {
                if (Page.Value == 1)
                {
                    await UpdateData();
                }
                else
                {
                    Page.Value = 1;
                }
            });

            Page.Subscribe(async _ =>
            {
                await UpdateData();
            });
        }

        public async Task UpdateData()
        {
            if (IsIdle.Value)
            {
                IsIdle.Value = false;

                Titles.ClearOnScheduler();
                Scraping.Page = Page.Value;
                Scraping.Mode = Mode.Value;
                var items = await Scraping.GetBooksInfo() as TitleInfo[];
                Titles.AddRangeOnScheduler(items);

                IsIdle.Value = true;
            }
        }
    }
}
