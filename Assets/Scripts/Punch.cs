using UnityEngine;

public class Punch : MonoBehaviour
{

    private string playerTag = "Player";
    private string enemyTag = "Enemy";
    private HealthAndDamage healthAndDamage;

    private void Awake()
    {
        healthAndDamage = GetComponentInParent<HealthAndDamage>();

        if (healthAndDamage == null)
        {
            Debug.LogError(gameObject.name + ": no HealthAndDamage on this object or its parents");
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
        {
            return;
        }

        if (collision.CompareTag(playerTag) || collision.CompareTag(enemyTag))
        {
            HealthAndDamage target = collision.GetComponent<HealthAndDamage>();
            if (target != null)
            {
                target.TakeDamage(healthAndDamage.GetDamage());
            }
        }
    }
}
