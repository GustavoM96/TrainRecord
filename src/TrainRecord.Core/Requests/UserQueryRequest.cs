using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Sort;
using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Requests;

public class UserQueryRequest : ICustomQueryable, IQuerySort
{
    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? Email { get; init; }

    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? FirstName { get; init; }

    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? LastName { get; init; }
    public Role? Role { get; set; }
    public string Sort { get; set; } = "";
}

public class TeacherQueryRequest : ICustomQueryable, IQuerySort
{
    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? Email { get; init; }

    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? FirstName { get; init; }

    [QueryOperator(Operator = WhereOperator.Contains, CaseSensitive = false)]
    public string? LastName { get; init; }
    public string Sort { get; set; } = "";
}
