﻿using System.ComponentModel;

namespace BackgroundWorker组件
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.DoWork += Worker_DoWork;
            bw.ProgressChanged += Worker_ProgressChanged;
            bw.RunWorkerCompleted += Worker_Completed;
            bw.RunWorkerAsync();
            Console.WriteLine("Press C to cancel work");
            do
            {
                if (Console.ReadKey(true).KeyChar == 'C') bw.CancelAsync();
            }
            while (bw.IsBusy);
        }
        static void Worker_DoWork(object sender,DoWorkEventArgs e)
        {
            Console.WriteLine("DoWork thread pool thread id {0}",Thread.CurrentThread.ManagedThreadId);
            var bw = (BackgroundWorker)sender;
            for (int i = 0;i<=100;i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                if(i%10==0)
                {
                    bw.ReportProgress(i);
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            }
            e.Result = 42;
        }
        static void Worker_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}% completad .Progress thread pool thread id {1}",e.ProgressPercentage,Thread.CurrentThread.ManagedThreadId);
        }
        static void Worker_Completed(object sender,RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Compeled  thread pool thread id {0}", Thread.CurrentThread.ManagedThreadId);
            if(e.Error != null)
            {
                Console.WriteLine($"Error: {e.Error.Message} has occured");
            }
            else if(e.Cancelled)
            {
                Console.WriteLine("Operation has been cancled");
            }
            else
            {
                Console.WriteLine("The answer is:{0}" ,e.Result);
            }
        }
    }
}
