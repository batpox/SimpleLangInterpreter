using System;
using System.Collections.Generic;
using System.Security.AccessControl;


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

public class SimpleLangCustomVisitor : SimpleLangBaseVisitor<object>
{
    private Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    private Dictionary<string, Dictionary<string, string>> classes = new Dictionary<string, Dictionary<string, string>>();

    
    /// <summary>
    /// Public dictionary of variable-name, variable object
    /// </summary>
    public Dictionary<string, Variable> Variables => variables;

    public override object? VisitProg(SimpleLangParser.ProgContext context)
    {

        foreach (var statement in context.stat())
        {
            Console.WriteLine($"Statement Start={statement.Start}");
            Visit(statement);
        }
        return null;
    }
    public override object? VisitClassDecl(SimpleLangParser.ClassDeclContext context)
    {
        string className = context.ID().GetText();
        var classFields = new Dictionary<string, string>();

        foreach (var classVarDecl in context.classVarDecl())
        {
            string varType = classVarDecl.type().GetText();
            string varName = classVarDecl.ID().GetText();
            classFields[varName] = varType;
        }

        classes[className] = classFields;
        return null;
    }

    public override object? VisitVarDecl(SimpleLangParser.VarDeclContext context)
    {
        string varType = context.type().GetText();
        string varName = context.ID().GetText();
        object? varValue = null;

        if (context.expr() != null)
        {
            varValue = Visit(context.expr());
        }

        if (varType == "int")
        {
            if (varValue == null) 
                varValue = 0;
            variables[varName] = new Variable(varName, varType, Convert.ToInt32(varValue));
        }
        else if (varType == "real")
        {
            if (varValue == null) 
                varValue = 0.0;
            variables[varName] = new Variable(varName, varType, Convert.ToDouble(varValue));
        }
        else if (varType == "string")
        {
            if (varValue == null) varValue = string.Empty;
            variables[varName] = new Variable(varName, varType, varValue.ToString());
        }
        else if (classes.ContainsKey(varType)) // Class type
        {
            var classInstance = new Dictionary<string, Variable>();
            foreach (var field in classes[varType])
            {
                classInstance[field.Key] = new Variable(field.Key, field.Value, null);
            }
            variables[varName] = new Variable(varName, varType, classInstance);
        }

        return null;
    }

    public override object? VisitAssign(SimpleLangParser.AssignContext context)
    {
        object varRef = Visit(context.varRef());
        object varValue = Visit(context.expr());

        if (varRef is Variable variable)
        {
            if (variable.Type == "int")
            {
                variable.Value = Convert.ToInt32(varValue);
            }
            else if (variable.Type == "real")
            {
                variable.Value = Convert.ToDouble(varValue);
            }
            else if (variable.Type == "string")
            {
                variable.Value = varValue.ToString();
            }
            else if (variable.Value is Dictionary<string, Variable> classInstance)
            {
                // Handle assignment to class field
                if (varValue is Variable fieldVariable)
                {
                    classInstance[fieldVariable.Name].Value = fieldVariable.Value;
                }
                else
                {
                    throw new Exception($"Incompatible value type for class field: {context.varRef().GetText()}");
                }
            }
        }
        else
        {
            throw new Exception($"Invalid variable reference in assignment: {context.varRef().GetText()}");
        }

        return null;
    }

    public override object? VisitExpr(SimpleLangParser.ExprContext context)
    {
        if (context.ChildCount == 1) // Single literal or ID
        {
            if (context.INT() != null)
            {
                return int.Parse(context.INT().GetText());
            }
            else if (context.REAL() != null)
            {
                return double.Parse(context.REAL().GetText());
            }
            else if (context.STRING() != null)
            {
                return context.STRING().GetText().Trim('"');
            }
            else if (context.varRef() != null)
            {
                return Visit(context.varRef());
            }
        }
        else if (context.ChildCount == 3) // Binary operation
        {
            if ( context.GetChild(0).GetText() == "(")
            {
                return Visit(context.expr(0));
            }

            var left = Visit(context.GetChild(0));
            if ( left is Variable leftVar)
                left = leftVar.Value;

            var op = context.GetChild(1).GetText();

            var right = Visit(context.GetChild(2));
            if ( right is Variable rightVar)
                right = rightVar.Value;

            if (left is int leftInt && right is int rightInt)
            {
                return EvaluateBinaryOperation(leftInt, rightInt, op);
            }
            else if (left is double leftDouble && right is double rightDouble)
            {
                return EvaluateBinaryOperation(leftDouble, rightDouble, op);
            }
            else if (left is int leftIntMixed && right is double rightDoubleMixed)
            {
                return EvaluateBinaryOperation((double)leftIntMixed, rightDoubleMixed, op);
            }
            else if (left is double leftDoubleMixed && right is int rightIntMixed)
            {
                return EvaluateBinaryOperation(leftDoubleMixed, (double)rightIntMixed, op);
            }
        }
        return base.VisitExpr(context);
    }

    private object EvaluateBinaryOperation(double left, double right, string op)
    {
        switch (op)
        {
            case "+":
                return left + right;
            case "-":
                return left - right;
            case "*":
                return left * right;
            case "/":
                if (right == 0)
                    throw new DivideByZeroException();
                return left / right;
            default:
                throw new InvalidOperationException($"Unknown operator: {op}");
        }
    }
    public override object VisitVarRef(SimpleLangParser.VarRefContext context)
    {
        string rootVarName = context.ID(0).GetText();
        if ( !variables.TryGetValue(rootVarName, out Variable? currentVar))
        {
            throw new Exception($"Undefined variable: {rootVarName}");
        }

        for (int i = 1; i < context.ID().Length; i++)
        {
            string fieldName = context.ID(i).GetText();

            if (currentVar.Value is Dictionary<string, Variable> classInstance)
            {
                if (!classInstance.TryGetValue(fieldName, out Variable? foundVar))
                {
                    throw new Exception($"Undefined field: {fieldName} in variable {currentVar.Name}");
                }
                currentVar = foundVar;
            }
            else
            {
                throw new Exception($"Variable {currentVar.Name} does not contain fields.");
            }
        }

        return currentVar;
    }
}
