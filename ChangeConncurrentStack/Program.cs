using System.Collections.Concurrent;
using System.Diagnostics;

namespace ChangeConncurrentStack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        static async Task RunProgrom()
        {
            var taskStack = new ConcurrentStack<CustomTask>();
            var cts= new CancellationTokenSource();
            var taskSource = Task.Run(() => TaskProducer(taskStack));
            Task[] proccessors = new Task[4];
            for (int i = 0; i < proccessors.Length; i++)
            {
                string processorid = i.ToString();
                proccessors[i - 1] = Task.Run(() => TaskProcessor(taskStack, "Processor" + processorid, cts.Token));
            }
            await taskSource;
            cts.CancelAfter(TimeSpan.FromSeconds(2));
             await Task.WhenAll(proccessors);

        }
        static async Task TaskProducer( ConcurrentStack<CustomTask> stack)
        {

        }
        static async Task TaskProcessor( ConcurrentStack<CustomTask> stack, string name , CancellationToken token)
        {

        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1,500);
            return Task.Delay(delay);
        }
        class CustomTask
        {
            public int Id { get; set; } 
        }
    }
}
