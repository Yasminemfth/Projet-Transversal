using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Tooltip("Temps de départ en secondes (180s = 3 minutes)")]
    public float startTime = 180f;

    [Tooltip("Référence au composant TextMeshProUGUI pour l'affichage")]
    public TextMeshProUGUI timerText;

    private float _remainingTime;

    void Start()
    {
        _remainingTime = startTime;
        UpdateDisplay();
    }

    void Update()
    {
        if (_remainingTime > 0f)
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime < 0f) _remainingTime = 0f;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(_remainingTime / 60f);
        int seconds = Mathf.FloorToInt(_remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
