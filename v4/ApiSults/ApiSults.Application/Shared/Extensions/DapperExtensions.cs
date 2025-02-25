using ApiSults.Domain.Shared.Extensions;
using Dapper;
using System.Text;

namespace ApiSults.Application.Shared.Extensions;

public static class DapperExtensions
{
    public static DynamicParameters AddConditionalParam(
        this DynamicParameters parameters,
        StringBuilder query,
        bool condition,
        string parameter,
        object value)
        => condition
            ? parameters.AddParam(query, parameter, value)
            : parameters;

    public static DynamicParameters AddParam(
        this DynamicParameters parameters,
        StringBuilder query,
        string parameter,
        object value)
    {
        if (query is null ||
            string.IsNullOrWhiteSpace(parameter) ||
            !parameter.Contains('@') ||
            value is null)
            return parameters;

        var parametersNames = parameter
            .Split(' ')
            .Where(x => x
                .StartsWith('@'))
            .Select(x => x.OnlyLetters())
            .Distinct();

        for (int i = 0; i < parametersNames.Count(); i++)
        {
            var parameterName = parametersNames.ElementAt(i);
            var parameterValue = TryGetValue(value, parameterName) ?? value;
            parameters.Add(parameterName, parameterValue);
        }

        query.AppendLine(parameter);

        return parameters;
    }

    private static object? TryGetValue(object value, string parameterName)
        => value.GetType().GetProperty(parameterName)?.GetValue(value);
}
