namespace Iris.Core;

public record GalleryItem
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public string Prompt { get; init; } = "";
    public StylePreset Style { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public int Seed { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string ImageFileName { get; init; } = "";
}
