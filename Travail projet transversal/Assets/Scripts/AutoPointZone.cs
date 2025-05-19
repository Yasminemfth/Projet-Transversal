using UnityEngine;
using System.Collections;

public class AutoPointZone : MonoBehaviour
{
    public DrunkEnemyAI[] bourres;
    public float intervalleDeTemps = 0.5f;
    public float bonheurParIntervalle = 1f;
    public float activationMinDelay = 5f;
    public float activationMaxDelay = 10f;
    public SpriteRenderer zoneRenderer;

    [Header("Chemin spécial (waypoints sous cet objet)")]
    [SerializeField] private Transform waypointContainer;
    private Transform[] cheminVersPointSpecial;

    private bool joueurDansZone = false;
    private bool isActive = false;
    private float timer = 0f;

    private void Start()
    {
        if (waypointContainer != null)
        {
            int count = waypointContainer.childCount;
            cheminVersPointSpecial = new Transform[count];
            for (int i = 0; i < count; i++)
                cheminVersPointSpecial[i] = waypointContainer.GetChild(i);
        }
        else
        {
            Debug.LogWarning($"{name} n'a pas de waypointContainer assigné !");
        }

        StartCoroutine(CycleActivation());
    }

    void Update()
    {
        if (joueurDansZone && isActive)
        {
            timer += Time.deltaTime;
            if (timer >= intervalleDeTemps)
            {
                GameManager.instance?.AjouterBonheur(bonheurParIntervalle);
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = true;
            timer = 0f;

            if (isActive)
            {
                foreach (DrunkEnemyAI ennemi in bourres)
                {
                    if (cheminVersPointSpecial != null && cheminVersPointSpecial.Length > 0)
                    {
                        Debug.Log("Appel AllerAuPointSpecial pour: " + ennemi.name);
                        ennemi.AllerAuPointSpecial(cheminVersPointSpecial);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = false;

            foreach (DrunkEnemyAI ennemi in bourres)
                ennemi.ReprendrePatrouille();
        }
    }

    IEnumerator CycleActivation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(activationMinDelay, activationMaxDelay));
            SetActiveState(true);
            yield return new WaitForSeconds(Random.Range(5f, 8f));
            SetActiveState(false);
        }
    }

    private void SetActiveState(bool state)
    {
        isActive = AHHHHJ;

        if (zoneRenderer != null)
            zoneRenderer.color = state ? Color.green : Color.gray;

        if (isActive && joueurDansZone)
        {
            foreach (DrunkEnemyAI ennemi in bourres)
            {
                if (cheminVersPointSpecial != null && cheminVersPointSpecial.Length > 0)
                {
                    Debug.Log("Appel AllerAuPointSpecial pour: " + ennemi.name);
                    ennemi.AllerAuPointSpecial(cheminVersPointSpecial);
                }
            }
        }
    }
}
