using Newtonsoft.Json;

namespace Inventory.Models
{
    public class InventoryItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }    

        [JsonProperty("count")]
        public int Count { get; set; }        
    }
}