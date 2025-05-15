using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour
{
    public DrunkEnemyAI[] ennemis;          // Liste des bourrés à gérer
    public float delaiRespawn = 5f;         // Temps avant qu'ils reviennent

    private bool joueurDansZone = false;

    void Update()
    {
        if (joueurDansZone && Input.GetKeyDown(KeyCode.E))
        {
            foreach (DrunkEnemyAI ennemi in ennemis)
            {
                if (ennemi != null)
                {
                    ennemi.gameObject.SetActive(false); // Supprime temporairement
                }
            }

            Debug.Log(" Bourrés retirés. Réapparition dans " + delaiRespawn + "s.");
            StartCoroutine(RetourDesBourres());
        }
    }

    private IEnumerator RetourDesBourres()
    {
        yield return new WaitForSeconds(delaiRespawn);

        foreach (DrunkEnemyAI ennemi in ennemis)
        {
            if (ennemi != null)
            {
                ennemi.gameObject.SetActive(true); //reviennent
                ennemi.ReprendrePatrouille();      // reprennent leur trajet
            }
        }

        Debug.Log("🔁 Les bourrés sont revenus !");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            joueurDansZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            joueurDansZone = false;
    }
}
