using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Application.Models
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

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public virtual List<AddressModel> Addresses { get; set; }

        public virtual List<BankAccountModel> Accounts { get; set; }
    }
}