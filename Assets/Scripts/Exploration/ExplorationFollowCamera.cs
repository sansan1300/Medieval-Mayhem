using UnityEngine;

// Starter Assets rotates its camera target from mouse/gamepad input.
[DefaultExecutionOrder(100)]
public class ExplorationFollowCamera : MonoBehaviour
{
    public Transform target;
    [Min(0.5f)] public float distance = 4f;
    [Min(0.05f)] public float collisionRadius = 0.2f;

    void LateUpdate()
    {
        if (target == null) return;

        float clearDistance = distance;
        Vector3 backward = -target.forward;
        foreach (RaycastHit hit in Physics.SphereCastAll(target.position,
                     collisionRadius, backward, distance, Physics.DefaultRaycastLayers,
                     QueryTriggerInteraction.Ignore))
        {
            // Soldiers should not make the camera jump as they walk past.
            if (hit.collider.GetComponentInParent<UnitController>() != null) continue;
            clearDistance = Mathf.Min(clearDistance, Mathf.Max(0.1f, hit.distance - 0.05f));
        }

        transform.SetPositionAndRotation(target.position + backward * clearDistance,
            target.rotation);
    }
}
