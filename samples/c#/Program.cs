using System;
using System.Threading;
using System.Threading.Tasks;

namespace csharp_sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Provide customer name:");
            var customerName = Console.ReadLine();

            Console.WriteLine("Provide API Key:");
            var apiKey = Console.ReadLine();

            Console.WriteLine("Provide API Secret:");
            var apiSecret = Console.ReadLine();

            Console.WriteLine("Executing GET request to Authentication Test endpoint");
            var bloxsClient = new BloxsClient(apiKey, apiSecret, customerName);
            var response = await bloxsClient.GetAsync("test", CancellationToken.None);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("Authentication Test succesfull");
            }
            else
            {
                Console.WriteLine($"Authentication Test failed: {response.StatusCode}");
            }

            Console.WriteLine("Goodbye World!");
            Console.ReadLine();
        }
    }
}
