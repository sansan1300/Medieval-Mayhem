using UnityEngine;

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

    void SpawnUnit()
    {
        GameObject unit = Instantiate(
            unitPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        UnitController controller = unit.GetComponent<UnitController>();

        controller.target = target;
    }

    public void StopForVictory()
    {
        stopped = true;
    }
}
