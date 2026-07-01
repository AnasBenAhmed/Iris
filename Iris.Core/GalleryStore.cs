using System.Text.Json;
using System.Text.Json.Serialization;

namespace Iris.Core;

public class GalleryStore
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _imagesDir;
    private readonly string _jsonPath;

    public GalleryStore(string root)
    {
        _imagesDir = Path.Combine(root, "images");
        _jsonPath = Path.Combine(root, "gallery.json");
        Directory.CreateDirectory(_imagesDir);
    }

    public static string DefaultRoot() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Iris");

    public string ImagePath(GalleryItem item) => Path.Combine(_imagesDir, item.ImageFileName);

    public List<GalleryItem> All() =>
        Load().OrderByDescending(i => i.CreatedAt).ToList();

    public GalleryItem Add(GenerationRequest req, byte[] imageBytes)
    {
        var (w, h) = AspectPresets.Size(req.Aspect);
        var item = new GalleryItem
        {
            Prompt = req.Prompt, Style = req.Style, Width = w, Height = h, Seed = req.Seed
        };
        item = item with { ImageFileName = item.Id + ".jpg" };

        File.WriteAllBytes(Path.Combine(_imagesDir, item.ImageFileName), imageBytes);
        var list = Load();
        list.Add(item);
        Save(list);
        return item;
    }

    public void Delete(string id)
    {
        var list = Load();
        var item = list.FirstOrDefault(i => i.Id == id);
        if (item is null) return;
        list.Remove(item);
        Save(list);
        var path = Path.Combine(_imagesDir, item.ImageFileName);
        if (File.Exists(path)) File.Delete(path);
    }

    private List<GalleryItem> Load()
    {
        if (!File.Exists(_jsonPath)) return new();
        try { return JsonSerializer.Deserialize<List<GalleryItem>>(File.ReadAllText(_jsonPath), Json) ?? new(); }
        catch { return new(); }
    }

    private void Save(List<GalleryItem> list) =>
        File.WriteAllText(_jsonPath, JsonSerializer.Serialize(list, Json));
}
