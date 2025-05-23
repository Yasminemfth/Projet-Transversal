using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour
{
    [Header("Liste de toutes les zones à gérer")]
    public List<AutoPointZone> toutesLesZones;

    [Header("Temps entre les changements de zone")]
    public float tempsEntreChangements = 10f;

    private void Start()
    {
        StartCoroutine(ActiverZonesSuccessivement());
    }

    IEnumerator ActiverZonesSuccessivement()
    {
        while (true)
        {
            if (toutesLesZones == null || toutesLesZones.Count == 0)
            {
                Debug.LogWarning(" Aucune zone assignée dans ZoneManager !");
                yield break;
            }

            // Choisir une zone aléatoire
            int index = Random.Range(0, toutesLesZones.Count);
            AutoPointZone zoneActive = toutesLesZones[index];

            // Désactiver toutes les autres
            foreach (var zone in toutesLesZones)
                zone.ForceSetState(false);

            // Activer celle choisie
            zoneActive.ForceSetState(true);
            Debug.Log($" Zone activée : {zoneActive.gameObject.name}");

            yield return new WaitForSeconds(tempsEntreChangements);
        }
    }
}