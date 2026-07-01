# Iris — Design Spec

**Date:** 2026-07-01
**Author:** Anas Ben Ahmed
**Status:** Approved — ready for implementation plan

---

## 1. What it is

**Iris** is a native Windows desktop app (C# / WPF / .NET) that turns a text
prompt into a real AI-generated image. Type a prompt, pick a style, hit
Generate, get an image — saved automatically to a local gallery. Dark UI in
Anas's crimson/gold brand. Project #5 in the portfolio.

## 2. Goals & non-goals

**Goals**
- Real, high-quality AI image generation from a text prompt.
- Works **out of the box** — no account, no API key, no billing.
- Style presets, aspect presets, seed control, a local gallery, prompt history.
- Native WPF (no Electron), MVVM, dark brand UI.
- Ships with an xUnit test suite over the pure logic (per project rule).

**Non-goals**
- No API-key management / Windows Credential Manager (the chosen backend is
  keyless — dropped from the old portfolio text).
- No negative prompts (the model doesn't use them meaningfully — dropped).
- No in-app image editing, upscaling, or inpainting (v1).

## 3. Image backend — Pollinations (verified)

Keyless HTTP image generation via **Pollinations.ai**.

```
GET https://image.pollinations.ai/prompt/{urlEncodedPrompt}
      ?width={w}&height={h}&seed={seed}&nologo=true&model=sana
```

- Returns raw `image/jpeg` bytes.
- **Verified 2026-07-01:** HTTP 200, real 512×512 photorealistic image; the free
  tier currently exposes a single model, **`sana`** (fast NVIDIA text-to-image).
- No key, no account, no rate-limit gate on the anonymous GET.
- Because there is one model, **style = prompt-keyword suffixes** (not model
  switching).

**Caveats (handled, not hidden)**
- A generation is a single blocking GET (~5–20 s) → **indeterminate** progress
  (spinner), not a percentage. Cancel = abort the request.
- Non-image / error response or timeout → surfaced as an inline error + Retry.

## 4. Architecture — 3-project solution

```
Iris.sln
  Iris.Core   (classlib, net8.0)          — all logic, no UI  → unit-tested
  Iris.App    (WPF, net8.0-windows)        — dark UI, MVVM
  Iris.Tests  (xUnit, net8.0)              — tests Iris.Core
```

MVVM via **CommunityToolkit.Mvvm** (source-generated `ObservableObject` /
`RelayCommand`). Keeping all logic in `Iris.Core` (no WPF references) is what
makes the app testable.

### Iris.Core

- **`PollinationsClient`**
  - `Uri BuildUrl(GenerationRequest req)` — pure, testable: URL-encodes the
    prompt, appends width/height/seed/`nologo=true`/`model=sana`.
  - `Task<byte[]> GenerateAsync(GenerationRequest req, CancellationToken ct)` —
    fetches image bytes via a shared `HttpClient`; throws `IrisGenerationException`
    (safe message) on non-200 / non-image / network failure.
- **`PromptComposer`** — `string Compose(string prompt, StylePreset style)`:
  appends the style's keyword suffix; `None` → prompt unchanged. Pure/testable.
- **`StylePreset`** enum + keyword map (suffix appended to the prompt):
  - `None` → *(nothing)*
  - `Photorealistic` → "photorealistic, ultra-detailed, sharp focus, 8k"
  - `Cinematic` → "cinematic, dramatic lighting, film still, shallow depth of field"
  - `Anime` → "anime style, cel shaded, vibrant, studio anime"
  - `ConceptArt` → "concept art, digital painting, ArtStation, highly detailed"
  - `Render3D` → "3D render, octane render, CGI, volumetric lighting"
- **`AspectPreset`** enum + size map: Square 1024×1024 · Portrait 768×1024 ·
  Landscape 1024×768.
- **`GenerationRequest`** — Prompt, StylePreset, AspectPreset, Seed (int).
- **`GalleryItem`** — Id (guid), Prompt, Style, Width, Height, Seed, CreatedAt,
  ImageFileName.
- **`GalleryStore`** — persists to `%APPDATA%/Iris/`:
  `images/{id}.jpg` + `gallery.json`. `Add`, `All`, `Delete`, `Load`. JSON via
  `System.Text.Json`. Pure serialization is testable with a temp dir.
- **`HistoryStore`** — recent prompts in `history.json`: most-recent-first,
  de-duplicated, capped (30). Testable.

### Iris.App (WPF, MVVM)

- **`MainWindow`** — two-panel layout:
  - **Left (controls):** multiline prompt box · style-preset chips · aspect
    toggle (Square/Portrait/Landscape) · seed field with 🎲 randomize ·
    **Generate** (gold gradient) / **Cancel**.
  - **Right (result):** the generated image, with **Save / Export / Copy**;
    a progress overlay (spinner) while generating; an inline error + Retry state.
  - **Gallery** view — responsive grid of past generations; click to open,
    re-use its prompt/settings, export, or delete.
  - **Prompt history** — quick chips/dropdown to re-run a recent prompt.
- **ViewModels:** `MainViewModel` (generate flow, progress, cancel, error),
  `GalleryViewModel` (list/open/delete).
- **Theme:** `Theme.xaml` ResourceDictionary — `#0D0D0D` base, surfaces
  `#151515`/`#1C1C1C`, crimson `#E11B22`, gold `#E0A82E`, crimson→gold gradient
  for the Generate button. Matches Cambio/VANTA.

## 5. Data flow

```
prompt + style + aspect + seed
  → MainViewModel.GenerateCommand
  → PromptComposer.Compose(prompt, style)
  → PollinationsClient.GenerateAsync(request, ct)
  → image bytes → display in result panel
  → GalleryStore.Add(item + bytes)  (auto-save)
  → gallery + history refresh
Cancel → ct.Cancel() → back to idle
```

## 6. Error handling

| Case | Behaviour |
|------|-----------|
| Empty prompt | Generate disabled |
| Network / timeout / non-200 | Inline error card + **Retry** |
| Non-image body | Same error path (safe message) |
| User cancel | Return to idle, no error |
| Corrupt/missing gallery/history file | Treat as empty, keep going |

## 7. Storage

`%APPDATA%/Iris/`
- `images/{guid}.jpg` — generated images
- `gallery.json` — `List<GalleryItem>`
- `history.json` — recent prompts (capped 30)

## 8. Testing (xUnit, Iris.Tests)

Pure-logic, no network:
- **PollinationsClient.BuildUrl** — prompt encoding, width/height/seed/model/
  nologo params, special characters.
- **PromptComposer.Compose** — each style appends its keywords; `None` unchanged;
  trims/spacing correct.
- **GalleryStore** — add appends; `All` returns most-recent-first; delete removes
  file + entry; missing file → empty; JSON round-trip.
- **HistoryStore** — add de-dupes, caps at 30, most-recent-first.

## 9. Build / delivery

- **Toolchain:** VS 2026 Community is installed (WPF/.NET ready). First build
  step: confirm the `dotnet` SDK/CLI is available; if not, install the standalone
  .NET SDK (flagged before any download).
- Target framework: `net8.0-windows` (LTS) with `<UseWPF>true</UseWPF>`; adjust
  to the installed LTS SDK if needed.
- Run/dev: `dotnet run --project Iris.App` (or F5 in VS).
- Ship: `dotnet publish Iris.App -c Release -r win-x64` → a Windows build the
  user runs directly.

## 10. Open items

- App icon + window branding (crimson/gold coin-equivalent for Iris) — after core
  works.
- Confirm installed .NET SDK version at build; pin TFM accordingly.
