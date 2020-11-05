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

            // Test service invocation
            await InvokeGrpcIncrementLoyaltyPointsAsync(client);

            // Test pub/sub
            await PublishIncrementPointsEventAsync(client);
        }

        internal static async Task InvokeGrpcIncrementLoyaltyPointsAsync(DaprClient client)
        {
            Console.WriteLine("Invoking update to loyalty points");

            // Initialize the payload for the request
            var incrementRequest = new IncrementRequest()
            {
                Id = 1,
                Points = 400
            };
            
            HTTPExtension httpExtension = new HTTPExtension(){ Verb = HTTPVerb.Post };

            // Make the service invocation request. 
            // Pass in an increment request and expect back a payload that
            // represents the loyalty account.
            var account = await client.InvokeMethodAsync<IncrementRequest, LoyaltyAccount>("grpc-loyalty-service", "increment", incrementRequest, httpExtension);

            // Display the results
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
