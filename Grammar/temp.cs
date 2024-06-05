using System;
using System.Collections.Generic;
using Generated;

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

    public Dictionary<string, Variable> Variables => variables;

    public override object VisitProg(SimpleLangParser.ProgContext context)
    {
        foreach (var statement in context.stat())
        {
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
        Console.WriteLine($"Class {className} defined.");
        return null;
    }

    public override object VisitVarDecl(SimpleLangParser.VarDeclContext context)
    {
        string varType = context.type().GetText();
        string varName = context.ID().GetText();
        object varValue = null;

        if (context.expr() != null)
        {
            varValue = Visit(context.expr());
        }

        if (classes.ContainsKey(varType))
        {
            var classInstance = new Dictionary<string, Variable>();
            foreach (var member in classes[varType].Members)
            {
                classInstance[member.Key] = new Variable(member.Value.Name, member.Value.Type, member.Value.Value);
            }
            variables[varName] = new Variable(varName, varType, classInstance);
        }
        else
        {
            switch (varType)
            {
                case "int":
                    if (varValue == null) varValue = 0;
                    variables[varName] = new Variable(varName, varType, Convert.ToInt32(varValue));
                    break;
                case "real":
                    if (varValue == null) varValue = 0.0;
                    variables[varName] = new Variable(varName, varType, Convert.ToDouble(varValue));
                    break;
                case "string":
                    if (varValue == null) varValue = string.Empty;
                    variables[varName] = new Variable(varName, varType, varValue.ToString());
                    break;
            }
        }

        Console.WriteLine($"Variable {varName} of type {varType} declared with value {varValue}");
        return null;
    }

    public override object VisitAssign(SimpleLangParser.AssignContext context)
    {
        var varRefContext = context.varRef();
        string varName = varRefContext.ID(0).GetText();
        object varValue = Visit(context.expr());

        if (varRefContext.ID().Length > 1)
        {
            // Handle member access
            var current = variables[varName].Value;
            for (int i = 1; i < varRefContext.ID().Length - 1; i++)
            {
                var memberName = varRefContext.ID(i).GetText();
                current = ((Dictionary<string, Variable>)current)[memberName].Value;
            }

            string finalMemberName = varRefContext.ID(varRefContext.ID().Length - 1).GetText();
            var classInstance = (Dictionary<string, Variable>)current;
            classInstance[finalMemberName].Value = varValue;
            Console.WriteLine($"Assigned value {varValue} to {varName}.{finalMemberName}");
        }
        else
        {
            // Handle simple variable
            if (variables.ContainsKey(varName))
            {
                switch (variables[varName].Type)
                {
                    case "int":
                        variables[varName].Value = Convert.ToInt32(varValue);
                        break;
                    case "real":
                        variables[varName].Value = Convert.ToDouble(varValue);
                        break;
                    case "string":
                        variables[varName].Value = varValue.ToString();
                        break;
                }

                Console.WriteLine($"Assigned value {varValue} to variable {varName}");
            }
            else
            {
                throw new Exception($"Undefined variable: {varName}");
            }
        }

        return null;
    }

    public override object VisitExpr(SimpleLangParser.ExprContext context)
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
        else if (context.ChildCount == 3) // Binary operation or parentheses
        {
            if (context.GetChild(0).GetText() == "(")
            {
                // Handle parentheses
                return Visit(context.expr(0));
            }

            var left = Visit(context.GetChild(0));
            var op = context.GetChild(1).GetText();
            var right = Visit(context.GetChild(2));

            if (left is Variable leftVar)
            {
                left = leftVar.Value;
            }

            if (right is Variable rightVar)
            {
                right = rightVar.Value;
            }

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
        string varName = context.ID(0).GetText();
        if (variables.ContainsKey(varName))
        {
            var current = variables[varName].Value;
            for (int i = 1; i < context.ID().Length; i++)
            {
                string memberName = context.ID(i).GetText();
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
            throw new Exception($"Undefined variable: {varNameTo enable class types that can contain members of other class types in your SimpleLang language, let's modify the visitor code to handle nested class types properly.

### Update the Visitor (continued)

We'll ensure the visitor code can handle nested member access and variable assignments correctly.

```csharp
using System;
using System.Collections.Generic;
using Generated;

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

    public Dictionary<string, Variable> Variables => variables;

    public override object VisitProg(SimpleLangParser.ProgContext context)
    {
        foreach (var statement in context.stat())
        {
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
        Console.WriteLine($"Class {className} defined.");
        return null;
    }

    public override object VisitVarDecl(SimpleLangParser.VarDeclContext context)
    {
        string varType = context.type().GetText();
        string varName = context.ID().GetText();
        object varValue = null;

        if (context.expr() != null)
        {
            varValue = Visit(context.expr());
        }

        if (classes.ContainsKey(varType))
        {
            var classInstance = new Dictionary<string, Variable>();
            foreach (var member in classes[varType].Members)
            {
                classInstance[member.Key] = new Variable(member.Value.Name, member.Value.Type, member.Value.Value);
            }
            variables[varName] = new Variable(varName, varType, classInstance);
        }
        else
        {
            switch (varType)
            {
                case "int":
                    if (varValue == null) varValue = 0;
                    variables[varName] = new Variable(varName, varType, Convert.ToInt32(varValue));
                    break;
                case "real":
                    if (varValue == null) varValue = 0.0;
                    variables[varName] = new Variable(varName, varType, Convert.ToDouble(varValue));
                    break;
                case "string":
                    if (varValue == null) varValue = string.Empty;
                    variables[varName] = new Variable(varName, varType, varValue.ToString());
                    break;
            }
        }

        Console.WriteLine($"Variable {varName} of type {varType} declared with value {varValue}");
        return null;
    }

    public override object VisitAssign(SimpleLangParser.AssignContext context)
    {
        var varRefContext = context.varRef();
        string varName = varRefContext.ID(0).GetText();
        object varValue = Visit(context.expr());

        if (varRefContext.ID().Length > 1)
        {
            // Handle member access
            var current = variables[varName].Value;
            for (int i = 1; i < varRefContext.ID().Length - 1; i++)
            {
                var memberName = varRefContext.ID(i).GetText();
                current = ((Dictionary<string, Variable>)current)[memberName].Value;
            }

            string finalMemberName = varRefContext.ID(varRefContext.ID().Length - 1).GetText();
            var classInstance = (Dictionary<string, Variable>)current;
            classInstance[finalMemberName].Value = varValue;
            Console.WriteLine($"Assigned value {varValue} to {varName}.{finalMemberName}");
        }
        else
        {
            // Handle simple variable
            if (variables.ContainsKey(varName))
            {
                switch (variables[varName].Type)
                {
                    case "int":
                        variables[varName].Value = Convert.ToInt32(varValue);
                        break;
                    case "real":
                        variables[varName].Value = Convert.ToDouble(varValue);
                        break;
                    case "string":
                        variables[varName].Value = varValue.ToString();
                        break;
                }

                Console.WriteLine($"Assigned value {varValue} to variable {varName}");
            }
            else
            {
                throw new Exception($"Undefined variable: {varName}");
            }
        }

        return null;
    }

    public override object VisitExpr(SimpleLangParser.ExprContext context)
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
        else if (context.ChildCount == 3) // Binary operation or parentheses
        {
            if (context.GetChild(0).GetText() == "(")
            {
                // Handle parentheses
                return Visit(context.expr(0));
            }

            var left = Visit(context.GetChild(0));
            var op = context.GetChild(1).GetText();
            var right = Visit(context.GetChild(2));

            if (left is Variable leftVar)
            {
                left = leftVar.Value;
            }

            if (right is Variable rightVar)
            {
                right = rightVar.Value;
            }

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
        string varName = context.ID(0).GetText();
        if (variables.ContainsKey(varName))
        {
            var current = variables[varName].Value;
            for (int i = 1; i < context.ID().Length; i++)
            {
                string memberName = context.ID(i).GetText();
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
}
