using UnityEngine;

public class Patrouille : MonoBehaviour
{
    [Header("Paths")]
    public Transform[] cheminPatrouille;

    [Header("Données")]
    public float speed = 2f; // Vitesse par défaut si pas de "data"
    public bool versPointSpecial = false; // Si tu veux t’arrêter à la fin
    private int currentPoint = 0;
    private bool isStopped = false;

    private Transform[] cheminActuel;

    private void Start()
    {
        cheminActuel = cheminPatrouille;
    }

    private void Update()
    {
        if (isStopped || cheminActuel == null || cheminActuel.Length == 0) return;

        Transform target = cheminActuel[currentPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= cheminActuel.Length)
            {
                if (versPointSpecial)
                    isStopped = true;
                else
                    currentPoint = 0;
            }
        }
    }

    public void ReprendrePatrouille()
    {
        currentPoint = 0;
        isStopped = false;
        cheminActuel = cheminPatrouille;
    }

    public void Arreter()
    {
        isStopped = true;
    }
}
