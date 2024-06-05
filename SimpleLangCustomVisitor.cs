using Antlr4.Runtime.Misc;

public class SimpleLangVisitor : SimpleLangBaseVisitor<object>
{
    public override object VisitMulDiv([NotNull] SimpleLangParser.MulDivContext context)
    {
        // Your logic for handling MulDiv
        return base.VisitMulDiv(context);
    }

    public override object VisitAddSub([NotNull] SimpleLangParser.AddSubContext context)
    {
        // Your logic for handling AddSub
        return base.VisitAddSub(context);
    }

    public override object VisitInt([NotNull] SimpleLangParser.IntContext context)
    {
        // Your logic for handling Int
        return base.VisitInt(context);
    }

    public override object VisitReal([NotNull] SimpleLangParser.RealContext context)
    {
        // Your logic for handling Real
        return base.VisitReal(context);
    }

    public override object VisitStr([NotNull] SimpleLangParser.StrContext context)
    {
        // Your logic for handling Str
        return base.VisitStr(context);
    }

    public override object VisitVarRefLabel([NotNull] SimpleLangParser.VarRefLabelContext context)
    {
        // Your logic for handling VarRefLabel
        return base.VisitVarRefLabel(context);
    }

    public override object VisitParens([NotNull] SimpleLangParser.ParensContext context)
    {
        // Your logic for handling Parens
        return base.VisitParens(context);
    }
}
