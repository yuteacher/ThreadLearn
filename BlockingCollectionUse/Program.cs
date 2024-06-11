using System.Collections.Concurrent;

namespace BlockingCollectionUse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Using a Queue inside of BlockingColletion");
            Console.WriteLine();
            Task t = RunProgram();
            t.Wait();

            Console.WriteLine();
            Console.WriteLine("Using a stack inside of BlocingCollection");
            Console.WriteLine();
            t = RunProgram(new ConcurrentStack<CustomTask>());
            t.Wait();
        }

        static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
        {
            var taskCollection = new BlockingCollection<CustomTask>();
            if (collection != null)
            {
                taskCollection = new BlockingCollection<CustomTask>(collection);

            }
            var taskSource = Task.Run(() => TaskProducer(taskCollection));
            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorId = "Processor " + i;
                processors[i - 1] = Task.Run(() => TaskProcessor(taskCollection, processorId));
            }
            await taskSource;
            await Task.WhenAll(processors);
        }
        static async Task TaskProducer(BlockingCollection<CustomTask> collection)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(20);
                var workItem = new CustomTask { Id = i };
                collection.Add(workItem);
                Console.WriteLine(" Task {0} have been posted", workItem.Id);
            }
            collection.CompleteAdding();

        }
        static async Task TaskProcessor(BlockingCollection<CustomTask> collection, string name)
        {
            await GetRandomDelay();
            foreach (var item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine(" Task {0} has been processed by {1} ", item.Id, name);
                await GetRandomDelay();
            }
        }
        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);

        }
        class CustomTask
        {
            public int Id { get; set; }
        }
    }
}
