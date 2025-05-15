using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public float speed = 5f;
    private Vector2 move;

    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        transform.Translate(move * speed * Time.deltaTime);
    }
}
