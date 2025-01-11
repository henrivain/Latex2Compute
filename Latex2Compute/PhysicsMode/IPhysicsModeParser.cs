namespace Latex2Compute.PhysicsMode;

internal interface IPhysicsModeParser
{
    TranslationErrors Errors { get; }
    string? Result { get; }
    string Translate();
}