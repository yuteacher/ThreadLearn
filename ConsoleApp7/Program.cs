using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ConsoleApp7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var observe = new CustomObserve();

            var goodObeservable = new CustomSequence(new[] { 1, 2, 3, 4 ,5});
            var badObeservable = new CustomSequence(null );

            using (IDisposable subscription = goodObeservable.Subscribe(observe))
            {

            }
            using (IDisposable subscription = goodObeservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observe))
            {
                Thread.Sleep(1000);
            }
            using (IDisposable subscription = badObeservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observe))
            {
               Console.ReadLine(); 
            }

        }
        public class CustomObserve : IObserver<int> 
        { 
            public void OnNext(int value)
            {
                Console.WriteLine("NEXT value :{0} ;Thread ID: {1} ",value,Thread.CurrentThread.ManagedThreadId);
            }
            public void OnError(Exception error)
            {
                Console.WriteLine("Error :{0}", error.Message);
            }

            public void OnCompleted()
            {
                Console.WriteLine("Completed");
            }
        }
        public class CustomSequence : IObservable<int> 
        {
            private readonly IEnumerable<int> _numbers;

            public CustomSequence( IEnumerable<int> numbers )
            {
                _numbers = numbers;
            }
            public IDisposable Subscribe( IObserver<int> observer  ) 
            {
                foreach( int i in _numbers )
                {
                    observer.OnNext(i);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            }

        }


    }
}
