using System.Data;
using Dapper;

namespace Pincred.Template.Data.Handlers;

public class NullableDateOnlyHandler : SqlMapper.TypeHandler<DateOnly?>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly? value)
    {
        parameter.Value = value.HasValue
            ? value.Value.ToDateTime(TimeOnly.MinValue)
            : DBNull.Value;
    }

    public override DateOnly? Parse(object value)
    {
        if (value is null || value is DBNull)
            return null;

        return value switch
        {
            DateTime dt => DateOnly.FromDateTime(dt),
            string s => DateOnly.Parse(s),
            _ => throw new DataException($"Fail on the handler {value.GetType()} em DateOnly")
        };
    }
}