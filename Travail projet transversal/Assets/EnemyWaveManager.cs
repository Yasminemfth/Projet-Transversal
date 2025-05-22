using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class EnemyType
{
    [Tooltip("Prefab de l'ennemi")] public GameObject prefab;
    [Tooltip("Points de patrouille spécifiques à ce type d'ennemi")] public Transform[] patrolPoints;
}

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager instance;

    [Header("Types d'ennemis")]
    [Tooltip("Liste des types d'ennemis et leurs patterns")] public EnemyType[] enemyTypes;

    [Header("Spawning Settings")]
    [Tooltip("Point unique d'apparition (hors caméra)")] public Transform spawnPoint;
    [Tooltip("Nombre d'ennemis par vague")] public int waveSize = 2;
    [Tooltip("Délai avant la première vague (secondes)")] public float initialDelay = 15f;
    [Tooltip("Intervalle entre chaque vague (secondes)")] public float waveInterval = 15f;
    [Tooltip("Nombre total d'ennemis à faire apparaître sur la partie")] public int totalEnemies = 8;

    private int _spawnedCount = 0;
    private List<GameObject> _activeEnemies = new List<GameObject>();
    private Coroutine _waveRoutine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _waveRoutine = StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        while (_spawnedCount < totalEnemies)
        {
            SpawnWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void SpawnWave()
    {
        int toSpawn = Mathf.Min(waveSize, totalEnemies - _spawnedCount);
        for (int i = 0; i < toSpawn; i++)
        {
            if (enemyTypes == null || enemyTypes.Length == 0 || spawnPoint == null)
            {
                Debug.LogWarning("EnemyWaveManager: types d'ennemis ou spawnPoint non configurés.");
                return;
            }

            // Choix aléatoire du type d'ennemi
            EnemyType type = enemyTypes[Random.Range(0, enemyTypes.Length)];
            if (type.prefab == null)
                continue;

            // Instanciation
            GameObject go = Instantiate(type.prefab, spawnPoint.position, spawnPoint.rotation);
            _activeEnemies.Add(go);
            _spawnedCount++;

            // Assignation du pattern et démarrage de la patrouille
            var ai = go.GetComponent<DrunkEnemyAI>();
            if (ai != null)
            {
                ai.cheminPatrouille = type.patrolPoints;
                ai.ActiverPatrouilleNormale();
            }
        }
    }

    /// <summary>
    /// Détruit tous les ennemis actifs et relance le cycle de vagues.
    /// </summary>
    public void ClearEnemiesAndResetWave()
    {
        if (_waveRoutine != null)
            StopCoroutine(_waveRoutine);
        foreach (var go in _activeEnemies)
            if (go != null) Destroy(go);
        _activeEnemies.Clear();
        _spawnedCount = 0;
        _waveRoutine = StartCoroutine(WaveRoutine());
    }

    /// <summary>
    /// Retourne la liste des ennemis actuellement actifs.
    /// </summary>
    public List<GameObject> GetActiveEnemies() => _activeEnemies;
}
