using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI Victory")]
    [Tooltip("Panel à afficher pour la victoire")]
    public GameObject victoryPanel;

    [Header("Scenes")]
    [Tooltip("Nom exact de la scène du menu principal")]
    public string mainMenuSceneName = "MainMenu";

    private bool _hasWon = false;

    void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    void Update()
    {
        if (_hasWon) return;

        if (GameManager.instance != null && GameManager.instance.bonheur >= 100f)
        {
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        _hasWon = true;
        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Debug.Log("Victory triggered");
    }

    public void OnPlayAgain()
    {
        Time.timeScale = 1f;
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log($"Loading main menu scene '{mainMenuSceneName}'");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
