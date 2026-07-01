using System.Text.Json;

namespace Iris.Core;

public class HistoryStore
{
    private const int Cap = 30;
    private readonly string _path;

    public HistoryStore(string root)
    {
        Directory.CreateDirectory(root);
        _path = Path.Combine(root, "history.json");
    }

    public List<string> All()
    {
        if (!File.Exists(_path)) return new();
        try { return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(_path)) ?? new(); }
        catch { return new(); }
    }

    public void Add(string prompt)
    {
        var p = (prompt ?? "").Trim();
        if (p.Length == 0) return;
        var list = All();
        list.RemoveAll(x => string.Equals(x, p, StringComparison.OrdinalIgnoreCase));
        list.Insert(0, p);
        if (list.Count > Cap) list = list.Take(Cap).ToList();
        File.WriteAllText(_path, JsonSerializer.Serialize(list));
    }
}
