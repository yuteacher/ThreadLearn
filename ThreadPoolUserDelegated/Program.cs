namespace ThreadPoolUserDelegated
{
    internal class Program
    {
        private delegate string RunOnThreadPool(out int threadid);
        static void Main(string[] args)
        {
           int threadid = 0;

            RunOnThreadPool runOnThreadPool = Test;

            var T = new Thread(() => Test(out threadid));
            T.Start();
            T.Join();
            Console.WriteLine($"ThreadId is {threadid}");

            IAsyncResult r = runOnThreadPool.BeginInvoke(out threadid, Callback, "a delegate asyncchronnous call");
            string result = runOnThreadPool.EndInvoke(out threadid, r);
            Console.WriteLine($" Thread pool worker thread id :{threadid}");
            Console.WriteLine(result);
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
        private static void Callback(IAsyncResult ar) 
        {
            Console.WriteLine("Starting a callback...");
            Console.WriteLine("State passed to a callback:{0}", ar.AsyncState);
            Console.WriteLine("Is thread pool thread:{0}，Thread pool worker thread id :{1}", Thread.CurrentThread.IsThreadPoolThread,Thread.CurrentThread.ManagedThreadId);
        }
        private static string Test (out int threadId)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine("Is thread pool thread:{0}", Thread.CurrentThread.IsThreadPoolThread);
            threadId =Thread.CurrentThread.ManagedThreadId;
            return string.Format("Thread pool worker thread id :{0}", threadId);
        }

    }
}
