using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp12
{
    internal class Program
    {
        const int BUFFER_SIZE = 4096;
        static void Main(string[] args)
        {
            var t = ProcessAsynchronousIO();
            t.GetAwaiter().GetResult();
        }
        async static Task ProcessAsynchronousIO()
        {
            using (var stream = new FileStream(
                "Test1.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE
                ))
            {
                Console.WriteLine("1 .use i/o Threads: {0}", stream.IsAsync);
                byte[] buffer = Encoding.UTF8.GetBytes(CreateFileContent());
                var writeTask = Task.Factory.FromAsync(
                    stream.BeginWrite, stream.EndWrite, buffer, 0, buffer.Length, null);
                await writeTask;
            }

            using (var stream = new FileStream("test2.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFFER_SIZE, FileOptions.Asynchronous))
            {
                Console.WriteLine("2. Uses I/O Threads:{0} ", stream.IsAsync);

                byte[] buffer = Encoding.UTF8.GetBytes(CreateFileContent());
                var writeTask = Task.Factory.FromAsync(
                    stream.BeginWrite, stream.EndWrite, buffer, 0, buffer.Length, null
                    );
                await writeTask;
            }

            using (var stream = File.Create("test3.txt", BUFFER_SIZE, FileOptions.Asynchronous))
            using (var sw = new StreamWriter(stream))
            {
                Console.WriteLine("3. Uses I/O Threads:{0} ", stream.IsAsync);
                await sw.WriteAsync(CreateFileContent());
            }

            using (var sw = new StreamWriter("test4.txt", true))
            {
                Console.WriteLine("4. Uses I/O Threads:{0} ", ((FileStream)sw.BaseStream).IsAsync);
                await sw.WriteAsync(CreateFileContent());
            }

            Console.WriteLine("Starting parsing files in parallel");

            Task<long>[] readTask = new Task<long>[4];
            for (int i = 0; i < 4; i++)
            {
                readTask[i] = SumFileContent(string.Format("Test{0}.txt", i + 1));
            }
            long[] sums = await Task.WhenAll(readTask);
            Console.WriteLine("Sum in all files :{0}",sums.Sum());

            Console.WriteLine("Deleting files ...");

            Task[] deleteTasks = new Task[4];
            for (int i = 0; i < 4; i++)
            {
                string fileName = string.Format("test{0}.txt", i + 1);
                deleteTasks[i] = SimulateAsynchronousDelete(fileName);
            }

            await Task.WhenAll(deleteTasks);

            Console.WriteLine("Deleting complete.");
        }
        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <returns></returns>
        static string CreateFileContent()
        {
            var sb =new StringBuilder();
            for (int i = 0;i < 100000; i++)
            {
                sb.AppendFormat("{0} ",new Random(i).Next(0,99999));
                sb.AppendLine();
            }
            return sb.ToString();
        }
        /// <summary>
        /// 读文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        async static Task<long> SumFileContent( string fileName)
        {
            using (var stream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read, FileShare.None, BUFFER_SIZE, FileOptions.Asynchronous)
                     )
            using ( var sr =new StreamReader(stream))
            {
                long sum = 0;
                while (sr.Peek() >-1)
                {
                    string line = await sr.ReadLineAsync();
                    sum += long.Parse(line);
                }
                return sum;
            }
        }
        static Task SimulateAsynchronousDelete(string fileName)
        {
            return Task.Run(() => File.Delete(fileName));
        }
    }
}
