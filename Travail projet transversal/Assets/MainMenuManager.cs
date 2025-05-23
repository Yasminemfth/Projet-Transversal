using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Tooltip("Nom exact de la scène de jeu à charger")]
    public string gameSceneName = "travailfini";

    // Relié au bouton Play → On Click()
    public void OnPlayPressed()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Relié au bouton Quit → On Click()
    public void OnQuitPressed()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
