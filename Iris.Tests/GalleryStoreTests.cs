using Iris.Core;

namespace Iris.Tests;

public class GalleryStoreTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "iris-test-" + Guid.NewGuid().ToString("N"));
    public void Dispose() { if (Directory.Exists(_root)) Directory.Delete(_root, true); }

    private static GenerationRequest Req() => new("a fox", StylePreset.Cinematic, AspectPreset.Portrait, 5);

    [Fact]
    public void All_EmptyWhenNoFile() => Assert.Empty(new GalleryStore(_root).All());

    [Fact]
    public void Add_PersistsItemAndImage()
    {
        var store = new GalleryStore(_root);
        var item = store.Add(Req(), new byte[] { 9, 9, 9 });

        Assert.True(File.Exists(store.ImagePath(item)));
        Assert.Equal(768, item.Width);
        Assert.Equal(1024, item.Height);

        var all = new GalleryStore(_root).All();
        Assert.Single(all);
        Assert.Equal("a fox", all[0].Prompt);
    }

    [Fact]
    public void All_MostRecentFirst()
    {
        var store = new GalleryStore(_root);
        store.Add(new GenerationRequest("first", StylePreset.None, AspectPreset.Square, 1), new byte[] { 1 });
        Thread.Sleep(5);
        store.Add(new GenerationRequest("second", StylePreset.None, AspectPreset.Square, 2), new byte[] { 2 });
        Assert.Equal("second", store.All()[0].Prompt);
    }

    [Fact]
    public void Delete_RemovesEntryAndFile()
    {
        var store = new GalleryStore(_root);
        var item = store.Add(Req(), new byte[] { 1 });
        store.Delete(item.Id);
        Assert.Empty(store.All());
        Assert.False(File.Exists(store.ImagePath(item)));
    }
}
