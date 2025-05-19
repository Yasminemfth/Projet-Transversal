using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public bool animationVerticaleActive = false; // Visible dans l'inspecteur

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (animator != null)
        {
            bool movingVertically = Mathf.Abs(moveInput.y) > 0.1f;
            animator.SetBool("VerticalInput", animationVerticaleActive && movingVertically);
        }
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveAmount);
    }
}
