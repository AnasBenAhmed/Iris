namespace Iris.Core;

public record GenerationRequest(string Prompt, StylePreset Style, AspectPreset Aspect, int Seed);
