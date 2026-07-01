namespace Iris.Core;

public static class PromptComposer
{
    public static string Compose(string prompt, StylePreset style)
    {
        var p = (prompt ?? "").Trim();
        var keywords = StylePresets.Keywords(style);
        return string.IsNullOrEmpty(keywords) ? p : $"{p}, {keywords}";
    }
}
