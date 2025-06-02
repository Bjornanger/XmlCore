using System.ComponentModel.DataAnnotations;

namespace XmlCore.Shared.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        
        public ICollection<Order> Orders { get; set; } = [];



    }
}
