using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrapingTestCommon;
using System.Diagnostics;

namespace ScrapingTestWebServer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync([FromQuery] FreeBookSearchMode mode = FreeBookSearchMode.Media, [FromQuery] int page = 1)
    {
        var scraping = new FreeBookScraping();
        scraping.Mode = mode;
        ViewData["page"] = scraping.Page = page;
        ViewData["mode"] = scraping.Mode = mode;
        ViewData["items"] = await scraping.GetBooksInfo();
    }
}
