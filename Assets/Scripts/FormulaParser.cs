using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class FormulaParser
{
    private static Regex minusRegex = new Regex("(?<=\\d)-(?=>\\d)");

    private static Regex functionRegex = new Regex("^(?<func>\\w*)\\((?<args>.*?)\\)$");

    private static Dictionary<string, Func<double[], double>> _funcs = new()
    {
        {"min", Min},
        {"max", Max}
    };
    
    public static double Calculate(string expression)
    {
        try
        {
            return Convert.ToDouble(expression);
        }
        catch (Exception)
        {
            // ignored
        }

        if (functionRegex.IsMatch(expression))
        {
            var match = functionRegex.Match(expression);
            return _funcs[match.Groups["func"].Value]
                (match.Groups["args"].Value.Split(";")
                    .Select(Convert.ToDouble).ToArray());
        }
        if (minusRegex.IsMatch(expression))
        {
            var parts = minusRegex.Split(expression, 2);
            parts[1] = parts[1].Replace("--","+")
                .Replace("+-","-").Replace("-", "plus").Replace("+", "-").Replace("plus","+");
            return Calculate(parts[0]) - Calculate(parts[1]);
        }

        if (expression.Contains("+"))
        {
            var parts = expression.Split("+", 2, StringSplitOptions.RemoveEmptyEntries);
            return Calculate(parts[0]) + Calculate(parts[1]);
        }
        if (expression.Contains("/"))
        {
            var parts = expression.Split("/");
            return Calculate(string.Join("/", parts[..^1])) / Calculate(parts[^1]);
        }
        if (expression.Contains("*"))
        {
            var parts = expression.Split("*");
            return Calculate(string.Join("*", parts[..^1])) * Calculate(parts[^1]);
        }
        if (expression.Contains("^"))
        {
            var parts = expression.Split("^");
            return Calculate(string.Join("^", parts[..^1])) * Calculate(parts[^1]);
        }
        
        return 0;
    }

    private static double Min(double[] s) => s.Min();

    private static double Max(double[] s) => s.Max();

}
