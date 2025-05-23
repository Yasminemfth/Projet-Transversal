using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class SafeBracelet : MonoBehaviour
{
    [Header("Spawn & Respawn Settings")]
    [Tooltip("Délai avant la première apparition du bracelet (en secondes)")]
    public float initialSpawnDelay = 20f;
    [Tooltip("Après usage, délai avant réapparition du bracelet (en secondes)")]
    public float respawnDelay = 20f;
    [Tooltip("Délai avant le respawn des vagues (en secondes)")]
    public float waveResetDelay = 15f;
    [Tooltip("Point où le bracelet réapparait (laisser null pour 0,0,0)")]
    public Transform respawnPoint;

    private Collider2D     _collider2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _collider2D    = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D.isTrigger = true;

        // On cache immédiatement le bracelet
        _spriteRenderer.enabled = false;
        _collider2D.enabled     = false;
    }

    private void Start()
    {
        // On attend initialSpawnDelay avant de faire apparaître le bracelet
        StartCoroutine(InitialSpawnCoroutine());
    }

    private IEnumerator InitialSpawnCoroutine()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        SpawnBracelet();
        Debug.Log("[SafeBracelet] Apparition initiale après " + initialSpawnDelay + "s");
    }

    private void SpawnBracelet()
    {
        // Positionne au point de respawn si défini
        transform.position = respawnPoint != null 
            ? respawnPoint.position 
            : Vector3.zero;

        // Réactive visuel & collider
        _spriteRenderer.enabled = true;
        _collider2D.enabled     = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_collider2D.enabled || !other.CompareTag("Player")) return;

        // Le joueur ramasse le bracelet
        inventory.instance?.PickupToken(this);

        // On cache immédiatement pour l'inventaire
        _spriteRenderer.enabled = false;
        _collider2D.enabled     = false;
    }

    /// <summary>
    /// Appelé par inventory quand on appuie sur la touche d’usage
    /// </summary>
    public void Use()
    {
        // 1) Détruire tous les ennemis actifs
        var ennemis = EnemyWaveManager.instance.GetActiveEnemies();
        foreach (var go in ennemis)
            if (go != null) Destroy(go);
        ennemis.Clear();

        // 2) Relancer les vagues après waveResetDelay
        StartCoroutine(ResetWavesAfterDelay());

        // 3) Programmer le respawn du bracelet
        StartCoroutine(RespawnBracelet());
    }

    private IEnumerator ResetWavesAfterDelay()
    {
        yield return new WaitForSeconds(waveResetDelay);
        EnemyWaveManager.instance.ClearEnemiesAndResetWave();
    }

    private IEnumerator RespawnBracelet()
    {
        // Attend respawnDelay
        yield return new WaitForSeconds(respawnDelay);

        // Réaffiche et repositionne le bracelet
        SpawnBracelet();
        Debug.Log("[SafeBracelet] Réapparu après usage");
    }
}
