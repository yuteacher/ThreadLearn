using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach(int i in EnumerableEventSequence())
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
            Console.WriteLine("IEnumerable");

            IObservable<int> o=
                EnumerableEventSequence().ToObservable();

            using (IDisposable subscription = o.Subscribe(Console.WriteLine))
            {
                Console.WriteLine();
                Console.WriteLine("IObservable");
            }
            o = EnumerableEventSequence().ToObservable()
                .SubscribeOn(TaskPoolScheduler.Default);
            using ( IDisposable subscription = o.Subscribe(Console.WriteLine))
            {
                Console.WriteLine();
                Console.WriteLine("IObservable async");
                Console.ReadLine();
            }
        }
        static IEnumerable<int> EnumerableEventSequence()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                yield return i;
            }
        }
    }
}
