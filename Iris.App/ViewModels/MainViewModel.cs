using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Iris.Core;

namespace Iris.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IImageClient _client;
    private readonly GalleryStore _gallery;
    private readonly HistoryStore _history;
    private CancellationTokenSource? _cts;

    public MainViewModel(IImageClient client, GalleryStore gallery, HistoryStore history)
    {
        _client = client;
        _gallery = gallery;
        _history = history;
    }

    [ObservableProperty] private string _prompt = "";
    [ObservableProperty] private StylePreset _style = StylePreset.None;
    [ObservableProperty] private AspectPreset _aspect = AspectPreset.Square;
    [ObservableProperty] private int _seed = new Random().Next();
    [ObservableProperty] private bool _isGenerating;
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private byte[]? _currentImage;

    public bool CanGenerate => !IsGenerating && !string.IsNullOrWhiteSpace(Prompt);

    partial void OnPromptChanged(string value) => GenerateCommand.NotifyCanExecuteChanged();
    partial void OnIsGeneratingChanged(bool value) => GenerateCommand.NotifyCanExecuteChanged();

    [RelayCommand(CanExecute = nameof(CanGenerate))]
    private async Task Generate()
    {
        ErrorMessage = null;
        IsGenerating = true;
        _cts = new CancellationTokenSource();
        try
        {
            var req = new GenerationRequest(Prompt.Trim(), Style, Aspect, Seed);
            var bytes = await _client.GenerateAsync(req, _cts.Token);
            CurrentImage = bytes;
            _gallery.Add(req, bytes);
            _history.Add(req.Prompt);
        }
        catch (OperationCanceledException) { /* back to idle */ }
        catch (IrisGenerationException ex) { ErrorMessage = ex.Message; }
        finally { IsGenerating = false; }
    }

    [RelayCommand]
    private void Cancel() => _cts?.Cancel();

    [RelayCommand]
    private void RandomizeSeed() => Seed = new Random().Next();
}
