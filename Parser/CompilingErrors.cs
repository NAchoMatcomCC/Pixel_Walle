public class CompilingError
    {
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

        public int Line {get; private set;}

        public CompilingError(int line, ErrorCode code, string argument)
        {
            this.Code = code;
            this.Argument = argument;
            Line = line;
        }

        public override string ToString()
        {
            return $"[Línea {Line}] {Code}: {Argument}";
        }
    }

    public enum ErrorCode
    {
        None,
        Expected,
        Invalid,
        Unknown,
    }