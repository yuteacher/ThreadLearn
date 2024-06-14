using System.Net;
using System.Runtime.CompilerServices;

namespace ConsoleApp13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new AsyncHttpServer(portNumber: 1234);
            _=Task.Run(() => server.Start());
            Console.WriteLine("Listening on port 1234.Open http://localhost:1234 in your browser");
            Console.WriteLine();
            GetResponseAsync("http://localhost:1234").GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press Enter to stop the server.");
            Console.ReadLine();
            server.Stop().GetAwaiter().GetResult();
        }
        static async Task GetResponseAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                string responseHeaders =  response.Headers.ToString();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response headers:{0}.  body :{1}", responseHeaders, responseBody);
            }
        }
    }
    class AsyncHttpServer 
    {
        readonly HttpListener _listener;
        const string RESOPOSE_TEMPLATE = 
            "<html><head><title>Test</title></head><body><h2>Test Page</h2><h4>Today is :{0}</h4></body></html>";

        public AsyncHttpServer( int portNumber)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format("http://+:{0}/",portNumber));
        }
        public async Task Start()
        {
            try
            {
                _listener.Start();
                while (true)
                {
                    var ctx = await _listener.GetContextAsync();
                    Console.WriteLine("Client connected..");
                    string response = string.Format(RESOPOSE_TEMPLATE, DateTime.Now);
                    using (var reader = new StreamWriter(ctx.Response.OutputStream))
                    {
                        await reader.WriteAsync(response);
                        await reader.FlushAsync();
                    }
                }
            }
            catch
            (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
        public async Task Stop()
        {
            _listener.Abort();
        }

    }

}
