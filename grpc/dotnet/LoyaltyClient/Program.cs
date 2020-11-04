using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Dapr.Client;
using Dapr.Client.Http;
using Grpc.Core;
using LoyaltyClient.Models;

namespace LoyaltyClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));

            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            var client = new DaprClientBuilder()
                .UseJsonSerializationOptions(jsonOptions)
                .Build();

            await InvokeGrpcIncrementLoyaltyPointsAsync(client);

        }

        internal static async Task InvokeGrpcIncrementLoyaltyPointsAsync(DaprClient client)
        {
            Console.WriteLine("Invoking update to loyalty points");

            await Task.Delay(TimeSpan.FromMilliseconds(1));

            var data = new 
            { 
                id = 1, 
                points = 100
            };

            HTTPExtension httpExtension = new HTTPExtension()
            {
                Verb = HTTPVerb.Post
            };

            var account = await client.InvokeMethodAsync<object, LoyaltyAccount>("grpc-loyalty-service", "increment", data, httpExtension);
            Console.WriteLine("Updated points: {0}", account.TotalPoints);
        }
    }
}
