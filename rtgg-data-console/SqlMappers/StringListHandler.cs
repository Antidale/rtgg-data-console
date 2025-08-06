using System.Data;
using Dapper;

namespace rtgg_data_console.SqlMappers;

public class StringListHandler : SqlMapper.TypeHandler<List<string>>
{
    public override List<string> Parse(object value)
    {
        if (value is null) return [];
        try
        {
            string[] typedValue = (string[])value;
            return [.. typedValue];
        }
        catch (Exception)
        {
            return [];
        }

    }

    public override void SetValue(IDbDataParameter parameter, List<string>? value)
    {
        parameter.Value = value;
    }
}
