using UnityEngine;

public class MalusZone : MonoBehaviour
{
    public float malusParCoup = 5f;
    public float intervalleMalus = 0.5f;

    private bool joueurDansZone = false;
    private float timer = 0f;

    private void Update()
    {
        if (joueurDansZone)
        {
            timer += Time.deltaTime;
            if (timer >= intervalleMalus)
            {
                GameManager.instance?.RetirerBonheur(malusParCoup);
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = true;
            timer = 0f; // pour commencer direct
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            joueurDansZone = false;
        }
    }
}
