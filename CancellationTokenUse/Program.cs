namespace CancellationTokenUse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(var ctm = new CancellationTokenSource())
            {
                CancellationToken ct = ctm.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperationg1(ct));
                Thread.Sleep(1000);
                ctm.Cancel();
            }
            using(var ctm = new CancellationTokenSource())
            {
                CancellationToken cancellationToken = ctm.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperationg2(cancellationToken));
                Thread.Sleep(1000);
                ctm.Cancel();
            }
            using (var ctm = new CancellationTokenSource())
            {
                CancellationToken cancellationToken = ctm.Token;
                ThreadPool.QueueUserWorkItem(_ => AsyncOperation3(cancellationToken));
                Thread.Sleep(1000);
                ctm.Cancel();
            }
            Thread.Sleep(1000);
        }
        static void AsyncOperationg1(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting the first task");
            for (int i = 0; i < 10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("The first task has been canceled.");
                    return;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("The first task has completed succesfully");
        }
        static void AsyncOperationg2(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Starting the second task");
                for (int i = 0; i < 5; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("The second task has completed succesfully");
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        private static void AsyncOperation3(CancellationToken cancellationToken)
        {
            bool cencellationFlag = false;
            cancellationToken.Register(()=>cencellationFlag=true);
            Console.WriteLine("Starting the third task");
            for(int i = 0;i<5;i++)
            {
                if (cencellationFlag)
                {
                    Console.WriteLine("The third task has been canceled");
                    return ;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("The third task has been succesfully");
        }
    }
}
