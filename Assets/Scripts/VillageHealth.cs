using UnityEngine;

public class VillageHealth : MonoBehaviour
{
    public string ownerTag;

    [Min(1f)]
    public float startingHealth = 500f;

    public float CurrentHealth { get; private set; }

    public bool IsDestroyed { get; private set; }

    private WorldHealthBar healthBar;

    void Awake()
    {
        CurrentHealth = Mathf.Max(1f, startingHealth);
        healthBar = gameObject.AddComponent<WorldHealthBar>();
        healthBar.Initialize(transform, CurrentHealth, CurrentHealth, 1.6f, 2.2f);
    }

    public void TakeDamage(float damage)
    {
        if (IsDestroyed || damage <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        healthBar.SetHealth(CurrentHealth);

        if (CurrentHealth == 0f)
        {
            IsDestroyed = true;
            Destroy(gameObject);
        }
    }
}
