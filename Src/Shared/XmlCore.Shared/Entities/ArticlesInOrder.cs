using System.ComponentModel.DataAnnotations;

namespace XmlCore.Shared.Entities;

public class ArticlesInOrder
{
    [Key]
    public int Id { get; set; }

    public  Article Article { get; set; }
   
    public  Order Order { get; set; } 

    public int Amount { get; set; }
}
