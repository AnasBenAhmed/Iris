using Iris.Core;

namespace Iris.Tests;

public class PollinationsClientBuildUrlTests
{
    private readonly PollinationsClient _client = new();

    [Fact]
    public void BuildUrl_EncodesPrompt_AndAddsParams()
    {
        var req = new GenerationRequest("a red fox", StylePreset.None, AspectPreset.Square, 42);
        var url = _client.BuildUrl(req).ToString();

        Assert.StartsWith("https://image.pollinations.ai/prompt/", url);
        Assert.Contains("a%20red%20fox", url);
        Assert.Contains("width=1024", url);
        Assert.Contains("height=1024", url);
        Assert.Contains("seed=42", url);
        Assert.Contains("nologo=true", url);
        Assert.Contains("model=sana", url);
    }

    [Fact]
    public void BuildUrl_PortraitUsesPortraitSize()
    {
        var req = new GenerationRequest("x", StylePreset.None, AspectPreset.Portrait, 1);
        var url = _client.BuildUrl(req).ToString();
        Assert.Contains("width=768", url);
        Assert.Contains("height=1024", url);
    }

    [Fact]
    public void BuildUrl_IncludesStyleKeywords()
    {
        var req = new GenerationRequest("cat", StylePreset.Anime, AspectPreset.Square, 7);
        var url = Uri.UnescapeDataString(_client.BuildUrl(req).ToString());
        Assert.Contains("anime style", url);
    }
}
