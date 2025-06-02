using System.Xml.Serialization;

namespace XmlCore.Shared.XmlDTO;

[Serializable]

public class CompanyDto
{

    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("LastName")]
    public string CompanyName { get; set; }
    [XmlElement("Email")]
    public string Email { get; set; }
    [XmlElement("Password")]
    public string Password { get; set; }

    [XmlElement("Phone")]
    public string Phone { get; set; }
    [XmlElement("StreetAddress")]
    public string Address { get; set; }
    [XmlElement("City")]
    public string City { get; set; }
    [XmlElement("Country")]
    public string Country { get; set; }

}