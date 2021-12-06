using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScrapingTestCommon;

public enum FreeBookSearchMode
{
    Media,
    Boy,
    Girl,
}

public class FreeBookScraping
{
    public Int32 Page { get; set; } = 1;
    public FreeBookSearchMode Mode { get; set; } = FreeBookSearchMode.Media;

    private CookieContainer Cookies { get; }
    private HttpClientHandler Handler { get; }
    private HttpClient Client { get; }

    public FreeBookScraping()
    {
        Cookies = new CookieContainer();
        Handler = new HttpClientHandler()
        {
            CookieContainer = Cookies,
        };
        Client = new HttpClient(Handler);
    }

    public FreeBookScraping(int page, FreeBookSearchMode mode) : this()
    {
        Page = page;
        Mode = mode;
    }

    public async Task<IEnumerable<TitleInfo>> GetBooksInfo()
    {
        if (Page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(Page));
        }

        var modeStr = Mode.ToString().ToLower();
        var url = $"https://www.cmoa.jp/freecontents/title/{modeStr}/?page={Page}";

        IConfiguration conf = Configuration.Default
            .With(Client)
            .WithCulture("ja-JP")
            .WithDefaultLoader();
        var context = BrowsingContext.New(conf);
        try
        {
            var doc = await context.OpenAsync(url);
            Trace.WriteLine(doc.Body?.InnerHtml);
            return doc
                .QuerySelectorAll("#freeTitle li")
                .Select(i => new TitleInfo
                {
                    Title = i.QuerySelector<IHtmlImageElement>("img")?.AlternativeText ?? String.Empty,
                    Author = i.QuerySelector<IHtmlAnchorElement>("div.author_name a")?.TextContent ?? String.Empty,
                    URL = i.QuerySelector<IHtmlAnchorElement>("div.title_name a")?.Href ?? String.Empty,
                    Thumbnail = i.QuerySelector<IHtmlImageElement>("img")?.Source ?? String.Empty,
                    Description = i.QuerySelector(".btn_box")?.TextContent ?? String.Empty,
                })
                .ToArray();
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
        }

        return new TitleInfo[0];
    }
}
