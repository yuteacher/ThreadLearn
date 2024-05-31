using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APMchanggeTask
{
    public class ClassAdd
    {


        public static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Starting a callback...");
            Console.WriteLine("State passed to a callback:(0}", ar.AsyncState);
            Console.WriteLine("Is thread pool thread:(0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Thread pool worker thread id:(0}", Thread.CurrentThread.ManagedThreadId);

        }

        public static string Test(string threadName)
        {
            Console.WriteLine("starting...");
            Console.WriteLine("Is thread pool thread: (0}",Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Thread.CurrentThread.Name = threadName;
            return string.Format("Thread name:(0)",Thread.CurrentThread.Name);

        }
        public static string Test(out int threadId)
        {
            Console.WriteLine("starting...");
            Console.WriteLine("Is thread pool thread: (0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            threadId = Thread.CurrentThread.ManagedThreadId;
            return string.Format("Thread pool worker thread id was :{0}",threadId);
        }
    
    }
}
    

