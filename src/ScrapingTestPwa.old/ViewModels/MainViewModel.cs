using Reactive.Bindings;
using ScrapingTestCommon;

namespace ScrapingTestPwa.ViewModels
{
    public class MainViewModel
    {
        public ReactiveCollection<TitleInfo> Titles { get; set; } = new ReactiveCollection<TitleInfo>();

        public int Page { get; set; } = 1;
        public FreeBookSearchMode Mode { get; set; } = FreeBookSearchMode.Media;

        private FreeBookScraping _scraping;

        public MainViewModel(FreeBookScraping scraping)
        {
            _scraping = scraping;
        }

        public async Task UpdateData()
        {
            Titles.Clear();
            _scraping.Page= Page;
            _scraping.Mode= Mode;
            Titles.AddRangeOnScheduler(await _scraping.GetBooksInfo());
        }

        public async Task UpdateData(int page, FreeBookSearchMode mode)
        {
            Titles.Clear();
            _scraping.Page = page;
            _scraping.Mode = mode;
            Titles.AddRangeOnScheduler(await _scraping.GetBooksInfo());
        }
    }
}
