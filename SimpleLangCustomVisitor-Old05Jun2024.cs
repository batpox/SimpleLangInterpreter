using System;
using System.Collections.Generic;
using System.Numerics;

public class Variable
{
    public string Name { get; set; }
    public string Type { get; set; }
    public object Value { get; set; }

    public Variable(string name, string type, object value)
    {
        Name = name;
        Type = type;
        Value = value;
    }
}

public class ClassType
{
    public string Name { get; set; }
    public Dictionary<string, Variable> Members { get; set; }

    public ClassType(string name)
    {
        Name = name;
        Members = new Dictionary<string, Variable>();
    }
}

public class SimpleLangCustomVisitor : SimpleLangBaseVisitor<object>
{
    private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    private Dictionary<string, ClassType> classes = new Dictionary<string, ClassType>();

    /// <summary>
    /// Public dictionary of variable-name, variable object
    /// </summary>
    public Dictionary<string, Variable> Variables => variables;

    public override object VisitProg(SimpleLangParser.ProgContext context)
    {
        foreach (var statement in context.stat())
        {
            Console.WriteLine($"Statement Start={statement.Start}");
            Visit(statement);
        }
        return null;
    }

    public override object VisitClassDecl(SimpleLangParser.ClassDeclContext context)
    {
        string className = context.ID().GetText();
        var classType = new ClassType(className);

        foreach (var classVarDecl in context.classVarDecl())
        {
            string varType = classVarDecl.type().GetText();
            string varName = classVarDecl.ID().GetText();
            classType.Members[varName] = new Variable(varName, varType, null);
        }

        classes[className] = classType;
        Console.Write($"Class={className} defined.");
        return null;
    }

    public override object VisitVarDecl(SimpleLangParser.VarDeclContext context)
    {
        string varType = context.type().GetText();
        string varName = context.ID().GetText();
        object varValue = context.expr() != null ? Visit(context.expr()) : null;

        variables[varName] = new Variable(varName, varType, varValue);
        Console.WriteLine($"Variable {varName} of type {varType} declared with value {varValue}.");
        return null;
    }

    public override object VisitAssign(SimpleLangParser.AssignContext context)
    {
        string varName = context.varReference().GetText();
        object varValue = Visit(context.expr());

        if (variables.ContainsKey(varName))
        {
            variables[varName].Value = varValue;
            Console.WriteLine($"Variable {varName} assigned value {varValue}.");
        }
        else
        {
            throw new Exception($"Undefined variable: {varName}");
        }
        return null;
    }

    public override object VisitMulDiv(SimpleLangParser.MulDivContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        string op = context.op.Text;

        return EvaluateBinaryOperation(left, right, op);
    }

    public override object VisitAddSub(SimpleLangParser.AddSubContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        string op = context.op.Text;

        return EvaluateBinaryOperation(left, right, op);
    }

    public override object VisitInt(SimpleLangParser.IntContext context)
    {
        return int.Parse(context.INT().GetText());
    }

    public override object VisitReal(SimpleLangParser.RealContext context)
    {
        return double.Parse(context.REAL().GetText());
    }

    public override object VisitStr(SimpleLangParser.StrContext context)
    {
        return context.STRING().GetText().Trim('"');
    }

    public override object VisitVarRefLabel(SimpleLangParser.VarRefLabelContext context)
    {
        string varName = context.varReference().ID(0).GetText();

        if (variables.ContainsKey(varName))
        {
            var current = variables[varName].Value;
            for (int i = 1; i < context.varReference().ID().Length; i++)
            {
                string memberName = context.varReference().ID(i).GetText();
                if (current is Dictionary<string, Variable> classInstance && classInstance.ContainsKey(memberName))
                {
                    current = classInstance[memberName].Value;
                }
                else
                {
                    throw new Exception($"Undefined member: {memberName}");
                }
            }
            return current;
        }
        else
        {
            throw new Exception($"Undefined variable: {varName}");
        }
    }

    public override object VisitParens(SimpleLangParser.ParensContext context)
    {
        return Visit(context.expr());
    }

    private object EvaluateBinaryOperation(object left, object right, string op)
    {
        if (left is int leftInt && right is int rightInt)
        {
            switch (op)
            {
                case "+":
                    return leftInt + rightInt;
                case "-":
                    return leftInt - rightInt;
                case "*":
                    return leftInt * rightInt;
                case "/":
                    if (rightInt == 0)
                        throw new DivideByZeroException();
                    return leftInt / rightInt;
                default:
                    throw new InvalidOperationException($"Unknown operator: {op}");
            }
        }
        else if (left is double leftDouble && right is double rightDouble)
        {
            switch (op)
            {
                case "+":
                    return leftDouble + rightDouble;
                case "-":
                    return leftDouble - rightDouble;
                case "*":
                    return leftDouble * rightDouble;
                case "/":
                    if (rightDouble == 0.0)
                        throw new DivideByZeroException();
                    return leftDouble / rightDouble;
                default:
                    throw new InvalidOperationException($"Unknown operator: {op}");
            }
        }
        else if (left is int leftIntMixed && right is double rightDoubleMixed)
        {
            switch (op)
            {
                case "+":
                    return leftIntMixed + rightDoubleMixed;
                case "-":
                    return leftIntMixed - rightDoubleMixed;
                case "*":
                    return leftIntMixed * rightDoubleMixed;
                case "/":
                    if (rightDoubleMixed == 0.0)
                        throw new DivideByZeroException();
                    return leftIntMixed / rightDoubleMixed;
                default:
                    throw new InvalidOperationException($"Unknown operator: {op}");
            }
        }
        else if (left is double leftDoubleMixed && right is int rightIntMixed)
        {
            switch (op)
            {
                case "+":
                    return leftDoubleMixed + rightIntMixed;
                case "-":
                    return leftDoubleMixed - rightIntMixed;
                case "*":
                    return leftDoubleMixed * rightIntMixed;
                case "/":
                    if (rightIntMixed == 0)
                        throw new DivideByZeroException();
                    return leftDoubleMixed / rightIntMixed;
                default:
                    throw new InvalidOperationException($"Unknown operator: {op}");
            }
        }
        throw new InvalidOperationException($"Unsupported operand types: {left.GetType()} and {right.GetType()}");
    }
}
