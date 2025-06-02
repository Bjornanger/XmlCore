using System.Xml.Serialization;

namespace XmlCore.Shared.XmlDTO;

[XmlRoot("OrderDto")]
[Serializable]
public class OrderDto
{
   
    public int Id { get; set; }

    public int OrderNumber { get; set; } 
    [XmlElement("User")]

    public CompanyDto Company { get; set; }
    [XmlElement("OrderSum")]
    
    public double TotalSum { get; set; }
    public DateTime DateOfOrder { get; set; } = DateTime.UtcNow;
    
    [XmlArray("OrderProducts")]
    [XmlArrayItem("OrderProductDto")]
  
    public List<ArticlesInOrderDto> ArticlesInOrderList { get; set; } = [];

}