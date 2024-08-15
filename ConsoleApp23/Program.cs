using System.Dynamic;
using System.Runtime.CompilerServices;

namespace ConsoleApp23
{
    internal class Program
    {
        async static Task AsynchronousProcessing()
        {
            string result = await GetDynamicAwaitableObject(true);
        }
        static  dynamic GetDynamicAwaitableObject(bool isSuccess)
        {
            dynamic result = new ExpandoObject();
            dynamic awaiter = new ExpandoObject();

            awaiter.message = "Completed synchronously";
            awaiter.iscompleted = isSuccess;
            awaiter.getresult = (Func<String>)(() => awaiter.message);

            awaiter.oncompleted = (Action<Action>)(callback => ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(1000);
                awaiter.message = "Completed asynchronously";
                if (isSuccess)
                {
                    callback();
                }
            }));
            //.netfromwork 4.0可用。
           //IAwaiter<string> proxy = Impromptu.Actlike(awaiter);
           // result.GetetResult = (Func<dynamic>)(() => proxy);
            return result;
        }
        static string GetInfo()
        {
            return string.Format("Task is running on a thread id {0}.Is thread pool thread :{1}",Thread.CurrentThread.ManagedThreadId,Thread.CurrentThread.IsThreadPoolThread);
        }
        public interface IAwaiter<T>:INotifyCompletion
        {
            bool IsCompleted { get; }
            T GetResult();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
