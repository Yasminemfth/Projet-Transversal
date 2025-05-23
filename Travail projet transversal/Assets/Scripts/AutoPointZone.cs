using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoPointZone : MonoBehaviour
{
    [Header("Visuel de la zone")]
    public GameObject zoneVisual;
    public Color couleurActive = new Color(1f, 1f, 1f, 1f);
    public Color couleurInactive = new Color(1f, 1f, 1f, 0.3f);

    [Header("Timing d’activation de la zone")]
    [Tooltip("Temps minimal avant la prochaine activation en secondes")]
    public float minActivationInterval = 5f;
    [Tooltip("Temps maximal avant la prochaine activation en secondes")]
    public float maxActivationInterval = 10f;
    [Tooltip("Durée minimale pendant laquelle la zone reste active en secondes")]
    public float minActiveDuration = 5f;
    [Tooltip("Durée maximale pendant laquelle la zone reste active en secondes")]
    public float maxActiveDuration = 8f;

    [Header("Bonus de bonheur")]
    [Tooltip("Intervalle (s) pour l'ajout de bonheur")]
    public float bonheurTick = 0.5f;
    [Tooltip("Quantité de bonheur ajoutée à chaque tick")]
    public float bonheurAmount = 2f;

    [Header("Notification Ennemi")]
    [Tooltip("Fréquence (s) d'appel à NotifyAllEnemies")]
    public float notifyCooldown = 1f;

    private SpriteRenderer _zoneSprite;
    private bool joueurDansZone = false;
    private bool isActive       = false;
    private float bonheurTimer  = 0f;
    private float notifyTimer   = 0f;
    private Coroutine activationRoutine;

    private void Awake()
    {
        if (zoneVisual != null)
            _zoneSprite = zoneVisual.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_zoneSprite != null)
            SetZoneVisual(false);

        activationRoutine = StartCoroutine(CycleActivation());
    }

    private void Update()
    {
        if (joueurDansZone && isActive)
        {
            // Bonus de bonheur
            bonheurTimer += Time.deltaTime;
            if (bonheurTimer >= bonheurTick)
            {
                GameManager.instance?.AjouterBonheur(bonheurAmount);
                bonheurTimer = 0f;
            }

            // Notification périodique aux ennemis
            notifyTimer += Time.deltaTime;
            if (notifyTimer >= notifyCooldown)
            {
                NotifyAllEnemies();
                notifyTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        joueurDansZone = true;
        if (isActive)
            NotifyAllEnemies();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        joueurDansZone = false;
        ResetAllEnemies();
    }

    private IEnumerator CycleActivation()
    {
        while (true)
        {
            // Attente avant activation
            yield return new WaitForSeconds(Random.Range(minActivationInterval, maxActivationInterval));
            SetZoneState(true);

            // Durée d'activation
            yield return new WaitForSeconds(Random.Range(minActiveDuration, maxActiveDuration));
            SetZoneState(false);
        }
    }

    private void SetZoneState(bool state)
    {
        isActive = state;
        SetZoneVisual(state);

        // Si on active pendant que le joueur est déjà dedans
        if (state && joueurDansZone)
            NotifyAllEnemies();
    }

    public void ForceSetState(bool state)
    {
        if (activationRoutine != null)
            StopCoroutine(activationRoutine);

        SetZoneState(state);
        activationRoutine = StartCoroutine(CycleActivation());
    }

    private void SetZoneVisual(bool state)
    {
        if (_zoneSprite != null)
            _zoneSprite.color = state ? couleurActive : couleurInactive;
    }

    private void NotifyAllEnemies()
    {
        var enemies = EnemyWaveManager.instance.GetActiveEnemies();
        foreach (var go in enemies)
        {
            if (go == null) continue;
            var ai = go.GetComponent<DrunkEnemyAI>();
            if (ai != null)
                ai.AllerVersPointUnique(transform);
        }
    }

    private void ResetAllEnemies()
    {
        var enemies = EnemyWaveManager.instance.GetActiveEnemies();
        foreach (var go in enemies)
        {
            if (go == null) continue;
            var ai = go.GetComponent<DrunkEnemyAI>();
            if (ai != null)
                ai.ReprendrePatrouille();
        }
    }
}
