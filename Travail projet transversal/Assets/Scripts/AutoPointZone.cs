using UnityEngine;

public class AutoPointZone : MonoBehaviour
{
    public DrunkEnemyAI[] bourres;
    public float intervalleDeTemps = 0.5f;
    public float bonheurParIntervalle = 1f;

    private bool joueurDansZone = false;
    private float timer = 0f;

    void Update()
    {
        if (joueurDansZone)
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

            foreach (DrunkEnemyAI ennemi in bourres)
            {
                ennemi.AllerAuPointSpecial();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = false;

            foreach (DrunkEnemyAI ennemi in bourres)
            {
                ennemi.ReprendrePatrouille();
            }
        }
    }
}
