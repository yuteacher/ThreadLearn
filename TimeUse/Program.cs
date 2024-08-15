namespace TimeUse
{
    internal class Program
    {
        static Timer _timer;
        static void TimerOpeeration(DateTime start) 
        {
            TimeSpan elapse = DateTime.Now - start;
            Console.WriteLine("{0} seconds from {1} .Time thread pool thread id :{2}",elapse.Seconds,start,Thread.CurrentThread.ManagedThreadId);

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Prese 'Enter' to stop the timer..");
            DateTime start = DateTime.Now;
            _timer = new Timer(_=>TimerOpeeration(start),null,TimeSpan.FromSeconds(1),TimeSpan.FromSeconds(2));
            Thread.Sleep(TimeSpan.FromSeconds(6));
            _timer.Change(TimeSpan.FromSeconds(1),TimeSpan.FromSeconds(4));
            Console.ReadLine();
            _timer.Dispose();
        }
    }
}
