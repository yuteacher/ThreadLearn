namespace TreadPoolUseAsync
{
    internal class Program
    {
        private static void AsyncOperation(object state)
        {
            Console.WriteLine($" Opetation state: {state ?? "null"}  Worker threadid:{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
        }
        static void Main(string[] args)
        {
            const int x =1; const int y = 2;
            const string lambdaState = "lambdate state 2";

            ThreadPool.QueueUserWorkItem(AsyncOperation);
            Thread.Sleep(1000);
            ThreadPool.QueueUserWorkItem(AsyncOperation,"async state");
            Thread.Sleep(1000);
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($" Opetation state: {state ?? "null"}  Worker threadid:{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
            }, "lambda state");
            Thread.Sleep(1000);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Console.WriteLine($" Opetation state: {x+y},{lambdaState},  Worker threadid:{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
            },"lambda state");
            Thread.Sleep(1000);
            Console.WriteLine("Hello, World!");
        }
    }
}
