public class CompilingError
    {
        public ErrorCode Code { get; private set; }

        public ErrorStage Stage { get; }

        public string Argument { get; private set; }

        public int Line {get; private set;}

        public CompilingError(int line, ErrorCode code, ErrorStage errorStage, string argument)
        {
            this.Code = code;
            this.Argument = argument;
            Stage=errorStage;
            Line = line;
            
        }

        public override string ToString()
        {
            return $"[Línea {Line}], [{Stage} Error], {Code}: {Argument}";
        }
    }

    public enum ErrorCode
    {
        None,
        Expected,
        Invalid,
        Unknown,

        
    }

    public enum ErrorStage
    {
        Lexical,
        Syntactic,
        Semantic,
        Runtime
    }