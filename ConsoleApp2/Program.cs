using System.Reflection;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parallelQuery = from t in GetTypes().AsParallel()
                                select EmulateProcessing(t);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(3));

            try
            {
                parallelQuery.WithDegreeOfParallelism(Environment.ProcessorCount).WithExecutionMode(ParallelExecutionMode.ForceParallelism).WithMergeOptions(ParallelMergeOptions.Default).WithCancellation(cts.Token).ForAll(Console.WriteLine);
            }
            catch(OperationCanceledException)
            {
                Console.WriteLine("----------");
                Console.WriteLine("Operation has been cancelled!");

            }
            Console.WriteLine("-----");
            Console.WriteLine("Unordered PLINQ query execution");
            var unorderedQuery = from t in ParallelEnumerable.Range(1, 30) select t;

            foreach(var t in unorderedQuery) { Console.WriteLine(t); }

            Console.WriteLine("-----");
            Console.WriteLine("Oedered PLINQ query execution");
            var oederdQuery = from i  in ParallelEnumerable.Range(1,30).AsOrdered() select i;

            foreach(var t in oederdQuery)
            { Console.WriteLine(t); }

        }
        static string EmulateProcessing(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random( DateTime.Now.Millisecond).Next(250, 350)));
            Console.WriteLine(" {0}  type was processed on a thread id {1} ", typeName, Thread.CurrentThread.ManagedThreadId);
            return typeName;
        }
        static IEnumerable<string> GetTypes() 
        { 
            return from Assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from Type in Assembly.GetExportedTypes()
                   where Type.Name.StartsWith("Web")
                   orderby Type.Name.Length
                   select Type.Name;
        
        }
    }
}
