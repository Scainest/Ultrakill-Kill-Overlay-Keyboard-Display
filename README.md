# Ultrakill Kill Overlay + Keyboard Display

A BepInEx-based QoL mod for ULTRAKILL. Displays a **kill counter** and a **live keyboard display** on screen at the same time.

## Features

- Real-time kill count in the top-right corner (read from `StatsManager`).
- Keyboard display in the top-left corner showing the currently pressed keys: `W A S D R F G SPACE SHIFT CTRL`.
- **AltGr** key cycles through presets:
  1. Kill + Keyboard (both visible)
  2. Kill only
  3. Keyboard only
  4. Both hidden

## Installation

1. Install [BepInEx](https://github.com/BepInEx/BepInEx) for ULTRAKILL.
2. Drop `UltrakillKillOverlay.dll` into:
   ```
   <ULTRAKILL>\BepInEx\plugins\
   ```
3. Launch the game.

## Build

```
dotnet build -c Release
```

The compiled DLL will be at `bin/Release/UltrakillKillOverlay.dll`.

---

# Türkçe

ULTRAKILL için BepInEx tabanlı bir QoL modu. Ekranda **kill sayacını** ve **canlı tuş göstergesini (keyboard display)** aynı anda gösterir.

## Özellikler

- Sağ üst köşede anlık kill sayısı (`StatsManager` üzerinden okunur).
- Sol üst köşede o an basılan tuşları gösteren keyboard display: `W A S D R F G SPACE SHIFT CTRL`.
- **AltGr** tuşuyla preset döngüsü:
  1. Kill + Keyboard (ikisi de açık)
  2. Sadece Kill
  3. Sadece Keyboard
  4. İkisi de kapalı

## Kurulum

1. ULTRAKILL için [BepInEx](https://github.com/BepInEx/BepInEx) yüklü olmalı.
2. `UltrakillKillOverlay.dll` dosyasını şu klasöre atın:
   ```
   <ULTRAKILL>\BepInEx\plugins\
   ```
3. Oyunu başlatın.

## Derleme

```
dotnet build -c Release
```

Çıkan DLL `bin/Release/UltrakillKillOverlay.dll` içindedir.
