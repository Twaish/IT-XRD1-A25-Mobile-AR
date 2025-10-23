using UnityEngine;
using SunCalcNet;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering;

[RequireComponent(typeof(WeatherService))]
public class SunLocationManager : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private Transform sunPrefab;
    [SerializeField] private Camera arCamera;

    private float initialHeading;
    private bool headingInitialized = false;

    private Transform sunInstance;
    private LocationInfo currentLocation;
    private readonly string[] sunVisibleKeywords = new string[]
    {
        "sunny", "clear", "partly", "mostly sunny", "partly cloudy", "mostly clear", "fair"
    };

    private void Awake()
    {
        if (weatherService == null)
        {
            weatherService = FindObjectOfType<WeatherService>();
            if (weatherService == null)
            {
                Debug.LogError("SunLocationManager: WeatherService not found.");
                enabled = false;
                return;
            }
        }

        if (sunPrefab == null)
        {
            Debug.LogError("SunLocationManager: SunPrefab not assigned.");
            enabled = false;
            return;
        }

        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        weatherService.OnWeatherUpdated += OnWeatherUpdated;
        StartCoroutine(StartLocationService());
        Input.compass.enabled = true;
        StartCoroutine(InitializeHeading());
    }

    private void OnDisable()
    {
        weatherService.OnWeatherUpdated -= OnWeatherUpdated;
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("SunLocationManager: Location services not enabled by user.");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("SunLocationManager: Unable to determine device location.");
            yield break;
        }

        currentLocation = Input.location.lastData;
        Input.compass.enabled = true;
    }
    private IEnumerator InitializeHeading()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (CompassManager.Instance != null)
            initialHeading = CompassManager.Instance.GetHeading();
        else
            initialHeading = Input.compass.trueHeading;

        headingInitialized = true;
        Debug.Log("Initial heading captured: " + initialHeading);
    }
    private void OnWeatherUpdated(WeatherData weather)
    {
        if (weather == null || arCamera == null) return;

        string condition = weather.current.condition?.text?.ToLower() ?? "";
        bool shouldShowSun = sunVisibleKeywords.Any(keyword => condition.Contains(keyword));

        if (!shouldShowSun)
        {
            SetSunActive(false);
            Debug.Log($"SunLocationManager: Sun hidden due to weather condition '{condition}'.");
            return;
        }

        SetSunActive(true);

        double latitude = weather.location.lat != 0 ? weather.location.lat : currentLocation.latitude;
        double longitude = weather.location.lon != 0 ? weather.location.lon : currentLocation.longitude;

        var sunPos = SunCalc.GetSunPosition(DateTime.Now, latitude, longitude);

        float azimuthDeg = Mathf.Rad2Deg * (float)sunPos.Azimuth;
        float altitudeDeg = Mathf.Rad2Deg * (float)sunPos.Altitude;

        Vector3 dir = SunDirection(azimuthDeg, altitudeDeg);

        float heading = CompassManager.Instance != null
            ? CompassManager.Instance.GetHeading()
            : Input.compass.trueHeading;

        dir = Quaternion.Euler(0, -heading, 0) * dir;
        Vector3 sunWorldPos = arCamera.transform.position + dir * 10f;

        if (sunInstance == null)
        {
            sunInstance = Instantiate(sunPrefab, sunWorldPos, Quaternion.LookRotation(-dir));
        }
        else
        {
            sunInstance.position = sunWorldPos;
            sunInstance.rotation = Quaternion.LookRotation(-dir);
        }

        float cloudAmount = weather.current.cloud; // 0–100
        float baseIntensity = 10f;

        // Scale intensity inversely with cloud cover
        // (100% cloud → 0.2 intensity, 0% cloud → 1.0)
        float intensity = Mathf.Lerp(0.2f, 1f, 1f - cloudAmount / 100f) * baseIntensity;

        if (condition.Contains("sunny") || condition.Contains("clear"))
            intensity = Mathf.Min(intensity + 0.2f, 1.2f) * baseIntensity;

        ApplySunIntensity(intensity);

        Debug.Log($"SunLocationManager: Sun visible. Alt: {altitudeDeg:F1}°, Az: {azimuthDeg:F1}° | Intensity: {intensity:F2} | Condition: {condition}");
    }


    private Vector3 SunDirection(float azimuthDeg, float altitudeDeg)
    {
        float az = Mathf.Deg2Rad * azimuthDeg;
        float alt = Mathf.Deg2Rad * altitudeDeg;

        float x = Mathf.Cos(alt) * Mathf.Sin(az);
        float y = Mathf.Sin(alt);
        float z = Mathf.Cos(alt) * Mathf.Cos(az);

        return new Vector3(x, y, z).normalized;
    }

    private void SetSunActive(bool active)
    {
        if (sunInstance == null)
        {
            if (active && sunPrefab != null)
            {
                sunInstance = Instantiate(sunPrefab);
                sunInstance.gameObject.SetActive(true);
            }
            return;
        }

        if (sunInstance.gameObject.activeSelf != active)
        {
            sunInstance.gameObject.SetActive(active);
        }
    }

    private void ApplySunIntensity(float intensity)
    {
        if (sunInstance == null) return;
        var flare = sunInstance.GetComponentInChildren<LensFlareComponentSRP>();
        if (flare != null)
            flare.intensity = intensity;
    }
    
    public void UpdateSunForTime(DateTime time, string condition, Location location, int cloudAmount)
    {
    if (arCamera == null || sunPrefab == null) return;

    double latitude = location.lat != 0 ? location.lat : currentLocation.latitude;
    double longitude = location.lon != 0 ? location.lon : currentLocation.longitude;

    var sunPos = SunCalc.GetSunPosition(time, latitude, longitude);

    float azimuthDeg = Mathf.Rad2Deg * (float)sunPos.Azimuth;
    float altitudeDeg = Mathf.Rad2Deg * (float)sunPos.Altitude;

    bool isSunAboveHorizon = altitudeDeg > 0f;
    if (!isSunAboveHorizon)
    {
        SetSunActive(false);
        Debug.Log($"SunLocationManager: Sun hidden due being under horizon '{altitudeDeg}'.");
        return;
    }

    Vector3 dir = SunDirection(azimuthDeg, altitudeDeg);

    if (!headingInitialized) return;

    dir = Quaternion.Euler(0, -initialHeading, 0) * dir;
    
    Vector3 sunWorldPos = arCamera.transform.position + dir * 40f;

    if (sunInstance == null)
    {
        sunInstance = Instantiate(sunPrefab, sunWorldPos, Quaternion.LookRotation(-dir));
    }
    else
    {
        sunInstance.position = sunWorldPos;
        sunInstance.rotation = Quaternion.LookRotation(-dir);
    }

    // Adjust intensity based on cloud coverage (if available)
    float baseIntensity = 10f;
    float intensity = Mathf.Lerp(0.2f, 1f, 1f - cloudAmount / 100f) * baseIntensity;
    ApplySunIntensity(intensity);

    SetSunActive(true);
    }
}
