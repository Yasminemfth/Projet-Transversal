using UnityEngine;

public class DrunkEnemyAI : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [Header("Paths")]
    public Transform[] cheminPatrouille;
    public Transform[] cheminVersPointSpecial;

    private Transform[] cheminActuel;
    private int currentPoint = 0;
    private bool isStopped = false;
    private bool versPointSpecial = false;

    private float speed;

    private void Start()
    {
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
        if (isStopped || cheminActuel == null || cheminActuel.Length == 0) return;

        Transform target = cheminActuel[currentPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentPoint++;

            if (currentPoint >= cheminActuel.Length)
            {
                currentPoint = 0; // boucle sur le chemin au lieu de s'arrêter
            }
        }
    }

    public void AllerAuPointSpecial()
    {
        if (cheminVersPointSpecial == null || cheminVersPointSpecial.Length == 0) return;

        cheminActuel = cheminVersPointSpecial;
        currentPoint = 0;
        versPointSpecial = true;
        isStopped = false;
    }

    public void AllerAuPointSpecial(Transform[] nouveauChemin)
    {
        if (nouveauChemin == null || nouveauChemin.Length == 0) return;

        cheminVersPointSpecial = nouveauChemin;
        cheminActuel = cheminVersPointSpecial;
        currentPoint = 0;
        versPointSpecial = true;
        isStopped = false;

        Debug.Log($"[{gameObject.name}] Reçu nouveau chemin spécial de {nouveauChemin.Length} waypoints.");
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
