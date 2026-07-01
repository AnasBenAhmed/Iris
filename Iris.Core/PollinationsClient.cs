namespace Iris.Core;

public class IrisGenerationException : Exception
{
    public IrisGenerationException(string message) : base(message) { }
}

public class PollinationsClient : IImageClient
{
    private const string Base = "https://image.pollinations.ai/prompt/";
    private readonly HttpClient _http;

    public PollinationsClient(HttpClient? http = null)
        => _http = http ?? new HttpClient { Timeout = TimeSpan.FromSeconds(90) };

    public Uri BuildUrl(GenerationRequest req)
    {
        var finalPrompt = PromptComposer.Compose(req.Prompt, req.Style);
        var (w, h) = AspectPresets.Size(req.Aspect);
        var encoded = Uri.EscapeDataString(finalPrompt);
        return new Uri($"{Base}{encoded}?width={w}&height={h}&seed={req.Seed}&nologo=true&model=sana");
    }

    public async Task<byte[]> GenerateAsync(GenerationRequest req, CancellationToken ct)
    {
        HttpResponseMessage res;
        try
        {
            res = await _http.GetAsync(BuildUrl(req), ct);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            throw new IrisGenerationException("No connection. Check your internet and try again.");
        }

        if (!res.IsSuccessStatusCode)
            throw new IrisGenerationException($"Generation failed ({(int)res.StatusCode}). Try again.");

        var contentType = res.Content.Headers.ContentType?.MediaType ?? "";
        var bytes = await res.Content.ReadAsByteArrayAsync(ct);

        if (!contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase) || bytes.Length == 0)
            throw new IrisGenerationException("Didn't get an image back. Try again.");

        return bytes;
    }
}
