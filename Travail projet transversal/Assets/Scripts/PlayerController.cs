using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public bool animationVerticaleActive = false;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        FlipBasedOnInput();

        //if (animator != null && animationVerticaleActive)
        {
            bool isMovingUp = moveInput.y > 0.1f;
            bool isMovingDown = moveInput.y < -0.1f;
            bool isMovingRight = moveInput.x > 0.1f;
            bool isMovingLeft = moveInput.x < -0.1f;

            animator.SetBool("IsMovingUp", isMovingUp);
            animator.SetBool("IsMovingDown", isMovingDown);
            animator.SetBool("IsMovingRight", isMovingRight);
            animator.SetBool("IsMovingLeft", isMovingLeft);

            Debug.Log($"Up: {isMovingUp}, Down: {isMovingDown}, Right: {isMovingRight}, Left: {isMovingLeft}");
        }
    }

    void FlipBasedOnInput()
        {
            if (moveInput.x > 0.1f)
                spriteRenderer.flipX = false;
            else if (moveInput.x < -0.1f)
                spriteRenderer.flipX = true;
        }

    void HandleAnimation()
    {
        if (animator == null) return;

        if (animationVerticaleActive)
        {
            bool isMovingUp = moveInput.y > 0.1f;
            bool isMovingDown = moveInput.y < -0.1f;

            animator.SetBool("IsMovingUp", isMovingUp);
            animator.SetBool("IsMovingDown", isMovingDown);


            if (isMovingUp) animator.SetBool("IsMovingDown", false);
            if (isMovingDown) animator.SetBool("IsMovingUp", false);

            Debug.Log($"Up: {isMovingUp}, Down: {isMovingDown}, InputY: {moveInput.y}");
        }
    }

    void HandleFlip()
    {
        if (spriteRenderer == null) return;

        if (moveInput.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveAmount);
    }
} 