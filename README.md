<!-- ============ HEADER BANNER ============ -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=0:E11B22,100:E0A82E&height=230&section=header&text=Iris&fontSize=90&fontColor=ffffff&fontAlignY=40&desc=AI%20Image%20Generator%20for%20Windows&descSize=18&descAlignY=64&descColor=ffffff" width="100%"/>

<!-- ============ BADGES ============ -->
<p align="center">
  <img src="https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  &nbsp;
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white"/>
  &nbsp;
  <img src="https://img.shields.io/badge/WPF-0C54C2?style=for-the-badge&logo=windows&logoColor=white"/>
  &nbsp;
  <img src="https://img.shields.io/badge/Tested-28%20passing-6E9F18?style=for-the-badge&logo=xunit&logoColor=white"/>
</p>

<!-- ============ TAGLINE ============ -->
<p align="center">
  <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&size=13&duration=2500&pause=99999&color=FFFFFF&center=true&vCenter=true&width=720&height=30&lines=Type+a+prompt.+Pick+a+style.+Get+a+real+AI+image.&repeat=false" alt="Tagline"/>
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E11B22,100:E0A82E&height=4" width="100%"/>

<!-- ============ ABOUT ============ -->
<h2 align="center"><img src="https://media.giphy.com/media/hvRJCLFzcasrR4ia7z/giphy.gif" width="26"> About</h2>

<p align="center">
  <b>Iris</b> is a native Windows AI image generator built in <b>C# / WPF</b>. Type a prompt, choose a style,
  and get a real AI-generated image in seconds — saved automatically to a local gallery.
</p>

<p align="center">
  <b>No account. No API key. No billing.</b> Iris generates images through <b>Pollinations.ai</b> (keyless),
  so it works the moment you open it. Dark, minimal UI in a crimson &amp; gold brand — no Electron, just native WPF.
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E0A82E,100:E11B22&height=4" width="100%"/>

<!-- ============ PREVIEW ============ -->
<h2 align="center">🖼️ Preview</h2>

<p align="center">
  <img src="screenshots/iris.png" width="90%" alt="Iris — dark AI image generator UI"/>
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E11B22,100:E0A82E&height=4" width="100%"/>

<!-- ============ FEATURES ============ -->
<h2 align="center">✨ Features</h2>

<p align="center">
🎨 &nbsp;<b>Prompt → image</b> — real AI generation, keyless, in seconds<br/><br/>
🖌️ &nbsp;<b>Style presets</b> — Photorealistic · Cinematic · Anime · Concept Art · 3D<br/><br/>
🖼️ &nbsp;<b>Aspect presets</b> — Square · Portrait · Landscape<br/><br/>
🎲 &nbsp;<b>Seed control</b> — lock a seed to reproduce, or randomize for variations<br/><br/>
🗂️ &nbsp;<b>Local gallery</b> — every generation saved with its prompt, style, size &amp; seed; browse, open, delete<br/><br/>
🕘 &nbsp;<b>Prompt history</b> — one click to re-run a recent prompt<br/><br/>
💾 &nbsp;<b>Save / export</b> — write any result to disk<br/><br/>
🌑 &nbsp;<b>Native &amp; dark</b> — WPF, no Electron, crimson &amp; gold brand
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E0A82E,100:E11B22&height=4" width="100%"/>

<!-- ============ TECH STACK ============ -->
<h2 align="center">🧰 Tech Stack</h2>

<p align="center">
  <img src="https://skillicons.dev/icons?i=cs,dotnet,visualstudio&theme=light" />
</p>
<p align="center">
  <b>C# · .NET 8 · WPF · CommunityToolkit.Mvvm · System.Text.Json · xUnit</b>
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E11B22,100:E0A82E&height=4" width="100%"/>

<!-- ============ HOW IT WORKS ============ -->
<h2 align="center">⚙️ How It Works</h2>

<p align="center">
  Iris composes your prompt with the selected style keywords, then requests an image from Pollinations'
  keyless endpoint:
</p>

```
GET https://image.pollinations.ai/prompt/{prompt}?width={w}&height={h}&seed={seed}&nologo=true&model=sana
```

<p align="center">
  The returned JPEG is shown, auto-saved to <code>%APPDATA%/Iris/</code>, and added to the gallery. All the
  logic — URL building, prompt composition, and the gallery/history stores — lives in <b>Iris.Core</b> with
  zero UI dependencies, which is what makes it unit-testable.
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E0A82E,100:E11B22&height=4" width="100%"/>

<!-- ============ GETTING STARTED ============ -->
<h2 align="center">🚀 Getting Started</h2>

<p align="center"><b>Prerequisites:</b> .NET 8 SDK · Windows</p>

```bash
# Run
dotnet run --project Iris.App

# Test
dotnet test

# Publish a runnable Windows build
dotnet publish Iris.App -c Release -r win-x64 --self-contained false
#   → Iris.App/bin/Release/net8.0-windows/win-x64/publish/Iris.App.exe
```

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E11B22,100:E0A82E&height=4" width="100%"/>

<!-- ============ TESTS ============ -->
<h2 align="center">🧪 Tests</h2>

<p align="center">
  The pure logic in <b>Iris.Core</b> is covered by <b>xUnit</b> — no network, deterministic, fast.
</p>

```bash
dotnet test
```

<p align="center">
  <b>28 tests</b> across the Pollinations URL builder, prompt composition, style/aspect maps,
  <code>GalleryStore</code>, <code>HistoryStore</code>, and the <code>MainViewModel</code> generate flow.
</p>

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E0A82E,100:E11B22&height=4" width="100%"/>

<!-- ============ ARCHITECTURE ============ -->
<h2 align="center">🏗️ Architecture</h2>

```
Iris.Core   — models, PromptComposer, PollinationsClient, GalleryStore, HistoryStore   (tested)
Iris.App    — WPF, MVVM (CommunityToolkit.Mvvm), dark brand theme
Iris.Tests  — xUnit over Iris.Core + the ViewModel
```

<!-- divider -->
<img src="https://capsule-render.vercel.app/api?type=rect&color=0:E11B22,100:E0A82E&height=4" width="100%"/>

<!-- ============ DISCLAIMER ============ -->
<h2 align="center">⚖️ Disclaimer</h2>

<p align="center">
  Iris is an independent, open-source project. It is <b>not affiliated with, endorsed by, or associated with</b>
  Pollinations or any model provider. Generated images come from a third-party service and are subject to that
  service's availability and terms. Provided for personal and educational use.
</p>

<p align="center">
  <sub>© 2026 Anas Ben Ahmed · Provided "as is", without warranty of any kind.</sub>
</p>

<!-- ============ FOOTER WAVE ============ -->
<img src="https://capsule-render.vercel.app/api?type=waving&color=0:E0A82E,100:E11B22&height=160&section=footer&text=Built%20by%20Anas%20Ben%20Ahmed&fontSize=22&fontColor=ffffff&fontAlignY=72&desc=Type%20.%20Style%20.%20Generate&descSize=14&descAlignY=88&descColor=ffffff" width="100%"/>
