using Iris.Core;

namespace Iris.Tests;

public class HistoryStoreTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "iris-hist-" + Guid.NewGuid().ToString("N"));
    public void Dispose() { if (Directory.Exists(_root)) Directory.Delete(_root, true); }

    [Fact]
    public void Add_MostRecentFirst()
    {
        var h = new HistoryStore(_root);
        h.Add("first");
        h.Add("second");
        Assert.Equal(new[] { "second", "first" }, h.All().ToArray());
    }

    [Fact]
    public void Add_DeDupesCaseInsensitive()
    {
        var h = new HistoryStore(_root);
        h.Add("Cat");
        h.Add("dog");
        h.Add("cat");
        Assert.Equal(new[] { "cat", "dog" }, h.All().ToArray());
    }

    [Fact]
    public void Add_CapsAt30()
    {
        var h = new HistoryStore(_root);
        for (var i = 0; i < 40; i++) h.Add("p" + i);
        Assert.Equal(30, h.All().Count);
        Assert.Equal("p39", h.All()[0]);
    }

    [Fact]
    public void Add_IgnoresBlank()
    {
        var h = new HistoryStore(_root);
        h.Add("   ");
        Assert.Empty(h.All());
    }
}
