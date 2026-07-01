namespace Iris.Core;

public enum StylePreset { None, Photorealistic, Cinematic, Anime, ConceptArt, Render3D }

public static class StylePresets
{
    public static string Keywords(StylePreset style) => style switch
    {
        StylePreset.Photorealistic => "photorealistic, ultra-detailed, sharp focus, 8k",
        StylePreset.Cinematic      => "cinematic, dramatic lighting, film still, shallow depth of field",
        StylePreset.Anime          => "anime style, cel shaded, vibrant, studio anime",
        StylePreset.ConceptArt     => "concept art, digital painting, ArtStation, highly detailed",
        StylePreset.Render3D       => "3D render, octane render, CGI, volumetric lighting",
        _ => ""
    };
}
