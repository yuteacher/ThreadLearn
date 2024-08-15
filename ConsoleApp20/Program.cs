﻿namespace ConsoleApp20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Incorrect counter"); ;
             var c = new Counter();
            var t1 =new Thread(()=>TestCounter(c));
            var t2 = new Thread(() => TestCounter(c));
            var t3 = new Thread(() => TestCounter(c));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Total count :{0}",c.Count);
            Console.WriteLine("------------");
            Console.WriteLine("Correct counter");
            var c1 = new CounterNoLock();
            t1 = new Thread(() => TestCounter(c1));
            t2 = new Thread(() => TestCounter(c1));
            t3 = new Thread(() => TestCounter(c1));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Total Count :{0}", c1.Count);
        }
        static void TestCounter(CounterBase c)
        {
            for (int i = 0; i < 1000; i++)
            {
                c.Increment();
                c.Decrement();
            }
        }
    }
   
    class Counter : CounterBase
    { 
        private int count;
        public int Count { get { return count; } }
        public override void Increment()
        {
            count++;
        }
        public override void Decrement()
        {
            count--;
        }
    }
    class CounterNoLock: CounterBase
    {
        private int count;
        public int Count { get { return count; } }
        public override void Increment()
        {
            Interlocked.Increment(ref count);
        }
        public override void Decrement()
        {
            Interlocked.Decrement(ref count);
        }
    }
    abstract class CounterBase
    {
        public abstract void Increment();
        public abstract void Decrement();
    }

}
