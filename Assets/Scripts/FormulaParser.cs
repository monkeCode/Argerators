using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class FormulaParser
{
    private static Regex minusRegex = new Regex(@"(?<![-+\^/\*])-(?![-+\^/\*])");

    private static Regex functionRegex = new Regex("(?<func>\\w*)\\((?<args>.*)\\)");

    private static Regex opsRegex = new Regex(@"[-+*\\()]");

    public static ICalculable CreateFunc(string expression)
    {
        List<(string, string)> functionReplacement = new List<(string, string)>();
        StringBuilder builder = new StringBuilder(expression);
        List<(string repl, ICalculable)> functions = new List<(string repl, ICalculable)>();

        while (builder.ToString().Contains("(") && builder.ToString().Contains(")"))
        {
            var s = builder.ToString();
            int startI = s.IndexOf("(");
            int endI = 0;
            int count = 1;
            for (int i = startI+1; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    //startI = i;
                    count += 1;
                }

                if (s[i] != ')') continue;
                count--;
                if (count != 0) continue;
                endI = i;
                break;
            }

            if (endI <= startI)
                throw new ArgumentException();
            while (startI > 0 && char.IsLetter(s[startI - 1]))
                startI--;

            var match = functionRegex.Match(s.Substring(startI, endI+1- startI));
            if (!match.Success)
                throw new ArgumentException();

            if (match.Value == expression) break;
            var func = match.Groups["func"].Value;
            func = func.Length > 0 ? func : "brackets";
                var replacement = opsRegex.Replace(match.Groups["args"].Value, "") +"param"+ func;
                functionReplacement.Add((replacement, match.Value));
                builder.Replace(match.Value, replacement);
        }

        foreach(var f in functionReplacement)
        {
            functions.Add((f.Item1, CreateFunc(f.Item2)));
        }

        return CreateLambda(builder.ToString(), functions);
    }
    private static ICalculable CreateLambda(string expression, List<(string repl, ICalculable)> functions)
    {
        if(functionRegex.IsMatch(expression))
        {
            var match = functionRegex.Match(expression);
            if(match.Value == expression)
            {
                var args = match.Groups["args"].Value.Split(",").Select(it => CreateFunc(it)).ToArray();
                return new FunctionOperation(match.Groups["func"].Value, args);
            }
        }

        if (minusRegex.IsMatch(expression))
        {
            var parts = minusRegex.Split(expression, 2);
            parts[1] = parts[1].Replace("--", "+")
                .Replace("+-", "-").Replace("-", "plus").Replace("+", "-").Replace("plus", "+");
            if (string.IsNullOrEmpty(parts[0]))
                return new BinaryOperation("-", CreateLambda("0", functions), CreateLambda(parts[1], functions));
            return new BinaryOperation("-", CreateLambda(parts[0], functions), CreateLambda(parts[1], functions));
        }

        if (expression.Contains("+"))
        {
            var parts = expression.Split("+", 2, StringSplitOptions.RemoveEmptyEntries);
            return new BinaryOperation("+", CreateLambda(parts[0], functions), CreateLambda(parts[1], functions));
        }
        if (expression.Contains("/"))
        {
            return CreateOp("/", expression, functions);
        }
        if (expression.Contains("*"))
        {
            return CreateOp("*", expression, functions);
        }
        if (expression.Contains("^"))
        {
            return CreateOp("^", expression, functions);
        }
        if(double.TryParse(expression.Replace(".",","), out double res))
        {
            return new ConstOperand(res);
        }

        var func = functions.FirstOrDefault(it => it.repl == expression).Item2;
        if(func != null)
        {
            return func;
        }

        return new VariableOperand(expression);
    }

    private static ICalculable CreateOp(string op, string expression, List<(string repl, ICalculable)> functions)
    {
        var parts = expression.Split(op);
        return new BinaryOperation(op, CreateLambda(string.Join(op, parts[..^1]), functions), CreateLambda(parts[^1], functions));
    }

}

public interface ICalculable
{
    double Calculate(Dictionary<string, double> parameters);
}

public class VariableOperand : ICalculable
{
    private string _expression;

    public VariableOperand(string expression)
    {
        _expression = expression;
    }

    public double Calculate(Dictionary<string, double> parameters)
    {
        return parameters[_expression];
    }
}
public class ConstOperand : ICalculable
{
    private double _value;

    public ConstOperand(double expression)
    {
        _value = expression;
    }

    public double Calculate(Dictionary<string, double> parameters)
    {
        return _value;
    }
}

public class BinaryOperation : ICalculable
{
    private static Dictionary<string, Func<Dictionary<string, double>, ICalculable, ICalculable, double>> _operations = new()
    {
        {"-", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) - a2.Calculate(param)},
        {"+", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) + a2.Calculate(param)},
        {"/", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) / a2.Calculate(param)},
        {"*", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) * a2.Calculate(param)},
        {"^", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => Math.Pow(a1.Calculate(param), a2.Calculate(param))},
    };

    private Func<Dictionary<string, double>, ICalculable, ICalculable, double> _operation;

    private ICalculable _left;

    private ICalculable _right;

    public BinaryOperation(string op, ICalculable left, ICalculable right)
    {
        _operation = _operations[op];
        _left = left;
        _right = right;
    }

    public double Calculate(Dictionary<string, double> parameters)
    {
        return _operation(parameters, _left, _right);
    }
}

public class FunctionOperation : ICalculable
{
    private static Dictionary<string, Func<Dictionary<string, double>, ICalculable[], double>> _operations = new()
    {
        {"sin", (param, a) => Math.Sin(a[0].Calculate(param))},
        {"cos", (param, a) => Math.Cos(a[0].Calculate(param))},
        {"tan", (param, a) => Math.Tan(a[0].Calculate(param))},
        {"exp", (param, a) => Math.Exp(a[0].Calculate(param))},
        {"sqrt", (param, a) => Math.Sqrt(a[0].Calculate(param))},
        {"max", (param, a) => a.Select(it => it.Calculate(param)).Max()},
        {"min", (param, a) => a.Select(it => it.Calculate(param)).Min()},
        {"", (param, a) => a[0].Calculate(param)}
    };

    private ICalculable[] _calculables;

    private Func<Dictionary<string, double>, ICalculable[], double> _func;

    public FunctionOperation(string op, ICalculable[] calculables)
    {
        _func = _operations[op];
        _calculables = calculables;
    }

    public double Calculate(Dictionary<string, double> parameters)
    {
       return _func(parameters, _calculables);
    }
}
