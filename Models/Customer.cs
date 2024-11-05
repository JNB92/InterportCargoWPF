namespace InterportCargoWPF.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string? Company { get; set; }
    public string Password { get; set; }

    // Navigation property for related Quotations
    public ICollection<Quotation> Quotations { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public Customer()
    {
        Quotations = new List<Quotation>();
    }

    public Customer(string firstName, string lastName, string email, string phoneNumber, string address, string? company, string password) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Company = company;
        Password = password;
    }
}