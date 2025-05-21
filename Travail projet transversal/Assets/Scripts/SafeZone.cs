// SafeZone.cs
using UnityEngine;
using System.Collections.Generic;

public class SafeZone : MonoBehaviour
{
    public int nombreASupprimer = 2; // combien d'ennemis on veut retirer
    private bool joueurDansZone = false;

    void Update()
    {
        if (joueurDansZone && Input.GetKeyDown(KeyCode.E))
        {
            List<DrunkEnemyAI> tous = new List<DrunkEnemyAI>(FindObjectsOfType<DrunkEnemyAI>());
            if (tous.Count == 0) return;

            int suppr = Mathf.Min(nombreASupprimer, tous.Count);
            for (int i = 0; i < suppr; i++)
            {
                DrunkEnemyAI cible = tous[i];
                if (cible != null)
                {
                    Debug.Log("☠ Suppression bourré : " + cible.name);
                    Destroy(cible.gameObject);
                }
            }
        }
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
