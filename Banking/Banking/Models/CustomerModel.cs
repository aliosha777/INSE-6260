using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    [Table("Customer")]
    public class CustomerModel
    {
        public CustomerModel()
        {
            Accounts = new List<BankAccountModel>();
            Addresses = new List<AddressModel>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<AddressModel> Addresses { get; set; }

        public ICollection<BankAccountModel> Accounts { get; set; }
    }
}