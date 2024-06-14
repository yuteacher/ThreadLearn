using System.Reflection;

namespace ConsoleApp14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        async static Task ProcessAsynchronousIO(string dbName)
        {
            try
            {
                const string connectionString = @"Data Source=(LocalDB)\V11.0;Initial 
                    Catalog = master;Integrated Security==True";
                string outputFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string dbFileName = Path.Combine(outputFolder, string.Format(@".\{0}.mdf",dbName));
                string dbLogFileName = Path.Combine(outputFolder, string.Format(@".\{0}_log.ldf", dbName));
                string dbConnectionSrting = string.Format(@"Data Source =(LocalDB)\v11.0;AttachDBFileName ={1};Initial Catalog={0};Integrated Security=True;", dbName, dbFileName);

                //using (var connection = new SqlConnection(connectionString))
                //{

                //}
            }catch (Exception ex)
            {

            }
        }
    }
}
