using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;

    private string playerTag = "Player";
    public float timeBetweenAttacks = 2f;
    public DetectionRange detectionRange;

    public float flipDeadZone = 0.1f;
    public float moveSpeed = 5.0f;
    private bool facingRight = true;
    private bool isDead = false;
    private Coroutine attackCoroutine;

    public float blinkDuration = 1.0f;
    public float blinkInterval = 0.1f;
    private string dieAnimationName = "Enemy_Die";
    private SpriteRenderer sr;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null || anim == null || sr == null || detectionRange == null || player == null ||
            moveSpeed <= 0 || timeBetweenAttacks < 0 || blinkInterval <= 0 || blinkDuration < 0)
        {
            Debug.LogError(gameObject.name + ": EnemyMovement has a missing reference or an invalid value");
            enabled = false;
        }
    }

    void Update()
    {
        if (isDead || IsBusy() || !HasTarget())
        {
            return;
        }
        HandleRotate();
    }

    private void FixedUpdate()
    {
        if (isDead || IsBusy())               
        {
            return;
        }

        if (!HasTarget())
        {
            StopAttacking();
            anim.SetBool("isWalking", false);
            return;
        }

        if (TryToDetectPlayer())             
        {
            anim.SetBool("isWalking", false);
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackLoop());
            }
            return;
        }

        StopAttacking();                      
        anim.SetBool("isWalking", true);
        rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position,
                                            moveSpeed * Time.fixedDeltaTime));
    }

    private bool HasTarget()
    {
        return player != null;
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            anim.SetTrigger("isAttacking");
        }
    }

    private void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        anim.ResetTrigger("isAttacking");     
    }

    private bool IsBusy()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Busy");
    }

    public void Hurt()
    {
        if (isDead || !enabled)
        {
            return;
        }
        StopAttacking();
        anim.SetTrigger("hurt");
    }

    public void Die()
    {
        if (isDead || !enabled)
        {
            return;
        }
        isDead = true;
        StopAttacking();
        anim.SetBool("isWalking", false);
        anim.SetTrigger("die");
        StartCoroutine(BlinkAndDisappear());
    }

    private bool TryToDetectPlayer()
    {
        return detectionRange.IsPlayerInRange();
    }

    private void HandleRotate()
    {
        float distanceX = player.transform.position.x - transform.position.x;

        if (distanceX > flipDeadZone && !facingRight)
        {
            Flip();
        }
        else if (distanceX < -flipDeadZone && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180.0f, 0f);
    }

    private IEnumerator BlinkAndDisappear()
    {
        while (!UtilityFunctions.IsAnimationFinished(anim, dieAnimationName))
        {
            yield return null;
        }

        float elapsed = 0f;
        while (elapsed < blinkDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        Destroy(gameObject);
    }
}
