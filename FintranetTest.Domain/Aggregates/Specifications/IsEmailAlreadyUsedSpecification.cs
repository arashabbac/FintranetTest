using Framework.Domain;
using System.Linq.Expressions;

namespace FintranetTest.Domain.Aggregates.Specifications;

public class IsEmailAlreadyUsedSpecification : Specification<Customer>
{
    private readonly string _email;
    public IsEmailAlreadyUsedSpecification(string email)
    {
        _email = email;
    }

    public override Expression<Func<Customer, bool>> ToExpression()
    {
        return customer => customer.Email.Value.Equals(_email, StringComparison.OrdinalIgnoreCase);
    }
}
