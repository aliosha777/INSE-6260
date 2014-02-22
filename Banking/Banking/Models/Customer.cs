using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    [Table("Customer")]
    public class Customer
    {
        public Customer()
        {
            Accounts = new List<Account>();
            Addresses = new List<Address>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}