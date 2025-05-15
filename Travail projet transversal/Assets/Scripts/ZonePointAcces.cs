using UnityEngine;

public class ZonePointAccess : MonoBehaviour
{
    private bool joueurDevant = false;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (joueurDevant && Input.GetKeyDown(KeyCode.E))
        {
            if (col != null)
            {
                col.enabled = false; 
                Debug.Log("Accès débloqué !");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            joueurDevant = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            joueurDevant = false;
        }
    }
}
