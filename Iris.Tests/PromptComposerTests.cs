using Iris.Core;

namespace Iris.Tests;

public class PromptComposerTests
{
    [Fact]
    public void None_ReturnsTrimmedPromptUnchanged() =>
        Assert.Equal("a red fox", PromptComposer.Compose("  a red fox  ", StylePreset.None));

    [Fact]
    public void Style_AppendsKeywords() =>
        Assert.Equal("a red fox, cinematic, dramatic lighting, film still, shallow depth of field",
            PromptComposer.Compose("a red fox", StylePreset.Cinematic));

    [Fact]
    public void Null_ReturnsEmpty() =>
        Assert.Equal("", PromptComposer.Compose(null!, StylePreset.None));
}
