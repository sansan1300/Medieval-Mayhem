using System.Collections.Generic;
using UnityEngine;

public enum ExplorationTeam
{
    Player1,
    Player2
}

// Team membership is separate from the tags used by soldier targeting.
[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(CharacterController))]
public class ExplorationPlayer : MonoBehaviour
{
    public ExplorationTeam team;
    public Renderer teamMarker;
    public Material player1Material;
    public Material player2Material;

    private CharacterController characterController;
    private readonly List<Collider> ignoredSoldiers = new List<Collider>();

    void OnEnable()
    {
        characterController = GetComponent<CharacterController>();
        teamMarker.sharedMaterial = team == ExplorationTeam.Player1
            ? player1Material : player2Material;
    }

    void Update()
    {
        // Only ignore nearby soldiers; ground and buildings remain solid.
        ignoredSoldiers.RemoveAll(collider => collider == null);
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 3f))
        {
            if (collider.GetComponentInParent<UnitController>() == null
                || ignoredSoldiers.Contains(collider)) continue;

            Physics.IgnoreCollision(characterController, collider, true);
            ignoredSoldiers.Add(collider);
        }
    }

    void OnDisable()
    {
        foreach (Collider collider in ignoredSoldiers)
            if (collider != null && characterController != null)
                Physics.IgnoreCollision(characterController, collider, false);
        ignoredSoldiers.Clear();
    }
}
