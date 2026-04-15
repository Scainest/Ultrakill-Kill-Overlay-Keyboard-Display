# Ultrakill Kill Overlay + Keyboard Display

ULTRAKILL için BepInEx tabanlı bir QoL modu. Ekranda **kill sayacı** ve **tuş göstergesi (keyboard display)** birlikte gösterir.

## Özellikler

- Sağ üst köşede anlık kill sayısı (StatsManager üzerinden okunur).
- Sol üst köşede basılan tuşları canlı gösteren keyboard display: `W A S D R F G SPACE SHIFT CTRL`.
- **AltGr** tuşuyla preset döngüsü:
  1. Kill + Keyboard (ikisi de açık)
  2. Sadece Kill
  3. Sadece Keyboard
  4. İkisi de kapalı

## Kurulum

1. [BepInEx](https://github.com/BepInEx/BepInEx) ULTRAKILL için yüklü olmalı.
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
