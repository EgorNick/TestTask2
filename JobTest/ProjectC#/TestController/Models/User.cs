using System.Text.Json.Serialization;

namespace TestController.Models;

public class User
{
    [JsonPropertyName(("name"))]
    public string Name { get; set; }
    
    [JsonPropertyName(("phone"))]
    public string Phone { get; set; }
    
    [JsonPropertyName(("email"))]
    public string Email { get; set; }
    
    [JsonPropertyName(("isActive"))]
    
    public bool IsActive { get; set; }
    
    [JsonPropertyName(("friends"))]
    public List<Friend> Friends { get; set; } = new();
}