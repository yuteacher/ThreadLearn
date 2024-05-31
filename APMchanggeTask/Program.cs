

using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using APMchanggeTask;

    int threadld;
    AsynchronousTask d=ClassAdd.Test;
    IncompatibleAsynchronousTask e =ClassAdd.Test;

    Console.WriteLine("Option 1");
    Task<string> task = Task<string>.Factory.FromAsync(
    d.BeginInvoke("AsyncTaskThread", ClassAdd.Callback, "a delegateasynchronous call"),d.EndInvoke);
    task.ContinueWith(t => Console.WriteLine("Callback isfinished,now running a continuation!Result:(0)",t.Result));

    while (!task.IsCompleted) 
    {

        Console.WriteLine(task.Status);
        Thread.Sleep(TimeSpan.FromSeconds(0.5));
    }
    Console.WriteLine(task.Status);
    Thread.Sleep(TimeSpan.FromSeconds(1));
    Console.WriteLine ("---------------------------------------");
    Console.WriteLine();
    Console.WriteLine("0ption 2");

    task = Task<string>.Factory.FromAsync(d.BeginInvoke, d.EndInvoke, "AsyncTaskThread", "adelegate asynchronous call");
    task.ContinueWith(t => Console.WriteLine("Task is completednow running a continuation!Result: 0)",t.Result));
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

    IAsyncResult ar=e.BeginInvoke(out threadld, ClassAdd.Callback, "adelegate asynchronous call");
    ar = e.BeginInvoke(out threadld, ClassAdd.Callback, "a delegateasynchronouscall");
    task = Task<string>.Factory.FromAsync(ar, _ => e.EndInvoke(out threadld, ar));
    task.ContinueWith(t =>Console.WriteLine("Task is completed,now running a continuation! Resuult:{0},ThreadId :{1}",t.Result,threadld));

    while (!task.IsCompleted)
    {
        Console.WriteLine(task.Status);
        Thread.Sleep (TimeSpan.FromSeconds(0.5));
    }
    Console.WriteLine(task.Status);
    Thread.Sleep (TimeSpan.FromSeconds(1));

    delegate string AsynchronousTask(string threadName);
    delegate string IncompatibleAsynchronousTask(out int threadId);
