// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using ScrapingTestCommon;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

var name = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
var root = new RootCommand(name);
root.AddOption(new Option(new string[] { "-p", "--page" }, "page index (default: 1)", typeof(Int32)));
root.AddOption(new Option(new string[] { "-m", "--mode" }, "search mode (default: Media)", typeof(FreeBookSearchMode)));
root.Handler = CommandHandler.Create<FreeBookScraping>(async s =>
{
    var items = await s.GetBooksInfo();
    foreach (var i in items)
    {
        Console.WriteLine("-----");
        Console.WriteLine($"  title: {i.Title}");
        Console.WriteLine($"  author: {i.Author}");
        Console.WriteLine($"  description: {i.Description}");
        Console.WriteLine($"   url: {i.URL}");
        Console.WriteLine($"   thumb: {i.Thumbnail}");
    }
});
root.Invoke(args);
