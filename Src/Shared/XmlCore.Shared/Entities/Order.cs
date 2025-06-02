using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace XmlCore.Shared.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    [JsonIgnore]
    public Company Company { get; set; } = null;
    public double TotalSum { get; set; }
    
    public DateTime DateOfOrder { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public ICollection<ArticlesInOrder> ArticlesInOrderList { get; set; } = [];

}