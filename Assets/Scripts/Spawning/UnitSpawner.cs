using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;

    public Transform spawnPoint;

    public Transform target;

    public float spawnInterval = 3f;

    private float timer;

    private bool stopped;

    void Update()
    {
        if (stopped || VictorySystem.IsGameOver)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnUnit();

            timer = 0f;
        }
    }

    public bool CanSpawnPurchasedUnit(out string problem)
    {
        problem = null;
        if (stopped || VictorySystem.IsGameOver)
        {
            problem = "The match has ended.";
            return false;
        }
        if (unitPrefab == null || spawnPoint == null || target == null)
            problem = "Spawn point or opposing village is unavailable.";
        else if (unitPrefab.GetComponent<UnitController>() == null)
            problem = "The soldier prefab needs a UnitController.";
        else
        {
            var village = target.GetComponent<VillageHealth>();
            var agent = unitPrefab.GetComponent<NavMeshAgent>();
            if (village == null || village.IsDestroyed || village.ownerTag == unitPrefab.tag)
                problem = "The opposing village is unavailable.";
            else if (agent == null || !agent.enabled || !unitPrefab.activeSelf)
                problem = "The soldier prefab needs an active navigation agent.";
            else
            {
                var filter = new NavMeshQueryFilter { agentTypeID = agent.agentTypeID, areaMask = agent.areaMask };
                if (!NavMesh.SamplePosition(spawnPoint.position, out _, 0.5f, filter))
                    problem = "No navigable ground at your spawn point.";
            }
        }
        return problem == null;
    }

    public bool TrySpawnPurchasedUnit(out UnitController controller, out string problem)
    {
        controller = null;
        if (!isActiveAndEnabled)
        {
            problem = "Spawn point or opposing village is unavailable.";
            return false;
        }
        if (!CanSpawnPurchasedUnit(out problem)) return false;

        controller = SpawnUnit();
        if (!controller.GetComponent<NavMeshAgent>().isOnNavMesh)
        {
            controller.gameObject.SetActive(false);
            Destroy(controller.gameObject);
            controller = null;
            problem = "The soldier could not be placed on navigable ground.";
            return false;
        }
        return true;
    }

    UnitController SpawnUnit()
    {
        GameObject unit = Instantiate(
            unitPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        UnitController controller = unit.GetComponent<UnitController>();

        controller.target = target;
        return controller;
    }

    public void StopForVictory()
    {
        stopped = true;
    }
}
