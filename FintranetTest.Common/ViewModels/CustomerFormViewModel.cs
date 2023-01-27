using System;
using System.ComponentModel.DataAnnotations;

namespace FintranetTest.Common.ViewModels;
public class CustomerFormViewModel
{
    public int? Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName is required")]
    public string Firstname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "LastName is required")]
    public string Lastname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is invalid")]
    public string Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Date of Birth is required")]
    public DateTime? DateOfBirth { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "PhoneNumber is required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "PhoneNumber must be digits only")]
    [MaxLength(11, ErrorMessage = "Max length is 11 digits")]
    [MinLength(11, ErrorMessage = "Min length is 11 digits")]
    public string PhoneNumber { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "BankAccountNumber is required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Bank Account Number must be digits only")]
    [MaxLength(18, ErrorMessage = "Max length is 18 digits")]
    [MinLength(9, ErrorMessage = "Min length is 9 digits")]
    public string BankAccountNumber { get; set; }
}
