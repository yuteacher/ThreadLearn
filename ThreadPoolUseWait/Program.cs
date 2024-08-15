namespace ThreadPoolUseWait
{
    internal class Program
    {
        static void Main(string[] args)
        {
           RunOpetations(TimeSpan.FromSeconds(5));
           RunOpetations(TimeSpan.FromSeconds(7));
        }
        static void RunOpetations(TimeSpan workerOperationTimeout)
        {
            using(var evt  = new ManualResetEvent(false)) 
            using(var cts  = new CancellationTokenSource())
            {
                Console.WriteLine("Registering timeout operations..");
                var worker = ThreadPool.RegisterWaitForSingleObject(evt, 
                    (state, isTimeout) => WorkerOperationWait(cts, isTimeout),null, workerOperationTimeout, true);


                Console.WriteLine("Starting long running operation..");
                ThreadPool.QueueUserWorkItem(_ => WorkerOperation(cts.Token,evt));
                Thread.Sleep(workerOperationTimeout.Add(TimeSpan.FromSeconds(2)));
            }
        }
        static void WorkerOperation(CancellationToken token,ManualResetEvent evt)
        {
            for (int i = 0; i < 6; i++)
            {
                if (token.IsCancellationRequested) { return; }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            evt.Set();
        }
        static void WorkerOperationWait(CancellationTokenSource token,bool isTimeOut)
        {
            if (isTimeOut)
            {
                token.Cancel();
                Console.WriteLine("Worker operation timed out and was canceled");
            }else
            {
                Console.WriteLine("Worker operation succeded");
            }
        }
    }
}
