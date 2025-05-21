// GameManager.cs
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Bonheur")]
    public float bonheur = 100f;
    public float bonheurMax = 100f;
    public bool limiterBonheurMax = true;

    [Header("UI")] 
    public Slider bonheurSlider;
    public Image bonheurFillImage;
    public Gradient bonheurCouleur;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (bonheurSlider != null)
        {
            bonheurSlider.maxValue = bonheurMax;
            bonheurSlider.value = bonheur;
            UpdateBonheurUI();
        }
    }

    public void AjouterBonheur(float valeur)
    {
        bonheur += valeur;

        if (limiterBonheurMax && bonheur > bonheurMax)
            bonheur = bonheurMax;

        UpdateBonheurUI();
        Debug.Log("Bonheur : " + (int)bonheur);
    }

    public void RetirerBonheur(float valeur)
    {
        bonheur -= valeur;

        if (bonheur < 0f)
            bonheur = 0f;

    
    }

    private void UpdateBonheurUI()
    {
        if (bonheurSlider != null)
        {
            bonheurSlider.value = bonheur;
            if (bonheurFillImage != null && bonheurCouleur != null)
                bonheurFillImage.color = bonheurCouleur.Evaluate(bonheurSlider.normalizedValue);
        }
    }
} 
