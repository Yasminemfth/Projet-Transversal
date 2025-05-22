using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DrunkEnemyAI : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [Header("Chemin de patrouille")]
    public Transform[] cheminPatrouille; // Patrouille normale

    private Transform[] cheminActuel;
    private int currentPoint = 0;
    private bool isStopped = false;

    private float speed = 1f;
    private Animator animator;
    private Vector2 lastDirection = Vector2.down;
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
            Debug.LogWarning($" EnemyData non assigné sur {name}. Vitesse par défaut utilisée : {speed}");
        }

        if (cheminPatrouille != null && cheminPatrouille.Length > 0)
        {
            ActiverPatrouilleNormale();
        }
        else
        {
            Debug.LogWarning($" Aucun chemin de patrouille défini pour {name}");
            isStopped = true;
        }
    }

    private void Update()
    {
        if (isStopped || cheminActuel == null || cheminActuel.Length == 0)
        {
            SetIdleAnimation();
            return;
        }

        Transform target = cheminActuel[currentPoint];
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (direction != Vector2.zero)
        {
            lastDirection = direction;
            MettreAJourAnimations(direction);
            UpdateSpriteFlip(direction);
        }

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % cheminActuel.Length;
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
        if (animator == null) return;

        animator.SetBool("IsUp", dir.y > 0.5f);
        animator.SetBool("IsDown", dir.y < -0.5f);
        animator.SetBool("IsRight", Mathf.Abs(dir.x) > 0.1f);
    }

    private void UpdateSpriteFlip(Vector2 dir)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.flipX = dir.x < -0.1f;
    }

    // est Appelé quand la zone est active
    public void AllerVersPointUnique(Transform point)
    {
        if (point == null)
        {
            Debug.LogWarning($"{name} : point unique non assigné !");
            return;
        }

        cheminActuel = new Transform[] { point };
        currentPoint = 0;
        isStopped = false;

        Debug.Log($"{name} se dirige vers {point.name}");
    }

    public void ActiverPatrouilleNormale()
    {
        if (cheminPatrouille == null || cheminPatrouille.Length == 0)
        {
            Debug.LogWarning($"{name} : cheminPatrouille vide !");
            isStopped = true;
            return;
        }

        cheminActuel = cheminPatrouille;
        currentPoint = 0;
        isStopped = false;

        Debug.Log($" {name} suit sa patrouille normale.");
    }

    public void ReprendrePatrouille()
    {
        ActiverPatrouilleNormale();
    }

    public void Arreter()
    {
        isStopped = true;
        Debug.Log($" {name} stoppé.");
    }
}
