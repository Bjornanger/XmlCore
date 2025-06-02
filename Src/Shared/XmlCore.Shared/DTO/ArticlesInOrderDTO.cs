using System.ComponentModel.DataAnnotations;

namespace XmlCore.Shared.DTO;

public class ArticlesInOrderDTO
{

    [Key]
    public int Id { get; set; }

    public ArticleDTO Article { get; set; }
    
    public OrderDTO? Order { get; set; }

    public int Amount { get; set; }
}