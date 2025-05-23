using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Conditions de défaite")]
    [Tooltip("Temps imparti en secondes pour atteindre 100 de bonheur")]
    public float timeLimit = 300f;

    [Header("UI")]
    [Tooltip("Panel Game Over à afficher")]
    public GameObject gameOverPanel;
    [Tooltip("TextMeshPro pour afficher la raison du Game Over")]
    public TextMeshProUGUI gameOverReasonText;

    [Header("Scenes")]
    [Tooltip("Nom exact de la scène du menu principal")]
    public string mainMenuSceneName = "MainMenu";

    private float _elapsedTime = 0f;
    private bool _isGameOver = false;

    void Awake()
    {
        _elapsedTime = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (_isGameOver) return;

        // 1) Vérifier le bonheur
        if (GameManager.instance != null && GameManager.instance.bonheur <= 0f)
        {
            TriggerGameOver("Vous avez perdu tout votre bonheur !");
            return;
        }

        // 2) Vérifier le temps écoulé
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= timeLimit &&
            GameManager.instance != null &&
            GameManager.instance.bonheur < 100f)
        {
            TriggerGameOver("Le temps est écoulé avant d'atteindre 100 de bonheur !");
        }
    }

    private void TriggerGameOver(string reason)
    {
        _isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverReasonText != null)
            gameOverReasonText.text = reason;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Debug.Log("Game Over : " + reason);
    }

    /// <summary>
    /// Relance la partie via un bouton UI
    /// </summary>
    public void OnPlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Retourne au menu principal via un bouton UI
    /// </summary>
    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log($"Loading main menu scene '{mainMenuSceneName}'");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
