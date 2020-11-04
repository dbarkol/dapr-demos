using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CloudNative.CloudEvents;
using Newtonsoft.Json;
using Inventory.Models;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }        

        [HttpGet("/dapr/subscribe")]
        public ActionResult<IEnumerable<string>> Get()
        {
            // Initialize an array of topic subscriptions. Each subscription
            // contains the name of the topic and the route.
            var topics = new [] 
            { 
                new 
                { 
                    pubsubname="pubsub", 
                    topic = "inventory", 
                    route = "additem"
                }
            };
                                
            return new OkObjectResult(topics);       
        }

        [HttpPost("/additem")]
        public async Task<IActionResult> AddItemToInventory(CloudEvent cloudEvent)
        {
            // The message is wrapped in a cloud event envelope. Which means that 
            // the domain-specific information is in the Data object.
            var item = JsonConvert.DeserializeObject<InventoryItem>(cloudEvent.Data.ToString());
            _logger.LogInformation($"New item: {item.Id} - {item.Name} - {item.Count} ");

            return new OkResult();
        }
        
    }

}