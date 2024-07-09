namespace SpinWaitUse
{
    internal class Program
    {
        static volatile bool running = false;
        static void UserModeWait()
        {
            while (!running)
            {
                Console.WriteLine(".");
            }
            Console.WriteLine();
            Console.WriteLine("Waiting is complere");
        }
        static void HybridSpinWait()
        {
            var w = new SpinWait();
            while (!running)
            {
                w.SpinOnce();
                Console.WriteLine(w.NextSpinWillYield);
            }
            Console.WriteLine("Waiting is  complete");
        }
        static void Main(string[] args)
        {
            var t1 = new Thread(UserModeWait);
            var t2 = new Thread(HybridSpinWait);

            Console.WriteLine("Runing user mode waiting");
            t1.Start();
            Thread.Sleep(20);
            running = true;
            Thread.Sleep(TimeSpan.FromSeconds(1));
            running = false;
            Console.WriteLine("Running hybrid SpinWait construct waiting");
            t2.Start();
            Thread.Sleep(5);
            running = false;
        }
    }
}
