using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LocationServiceManager : MonoBehaviour
{
    public static LocationServiceManager Instance { get; private set; }

    private bool locationReady = false;
    private LocationInfo currentLocation;
    private string cityName = "";
    private string countryName = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("Location services are not enabled by the user.");
            yield break;
        }

        Input.location.Start(1f, 0.1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.LogWarning("Location service timed out.");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Unable to determine device location.");
            yield break;
        }

        locationReady = true;
        currentLocation = Input.location.lastData;

        StartCoroutine(FetchCityAndCountry(currentLocation.latitude, currentLocation.longitude));
    }

    private void Update()
    {
        if (locationReady)
        {
            currentLocation = Input.location.lastData;
        }
    }

    public bool IsLocationReady()
    {
        return locationReady;
    }

    public Vector2 GetLatitudeLongitude()
    {
        return locationReady ? new Vector2(currentLocation.latitude, currentLocation.longitude) : Vector2.zero;
    }

    public string GetWeatherApiLocationString()
    {
        if (locationReady)
        {
            return $"{currentLocation.latitude},{currentLocation.longitude}";
        }
        else
        {
            return "Unknown";
        }
    }

    public string GetCity()
    {
        return string.IsNullOrEmpty(cityName) ? "Unknown" : cityName;
    }

    public string GetCountry()
    {
        return string.IsNullOrEmpty(countryName) ? "Unknown" : countryName;
    }

    public string GetCityAndCountry()
    {
        if (string.IsNullOrEmpty(cityName) && string.IsNullOrEmpty(countryName))
            return "Unknown";

        return $"{cityName}, {countryName}";
    }

    private IEnumerator FetchCityAndCountry(float latitude, float longitude)
    {
        string url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude}&lon={longitude}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("User-Agent", "UnityLocationApp");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var json = request.downloadHandler.text;
                var data = JsonUtility.FromJson<NominatimResponse>(json);

                if (data != null && data.address != null)
                {
                    cityName = !string.IsNullOrEmpty(data.address.city) ? data.address.city :
                               !string.IsNullOrEmpty(data.address.town) ? data.address.town :
                               !string.IsNullOrEmpty(data.address.village) ? data.address.village : "Unknown";

                    countryName = !string.IsNullOrEmpty(data.address.country) ? data.address.country : "Unknown";

                    Debug.Log($"Detected location: {cityName}, {countryName}");
                }
            }
            else
            {
                Debug.LogWarning("Failed to fetch city and country: " + request.error);
            }
        }
    }

    [System.Serializable]
    private class NominatimResponse
    {
        public Address address;
    }

    [System.Serializable]
    private class Address
    {
        public string city;
        public string town;
        public string village;
        public string country;
    }
}