using System.Runtime.CompilerServices;

namespace ConsoleApp16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        static async Task ProcessAsynchronously()
        {
            var unsafeState = new UnsafeState();

        }
        static void Worker(IHasValue state)
        {

        }
        static void Worker(Lazy<ValueToAccess> state)
        {

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
    }
}
