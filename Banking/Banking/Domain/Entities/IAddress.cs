namespace Banking.Domain.Entities
{
    public interface IAddress
    {
        int AddressId { get; set; }

        string Line1 { get; set; }

        string Line2 { get; set; }

        string City { get; set; }

        string Province { get; set; }

        string PostalCode { get; set; }

        bool IsActive { get; set; }
    }
}