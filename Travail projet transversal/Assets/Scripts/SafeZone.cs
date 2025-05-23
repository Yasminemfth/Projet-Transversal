using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SafeZone : MonoBehaviour
{
    [Tooltip("Nombre maximal d'ennemis retirés par activation")]
    public int maxRemovalsPerUse = 8;
    [Tooltip("Cooldown en secondes entre deux utilisations")]
    public float removalCooldown = 15f;

    [Header("UI Interaction Prompt")]
    [Tooltip("Objet UI (Text ou panel) à afficher pour l'interaction")]
    public GameObject promptUI;

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
            if (canRemove && promptUI != null)
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
        // On ne fait rien si on n'est pas dans la zone ou si on est en cooldown
        if (!joueurDansZone || !canRemove) 
            return;

        // Seulement au clic sur E
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

        // 1) Supprime jusqu'à maxRemovalsPerUse ennemis
        var actifs = EnemyWaveManager.instance.GetActiveEnemies();
        int removed = 0;
        for (int i = actifs.Count - 1; i >= 0 && removed < maxRemovalsPerUse; i--)
        {
            var ennemi = actifs[i];
            if (ennemi != null)
            {
                // Stoppe l'IA
                var ai = ennemi.GetComponent<DrunkEnemyAI>();
                if (ai != null) ai.Arreter();

                // Détruit l'ennemi
                Destroy(ennemi);
                removed++;
            }
        }

        Debug.Log($"SafeZone: {removed} ennemis retirés, cooldown de {removalCooldown}s.");

        // 2) Attend le cooldown
        yield return new WaitForSeconds(removalCooldown);

        // 3) Relance le cycle de vagues
        EnemyWaveManager.instance.ClearEnemiesAndResetWave();
        Debug.Log("SafeZone: vagues réinitialisées.");

        // 4) Réautorise l'utilisation et affiche de nouveau le prompt si on est toujours dedans
        canRemove = true;
        if (joueurDansZone && promptUI != null)
            promptUI.SetActive(true);

        Debug.Log("SafeZone: prêt pour une nouvelle activation.");
    }
}