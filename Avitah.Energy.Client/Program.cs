using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace Avitah.Energy.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new Energy.EnergyClient(channel);
            var request = new TimeSeriesRequest { Country = "UK", Quantity = "gas", FromDate = 1567296000000 };
            try
            {
                var response = await client.GetTimeSeriesAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
