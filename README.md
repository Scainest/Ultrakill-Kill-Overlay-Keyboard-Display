# Ultrakill Kill Overlay + Keyboard Display

### [⬇️ Download UltrakillKillOverlay.dll](https://github.com/Scainest/Ultrakill-Kill-Overlay-Keyboard-Display/raw/main/UltrakillKillOverlay.dll) · [📦 Releases](https://github.com/Scainest/Ultrakill-Kill-Overlay-Keyboard-Display/releases/latest)

A BepInEx-based QoL mod for ULTRAKILL. Displays a **kill counter** and a **live keyboard display** on screen at the same time.

## Features

- Real-time kill count in the top-right corner (read from `StatsManager`).
- Keyboard display in the top-left corner showing the currently pressed keys: `W A S D R F G SPACE SHIFT CTRL`.
- Configurable positions via `BepInEx/config/com.scainest.killoverlay.cfg` (X/Y offsets for each overlay). Use [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) to edit live in-game with F1.
- **AltGr** key cycles through presets:
  1. Kill + Keyboard (both visible)
  2. Kill only
  3. Keyboard only
  4. Both hidden

## Installation

1. Install [BepInEx](https://github.com/BepInEx/BepInEx) for ULTRAKILL.
2. Download `UltrakillKillOverlay.dll` from the [latest release](https://github.com/Scainest/Ultrakill-Kill-Overlay-Keyboard-Display/releases/latest).
3. Drop it into:
   ```
   <ULTRAKILL>\BepInEx\plugins\
   ```
4. Launch the game.

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
- Konumlar `BepInEx/config/com.scainest.killoverlay.cfg` dosyasından ayarlanabilir (her overlay için X/Y offset). Oyun içinde anlık değişiklik için [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) modunu kurup F1'e basabilirsin.
- **AltGr** tuşuyla preset döngüsü:
  1. Kill + Keyboard (ikisi de açık)
  2. Sadece Kill
  3. Sadece Keyboard
  4. İkisi de kapalı

## Kurulum

1. ULTRAKILL için [BepInEx](https://github.com/BepInEx/BepInEx) yüklü olmalı.
2. `UltrakillKillOverlay.dll` dosyasını [en son sürümden](https://github.com/Scainest/Ultrakill-Kill-Overlay-Keyboard-Display/releases/latest) indirin.
3. Şu klasöre atın:
   ```
   <ULTRAKILL>\BepInEx\plugins\
   ```
4. Oyunu başlatın.

## Derleme

```
dotnet build -c Release
```

Çıkan DLL `bin/Release/UltrakillKillOverlay.dll` içindedir.
