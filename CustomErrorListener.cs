using Antlr4.Runtime;

public class CustomErrorListener : BaseErrorListener
{
    public List<string> Errors { get; } = [];

    
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        Errors.Add($"Line {line}:{charPositionInLine} {msg}");
    }
}

