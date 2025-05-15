using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    private bool joueurDansZone = false;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (joueurDansZone && Input.GetKeyDown(KeyCode.E))
        {
            col.enabled = false; // 🔓 Désactive la barrière
            Debug.Log("Barrière ouverte !");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            joueurDansZone = true;
            Debug.Log("🚧 Collision détectée avec le joueur !");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            joueurDansZone = false;
        }
    }
}
