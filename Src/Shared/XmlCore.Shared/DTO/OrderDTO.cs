using System.ComponentModel.DataAnnotations;

namespace XmlCore.Shared.DTO;

public class OrderDTO
{
    [Key]
    public int Id { get; set; }
    public int OrderNumber { get; set; }
    public CompanyDTO Company { get; set; } 
    public double TotalSum { get; set; }

    public DateTime DateOfOrder { get; set; } = DateTime.UtcNow;
    
    public ICollection<ArticlesInOrderDTO> ArticlesInOrderList { get; set; } = [];
}