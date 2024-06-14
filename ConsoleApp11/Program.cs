using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Timers;

namespace ConsoleApp11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IObservable<string> observable = LongRunningOperationAsync("Task");
            using (var sub = OutputToConsole(observable))
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            Console.WriteLine("------------");

            Task<string> t = LongRunningOperationTaskAsync("Task");
            using (var sub = OutputToConsole(t.ToObservable()))
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            Console.WriteLine("----------");

            AsyncDelegate asyncMethod = LongRunningOperation;

            Func<string, IObservable<string>> observableFactory = Observable.FromAsyncPattern<string, string>(asyncMethod.BeginInvoke, asyncMethod.EndInvoke);

            observable = observableFactory("Task3");
            using (var sub = OutputToConsole(observable))
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            Console.WriteLine("------------");

            using (var timer =new System.Timers.Timer(1000))
            {
                var ot = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
                    h => timer.Elapsed += h,
                    h => timer.Elapsed -= h);
                timer.Start();

                using (var sub=OutputToConsole(ot))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                Console.WriteLine("-------");
                timer.Stop();    
            }
        }
        static async Task<T> AwaitOnObservable<T>(IObservable<T> observable)
        {
            T obj = await observable;
            Console.WriteLine($"{obj}");
            return obj;
        }
        static Task<string> LongRunningOperationTaskAsync(string name)
        {
            return Task.Run(() => LongRunningOperation(name));
        }
        static IObservable<string> LongRunningOperationAsync(string name)
        {
            return Observable.Start(() => LongRunningOperation(name));
        }
        static string LongRunningOperation(string name)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return string.Format("Task {0} is completed .Thread Id {1} ", name, Thread.CurrentThread.ManagedThreadId);
        }
        static IDisposable OutputToConsole(IObservable<EventPattern<ElapsedEventArgs>> events)
        {
            return events.Subscribe(

                obj => Console.WriteLine($"{obj.EventArgs.SignalTime}"),
                ex => Console.WriteLine($"Error: {ex.Message}"),
                () => Console.WriteLine("Completed")
            );
        }
        static IDisposable OutputToConsole<T>(IObservable<T> events)
        {
            return events.Subscribe(
                 obj => Console.WriteLine($"{obj}"),
                ex => Console.WriteLine($"Error: {ex.Message}"),
                () => Console.WriteLine("Completed")
                );
        }
    }
}
