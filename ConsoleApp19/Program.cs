using System.Runtime.CompilerServices;

namespace ConsoleApp19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        async static Task AsyncchroousProcessing()
        {
            var sync = new CustomAwaitable(true);
            string result = await sync;
            Console.WriteLine(result);

            var async = new CustomAwaitable(false);
            result = await async;
            Console.WriteLine(result);
        }

    }
    class CustomAwaitable 
    { 

        private readonly bool _completedSynchronously;
        public CustomAwaitable(bool completedSynchronously)
        {
            _completedSynchronously = completedSynchronously;
        }
        public CustomAwaiter GetAwaiter()
        {
            return new CustomAwaiter(_completedSynchronously);
        }
    }
    class CustomAwaiter : INotifyCompletion
    {
        private string _result = "Completed synchronously";
        public readonly bool _completedSynchronously;
        public bool IsCompleted { get { return _completedSynchronously; } }
        public CustomAwaiter(bool completedSynchronously)
        {
            _completedSynchronously = completedSynchronously;
        }
        public string GetResult()
        {
            return _result;
        }
        public void OnCompleted(Action continuation ) 
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                _result = GetInfo();
                if (continuation != null) continuation();
            });
        }
        private string GetInfo()
        {
            return string.Format("Task is running on a thread id {0}.Is thread pool thread:{1}",Thread.CurrentThread.ManagedThreadId,Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
