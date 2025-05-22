using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SafeZone : MonoBehaviour
{
    [Tooltip("Nombre maximal d'ennemis retirés par activation")] public int maxRemovalsPerUse = 2;
    [Tooltip("Cooldown en secondes entre deux utilisations")] public float removalCooldown = 15f;

    [Header("UI Interaction Prompt")]
    [Tooltip("Objet UI (Text ou panel) à afficher pour l'interaction")] public GameObject promptUI;

    private bool joueurDansZone = false;
    private bool canRemove = true;

    private void Awake()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = false;
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!joueurDansZone || !canRemove) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (promptUI != null)
                promptUI.SetActive(false);
            StartCoroutine(RemoveEnemiesCoroutine());
        }
    }

    private IEnumerator RemoveEnemiesCoroutine()
    {
        canRemove = false;

        List<GameObject> actifs = EnemyWaveManager.instance.GetActiveEnemies();
        int removed = 0;

        for (int i = actifs.Count - 1; i >= 0 && removed < maxRemovalsPerUse; i--)
        {
            GameObject ennemi = actifs[i];
            if (ennemi != null)
            {
                var ai = ennemi.GetComponent<DrunkEnemyAI>();
                if (ai != null)
                    ai.Arreter();

                Destroy(ennemi);
                actifs.RemoveAt(i);
                removed++;
            }
        }

        Debug.Log($"SafeZone: {removed} ennemis retirés, cooldown de {removalCooldown}s.");

        yield return new WaitForSeconds(removalCooldown);
        canRemove = true;

        if (joueurDansZone && promptUI != null)
            promptUI.SetActive(true);

        Debug.Log("SafeZone: prêt à retirer de nouveaux ennemis.");
    }
}
