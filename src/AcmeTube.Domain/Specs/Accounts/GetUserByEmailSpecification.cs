using System;
using System.Linq.Expressions;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Specs.Core;

namespace AcmeTube.Domain.Specs.Accounts
{
    public sealed record GetUserByEmailSpecification(string Email) : Specification<User>
    {
        public override Expression<Func<User, bool>> ToExpression() => user => user.Email.Equals(Email);
    }
}
