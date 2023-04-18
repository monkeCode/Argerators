using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class FormulaParser
{
    private static string[] _operators = { "-", "+", "/", "*","^"};
    private static  Dictionary<string, Func<double, double, double>>  _operations = new() {
        {"-", (a1, a2) => a1 - a2},
        {"+", (a1, a2) => a1 + a2},
        {"/",(a1, a2) => a1 / a2},
        {"*", (a1, a2) => a1 * a2},
        {"^", (a1, a2) => Math.Pow(a1, a2)}
    };
    
    public static double Calculate(string expression)
    {
        try
        {
            return Double.Parse(expression);
        }
        catch (Exception _)
        {
            // ignored
        }

        if (expression.Contains("-"))
        {
            var parts = expression.Split("0", 2, StringSplitOptions.RemoveEmptyEntries);
            parts[1] = parts[1].Replace("-", "plus").Replace("+", "-").Replace("plus","+");
            return Calculate(parts[0]) - Calculate(parts[1]);
        }

        return 0;
    }
    
}
