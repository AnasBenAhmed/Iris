using Iris.Core;

namespace Iris.Tests;

public class ModelsTests
{
    [Fact]
    public void Keywords_None_IsEmpty() =>
        Assert.Equal("", StylePresets.Keywords(StylePreset.None));

    [Fact]
    public void Keywords_Cinematic_HasExpectedSuffix() =>
        Assert.Equal("cinematic, dramatic lighting, film still, shallow depth of field",
            StylePresets.Keywords(StylePreset.Cinematic));

    [Theory]
    [InlineData(AspectPreset.Square, 1024, 1024)]
    [InlineData(AspectPreset.Portrait, 768, 1024)]
    [InlineData(AspectPreset.Landscape, 1024, 768)]
    public void Size_MapsCorrectly(AspectPreset a, int w, int h)
    {
        var (width, height) = AspectPresets.Size(a);
        Assert.Equal(w, width);
        Assert.Equal(h, height);
    }
}
