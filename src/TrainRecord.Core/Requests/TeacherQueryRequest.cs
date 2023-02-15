using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.IQueryable.Extensions;
using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Requests;

public class UserQueryRequest : ICustomQueryable
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public Role? Role { get; init; }
}
