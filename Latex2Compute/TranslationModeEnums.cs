/// Copyright 2024 Henri Vainio 

namespace MekLatexTranslationLibrary;

/// <summary>
/// Enum defining how to handle units in translation.
/// </summary>
public enum UnitTranslationMode
{
    /// <summary>
    /// [Default] Old math mode, only remove greek symbols.
    /// </summary>
    None = 0,

    /// <summary>
    /// Old Physics mode 1, remove all units.
    /// </summary>
    Remove = 1,

    /// <summary>
    /// Old physics mode 2, translate all units.
    /// </summary>
    Translate = 2
}

/// <summary>
/// Enum defining any cleanup actions to be done to the output.
/// </summary>
public enum EndChanges
{
    /// <summary>
    /// No actions to the output.
    /// </summary>
    None = 0,

    /// <summary>
    /// Make <see cref="EmptyFrac"/> changes and remove "…" symbols.
    /// </summary>
    All = 1,

    /// <summary>
    /// Remove all empty fractions without meaning from 
    /// the output like ()/() or (^2)/() or ^2/^2.
    /// </summary>
    EmptyFrac = 2
}
