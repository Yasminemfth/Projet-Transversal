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
    private bool justArrived = false;

    private float speed = 1f;
    private Animator animator;
    private Vector2 lastDirection = Vector2.down;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (data != null)
            speed = data.speed;
        else
            Debug.LogWarning($"EnemyData non assigné sur {name}. Vitesse par défaut utilisée : {speed}");

        if (cheminPatrouille != null && cheminPatrouille.Length > 0)
            ActiverPatrouilleNormale();
        else
        {
            Debug.LogWarning($"Aucun chemin de patrouille défini pour {name}");
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

        // Positions en 2D
        Vector2 currentPos2D = transform.position;
        Vector2 targetPos2D = (Vector2)cheminActuel[currentPoint].position;

        // Avance vers la cible
        Vector2 newPos = Vector2.MoveTowards(currentPos2D, targetPos2D, speed * Time.deltaTime);
        transform.position = newPos;

        // Calcul de la direction pour l'animation
        Vector2 direction = (targetPos2D - currentPos2D).normalized;
        if (direction != Vector2.zero)
        {
            lastDirection = direction;
            MettreAJourAnimations(direction);
            UpdateSpriteFlip(direction);
        }

        // Passage au point suivant une seule fois à l'arrivée exacte
        if (newPos == targetPos2D)
        {
            if (!justArrived)
            {
                justArrived = true;
                currentPoint = (currentPoint + 1) % cheminActuel.Length;
                Debug.Log($"[{Time.time:F2}] {name} arrivé au point {cheminActuel[currentPoint].name}, prochaine cible index {currentPoint}");
            }
        }
        else
        {
            justArrived = false;
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

    // Appelé quand la zone est active
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
        justArrived = false;

        Debug.Log($"[{Time.time:F2}] {name} se dirige vers {point.name}");
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
        justArrived = false;

        Debug.Log($"[{Time.time:F2}] {name} suit sa patrouille normale.");
    }

    public void ReprendrePatrouille()
    {
        Debug.Log($"[{Time.time:F2}] {name} → ReprendrePatrouille()");
        ActiverPatrouilleNormale();
    }

    public void Arreter()
    {
        isStopped = true;
        Debug.Log($"[{Time.time:F2}] {name} stoppé.");
    }
}
