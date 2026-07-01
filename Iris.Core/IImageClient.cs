namespace Iris.Core;

public interface IImageClient
{
    Uri BuildUrl(GenerationRequest req);
    Task<byte[]> GenerateAsync(GenerationRequest req, CancellationToken ct);
}
