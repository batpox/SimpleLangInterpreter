//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from SimpleLang.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="SimpleLangParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface ISimpleLangListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.prog"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProg([NotNull] SimpleLangParser.ProgContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.prog"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProg([NotNull] SimpleLangParser.ProgContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStat([NotNull] SimpleLangParser.StatContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.stat"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStat([NotNull] SimpleLangParser.StatContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.classDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassDecl([NotNull] SimpleLangParser.ClassDeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.classDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassDecl([NotNull] SimpleLangParser.ClassDeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.classVarDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassVarDecl([NotNull] SimpleLangParser.ClassVarDeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.classVarDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassVarDecl([NotNull] SimpleLangParser.ClassVarDeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.varDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarDecl([NotNull] SimpleLangParser.VarDeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.varDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarDecl([NotNull] SimpleLangParser.VarDeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.assign"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssign([NotNull] SimpleLangParser.AssignContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.assign"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssign([NotNull] SimpleLangParser.AssignContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.methodDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodDecl([NotNull] SimpleLangParser.MethodDeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.methodDecl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodDecl([NotNull] SimpleLangParser.MethodDeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] SimpleLangParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] SimpleLangParser.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.dimensions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDimensions([NotNull] SimpleLangParser.DimensionsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.dimensions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDimensions([NotNull] SimpleLangParser.DimensionsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.indexList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIndexList([NotNull] SimpleLangParser.IndexListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.indexList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIndexList([NotNull] SimpleLangParser.IndexListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgList([NotNull] SimpleLangParser.ArgListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgList([NotNull] SimpleLangParser.ArgListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] SimpleLangParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] SimpleLangParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Str</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStr([NotNull] SimpleLangParser.StrContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Str</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStr([NotNull] SimpleLangParser.StrContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>MulDiv</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMulDiv([NotNull] SimpleLangParser.MulDivContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>MulDiv</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMulDiv([NotNull] SimpleLangParser.MulDivContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>AddSub</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAddSub([NotNull] SimpleLangParser.AddSubContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>AddSub</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAddSub([NotNull] SimpleLangParser.AddSubContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Parens</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParens([NotNull] SimpleLangParser.ParensContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Parens</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParens([NotNull] SimpleLangParser.ParensContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Real</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReal([NotNull] SimpleLangParser.RealContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Real</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReal([NotNull] SimpleLangParser.RealContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>VarRefLabel</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarRefLabel([NotNull] SimpleLangParser.VarRefLabelContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>VarRefLabel</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarRefLabel([NotNull] SimpleLangParser.VarRefLabelContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Int</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInt([NotNull] SimpleLangParser.IntContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Int</c>
	/// labeled alternative in <see cref="SimpleLangParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInt([NotNull] SimpleLangParser.IntContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="SimpleLangParser.varReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVarReference([NotNull] SimpleLangParser.VarReferenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="SimpleLangParser.varReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVarReference([NotNull] SimpleLangParser.VarReferenceContext context);
}
