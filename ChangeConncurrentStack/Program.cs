using System.Collections.Concurrent;
using System.Diagnostics;

namespace ChangeConncurrentStack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task t = RunProgrom();
            t.Wait();
        }
        static async Task RunProgrom()
        {
            var taskStack = new ConcurrentStack<CustomTask>();
            var cts= new CancellationTokenSource();
            var taskSource = Task.Run(() => TaskProducer(taskStack));
            Task[] proccessors = new Task[4];
            for (int i = 1; i <=4; i++)
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
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(50);
                var workitem = new CustomTask { Id = i };
                stack.Push(workitem);
                Console.WriteLine("Task {0} has been posted", workitem.Id);
            }
        }
        static async Task TaskProcessor( ConcurrentStack<CustomTask> stack, string name , CancellationToken token)
        {
            await GetRandomDelay();
            do
            {
                CustomTask workItem;
                bool popSuccesful =stack.TryPop(out workItem);
                if (popSuccesful)
                {
                    Console.WriteLine("Task {0} has been processed by {1}", workItem.Id,name);
                }
                await GetRandomDelay();
            }
            while(!token.IsCancellationRequested);
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
