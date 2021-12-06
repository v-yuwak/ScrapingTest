using Microsoft.AspNetCore.Mvc;
using ScrapingTestCommon;

namespace ScrapingTestPwa.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class FreeBookController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TitleInfo>> Get(int page = 1, FreeBookSearchMode mode = FreeBookSearchMode.Media)
    {
        var scraping = new FreeBookScraping()
        {
            Page = page,
            Mode = mode,
        };
        return await scraping.GetBooksInfo();
    }
}
