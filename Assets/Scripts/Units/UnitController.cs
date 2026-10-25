using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public Transform target;

    public float health = 100f;

    public float attackRange = 2f;

    public float attackDamage = 20f;

    public float attackRate = 1f;

    private float attackTimer;

    private NavMeshAgent agent;

    private UnitController enemyTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Update()
    {
        FindEnemy();

        if (enemyTarget != null)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemyTarget.transform.position
            );

            if (distance <= attackRange)
            {
                agent.isStopped = true;

                AttackEnemy();
            }
            else
            {
                agent.isStopped = false;

                agent.SetDestination(enemyTarget.transform.position);
            }
        }
    }

    void FindEnemy()
    {
        string enemyTag =
            gameObject.tag == "Player1"
            ? "Player2"
            : "Player1";

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestDistance = Mathf.Infinity;

        UnitController closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestEnemy = enemy.GetComponent<UnitController>();
            }
        }

        enemyTarget = closestEnemy;
    }

    void AttackEnemy()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackRate)
        {
            enemyTarget.TakeDamage(attackDamage);

            attackTimer = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}