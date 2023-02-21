namespace MekLatexTranslationLibrary.PhysicsMode;

internal interface IPhysicsModeParser
{
    TranslationError[]? Errors { get; }
    string? Result { get; }
    string Translate();
}