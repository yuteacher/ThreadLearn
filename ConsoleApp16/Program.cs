using System.Runtime.CompilerServices;

namespace ConsoleApp16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var t=ProcessAsynchronously();
            t.GetAwaiter().GetResult();

            Console.WriteLine("Press ENTER to  exit");
            Console.ReadLine();
        }
        static async Task ProcessAsynchronously()
        {
            var unsafeState = new UnsafeState();
            Task[] tasks = new Task[4];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Worker(unsafeState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-------------");

            var firstState = new DoubleCheckedLocking();
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Worker(firstState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-------------");

            var secondState = new BCLDoubleChecked();
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Worker(secondState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-------------");

            var thirdState = new Lazy<ValueToAccess>(Compute);
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Worker(thirdState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-------------");

            var fourthState = new BCLThreadSafeFactory();
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Worker(fourthState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-------------");

        }
        static void Worker(IHasValue state)
        {
            Console.WriteLine("Worker runs on thread id {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("State value :{0}", state.Value.Text);
        }
        static void Worker(Lazy<ValueToAccess> state)
        {
            Console.WriteLine("Worker runs on thread id {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("State value :{0}", state.Value.Text);
        }
        static ValueToAccess Compute()
        {
            Console.WriteLine("The value is being constructed on a thread id {0}",Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            return new ValueToAccess(string.Format("Constructed on thread id {0}", Thread.CurrentThread.ManagedThreadId));

        }
        class ValueToAccess
        {
            private readonly string _text;
            public ValueToAccess(string text)
            {
                _text = text;
            }
            public string Text { get { return _text; } }
        }
        interface IHasValue { ValueToAccess Value { get; } }
        class UnsafeState : IHasValue 
        {
            private ValueToAccess _value;
            public ValueToAccess Value { get { if (_value == null) _value = Compute(); return _value; } }
        }
        class DoubleCheckedLocking : IHasValue
        {
            private object _syncRoot = new object();
            public volatile ValueToAccess _Value;
            public ValueToAccess Value { get { if (_Value == null) lock (_syncRoot) if (_Value == null) _Value = Compute(); return _Value;  } }

        }
        class BCLDoubleChecked :IHasValue
        {
            private object _syncRoot = new object();
            private ValueToAccess _Value;
            private bool _initialized = false;
            public ValueToAccess Value
            {
                get { return LazyInitializer.EnsureInitialized(ref _Value, ref _initialized ,ref _syncRoot,Compute); }
            }
        }
        class BCLThreadSafeFactory : IHasValue
        {
            private ValueToAccess _Value;
            public ValueToAccess Value
            {
                get { return LazyInitializer.EnsureInitialized(ref _Value, Compute); }
            }
        }
    }
}
