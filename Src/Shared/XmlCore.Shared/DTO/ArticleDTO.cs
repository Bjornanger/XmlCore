using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XmlCore.Shared.Entities;

namespace XmlCore.Shared.DTO;

public class ArticleDTO
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
    public int Category { get; set; }
    [JsonIgnore]
    public ICollection<ArticlesInOrder> InOrder { get; set; } = new List<ArticlesInOrder>();

    public int Quantity { get; set; }

    public bool Status { get; set; } = true;

    public int Stock { get; set; }

    

    public string? ErrorMessage { get; set; }     
  
}


