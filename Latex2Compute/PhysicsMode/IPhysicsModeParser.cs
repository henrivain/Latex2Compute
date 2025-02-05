namespace Latex2Compute.PhysicsMode;

internal interface IPhysicsModeParser
{
    Errors Errors { get; }
    string? Result { get; }
    string Translate();
}