using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UltrakillKillOverlay
{
    [BepInPlugin("com.scainest.killoverlay", "ULTRAKILL Kill Overlay", "1.3.0")]
    public class Plugin : BaseUnityPlugin
    {
        // Config'ler BepInEx/config/com.scainest.killoverlay.cfg dosyasından düzenlenebilir
        internal static ConfigEntry<int> KillOffsetX;
        internal static ConfigEntry<int> KillOffsetY;
        internal static ConfigEntry<int> KeyOffsetX;
        internal static ConfigEntry<int> KeyOffsetY;

        private void Awake()
        {
            KillOffsetX = Config.Bind("KillOverlay", "OffsetX", -30,
                "Kill sayacının sağ kenardan yatay uzaklığı (negatif = sağdan içe)");
            KillOffsetY = Config.Bind("KillOverlay", "OffsetY", -20,
                "Kill sayacının üst kenardan dikey uzaklığı (negatif = üstten aşağı)");
            KeyOffsetX = Config.Bind("KeyboardDisplay", "OffsetX", 16,
                "Keyboard display'in sol kenardan yatay uzaklığı");
            KeyOffsetY = Config.Bind("KeyboardDisplay", "OffsetY", 16,
                "Keyboard display'in üst kenardan dikey uzaklığı");

            SceneManager.sceneLoaded += OnSceneLoaded;
            SpawnHost();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => SpawnHost();

        private static void SpawnHost()
        {
            var host = new GameObject("UltrakillOverlayHost");
            host.AddComponent<KillOverlayWorker>();
            host.AddComponent<KeyDisplayWorker>();
            host.AddComponent<OverlayToggleManager>();
        }
    }

    // AltGr (RightAlt) ile preset döngüsü:
    // 0: Kill + Keyboard açık | 1: Sadece Kill | 2: Sadece Keyboard | 3: İkisi kapalı
    public class OverlayToggleManager : MonoBehaviour
    {
        private int _preset = 0;
        private const int PresetCount = 4;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.AltGr) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                _preset = (_preset + 1) % PresetCount;
                Apply();
            }
        }

        private void Apply()
        {
            bool killOn     = _preset == 0 || _preset == 1;
            bool keyboardOn = _preset == 0 || _preset == 2;

            var killCanvas = transform.Find("KillOverlayCanvas");
            var keyCanvas  = transform.Find("KeyDisplayCanvas");
            if (killCanvas != null) killCanvas.gameObject.SetActive(killOn);
            if (keyCanvas  != null) keyCanvas.gameObject.SetActive(keyboardOn);
        }
    }

    public class KillOverlayWorker : MonoBehaviour
    {
        private Text _label;
        private Text _shadow;

        private void Awake()
        {
            var canvasGo = new GameObject("KillOverlayCanvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32760;
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            Font font = Resources.GetBuiltinResource<Font>("Arial.ttf")
                        ?? Font.CreateDynamicFontFromOSFont("Arial", 80);

            _shadow = MakeText(canvasGo.transform, font, new Color(0f, 0f, 0f, 0.9f), new Vector2(4f, -4f));
            _label  = MakeText(canvasGo.transform, font, new Color(1f, 0.1f, 0.1f, 1f), Vector2.zero);
        }

        private void Update()
        {
            if (_label == null) return;
            var sm = StatsManager.Instance;
            string txt = sm != null ? sm.kills.ToString() : "--";
            if (_label.text != txt)
            {
                _label.text = txt;
                _shadow.text = txt;
            }
        }

        private Text MakeText(Transform parent, Font font, Color color, Vector2 offset)
        {
            var textGo = new GameObject("KillText");
            textGo.transform.SetParent(parent, false);
            var rt = textGo.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(1f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot     = new Vector2(1f, 1f);
            rt.anchoredPosition = new Vector2(Plugin.KillOffsetX.Value + offset.x, Plugin.KillOffsetY.Value + offset.y);
            rt.sizeDelta = new Vector2(600f, 160f);

            var t = textGo.AddComponent<Text>();
            t.font = font;
            t.fontSize = 80;
            t.fontStyle = FontStyle.Bold;
            t.alignment = TextAnchor.UpperRight;
            t.color = color;
            t.text = "--";
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }
    }

    public class KeyDisplayWorker : MonoBehaviour
    {
        private const float KEY = 48f;
        private const float GAP = 5f;
        // PAD artık Plugin.KeyOffsetX/Y üzerinden dinamik geliyor

        private static readonly Color BgIdle    = new Color(0f, 0f, 0f, 0.55f);
        private static readonly Color BgPressed = new Color(0.85f, 0.05f, 0.05f, 0.95f);
        private static readonly Color FgIdle    = new Color(0.7f, 0.7f, 0.7f, 0.9f);
        private static readonly Color FgPressed = new Color(1f, 1f, 1f, 1f);
        private static readonly Color BorderIdle    = new Color(0.4f, 0.4f, 0.4f, 0.8f);
        private static readonly Color BorderPressed = new Color(1f, 0.3f, 0.3f, 1f);

        private Image _wBg, _aBg, _sBg, _dBg, _fBg, _gBg, _rBg, _spaceBg, _shiftBg, _ctrlBg;
        private Text  _wTx, _aTx, _sTx, _dTx, _fTx, _gTx, _rTx, _spaceTx, _shiftTx, _ctrlTx;
        private Outline _wOl, _aOl, _sOl, _dOl, _fOl, _gOl, _rOl, _spaceOl, _shiftOl, _ctrlOl;

        private void Awake()
        {
            var canvasGo = new GameObject("KeyDisplayCanvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32761;
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            Font font = Resources.GetBuiltinResource<Font>("Arial.ttf")
                        ?? Font.CreateDynamicFontFromOSFont("Arial", 30);

            float padX = Plugin.KeyOffsetX.Value;
            float padY = Plugin.KeyOffsetY.Value;
            float row1Y = -padY;
            float row2Y = -(padY + KEY + GAP);
            float row3Y = -(padY + (KEY + GAP) * 2f);
            float col1X = padX;
            float col2X = padX + (KEY + GAP);
            float col3X = padX + (KEY + GAP) * 2f;
            float col4X = padX + (KEY + GAP) * 3f;
            float col5X = padX + (KEY + GAP) * 4f;

            MakeKey(canvasGo.transform, font, "W", new Vector2(col2X, row1Y), KEY, KEY,
                    out _wBg, out _wTx, out _wOl);
            MakeKey(canvasGo.transform, font, "A", new Vector2(col1X, row2Y), KEY, KEY,
                    out _aBg, out _aTx, out _aOl);
            MakeKey(canvasGo.transform, font, "S", new Vector2(col2X, row2Y), KEY, KEY,
                    out _sBg, out _sTx, out _sOl);
            MakeKey(canvasGo.transform, font, "D", new Vector2(col3X, row2Y), KEY, KEY,
                    out _dBg, out _dTx, out _dOl);
            MakeKey(canvasGo.transform, font, "R", new Vector2(col4X, row1Y), KEY, KEY,
                    out _rBg, out _rTx, out _rOl);
            MakeKey(canvasGo.transform, font, "F", new Vector2(col4X, row2Y), KEY, KEY,
                    out _fBg, out _fTx, out _fOl);
            MakeKey(canvasGo.transform, font, "G", new Vector2(col5X, row2Y), KEY, KEY,
                    out _gBg, out _gTx, out _gOl);

            float spaceW = KEY * 3f + GAP * 2f;
            MakeKey(canvasGo.transform, font, "SPACE", new Vector2(col1X, row3Y), spaceW, KEY,
                    out _spaceBg, out _spaceTx, out _spaceOl);

            // Shift ve Ctrl: SPACE'in sağına, col4 ve col5 konumlarına
            MakeKey(canvasGo.transform, font, "SHIFT", new Vector2(col4X, row3Y), KEY, KEY,
                    out _shiftBg, out _shiftTx, out _shiftOl);
            MakeKey(canvasGo.transform, font, "CTRL", new Vector2(col5X, row3Y), KEY, KEY,
                    out _ctrlBg, out _ctrlTx, out _ctrlOl);
        }

        private void Update()
        {
            ApplyState(_wBg,     _wTx,     _wOl,     Input.GetKey(KeyCode.W));
            ApplyState(_aBg,     _aTx,     _aOl,     Input.GetKey(KeyCode.A));
            ApplyState(_sBg,     _sTx,     _sOl,     Input.GetKey(KeyCode.S));
            ApplyState(_dBg,     _dTx,     _dOl,     Input.GetKey(KeyCode.D));
            ApplyState(_fBg,     _fTx,     _fOl,     Input.GetKey(KeyCode.F));
            ApplyState(_gBg,     _gTx,     _gOl,     Input.GetKey(KeyCode.G));
            ApplyState(_rBg,     _rTx,     _rOl,     Input.GetKey(KeyCode.R));
            ApplyState(_spaceBg, _spaceTx, _spaceOl, Input.GetKey(KeyCode.Space));
            ApplyState(_shiftBg, _shiftTx, _shiftOl, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
            ApplyState(_ctrlBg,  _ctrlTx,  _ctrlOl,  Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        }

        private static void ApplyState(Image bg, Text tx, Outline ol, bool pressed)
        {
            if (bg == null || tx == null) return;
            bg.color = pressed ? BgPressed : BgIdle;
            tx.color = pressed ? FgPressed : FgIdle;
            if (ol != null) ol.effectColor = pressed ? BorderPressed : BorderIdle;
        }

        private static void MakeKey(Transform parent, Font font, string label, Vector2 topLeftPos,
                                    float w, float h,
                                    out Image bgOut, out Text txOut, out Outline olOut)
        {
            var keyGo = new GameObject("Key_" + label);
            keyGo.transform.SetParent(parent, false);
            var rt = keyGo.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot     = new Vector2(0f, 1f);
            rt.anchoredPosition = topLeftPos;
            rt.sizeDelta = new Vector2(w, h);

            bgOut = keyGo.AddComponent<Image>();
            bgOut.color = BgIdle;

            olOut = keyGo.AddComponent<Outline>();
            olOut.effectColor = BorderIdle;
            olOut.effectDistance = new Vector2(2f, -2f);

            var textGo = new GameObject("Label");
            textGo.transform.SetParent(keyGo.transform, false);
            var trt = textGo.AddComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            txOut = textGo.AddComponent<Text>();
            txOut.font = font;
            // Boyut küçültme: tek harf tuşları 22, çok harfli (SPACE/SHIFT/CTRL) 14
            txOut.fontSize = label.Length > 1 ? 14 : 22;
            txOut.fontStyle = FontStyle.Bold;
            txOut.alignment = TextAnchor.MiddleCenter;
            txOut.color = FgIdle;
            txOut.text = label;
            txOut.horizontalOverflow = HorizontalWrapMode.Overflow;
            txOut.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }
}
