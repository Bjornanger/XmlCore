using System.Xml.Serialization;

namespace XmlCore.Shared.XmlDTO;

[Serializable]
public class ArticlesInOrderDto
{
    

    public int Id { get; set; }
    [XmlElement("Product")]
  
    public ArticleDto Article { get; set; }

    public OrderDto? Order { get; set; }
    [XmlElement("Quantity")]


    public int Amount { get; set; }
}