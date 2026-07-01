namespace Iris.Core;

public enum AspectPreset { Square, Portrait, Landscape }

public static class AspectPresets
{
    public static (int Width, int Height) Size(AspectPreset aspect) => aspect switch
    {
        AspectPreset.Portrait  => (768, 1024),
        AspectPreset.Landscape => (1024, 768),
        _                      => (1024, 1024)
    };
}
