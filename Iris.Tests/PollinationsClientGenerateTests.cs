using System.Net;
using Iris.Core;

namespace Iris.Tests;

file class StubHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _code;
    private readonly byte[] _body;
    private readonly string _contentType;

    public StubHandler(HttpStatusCode code, byte[] body, string contentType)
        => (_code, _body, _contentType) = (code, body, contentType);

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var res = new HttpResponseMessage(_code) { Content = new ByteArrayContent(_body) };
        res.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);
        return Task.FromResult(res);
    }
}

public class PollinationsClientGenerateTests
{
    private static PollinationsClient Client(HttpMessageHandler h) => new(new HttpClient(h));
    private static GenerationRequest Req() => new("x", StylePreset.None, AspectPreset.Square, 1);

    [Fact]
    public async Task Success_ReturnsBytes()
    {
        var bytes = new byte[] { 1, 2, 3 };
        var client = Client(new StubHandler(HttpStatusCode.OK, bytes, "image/jpeg"));
        var result = await client.GenerateAsync(Req(), CancellationToken.None);
        Assert.Equal(bytes, result);
    }

    [Fact]
    public async Task ServerError_Throws()
    {
        var client = Client(new StubHandler(HttpStatusCode.InternalServerError, Array.Empty<byte>(), "text/plain"));
        await Assert.ThrowsAsync<IrisGenerationException>(() => client.GenerateAsync(Req(), CancellationToken.None));
    }

    [Fact]
    public async Task NonImageBody_Throws()
    {
        var client = Client(new StubHandler(HttpStatusCode.OK, new byte[] { 1 }, "text/html"));
        await Assert.ThrowsAsync<IrisGenerationException>(() => client.GenerateAsync(Req(), CancellationToken.None));
    }
}
