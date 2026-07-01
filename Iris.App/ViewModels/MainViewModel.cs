using System.Collections.ObjectModel;
using System.IO;
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
        LoadCollections();
    }

    [ObservableProperty] private string _prompt = "";
    [ObservableProperty] private StylePreset _style = StylePreset.None;
    [ObservableProperty] private AspectPreset _aspect = AspectPreset.Square;
    [ObservableProperty] private int _seed = new Random().Next();
    [ObservableProperty] private bool _isGenerating;
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private byte[]? _currentImage;

    public ObservableCollection<GalleryItem> GalleryItems { get; } = new();
    public ObservableCollection<string> HistoryItems { get; } = new();

    public bool CanGenerate => !IsGenerating && !string.IsNullOrWhiteSpace(Prompt);
    private bool CanSave => CurrentImage is not null;

    partial void OnPromptChanged(string value) => GenerateCommand.NotifyCanExecuteChanged();
    partial void OnIsGeneratingChanged(bool value) => GenerateCommand.NotifyCanExecuteChanged();
    partial void OnCurrentImageChanged(byte[]? value) => SaveCommand.NotifyCanExecuteChanged();

    public void LoadCollections()
    {
        GalleryItems.Clear();
        foreach (var i in _gallery.All()) GalleryItems.Add(i);
        HistoryItems.Clear();
        foreach (var p in _history.All()) HistoryItems.Add(p);
    }

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
            LoadCollections();
        }
        catch (OperationCanceledException) { /* back to idle */ }
        catch (IrisGenerationException ex) { ErrorMessage = ex.Message; }
        finally { IsGenerating = false; }
    }

    [RelayCommand]
    private void Cancel() => _cts?.Cancel();

    [RelayCommand]
    private void RandomizeSeed() => Seed = new Random().Next();

    [RelayCommand]
    private void UseHistoryPrompt(string prompt) => Prompt = prompt;

    [RelayCommand]
    private void DeleteGalleryItem(GalleryItem item)
    {
        _gallery.Delete(item.Id);
        GalleryItems.Remove(item);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (CurrentImage is null) return;
        var dlg = new Microsoft.Win32.SaveFileDialog { Filter = "JPEG image|*.jpg", FileName = "iris.jpg" };
        if (dlg.ShowDialog() == true) File.WriteAllBytes(dlg.FileName, CurrentImage);
    }
}
