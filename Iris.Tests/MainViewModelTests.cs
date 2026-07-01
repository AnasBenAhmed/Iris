using Iris.App.ViewModels;
using Iris.Core;

namespace Iris.Tests;

file class FakeClient : IImageClient
{
    public byte[]? Bytes;
    public Exception? Throw;
    public Uri BuildUrl(GenerationRequest req) => new("https://x/");
    public Task<byte[]> GenerateAsync(GenerationRequest req, CancellationToken ct)
        => Throw is not null ? Task.FromException<byte[]>(Throw) : Task.FromResult(Bytes ?? Array.Empty<byte>());
}

public class MainViewModelTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "iris-vm-" + Guid.NewGuid().ToString("N"));
    public void Dispose() { if (Directory.Exists(_root)) Directory.Delete(_root, true); }

    private MainViewModel Vm(IImageClient client) =>
        new(client, new GalleryStore(_root), new HistoryStore(_root)) { Prompt = "a fox" };

    [Fact]
    public async Task Generate_Success_SetsImage_SavesGalleryAndHistory()
    {
        var vm = Vm(new FakeClient { Bytes = new byte[] { 1, 2, 3 } });
        await vm.GenerateCommand.ExecuteAsync(null);

        Assert.Equal(new byte[] { 1, 2, 3 }, vm.CurrentImage);
        Assert.False(vm.IsGenerating);
        Assert.Null(vm.ErrorMessage);
        Assert.Single(new GalleryStore(_root).All());
        Assert.Contains("a fox", new HistoryStore(_root).All());
    }

    [Fact]
    public async Task Generate_Error_SetsErrorMessage()
    {
        var vm = Vm(new FakeClient { Throw = new IrisGenerationException("boom") });
        await vm.GenerateCommand.ExecuteAsync(null);

        Assert.Equal("boom", vm.ErrorMessage);
        Assert.Null(vm.CurrentImage);
    }

    [Fact]
    public void CanGenerate_FalseWhenPromptEmpty()
    {
        var vm = new MainViewModel(new FakeClient(), new GalleryStore(_root), new HistoryStore(_root));
        vm.Prompt = "   ";
        Assert.False(vm.CanGenerate);
    }

    [Fact]
    public async Task Generate_RefreshesGalleryItems()
    {
        var vm = Vm(new FakeClient { Bytes = new byte[] { 1 } });
        await vm.GenerateCommand.ExecuteAsync(null);
        Assert.Single(vm.GalleryItems);
    }

    [Fact]
    public void UseHistoryPrompt_SetsPrompt()
    {
        var vm = new MainViewModel(new FakeClient(), new GalleryStore(_root), new HistoryStore(_root));
        vm.UseHistoryPromptCommand.Execute("a cat");
        Assert.Equal("a cat", vm.Prompt);
    }
}
