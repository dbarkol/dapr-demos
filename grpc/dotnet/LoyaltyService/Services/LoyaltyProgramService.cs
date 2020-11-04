using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Dapr.Client;
using Google.Protobuf.WellKnownTypes;
using LoyaltyService.Models;
using Google.Protobuf;

namespace LoyaltyService
{
    public class LoyaltyProgramService : AppCallback.AppCallbackBase
    {
        private readonly ILogger<LoyaltyProgramService> _logger;
        private readonly DaprClient _daprClient;        
        readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public LoyaltyProgramService(DaprClient client, ILogger<LoyaltyProgramService> logger)
        {
            _daprClient = client;
            _logger = logger;
        }

        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            _logger.LogDebug("OnInvoke");
            var response = new InvokeResponse();

            switch (request.Method)
            {
                case "increment":
                    var incrementRequest = JsonSerializer.Deserialize<IncrementRequest>(request.Data.Value.ToByteArray(), this.jsonOptions);                                    
                    var account = new LoyaltyAccount()
                    {
                        Id = incrementRequest.Id,
                        TotalPoints = incrementRequest.Points * 2
                    };

                    response.Data = new Any
                    {
                        Value = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes<LoyaltyAccount>(account, this.jsonOptions))
                    };
                    break;
                default:
                    break;
            }
            
            //await Task.Delay(TimeSpan.FromSeconds(1));
            
            return response;
        }
        
        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var result = new ListTopicSubscriptionsResponse();

            // TODO: Implement

            return Task.FromResult(result);
        }        

        public override async Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            // TODO: Implement

            return await Task.FromResult(default(TopicEventResponse));
        }        


        // ListInputBindings
        // OnBindingEvent

    }
}
