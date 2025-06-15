using System;
using System.Collections;
using System.Collections.Generic;

public class TokenStream : IEnumerable<Token>
{
    private readonly List<Token> tokens;
    private int position;

    public int Position => position;

    public TokenStream(IEnumerable<Token> tokens)
    {
        this.tokens = new List<Token>(tokens);
        this.position = 0;
    }

    public bool End => position >= tokens.Count;

    public void MoveNext(int k) => position = Math.Min(position + k, tokens.Count);

    public void MoveBack(int k) => position = Math.Max(position - k, 0);

    public bool Next()
    {
        if (position < tokens.Count - 1)
            position++;
        return position < tokens.Count;
    }

    public bool Next(TokenType type)
    {
        if (position < tokens.Count - 1 && LookAhead(1).Type == type)
        {
            position++;
            return true;
        }
        return false;
    }

    public bool Next(string value)
    {
        if (position < tokens.Count - 1 && LookAhead(1).Lexeme == value)
        {
            position++;
            return true;
        }
        return false;
    }

    public bool Match(TokenType type)
    {
        if (Peek().Type == type)
        {
            Advance();
            return true;
        }
        return false;
    }

    public Token Advance()
    {
        if (!End) position++;
        return Peek();
    }

    public Token Peek() => LookAhead(0);

    public bool CanLookAhead(int k = 0) => tokens.Count - position > k;

    public Token LookAhead(int k = 0)
    {
        int index = position + k;

        if (index < 0) index = 0;
        if (index >= tokens.Count) index = tokens.Count - 1;

        return tokens[index];
    }

    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Count; i++)
            yield return tokens[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
