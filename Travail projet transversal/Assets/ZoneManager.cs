using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour
{
    public List<AutoPointZone> zones;
    public float switchInterval = 10f;

    private AutoPointZone zoneActive;

    void Start()
    {
        StartCoroutine(SwitchZonesLoop());
    }

    IEnumerator SwitchZonesLoop()
    {
        while (true)
        {
            ActiverUneZoneAleatoire();
            yield return new WaitForSeconds(switchInterval);
        }
    }

    void ActiverUneZoneAleatoire()
    {
        if (zones.Count == 0) return;

        // Désactiver toutes les zones (grises)
        foreach (var zone in zones)
            zone.SetZoneActive(false);

        // Activer une au hasard (verte)
        int index = Random.Range(0, zones.Count);
        zoneActive = zones[index];
        zoneActive.SetZoneActive(true);
    }
}
