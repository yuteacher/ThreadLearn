namespace ConsoleApp21
{
    internal class Program
    {
        static void Main(string[] args)
        {

            const string MuteName = "CSharpThreadingCookbook";
            using (var mute = new Mutex(false, MuteName))
            {
                if (!mute.WaitOne(TimeSpan.FromSeconds(5), false))
                {
                    Console.WriteLine("Already running");

                }
                else
                {
                    Console.WriteLine("Running");
                    Console.ReadLine();
                    mute.ReleaseMutex();
                }
            }
        }
    }
}
