using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Panel à afficher quand le jeu est en pause")]
    public GameObject pausePanel;

    [Header("Scenes")]
    [Tooltip("Nom  du menu principal")]
    public string mainMenuSceneName = "MainMenu";

    private bool _isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        // Basculer la pause à l'appui Échap
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0f : 1f;
    }

    /// <summary>
    /// Bouton “Resume” dans le menu pause
    /// </summary>
    public void OnResumePressed()
    {
        if (_isPaused)
            TogglePause();
    }

    /// <summary>
    /// Bouton “Play Again” dans le menu pause : relance la scène actuelle depuis le début
    /// </summary>
    public void OnPlayAgainPressed()
    {
        Time.timeScale = 1f;
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    /// <summary>
    /// Bouton “Main Menu” dans le menu pause
    /// </summary>
    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Bouton “Quit Game” dans le menu pause
    /// </summary>
    public void OnQuitGamePressed()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
