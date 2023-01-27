using Framework.Domain;
using System.Linq.Expressions;

namespace FintranetTest.Domain.Aggregates.Specifications;

public class IsCustomerExistedSpecification : Specification<Customer>
{
    private readonly string _firstname;
    private readonly string _lastname;
    private readonly DateOnly _dateOfBirth;

    public IsCustomerExistedSpecification(string firstname, string lastname, DateOnly dateOfBirth)
    {
        _firstname = firstname;
        _lastname = lastname;
        _dateOfBirth = dateOfBirth;
    }


    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.Firstname.Value.Equals(_firstname, StringComparison.OrdinalIgnoreCase)
        && customer.Lastname.Value.Equals(_lastname, StringComparison.OrdinalIgnoreCase)
        && customer.DateOfBirth == _dateOfBirth;
    }
}
