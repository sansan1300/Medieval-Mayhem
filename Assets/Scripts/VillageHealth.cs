using UnityEngine;

public class VillageHealth : MonoBehaviour
{
    public string ownerTag;

    [Min(1f)]
    public float startingHealth = 500f;

    public float CurrentHealth { get; private set; }

    public bool IsDestroyed { get; private set; }

    void Awake()
    {
        CurrentHealth = Mathf.Max(1f, startingHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDestroyed || damage <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);

        if (CurrentHealth == 0f)
        {
            IsDestroyed = true;
            Destroy(gameObject);
        }
    }
}
