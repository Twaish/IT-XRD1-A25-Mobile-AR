using UnityEngine;
using SunCalcNet;
using System;
using System.Collections;

public class SunLocation : MonoBehaviour
{
    private LocationInfo currentLocation;

    public Transform sunPrefab;
    public Camera arCamera;

    IEnumerator Start()
    {
        // Ask for location permission
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services not enabled by user.");
            yield break;
        }

        // Start location service
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If service didn't initialize
        if (maxWait <= 0 || Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("Unable to determine device location.");
            yield break;
        }

        // Success: grab location
        currentLocation = Input.location.lastData;

        Debug.Log($"Location: {currentLocation.latitude}, {currentLocation.longitude}");

        // Get sun position
        var sunPos = SunCalc.GetSunPosition(DateTime.Now, currentLocation.latitude, currentLocation.longitude);

        Debug.Log($"Sun Altitude: {sunPos.Altitude} rad, Azimuth: {sunPos.Azimuth} rad");

        Input.compass.enabled = true;
        // Convert to Unity direction
        Vector3 dir = SunDirection(Mathf.Rad2Deg * (float)sunPos.Azimuth, Mathf.Rad2Deg * (float)sunPos.Altitude);

        // Get compass heading (degrees from North)
        float heading = Input.compass.trueHeading;

        // Rotate the direction so it aligns with real-world North
        dir = Quaternion.Euler(0, -heading, 0) * dir;

        // Place sun object 10m away in that direction
        Vector3 sunWorldPos = arCamera.transform.position + dir * 10f;
        Instantiate(sunPrefab, sunWorldPos, Quaternion.LookRotation(-dir));
    }

    // Convert azimuth/altitude to Unity direction vector
    Vector3 SunDirection(float azimuthDeg, float altitudeDeg)
    {
        azimuthDeg = 180f;
        float az = Mathf.Deg2Rad * azimuthDeg;
        float alt = Mathf.Deg2Rad * altitudeDeg;

        float x = Mathf.Cos(alt) * Mathf.Sin(az);
        float y = Mathf.Sin(alt);
        float z = Mathf.Cos(alt) * Mathf.Cos(az);

        return new Vector3(x, y, z).normalized;
    }
}
