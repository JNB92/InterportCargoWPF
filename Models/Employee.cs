namespace InterportCargoWPF.Models;

public class Employee
{
    public string EmployeeID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string EmployeeType { get; set; }
    
    public string Address { get; set; }
    
    public string PasswordHash { get; set; }
}
