using System.Collections.Concurrent;

namespace ConcurrentBagUse
{
    internal class Program
    {
        static Dictionary<string, string[]> _contentEmulation =new Dictionary<string, string[]>();
        static void Main(string[] args)
        {
            CreateLinks();
            Task task = RunProgram();
            task.Wait();
        }
        static async Task RunProgram()
        {
            var bag = new ConcurrentBag<CrawlingTask>();
            string[] urls = { "http://microsoft.com", "http://baidu.com" };
            var crawlers = new Task[2];
            for (int i = 1; i <= urls.Length; i++)
            {
                string crawlerName ="Crawler"+i.ToString();
                bag.Add(new CrawlingTask { UrlToCrawl = urls[i - 1], ProducerName = "root" });
                crawlers[i-1] =Task.Run(()=>Crawl(bag, crawlerName));
            }
            await Task.WhenAll(crawlers);
        }
        static async Task Crawl(ConcurrentBag<CrawlingTask> bag, string crawlerName)
        {
            CrawlingTask task;
            while(bag.TryTake(out task))
            {
                IEnumerable<string> urls =await GetLinksFromContent(task);
                if (urls is not null)
                {
                    foreach (string url in urls)
                    {
                        var t = new CrawlingTask
                        {
                            UrlToCrawl = url,
                            ProducerName = crawlerName
                        };
                        bag.Add(t);
                    }
                }
                Console.WriteLine("Indexing url {0} postred by {1} is completed by {2}", task.UrlToCrawl, task.ProducerName, crawlerName);
            }
           
        }
        static async Task<IEnumerable<string>> GetLinksFromContent(CrawlingTask task)
        {
            await GetRandomDelay();
            if(_contentEmulation.ContainsKey(task.UrlToCrawl))
                return _contentEmulation[task.UrlToCrawl];
            return null;
        }
        static void CreateLinks()
        {
            _contentEmulation["http://microsoft.com/"] = new[]
            {
                "http://microsoft.com/a.html",
                "http://microsoft.com/b.html",

            };
            _contentEmulation["http://microsoft.com/a.html"] = new [] 
            {
                "http://microsoft.com/c.html",
                "http://microsoft.com/d.html",
            };
            _contentEmulation["http://microsoft.com/b.html"] = new[]
           {
                "http://microsoft.com/e.html"
            };
            _contentEmulation["http://baidu.com/"] = new[]
            {
                "http://baidu.com/a.html",
                "http://baidu.com/b.html",
            };
            _contentEmulation["http://baidu.com/a.html"] = new[]
           {
                "http://baidu.com/c.html",
                "http://baidu.com/d.html",
            };

        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(150, 200);
            return Task.FromResult(delay);
        }
        class CrawlingTask
        {
            public string UrlToCrawl { get; set; }
            public string ProducerName { get; set; }
        }
    }
}
