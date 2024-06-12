
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ConsoleApp9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IObservable<int> O=Observable.Return(0);
            using (var sub = OutputToConsole(O)) 
            Console.WriteLine("-----------");

            O = Observable.Empty<int>();
            using (var sub = OutputToConsole(O)) 
            Console.WriteLine("-----------");

            O = Observable.Throw<int>(new Exception());
            using (var sub = OutputToConsole(O)) 
            Console.WriteLine("-----------");

            O = Observable.Repeat(42);
            using (var sub = OutputToConsole(O.Take(5)))
                Console.WriteLine("-----------");

            O = Observable.Range(0,10);
            using (var sub = OutputToConsole(O))
                Console.WriteLine("-----------");

            O = Observable.Create<int>(ob =>
            {
                for (int i = 0;i<10; i++)
                    ob.OnNext(i);

                return Disposable.Empty;
            });
            using (var sub = OutputToConsole(O))
            Console.WriteLine("-----------");

            O = Observable.Generate(0,
                i=>i<5,
                i=>++i,
                i=>i+2);
            using (var sub = OutputToConsole(O))
            Console.WriteLine("-----------");

            IObservable<long> o1 =Observable.Interval(TimeSpan.FromSeconds(1));
            using(var sub = OutputToConsole(o1))
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("------------");

            o1=Observable.Timer(DateTime.Now.AddSeconds(2));
            using( var sub = OutputToConsole(o1))
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("-----------");
        }
        static IDisposable OutputToConsole<T>(IObservable<T> sequence)
        {
            return sequence.Subscribe(
                obj => Console.WriteLine($"{obj}"),
                ex => Console.WriteLine($"Error :{ex.Message}"),
                () => Console.WriteLine("Complete")
                );
        }
    }
}
