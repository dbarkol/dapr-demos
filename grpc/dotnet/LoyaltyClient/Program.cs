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

            await PublishIncrementPointsEventAsync(client);

        }

        internal static async Task InvokeGrpcIncrementLoyaltyPointsAsync(DaprClient client)
        {
            Console.WriteLine("Invoking update to loyalty points");

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

        internal static async Task PublishIncrementPointsEventAsync(DaprClient client)
        {
            var eventData = new { id = 1, points = 400 };
            await client.PublishEventAsync("pubsub", "points", eventData);
            Console.WriteLine("Published points event!");
        }

    }
}
