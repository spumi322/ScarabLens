# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ScarabLens is a WPF .NET 8 transparent desktop overlay for Windows that displays Path of Exile scarab prices from poe.ninja. The window is borderless, always-on-top, and click-through by default.

## Build & Run

```bash
# Debug run
dotnet run --project ScarabLens.csproj

# Release build
dotnet build ScarabLens.csproj -c Release

# Publish as single self-contained exe (win-x64 is baked into the csproj)
dotnet publish ScarabLens.csproj -c Release
```

The solution file is `ScarabLens.slnx` (VS 2022+ XML solution format). There are no tests.

## Architecture

**Entry flow:** `App.xaml.cs → OnStartup` checks a named mutex for single-instance enforcement, constructs `ApiService → PriceCache → OverlayViewModel`, shows `MainWindow` hidden, then fires `priceCache.RefreshAsync()` in the background and calls `vm.RefreshDisplay()` on completion.

**Overlay population** happens in `Views/MainWindow.xaml.cs → Window_Loaded`: iterates `SlotPositions.All` and dynamically creates a `TextBlock` for each slot (yellow, bold, 11px, with a drop-shadow effect), positioned at `(x+5, y+10)` on the canvas. TextBlocks are collected into a `labels` list and inserted before `HitSurface` (the last child) to stay below it in Z-order. The handler then subscribes to `vm.PricesRefreshed` to update label text on each refresh — the event fires on a non-UI thread, so the handler marshals via `Dispatcher.Invoke`.

**Price data flow:** `ApiService.FetchScarabsAsync` hits the poe.ninja API, deserializes `ScarabResponse` (which contains separate `Lines` and `Items` lists joined by `Id`), and returns a `Dictionary<string, decimal>` keyed by item name. `PriceCache` wraps this: it replaces its internal dictionary atomically on each refresh and runs a `PeriodicTimer`-based background loop refreshing every hour. `OverlayViewModel.RefreshDisplay` reads prices from the cache via `IPriceCache.GetPrice(slot.Name)` and raises `PricesRefreshed` with a formatted string list index-matched to `SlotPositions.All`.

**Click-through & hotkey wiring** happen in `Window_SourceInitialized`:
- `NativeMethods.SetWindowLong` ORs in `WS_EX_LAYERED | WS_EX_TRANSPARENT` for full click-through.
- `HwndSource.AddHook(WndProc)` intercepts `WM_HOTKEY`.
- **Ctrl+Alt+S** (id 9001) — toggle overlay visibility.
- **F2** (id 9002) — toggle coordinate helper mode (removes `WS_EX_TRANSPARENT`, shows `HitSurface` and `CoordLabel` with live cursor pixel coordinates).

**HitSurface fill trick:** `HitSurface` uses `Fill="#01000000"` (alpha=1, not 0). On `WS_EX_LAYERED` windows, truly alpha=0 pixels bypass hit-testing regardless of `WS_EX_TRANSPARENT`, so alpha=1 is used — invisible but hit-testable in coord mode.

**Window geometry:** hardcoded to 1920×1080 at Left=0 Top=0. Slot coordinates in `SlotPositions.All` are pixel-absolute on this canvas, organized into left, middle, and right groups matching the in-game stash tab layout.

## File Responsibilities

| File | Purpose |
|---|---|
| `Native/NativeMethods.cs` | All P/Invoke declarations and Win32 constants |
| `Models/SlotPositions.cs` | Static list of `(string Name, double X, double Y)` for every scarab slot; `Name` is the exact poe.ninja item name used as the lookup key |
| `Models/ScarabLine.cs` | JSON response models: `ScarabResponse`, `ScarabLine`, `ScarabItem` |
| `Services/IApiService.cs` | Interface: `Task<Dictionary<string, decimal>> FetchScarabsAsync()` |
| `Services/ApiService.cs` | Hits the poe.ninja scarab endpoint; joins `Lines` and `Items` by `Id` to produce a name→price map |
| `Services/IPriceCache.cs` | Interface: `decimal? GetPrice(string name)` |
| `Services/PriceCache.cs` | Wraps `IApiService`; stores last-fetched dictionary atomically; runs hourly `PeriodicTimer` refresh loop |
| `ViewModels/OverlayViewModel.cs` | Holds `IPriceCache`, raises `PricesRefreshed` event with formatted price strings; `FormatPrice` rounds to one decimal or integer for values > 999 |
| `Views/MainWindow.xaml` | Transparent `Canvas`-rooted window; `HitSurface` and `CoordLabel` elements for coord helper mode |
| `Views/MainWindow.xaml.cs` | Hotkey registration, coord mode toggle, dynamic label creation, `PricesRefreshed` subscription |
| `App.xaml.cs` | Single-instance mutex, object graph construction, startup/exit lifecycle |
