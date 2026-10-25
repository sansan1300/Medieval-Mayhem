using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-90)]
public class WorldExploration : MonoBehaviour
{
    [Header("Prototype options")]
    public bool explorationEnabled = true;
    public ExplorationTeam team = ExplorationTeam.Player1;
    public Vector3 spawnOffset = new Vector3(0f, 0.05f, 4f);

    [Header("Shared scene references")]
    public Camera overviewCamera;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Exploration prefab references")]
    public GameObject playerPrefab;
    public Camera explorationCamera;
    public ExplorationFollowCamera followCamera;

    public ExplorationPlayer Player { get; private set; }
    public bool PurchaseMenuOpen { get; private set; }

    private StarterAssetsInputs inputs;
    private bool appliedMode;
    private bool started;
    private bool initialized;
    private bool overviewWasActive;
    private bool cursorCaptured;
    private CursorLockMode previousCursorLock;
    private bool previousCursorVisible;
    private GUIStyle helpStyle;
    private int controlsRestoredFrame = -1;

    void OnEnable()
    {
        if (!initialized) return;
        started = true;
        ApplyMode();
    }

    void Start()
    {
        previousCursorLock = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        if (overviewCamera == null || player1SpawnPoint == null
            || player2SpawnPoint == null || playerPrefab == null
            || explorationCamera == null || followCamera == null)
        {
            Debug.LogError("WorldExploration needs its camera, spawn points, and prefab assigned.", this);
            enabled = false;
            return;
        }

        overviewWasActive = overviewCamera.gameObject.activeSelf;
        initialized = true;
        started = true;
        ApplyMode();
    }

    void Update()
    {
        if (VictorySystem.IsGameOver)
        {
            CaptureCursor(false);
            StopPlayerInput();
            return;
        }

        if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame)
            explorationEnabled = !explorationEnabled;

        if (explorationEnabled != appliedMode) ApplyMode();
        if (!explorationEnabled) return;

        if (Player == null || Player.team != team || Player.transform.position.y < -10f)
            SpawnPlayer();

        if (PurchaseMenuOpen)
        {
            CaptureCursor(false);
            StopPlayerInput();
            return;
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame
            && Time.frameCount != controlsRestoredFrame)
            CaptureCursor(false);
        else if (!cursorCaptured && Application.isFocused && Mouse.current != null
                 && Mouse.current.leftButton.wasPressedThisFrame)
            CaptureCursor(true);

        if (!cursorCaptured) StopPlayerInput();
    }

    void StopPlayerInput()
    {
        if (inputs == null) return;
        inputs.MoveInput(Vector2.zero);
        inputs.LookInput(Vector2.zero);
        inputs.JumpInput(false);
        inputs.SprintInput(false);
    }

    public void SetPurchaseMenuOpen(bool open)
    {
        PurchaseMenuOpen = open;
        if (!started) return;
        CaptureCursor(explorationEnabled && !open && !VictorySystem.IsGameOver);
        StopPlayerInput();
        if (!open) controlsRestoredFrame = Time.frameCount;
    }

    void ApplyMode()
    {
        appliedMode = explorationEnabled;
        // Disable the original camera object before the player's Awake looks for MainCamera.
        overviewCamera.gameObject.SetActive(explorationEnabled ? false : overviewWasActive);
        explorationCamera.gameObject.SetActive(explorationEnabled);
        if (explorationEnabled)
        {
            if (Player == null || Player.team != team) SpawnPlayer();
            else Player.gameObject.SetActive(true);
            CaptureCursor(!PurchaseMenuOpen);
        }
        else
        {
            if (Player != null) Player.gameObject.SetActive(false);
            CaptureCursor(false);
        }
    }

    void SpawnPlayer()
    {
        if (Player != null)
        {
            Player.gameObject.SetActive(false);
            Destroy(Player.gameObject);
        }

        Transform home = team == ExplorationTeam.Player1 ? player1SpawnPoint : player2SpawnPoint;
        Transform opponent = team == ExplorationTeam.Player1 ? player2SpawnPoint : player1SpawnPoint;
        Quaternion facing = Quaternion.LookRotation(opponent.position - home.position, Vector3.up);
        GameObject avatar = Instantiate(playerPrefab, home.position + spawnOffset, facing, transform);
        avatar.name = "ExplorationPlayer";
        Player = avatar.GetComponent<ExplorationPlayer>();
        Player.team = team;
        inputs = avatar.GetComponent<StarterAssetsInputs>();
        var controller = avatar.GetComponent<ThirdPersonController>();
        followCamera.target = controller.CinemachineCameraTarget.transform;
        followCamera.target.rotation = facing * Quaternion.Euler(15f, 0f, 0f);
        // The prefab starts inactive so team, camera, and spawn are ready before Awake/Start.
        avatar.SetActive(true);
        CaptureCursor(cursorCaptured && !PurchaseMenuOpen);
    }

    void CaptureCursor(bool capture)
    {
        cursorCaptured = capture;
        if (inputs != null)
        {
            inputs.cursorLocked = capture;
            inputs.cursorInputForLook = capture;
        }
        Cursor.lockState = capture ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !capture;
    }

    void OnApplicationFocus(bool focused)
    {
        if (started && !focused) CaptureCursor(false);
    }

    void OnDisable()
    {
        if (!started) return;
        if (Player != null) Player.gameObject.SetActive(false);
        if (explorationCamera != null) explorationCamera.gameObject.SetActive(false);
        if (overviewCamera != null) overviewCamera.gameObject.SetActive(overviewWasActive);
        Cursor.lockState = previousCursorLock;
        Cursor.visible = previousCursorVisible;
        PurchaseMenuOpen = false;
        started = false;
    }

    void OnGUI()
    {
        if (!started) return;
        if (helpStyle == null)
            helpStyle = new GUIStyle(GUI.skin.box) { fontSize = 16, alignment = TextAnchor.MiddleLeft };
        string help = PurchaseMenuOpen
            ? "  Shopping — player movement and look paused\n  B / Esc: close shop   |   Battle continues"
            : explorationEnabled
                ? $"  Team: {team}   |   WASD: walk   Shift: run   Space: jump\n  Mouse: look   Esc: release cursor   Click: resume   F5: overview"
                : "  Overview   |   F5: explore the battlefield   B: shop";
        GUI.Box(new Rect(12, 12, 620, explorationEnabled || PurchaseMenuOpen ? 56 : 34), help, helpStyle);
    }
}
