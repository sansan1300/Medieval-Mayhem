using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public Transform target;

    public float health = 100f;

    public float maxHealth = 100f;

    public float movementSpeed = 5.335f;

    public float attackRange = 2f;

    public float villageAttackRange = 1.5f;

    public float attackDamage = 20f;

    public float attackRate = 1f;

    public float enemyChaseRange = 8f;

    public float villagePriorityRange = 8f;

    private float attackTimer;

    private NavMeshAgent agent;

    private UnitController enemyTarget;

    private VillageHealth targetVillage;

    private WorldHealthBar healthBar;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        agent.autoBraking = false;
        maxHealth = Mathf.Max(1f, maxHealth);
        health = Mathf.Clamp(health, 0f, maxHealth);
        healthBar = gameObject.AddComponent<WorldHealthBar>();
        healthBar.Initialize(transform, health, maxHealth, 1.35f, 0.9f);

        if (target != null)
        {
            agent.SetDestination(target.position);
            targetVillage = target.GetComponent<VillageHealth>();
        }
    }

    void Update()
    {
        if (target != null && targetVillage == null)
        {
            targetVillage = target.GetComponent<VillageHealth>();
        }

        if (target == null || (targetVillage != null && targetVillage.IsDestroyed))
        {
            agent.isStopped = true;
            enemyTarget = null;
            return;
        }

        // Once the soldier reaches its objective, it attacks the village even if
        // defenders are nearby. This prevents defenders at their spawn point
        // from keeping attackers away from the base.
        if (IsVillageInAttackRange())
        {
            enemyTarget = null;
            agent.isStopped = true;
            AttackVillage();
            return;
        }

        if (IsVillagePriorityRange())
        {
            enemyTarget = null;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            attackTimer = 0f;
            return;
        }

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
                attackTimer = 0f;
            }
        }
        else
        {
            // Resume the assigned village objective after nearby enemies die or
            // leave the chase range.
            agent.isStopped = false;
            agent.SetDestination(target.position);
            attackTimer = 0f;
        }
    }

    void FindEnemy()
    {
        if (enemyTarget != null)
        {
            float currentDistance = Vector3.Distance(transform.position, enemyTarget.transform.position);
            if (currentDistance <= enemyChaseRange)
            {
                return;
            }

            enemyTarget = null;
        }

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

            if (distance <= enemyChaseRange && distance < closestDistance)
            {
                UnitController candidate = enemy.GetComponent<UnitController>();
                if (candidate != null)
                {
                    closestDistance = distance;
                    closestEnemy = candidate;
                }
            }
        }

        enemyTarget = closestEnemy;
    }

    bool IsVillageInAttackRange()
    {
        if (targetVillage == null || targetVillage.ownerTag == gameObject.tag)
        {
            return false;
        }

        Collider villageCollider = targetVillage.GetComponent<Collider>();
        Vector3 closestPoint = villageCollider != null
            ? villageCollider.ClosestPoint(transform.position)
            : target.position;

        return Vector3.Distance(transform.position, closestPoint) <= villageAttackRange;
    }

    bool IsVillagePriorityRange()
    {
        return targetVillage != null
            && targetVillage.ownerTag != gameObject.tag
            && Vector3.Distance(transform.position, target.position) <= villagePriorityRange;
    }

    void AttackVillage()
    {
        if (targetVillage == null || targetVillage.ownerTag == gameObject.tag)
        {
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackRate)
        {
            targetVillage.TakeDamage(attackDamage);
            attackTimer = 0f;
        }
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
        if (health <= 0f || damage <= 0f)
        {
            return;
        }

        health = Mathf.Max(0f, health - damage);
        healthBar.SetHealth(health);

        if (health == 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
