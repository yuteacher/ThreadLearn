
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ConsoleApp15
{
    internal class Program
    {
        const string SERVICE_URL = "Http://localhost:1234/HelloWorld";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        static async Task RunServiceClient()
        {
            var endppoint= new EndpointAddress(SERVICE_URL);
            //BasicHttpBinding binding = new BasicHttpBinding();
            //var channel = ChannelFactory<IHelloWorldServiceClient>.CreateChannel(new BasicHttpBinding(), endppoint);
        }
        public interface IHelloWorldService 
        {
            [OperationContract]
            string Greet(string name);
        }
        [ServiceContract(Namespace = "Packt", Name = "HelloWorldServiceContract")]
        public interface IHelloWorldServiceClient 
        {
            [OperationContract]
            string Greet(string name);
            [OperationContract]
            Task<string> GreetAsync(string name);
        }
        public class HelloWorldService : IHelloWorldService
        {
            public string Greet(string name)=>string.Format( $"Greetings {name}");
        }


    }
}
