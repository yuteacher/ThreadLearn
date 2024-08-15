using System.Diagnostics;

namespace ThreadPoolParallelism
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int number = 500;
            var sw =new Stopwatch();
            sw.Start();
            UseThreads(number);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());

            sw.Restart();
            UseThreadPool(number);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.ToString());
        }
        static void UseThreads(int number)
        {
            using( var countdown = new CountdownEvent(number))
            {
                Console.WriteLine("Scheduling work by creating threads");
                for (int i = 0; i < number; i++)
                {
                    var thread = new Thread(() =>
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(100);
                        countdown.Signal();
                    });
                    thread.Start();
                }
                countdown.Wait();
                Console.WriteLine();
            }
        }
        static void UseThreadPool(int number)
        {
            using(var countdown = new CountdownEvent(number))
            { Console.WriteLine("Starting work on a threadpool");
                for(int i = 0;i < number; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(100);
                        countdown.Signal();
                    });
                }
                countdown.Wait();
                Console.WriteLine();
            }
        }
    }
}
