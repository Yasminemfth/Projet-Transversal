using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DrunkEnemyAI : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [Header("Paths")]
    public Transform[] cheminPatrouille;

    private Transform[] cheminVersPointSpecial;
    private Transform[] cheminActuel;
    private int currentPoint = 0;
    private bool isStopped = false;
    private bool versPointSpecial = false;

    private float speed;
    private Animator animator;
    private Vector2 lastDirection;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (data != null)
        {
            speed = data.speed;
        }
        else
        {
            Debug.LogWarning(" EnemyData non mis sur " + gameObject.name);
        }

        cheminActuel = cheminPatrouille;
    }

    private void Update()
    {
        if (isStopped || cheminActuel == null || cheminActuel.Length == 0)
        {
            if (isStopped)
                Debug.Log(name + " est stoppé");

            SetIdleAnimation();
            return;
        }

        Transform target = cheminActuel[currentPoint];
        Vector2 direction = (target.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (direction != Vector2.zero)
        {
            lastDirection = direction;

            if (animator != null)
            {
                MettreAJourAnimations(direction);
            }

            UpdateSpriteFlip(direction);
        }

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= cheminActuel.Length)
            {
                currentPoint = 0;
            }
        }
    }

    private void SetIdleAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsUp", false);
        animator.SetBool("IsDown", false);
        animator.SetBool("IsRight", false);

        if (lastDirection.y > 0.5f) animator.SetBool("IsUp", true);
        else if (lastDirection.y < -0.5f) animator.SetBool("IsDown", true);
        else if (Mathf.Abs(lastDirection.x) > 0.1f) animator.SetBool("IsRight", true);
    }

    private void MettreAJourAnimations(Vector2 dir)
{
    animator.SetBool("IsUp", dir.y > 0.5f);
    animator.SetBool("IsDown", dir.y < -0.5f);
    animator.SetBool("IsRight", Mathf.Abs(dir.x) > 0.1f);

} 
    private void UpdateSpriteFlip(Vector2 dir)
{
    if (spriteRenderer == null) return;
    spriteRenderer.flipX = dir.x < -0.1f;
}
    public void AllerAuPointSpecial(Transform[] chemin)
    {
        Debug.Log(name + " → AllerAuPointSpecial() appelé");

        if (chemin == null || chemin.Length == 0)
        {
            Debug.LogWarning("Chemin vide pour " + name);
            return;
        }

        cheminVersPointSpecial = chemin;
        cheminActuel = cheminVersPointSpecial;
        currentPoint = 0;
        versPointSpecial = true;
        isStopped = false;

        Debug.Log($"[{gameObject.name}] Reçu nouveau chemin spécial de {chemin.Length} waypoints.");
    }

    public void ReprendrePatrouille()
    {
        cheminActuel = cheminPatrouille;
        currentPoint = 0;
        versPointSpecial = false;
        isStopped = false;
    }

    public void Arreter()
    {
        isStopped = true;
    }
}