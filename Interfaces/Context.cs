using System;
using System.Collections.Generic;

public class SemanticContext
{
    public HashSet<string> DefinedVariables { get; } = new();
    public Dictionary<string, bool> VariableTypes { get; } = new(); // true = numérico, false = booleano

    public HashSet<string> Labels { get; } = new();
    public bool SpawnCalled { get; set; } = false;

    public void DefineVariable(string name, bool isNumeric)
    {
        if (!DefinedVariables.Contains(name))
        {
            DefinedVariables.Add(name);
            VariableTypes[name] = isNumeric;
        }
    }

    public bool IsVariableDefined(string name) => DefinedVariables.Contains(name);

    public bool IsVariableNumeric(string name) =>
        VariableTypes.TryGetValue(name, out bool isNumeric) && isNumeric;

    public void DefineLabel(string name) => Labels.Add(name);

    public bool IsLabelDefined(string name) => Labels.Contains(name);
}
