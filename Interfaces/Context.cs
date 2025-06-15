using System;
using System.Collections.Generic;

public class SemanticContext
{
    // Guarda el tipo de cada variable: true = numérico, false = booleano
    public Dictionary<string, bool> VariableTypes { get; } = new();

    // Guarda etiquetas definidas
    public HashSet<string> Labels { get; } = new();

    // Bandera para verificar si Spawn fue llamado
    public bool SpawnCalled { get; set; } = false;

    public void DefineVariable(string name, bool isNumeric)
    {
        if (!VariableTypes.ContainsKey(name))
            VariableTypes[name] = isNumeric;
    }

    public bool IsVariableDefined(string name) => VariableTypes.ContainsKey(name);

    public bool IsVariableNumeric(string name) =>
        VariableTypes.TryGetValue(name, out bool isNumeric) && isNumeric;

    public void DefineLabel(string name) => Labels.Add(name);

    public bool IsLabelDefined(string name) => Labels.Contains(name);
}
