using Reactive.Bindings;
using ScrapingTestCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScrapingTestWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is TitleInfo titleInfo)
            {
                var url = titleInfo.URL.Replace("&", "^&");
                System.Diagnostics.Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
        }
    }

    public class MainWindowViewModel
    {
        public ReactiveProperty<ComboBoxItem> SelectedMode { get; set; } = new ReactiveProperty<ComboBoxItem>();
        public ReactiveProperty<bool> IsIdle { get; set; } = new ReactiveProperty<bool>(true);
        public ReactiveCommand NextPageCommand { get; set; }
        public ReactiveProperty<int> Page { get; set; } = new ReactiveProperty<int>(1);
        public ReactiveCollection<TitleInfo> Titles { get; set; } = new ReactiveCollection<TitleInfo>();

        public MainWindowViewModel()
        {
            SelectedMode.Subscribe(async v =>
            {
                if (IsIdle.Value)
                {
                    if (Page.Value == 1)
                    {
                        await UpdateList();
                    }
                    else
                    {
                        Page.Value = 1;
                    }
                }
            });

            NextPageCommand = IsIdle.ToReactiveCommand();
            NextPageCommand.Subscribe(() =>
            {
                if (IsIdle.Value)
                {
                    Page.Value++;
                }
            });

            Page.Subscribe(async p =>
            {
                if (IsIdle.Value)
                {
                    IsIdle.Value = false;

                    await UpdateList();

                    IsIdle.Value = true;
                }
            });
        }

        private async Task UpdateList()
        {
            IsIdle.Value = false;
            Titles.ClearOnScheduler();

            var mode = (SelectedMode.Value as ComboBoxItem)?.Tag switch
            {
                "Media" => FreeBookSearchMode.Media,
                "Boy" => FreeBookSearchMode.Boy,
                "Girl" => FreeBookSearchMode.Girl,
                _ => FreeBookSearchMode.Media,
            };
            var scraping = new FreeBookScraping(Page.Value, mode);
            var items = await scraping.GetBooksInfo();
            Titles.AddRangeOnScheduler(items);

            IsIdle.Value = true;
        }
    }

    public class ModeItem
    {
        public String Label { get; set; } = String.Empty;
        public String Mode { get; set; } = String.Empty;
    }
}
