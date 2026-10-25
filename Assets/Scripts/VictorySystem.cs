using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Ends the two-village match and builds a simple winner screen at runtime.
/// A VillageHealth component creates this system automatically when the scene starts.
/// </summary>
public class VictorySystem : MonoBehaviour
{
    public static VictorySystem Instance { get; private set; }

    public static bool IsGameOver { get; private set; }

    private bool screenCreated;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Instance = null;
        IsGameOver = false;
    }

    public static void EnsureExists()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject victoryObject = new GameObject("VictorySystem");
        victoryObject.AddComponent<VictorySystem>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DeclareWinner(string defeatedVillageOwnerTag)
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        StopMatchActivity();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string winner = GetWinnerName(defeatedVillageOwnerTag);
        CreateVictoryScreen(winner + " has won!");
    }

    void StopMatchActivity()
    {
        UnitController[] units = FindObjectsByType<UnitController>(FindObjectsSortMode.None);
        foreach (UnitController unit in units)
        {
            unit.StopForVictory();
        }

        UnitSpawner[] spawners = FindObjectsByType<UnitSpawner>(FindObjectsSortMode.None);
        foreach (UnitSpawner spawner in spawners)
        {
            spawner.StopForVictory();
        }
    }

    string GetWinnerName(string defeatedVillageOwnerTag)
    {
        if (defeatedVillageOwnerTag == "Player1")
        {
            return "Player 2";
        }

        if (defeatedVillageOwnerTag == "Player2")
        {
            return "Player 1";
        }

        return "The opposing player";
    }

    void CreateVictoryScreen(string message)
    {
        if (screenCreated)
        {
            return;
        }

        screenCreated = true;
        EnsureEventSystem();

        GameObject canvasObject = new GameObject("VictoryScreen", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);

        GameObject panel = CreateUiObject("Panel", canvasObject.transform);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        panel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.78f);

        Text winnerText = CreateText("WinnerText", panel.transform, message, 64, FontStyle.Bold);
        RectTransform textRect = winnerText.rectTransform;
        textRect.anchorMin = new Vector2(0.1f, 0.52f);
        textRect.anchorMax = new Vector2(0.9f, 0.72f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        winnerText.color = Color.white;

        GameObject buttonObject = CreateUiObject("RestartButton", panel.transform);
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.4f, 0.32f);
        buttonRect.anchorMax = new Vector2(0.6f, 0.43f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(0.72f, 0.18f, 0.12f, 1f);
        Button restartButton = buttonObject.AddComponent<Button>();
        restartButton.targetGraphic = buttonImage;
        restartButton.onClick.AddListener(RestartGame);

        Text buttonText = CreateText("ButtonText", buttonObject.transform, "Restart", 36, FontStyle.Bold);
        RectTransform buttonTextRect = buttonText.rectTransform;
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;
        buttonText.color = Color.white;
    }

    void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem", typeof(EventSystem));
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }

    GameObject CreateUiObject(string objectName, Transform parent)
    {
        GameObject uiObject = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        uiObject.transform.SetParent(parent, false);
        return uiObject;
    }

    Text CreateText(string objectName, Transform parent, string value, int fontSize, FontStyle fontStyle)
    {
        GameObject textObject = new GameObject(objectName, typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(parent, false);

        Text text = textObject.GetComponent<Text>();
        text.text = value;
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return text;
    }

    void RestartGame()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < 0)
        {
            Debug.LogError("The active scene is not in Build Settings, so it cannot be restarted.");
            return;
        }

        IsGameOver = false;
        Instance = null;
        SceneManager.LoadScene(sceneIndex);
    }
}
