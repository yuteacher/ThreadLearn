﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APMchangeTASK
{
    internal class Program
    {
        delegate string AsynchronousTask(string threadName);
        delegate string IncompatibleAsynchronousTask(out int threadId);
        static void Main(string[] args)
        {
            int threadld;
            AsynchronousTask d = Test;
            IncompatibleAsynchronousTask e = Test;

            Console.WriteLine("Option 1");
            Task<string> task = Task<string>.Factory.FromAsync(
            d.BeginInvoke("AsyncTaskThread", Callback, "a delegateasynchronous call"), d.EndInvoke);
            task.ContinueWith(t => Console.WriteLine("Callback isfinished,now running a continuation!Result:{0}", t.Result));

            while (!task.IsCompleted)
            {

                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.WriteLine("0ption 2");

            task = Task<string>.Factory.FromAsync(d.BeginInvoke, d.EndInvoke, "AsyncTaskThread", "adelegate asynchronous call");
            task.ContinueWith(t => Console.WriteLine("Task is completednow running a continuation!Result: {0}", t.Result));
            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.WriteLine("0ption 3");

            IAsyncResult ar = e.BeginInvoke(out threadld, Callback, "adelegate asynchronous call");
            ar = e.BeginInvoke(out threadld, Callback, "a delegateasynchronouscall");
            task = Task<string>.Factory.FromAsync(ar, _ => e.EndInvoke(out threadld, ar));
            task.ContinueWith(t => Console.WriteLine("Task is completed,now running a continuation! Resuult:{0},ThreadId :{1}", t.Result, threadld));

            while (!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        public static void Callback(IAsyncResult ar)
        {
            Console.WriteLine("Starting a callback...");
            Console.WriteLine("State passed to a callback:{0}", ar.AsyncState);
            Console.WriteLine("Is thread pool thread:{0}", Thread.CurrentThread.IsThreadPoolThread);
            Console.WriteLine("Thread pool worker thread id:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        public static string Test(string threadName)
        {
            Console.WriteLine("starting...");
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Thread.CurrentThread.Name = threadName;
            return string.Format("Thread name:{0}", Thread.CurrentThread.Name);
        }
        public static string Test(out int threadId)
        {
            Console.WriteLine("starting...");
            Console.WriteLine("Is thread pool thread: {0}", Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            threadId = Thread.CurrentThread.ManagedThreadId;
            return string.Format("Thread pool worker thread id was :{0}", threadId);
        }

    }
}
