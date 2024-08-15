namespace ConsoleApp26
{
    //MeReduce 出现异常，暂不处理。
    internal class Program
    {
        
        private static readonly char[] delimiters = Enumerable.Range(0, 256).Select(i => (char)i).Where(c => !char.IsLetterOrDigit(c)).ToArray();
        private const string textToPrase = "Hello, World!";
        static void Main(string[] args)
        {
            //var q = textToPrase.Split(delimiters)
            //    .AsParallel()
            //    .MapReduce(
            //    s => s.ToLower().ToCharArrat(),
            //    c => c,
            //    g => new[] { new { Char = g.Key, Count = g.Count() } })
            //    .Where(c => char.IsLetterOrDigit(c.Char))
            //    .OrderByDescending(c => c.Count);
        }
    }
}
