using MedievalMayhem.Economy;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-110)]
public class UnitPurchaseMenu : MonoBehaviour
{
    [Header("Prototype economy")]
    [Min(0)] public int startingGold = 100;
    [Min(1)] public int soldierCost = 25;

    [Header("Shared battlefield references")]
    public WorldExploration exploration;
    public UnitSpawner player1Spawner;
    public UnitSpawner player2Spawner;
    public VillageHealth player1Village;
    public VillageHealth player2Village;

    public bool IsOpen { get; private set; }
    public string Feedback { get; private set; }

    private GoldWallet player1Gold;
    private GoldWallet player2Gold;
    private ExplorationTeam displayedTeam;
    private GUIStyle titleStyle;
    private GUIStyle textStyle;
    private GUIStyle buttonStyle;

    void Start()
    {
        player1Gold = new GoldWallet(Mathf.Max(0, startingGold));
        player2Gold = new GoldWallet(Mathf.Max(0, startingGold));
    }

    void Update()
    {
        if (VictorySystem.IsGameOver || exploration == null || !exploration.isActiveAndEnabled)
        {
            if (IsOpen) SetOpen(false);
            return;
        }

        if (displayedTeam != exploration.team)
        {
            displayedTeam = exploration.team;
            Feedback = null;
        }

        if (Keyboard.current == null) return;
        if (Keyboard.current.bKey.wasPressedThisFrame) SetOpen(!IsOpen);
        else if (IsOpen && Keyboard.current.escapeKey.wasPressedThisFrame) SetOpen(false);
    }

    public void SetOpen(bool open)
    {
        IsOpen = open && !VictorySystem.IsGameOver && exploration != null && exploration.isActiveAndEnabled;
        Feedback = null;
        if (exploration != null) exploration.SetPurchaseMenuOpen(IsOpen);
    }

    public int GetGold(ExplorationTeam team)
    {
        return Wallet(team)?.Balance ?? 0;
    }

    GoldWallet Wallet(ExplorationTeam team)
    {
        return team == ExplorationTeam.Player1 ? player1Gold : player2Gold;
    }

    string PurchaseProblem()
    {
        if (VictorySystem.IsGameOver) return "The match has ended.";
        if (exploration == null || !exploration.isActiveAndEnabled)
            return "Player team is unavailable.";
        var wallet = Wallet(exploration.team);
        if (wallet == null) return "Preparing the shop...";
        if (soldierCost <= 0) return "Soldier cost must be greater than zero.";
        var home = exploration.team == ExplorationTeam.Player1 ? player1Village : player2Village;
        var spawner = exploration.team == ExplorationTeam.Player1 ? player1Spawner : player2Spawner;
        if (home == null || home.IsDestroyed) return "Your village is unavailable.";
        if (spawner == null || !spawner.isActiveAndEnabled) return "Your spawn point is unavailable.";
        if (home.ownerTag != exploration.team.ToString() || spawner.unitPrefab == null
            || !spawner.unitPrefab.CompareTag(exploration.team.ToString()))
            return "Your team's soldier is not assigned correctly.";
        if (!spawner.CanSpawnPurchasedUnit(out string problem)) return problem;
        if (!wallet.CanAfford(soldierCost)) return $"Not enough gold. You need {soldierCost} gold.";
        return null;
    }

    public bool TryBuySoldier()
    {
        if (!IsOpen) return false;
        Feedback = PurchaseProblem();
        if (Feedback != null) return false;

        var spawner = exploration.team == ExplorationTeam.Player1 ? player1Spawner : player2Spawner;
        if (!spawner.TrySpawnPurchasedUnit(out _, out string problem))
        {
            Feedback = problem;
            return false;
        }

        // Spawning succeeded. The single-threaded purchase checked funds above.
        Wallet(exploration.team).TrySpend(soldierCost);
        Feedback = "Basic Soldier purchased!";
        return true;
    }

    void OnDisable()
    {
        SetOpen(false);
    }

    void OnGUI()
    {
        if (VictorySystem.IsGameOver || exploration == null || player1Gold == null) return;
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 23, fontStyle = FontStyle.Bold };
            textStyle = new GUIStyle(GUI.skin.label) { fontSize = 17, wordWrap = true };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 18 };
        }

        GUI.Box(new Rect(12, 84, 260, 38), GUIContent.none);
        GUI.Label(new Rect(20, 91, 248, 26), $"{exploration.team}   |   {GetGold(exploration.team)} gold   |   B: shop", textStyle);
        if (!IsOpen) return;

        float width = Mathf.Min(390, Screen.width - 24);
        GUILayout.BeginArea(new Rect((Screen.width - width) / 2, Mathf.Max(135, (Screen.height - 310) / 2), width, 310), GUI.skin.box);
        GUILayout.Label("Reinforcements", titleStyle);
        GUILayout.Label($"{exploration.team}   •   {GetGold(exploration.team)} gold", textStyle);
        GUILayout.Space(14);
        GUILayout.Label($"Basic Soldier — {soldierCost} gold", textStyle);
        GUILayout.Label("Spawns at your team's spawn point.", textStyle);
        GUILayout.Space(12);
        string problem = PurchaseProblem();
        bool previousEnabled = GUI.enabled;
        GUI.enabled = previousEnabled && problem == null;
        if (GUILayout.Button("Buy Basic Soldier", buttonStyle, GUILayout.Height(44))) TryBuySoldier();
        GUI.enabled = previousEnabled;
        GUILayout.Label(problem ?? Feedback ?? "Automatic reinforcements continue for both teams.", textStyle, GUILayout.MinHeight(46));
        if (GUILayout.Button("Close (B / Esc)", buttonStyle, GUILayout.Height(34))) SetOpen(false);
        GUILayout.EndArea();
    }
}
