using UnityEngine;
using UnityEngine.Events;

public class HealthAndDamage : MonoBehaviour
{
    public float health;
    public float damage;

    public UnityEvent whenHit;
    public UnityEvent whenDie;

    private void Awake()
    {
        if (health <= 0 || damage < 0)
        {
            Debug.LogError(gameObject.name + ": health must be above 0 and damage can't be negative");
        }
    }

    public void TakeDamage(float damageTaken)
    {
        if (health <= 0 || damageTaken < 0)
        {
            return;
        }

        health -= damageTaken;
        if (health < 0)
        {
            health = 0;
        }
        Debug.Log(gameObject.name + " health: " + health);

        if (health == 0)          
        {
            whenDie.Invoke();
        }
        else
        {
            whenHit.Invoke();
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
