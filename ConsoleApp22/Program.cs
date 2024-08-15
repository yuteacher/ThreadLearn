namespace ConsoleApp22
{
    internal class Program
    {
        static SemaphoreSlim _semaphore = new SemaphoreSlim(4);
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                string name = $"Thread {i}";
                int secends = 2+2*i;
                Thread thread = new Thread(() => AccessDatebase(name,secends));
                thread.Start();
            }
        }
        static void AccessDatebase(string name,int secends)
        {
            Console.WriteLine($"{name} is accessing the database");
            _semaphore.Wait();
            Console.WriteLine("{0} was granted an access to a database",name);
            Thread.Sleep(TimeSpan.FromSeconds(secends));
            Console.WriteLine("{0} is completed", name);
            _semaphore.Release();
        }
    }
}
