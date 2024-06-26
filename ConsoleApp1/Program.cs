﻿using System.Xml.Linq;

namespace UseAwait
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronywithTPL();
            t.Wait();
            t= AsynchronywithAwait();
            t.Wait();
        }
        static Task AsynchronywithTPL() 
        {
            Task<string> t = GetInfoAsync("Task 1");
            Task t2 = t.ContinueWith(task => Console.WriteLine(t.Result), TaskContinuationOptions.NotOnFaulted);
            Task t3 = t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);
            return Task.WhenAny(t2,t3);
        }

        async static Task AsynchronywithAwait() 
        {
            try
            {
                string result = await GetInfoAsync("Task 2");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        async static Task<string> GetInfoAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            //throw new Exception("Boom!");
            return string.Format("Task {0}is running on a thread id{1}.Is thread pool thread:{2}",name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
           
    }
}
