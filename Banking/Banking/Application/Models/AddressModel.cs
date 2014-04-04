using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Application.Models
{
    [Table("Address")]
    public class AddressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public CustomerModel Customer { get; set; }
    }
}