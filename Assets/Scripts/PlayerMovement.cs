using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private string punchingAnimationName = "Player_punch";


    public float moveSpeed;
    private Vector2 movementVector;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null || anim == null || sr == null)
        {
            Debug.LogError(gameObject.name + ": missing Rigidbody2D, Animator or SpriteRenderer");
            enabled = false;
        }

        if (moveSpeed <= 0)
        {
            Debug.LogWarning(gameObject.name + ": moveSpeed must be greater than 0");
        }
    }

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null)
        {
            movementVector = Vector2.zero;
            return;
        }

        if (UtilityFunctions.IsAnimationPlaying(anim, punchingAnimationName))
        {
            movementVector = Vector2.zero;
        }
        else
        {
            HandleMovement();
            HandleFlip();
        }
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementVector * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("speed", movementVector.sqrMagnitude);

        if (Mouse.current.leftButton.wasPressedThisFrame && !UtilityFunctions.IsAnimationPlaying(anim, punchingAnimationName))
        {
            anim.SetTrigger("isPunching");
        }
    }

    private void HandleMovement()
    {
        movementVector.x = GetHorizontalAxis();
        movementVector.y = GetVerticalAxis();

        if (movementVector.sqrMagnitude > 1.0f)
        {
            movementVector.Normalize();
        }
    }

    private float GetHorizontalAxis()
    {
        bool right = Keyboard.current.dKey.isPressed;
        bool left = Keyboard.current.aKey.isPressed;

        if (left == right)
        {
            return 0f;
        }

        return (right ? 1.0f : -1.0f);
    }

    private float GetVerticalAxis()
    {
        bool up = Keyboard.current.wKey.isPressed;
        bool down = Keyboard.current.sKey.isPressed;

        if (up == down)
        {
            return 0f;
        }

        return (up ? 1.0f : -1.0f);
    }

    private void HandleFlip()
    {
        if (Keyboard.current.dKey.isPressed && !facingRight)
        {
            Flip();
        }
        else if (Keyboard.current.aKey.isPressed && facingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180.0f, 0f);
    }

    public Vector2 GetMovementVector()
    {
        return movementVector;
    }

    public void Hurt()
    {
        if (sr == null)
        {
            return;
        }
        StartCoroutine(hit());
    }

    public void Die()
    {
        Debug.Log("dead");
        Destroy(gameObject);
    }

    private IEnumerator hit()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}
