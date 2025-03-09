using System.Text.Json.Serialization;

namespace TestController.Models;

public class Friend
{
    [JsonPropertyName(("id"))]
    public int Id { get; set; }
    
    [JsonPropertyName(("name"))]
    public string Name { get; set; }
}