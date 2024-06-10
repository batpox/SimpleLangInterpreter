using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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

    public override string ToString()
    {
        return Value == null ? "(null)" : (Value is Dictionary<string, Variable> dictionary ? ToDictionaryString(dictionary) : Value.ToString());
    }

    private string ToDictionaryString(Dictionary<string, Variable> dictionary)
    {
        StringBuilder result = new StringBuilder();
        result.Append('[');

        foreach (var kvp in dictionary)
        {
            if (result.Length > 1)
            {
                result.Append(", ");
            }
            result.Append($"{kvp.Key}={kvp.Value}");
        }

        result.Append(']');
        return result.ToString();
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

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Class=[{Name}]");
        foreach (var member in Members.Values) 
        { 
            sb.AppendLine($"  {member.Name} ({member.Type}) = {member.Value}");
        }
        return sb.ToString();
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
        Console.WriteLine($"VisitProg: Entering. Statement Count={context.stat().Count()}");
        foreach (var statement in context.stat())
        {
            try
            {
                Console.WriteLine($"VisitProg: Visiting Statement: {statement.GetText()}");
                Visit(statement);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VisitProg: Error visiting statement: {ex.Message}");
            }
        }
        Console.WriteLine($"VisitProg: Exiting");
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
            Console.WriteLine($"VisitClassDecl: Class member {varName} of type {varType} declared.");
        }

        classes[className] = classType;
        Console.Write($"VisitClassDecl: Class={className} defined.");
        return null;
    }

    public override object VisitVarDecl(SimpleLangParser.VarDeclContext context)
    {
        string varType = context.type().GetText();
        string varName = context.ID().GetText();
        object varValue = null;

        // Initialize class type variables
        if (classes.ContainsKey(varType))
        {
            varValue = new Dictionary<string, Variable>(classes[varType].Members);
        }
        else if (context.expr() != null)
        {
            varValue = Visit(context.expr());
        }

        variables[varName] = new Variable(varName, varType, varValue);
        Console.WriteLine($"VisitVarDecl: Variable {varName} of type {varType} declared with value {varValue}.");
        return null;
    }

    public override object VisitAssign(SimpleLangParser.AssignContext context)
    {
        string varName = context.varReference().ID(0).GetText();
        object varValue = Visit(context.expr());

        if (variables.ContainsKey(varName))
        {
            if (variables[varName].Value is Dictionary<string, Variable> classInstance)
            {
                string memberName = context.varReference().ID(1).GetText();
                if (classInstance.ContainsKey(memberName))
                {
                    classInstance[memberName].Value = varValue;
                    Console.WriteLine($"VisitAssign: Member {memberName} of variable {varName} assigned value {varValue}.");
                }
                else
                {
                    throw new Exception($"Undefined member: {memberName}");
                }
            }
            else
            {
                variables[varName].Value = varValue;
                Console.WriteLine($"VisitAssign: Variable {varName} assigned value {varValue}.");
            }
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

        Console.WriteLine($"VisitMulDiv: {left} {op} {right}");
        return EvaluateBinaryOperation(left, right, op);
    }

    public override object VisitAddSub(SimpleLangParser.AddSubContext context)
    {
        var left = Visit(context.expr(0));
        var right = Visit(context.expr(1));
        string op = context.op.Text;

        Console.WriteLine($"VisitAddSub: {left} {op} {right}"); 
        return EvaluateBinaryOperation(left, right, op);
    }

    public override object VisitInt(SimpleLangParser.IntContext context)
    {
        Console.WriteLine($"VisitInt: {context.INT().GetText()}");
        return int.Parse(context.INT().GetText());
    }

    public override object VisitReal(SimpleLangParser.RealContext context)
    {
        Console.WriteLine($"VisitReal: {context.REAL().GetText()}");
        return double.Parse(context.REAL().GetText());
    }

    public override object VisitStr(SimpleLangParser.StrContext context)
    {
        Console.WriteLine($"VisitStr: {context.STRING().GetText()}");
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
            Console.WriteLine($"VisitVarRefLabel: Variable {varName} resolved to value {current}.");
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
