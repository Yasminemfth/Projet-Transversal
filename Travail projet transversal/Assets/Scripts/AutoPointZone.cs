// AutoPointZone.cs
using UnityEngine;
<<<<<<< Updated upstream
using System.Collections;
=======
using System.Collections.Generic;
>>>>>>> Stashed changes

public class AutoPointZone : MonoBehaviour
{
    public GameObject prefabEnnemi;
    public Transform[] spawnPoints;
    public float intervalleDeTemps = 0.5f;
    public float bonheurParIntervalle = 1f;
<<<<<<< Updated upstream
    public float activationMinDelay = 5f;
    public float activationMaxDelay = 10f;
=======
>>>>>>> Stashed changes
    public SpriteRenderer zoneRenderer;

    [Header("Chemin spécial (waypoints sous cet objet)")]
    [SerializeField] private Transform waypointContainer;
    private Transform[] cheminVersPointSpecial;

    private bool joueurDansZone = false;
<<<<<<< Updated upstream
    private bool isActive = false;
=======
    private bool isActivated = false;
>>>>>>> Stashed changes
    private float timer = 0f;
    private List<DrunkEnemyAI> spawnedBourres = new List<DrunkEnemyAI>();

    private void Start()
<<<<<<< Updated upstream
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
=======
    {
        if (waypointContainer != null)
>>>>>>> Stashed changes
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
    }

    public void SetZoneActive(bool state)
    {
        isActivated = state;

        if (zoneRenderer != null)
            zoneRenderer.color = state ? Color.green : Color.gray;

        if (isActivated)
            SpawnerNouveauxEnnemis(2);

        if (isActivated && joueurDansZone)
            AppelerEnnemis();
    }

    private void Update()
    {
        if (!isActivated || !joueurDansZone) return;

        timer += Time.deltaTime;

        if (timer >= intervalleDeTemps)
        {
            GameManager.instance?.AjouterBonheur(bonheurParIntervalle);
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

<<<<<<< Updated upstream
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
=======
        joueurDansZone = true;
        timer = 0f;

        if (isActivated)
            AppelerEnnemis();
>>>>>>> Stashed changes
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

<<<<<<< Updated upstream
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
=======
        joueurDansZone = false;
    }

    private void AppelerEnnemis()
    {
        Debug.Log("→ AppelerEnnemis() appelé sur zone " + name);

        foreach (DrunkEnemyAI ennemi in spawnedBourres)
        {
            if (cheminVersPointSpecial != null && cheminVersPointSpecial.Length > 0)
            {
                Debug.Log("→ Appel AllerAuPointSpecial pour: " + ennemi.name);
                ennemi.AllerAuPointSpecial(cheminVersPointSpecial);
            }
            else
            {
                Debug.LogWarning("Aucun chemin spécial assigné à " + name);
>>>>>>> Stashed changes
            }
        }
    }

    private void SpawnerNouveauxEnnemis(int nombre)
    {
        for (int i = 0; i < nombre; i++)
        {
            int indexSpawn = i % spawnPoints.Length;
            Transform point = spawnPoints[indexSpawn];

            GameObject go = Instantiate(prefabEnnemi, point.position, Quaternion.identity);
            DrunkEnemyAI nouveau = go.GetComponent<DrunkEnemyAI>();

            if (nouveau != null)
                spawnedBourres.Add(nouveau);
        }
    }
}
