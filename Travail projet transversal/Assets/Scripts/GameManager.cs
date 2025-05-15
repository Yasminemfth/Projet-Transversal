using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float bonheur = 100f;

    [Header("Limites")]
    public bool limiterBonheurMax = true;
    public float bonheurMax = 100f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AjouterBonheur(float valeur)
    {
        bonheur += valeur;

        if (limiterBonheurMax && bonheur > bonheurMax)
            bonheur = bonheurMax;

        Debug.Log("Bonheur : " + (int)bonheur);
    }

    public void RetirerBonheur(float valeur)
    {
        bonheur -= valeur;

        if (bonheur < 0f)
            bonheur = 0f;

        Debug.Log("Bonheur : " + (int)bonheur);
    }
}
