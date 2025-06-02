using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace XmlCore.Shared.Entities;

public class Article
{
    [Key]
    public Guid Id { get; set; }
    
    public string ArticleNumber { get; set; }
    [Required]
    public string Name { get; set; } 
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public Category Category { get; set; }

    [JsonIgnore]
    public ICollection<ArticlesInOrder> ArticlesInOrderList { get; set; } = [];

    public bool Status { get; set; } = true;

    public int Stock { get; set; }
}