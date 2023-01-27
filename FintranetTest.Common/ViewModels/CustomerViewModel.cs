using System;

namespace FintranetTest.Common.ViewModels;

public class CustomerViewModel
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string BankAccountNumber { get; set; }
}
