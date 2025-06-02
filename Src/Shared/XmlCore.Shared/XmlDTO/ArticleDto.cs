using System.Xml.Serialization;

namespace XmlCore.Shared.XmlDTO;

[Serializable]

public class ArticleDto
{

    [XmlElement("ProductId")]
    public Guid Id { get; set; }

    public string ArticleNumber { get; set; }
    [XmlElement("Name")]


    public string Name { get; set; }
    [XmlElement("Description")]


    public string Description { get; set; }
   

    public int Quantity { get; set; }


    [XmlElement("Price")]
    public double Price { get; set; }

    public CategoryDto Category { get; set; }

    
    public List<ArticlesInOrderDto> ArticlesInOrderList { get; set; } = [];
    [XmlElement("IsActive")]
    public bool Status { get; set; } = true;

    public int Stock { get; set; }
}