using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]  // on ajoute AudioSource
public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public bool animationVerticaleActive = false;

    [Header("Bruits de pas")]
    [Tooltip("Clips à jouer pour les pas")]
    public AudioClip[] footstepClips;
    [Tooltip("Intervalle minimum (s) entre deux pas")]
    public float stepInterval = 0.5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private float stepTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // AudioSource setup
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        // Lecture de l'input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        HandleFlip();
        HandleAnimation();
        HandleFootsteps();
    }

    void HandleFlip()
    {
        if (moveInput.x > 0.1f) spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f) spriteRenderer.flipX = true;
    }

    void HandleAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (animationVerticaleActive)
        {
            animator.SetBool("IsMovingUp", moveInput.y > 0.1f);
            animator.SetBool("IsMovingDown", moveInput.y < -0.1f);
            animator.SetBool("IsMovingRight", moveInput.x > 0.1f);
            animator.SetBool("IsMovingLeft", moveInput.x < -0.1f);
        }
    }

    void HandleFootsteps()
    {
        // Si on ne se déplace pas, on reset le timer et on ne joue pas de son
        if (moveInput.magnitude < 0.1f)
        {
            stepTimer = stepInterval;
            return;
        }

        // On avance le timer
        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f && footstepClips.Length > 0)
        {
            PlayRandomFootstep();
            stepTimer = stepInterval;
        }
    }

    void PlayRandomFootstep()
    {
        int idx = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[idx]);
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveAmount);
    }
}
