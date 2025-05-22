using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Bonheur actuel")]
    public float bonheur = 100f;

    [Header("Limites")]
    public bool limiterBonheurMax = true;
    public float bonheurMax = 100f;

    [Header("Référence UI")]
    public Slider bonheurSlider;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Initialise le slider si présent
        if (bonheurSlider != null)
        {
            bonheurSlider.maxValue = bonheurMax;
            bonheurSlider.value = bonheur;
        }
        else
        {
            Debug.LogWarning("🎛️ Slider de bonheur non assigné dans GameManager !");
        }
    }

    private void Update()
    {
        // Met à jour le slider chaque frame
        if (bonheurSlider != null)
        {
            bonheurSlider.maxValue = bonheurMax;
            bonheurSlider.value = bonheur;
        }
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
