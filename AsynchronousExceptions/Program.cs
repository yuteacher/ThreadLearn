namespace AsynchronousExceptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task t = AsynchronousProcessing();
            t.Wait();
        }
        async static Task AsynchronousProcessing()
        {
            Console.WriteLine("1 Single exception");
            try
            {
                string result = await GetInfoAsync("Task 1", 2);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("2 . Multipe exceptions");

            Task<string> t1 = GetInfoAsync("Task 1",3);
            Task<string> t2 = GetInfoAsync("Task 2", 2);
            try
            {
                string[] result = await Task.WhenAll(t1, t2);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine();
            Console.WriteLine("2 . Multipe exceptions with AggregateException");
             t1 = GetInfoAsync("Task 1", 3);
             t2 = GetInfoAsync("Task 2", 2);
            Task<string[]> t3 = Task.WhenAll(t1, t2);
            try
            {
                String[] result = await t3;
                Console.WriteLine(result.Length);
            }
            catch 
            {
                var ae = t3.Exception.Flatten();
                var exception = ae.InnerExceptions;
                Console.WriteLine(" Exception caught :{0} ", exception.Count);
                foreach (var ex in exception) Console.WriteLine("Exception details:{0}", ex);
            }

        }
        async static Task<string> GetInfoAsync(string name,int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            throw new Exception(string.Format("BOOM form {0}",name));
        }
    }
}
