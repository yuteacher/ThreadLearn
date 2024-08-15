namespace ConsoleApp18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task t = AsyncTask();
            t.Wait();

            AsyncViod();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            t = AsyncTaskWithErrors();
            while (!t.IsFaulted)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Console.WriteLine(t.Exception.InnerException.Message);

            try
            {
                AsyncVoidWithErrors();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            int[] nunmbers = new int[] { 1, 2, 3, 4, 5 };
            Array.ForEach(nunmbers, async number =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                if (number == 3)
                    throw new Exception("Boom from async forEach!");
                Console.WriteLine(number);
            });
            Console.ReadLine();
        }
        async static Task AsyncTaskWithErrors()
        {
            string result = await GetInfoAsync("AsyncTaskException", 2);
            Console.WriteLine(result);
        }
        async static Task AsyncVoidWithErrors()
        {
            string result = await GetInfoAsync("AsyncVoidException", 2);
            Console.WriteLine(result);
        }
        private static async Task AsyncTask()
        {
            string result = await GetInfoAsync("AsyncTask",2);
            Console.WriteLine(result);
        }
        private static async void AsyncViod()
        {
            string result = await GetInfoAsync("AsyncVoid",2);
            Console.WriteLine(result);
        }
        async static Task<string> GetInfoAsync( string name,int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            if(name.Contains("Exception"))
                throw new Exception(string.Format("Boom from {0}!",name));
            return string.Format("Task {0} is running on a thread id {1}.Is thread pool thread:{2}", name,Thread.CurrentThread.ManagedThreadId,Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
