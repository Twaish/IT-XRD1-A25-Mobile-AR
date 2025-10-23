using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ApiConfig
{
  public string WeatherApiKey;
  public string Location;
}

public class WeatherApiConfigLoader
{
  public static ApiConfig LoadWeatherApiConfig()
  {
    string path = Path.Combine(Application.streamingAssetsPath, "Config/weather_api.json");

    if (!File.Exists(path))
    {
      Debug.LogError("API key file not found! Returning a dummy object.");
      return new ApiConfig
      {
        //Used to handle APK where paths gets corrupt due to compression
        WeatherApiKey = "*************************",
        Location = "N/A"
      };
    }

    string json = File.ReadAllText(path);
    return JsonUtility.FromJson<ApiConfig>(json);
  }
}

public class WeatherService : MonoBehaviour
{
  private string apiKey;
  private string location;
  private string cacheFileName = "weather_cache.json";
  private float cacheDuration = 600f; // in seconds

  [SerializeField] private int forecastDays = 3;

  [Header("Debug Overrides")]
  [SerializeField] private bool enableDebugMode = false;
  [SerializeField] private float debugPrecipitation;
  [SerializeField] private float debugWindKph;
  [SerializeField] private int debugWindDegree;
  public WeatherSlider hourlyWeatherSlider;
  private WeatherData cachedWeather;
  private DateTime lastFetchTime;

  private string CacheFilePath => Path.Combine(Application.persistentDataPath, cacheFileName);

  public event Action<WeatherData> OnWeatherUpdated;

  private float lastDebugPrecip;
  private float lastDebugWindKph;
  private int lastDebugWindDegree;

  private void Awake()
  {
    ApiConfig apiConfig = WeatherApiConfigLoader.LoadWeatherApiConfig();
    if (apiConfig.WeatherApiKey == string.Empty)
    {
      Debug.LogError("WeatherService: API key is empty. Please check the weather_api.json file.");
      enabled = false;
      return;
    }
    if (apiConfig.Location == string.Empty)
    {
      Debug.LogError("WeatherService: Location is empty. Please check the weather_api.json file.");
      enabled = false;
      return;
    }
    apiKey = apiConfig.WeatherApiKey;
    location = apiConfig.Location;
  }

  private void Start()
  {
    LoadCachedWeather();

    if (cachedWeather != null)
      OnWeatherUpdated?.Invoke(cachedWeather);
    else
      FetchWeather(location);
  }

  private void Update()
  {
    if (!enableDebugMode || cachedWeather == null) return;

    if (debugPrecipitation != lastDebugPrecip ||
      debugWindKph != lastDebugWindKph ||
      debugWindDegree != lastDebugWindDegree)
    {
      cachedWeather.current.precip_mm = debugPrecipitation;
      cachedWeather.current.wind_kph = debugWindKph;
      cachedWeather.current.wind_degree = debugWindDegree;

      OnWeatherUpdated?.Invoke(cachedWeather);

      lastDebugPrecip = debugPrecipitation;
      lastDebugWindKph = debugWindKph;
      lastDebugWindDegree = debugWindDegree;
    }
  }

  private void SaveWeatherCache(string json)
  {
    try
    {
      File.WriteAllText(CacheFilePath, json);
    }
    catch (Exception e)
    {
      Debug.LogError($"Failed to save weather cache: {e}");
    }
  }

  private void LoadCachedWeather()
  {
    if (File.Exists(CacheFilePath))
    {
      try
      {
        string json = File.ReadAllText(CacheFilePath);
        cachedWeather = JsonUtility.FromJson<WeatherData>(json);
        lastFetchTime = DateTime.Now;
      }
      catch (Exception e)
      {
        Debug.LogError($"Failed to load weather cache: {e}");
      }
    }
  }

  public void FetchWeather(string city)
  {
    FetchWeather(city, null);
  }
  public void FetchWeather(string city, Action<WeatherData> onComplete)
  {
    if (cachedWeather != null && (DateTime.Now - lastFetchTime).TotalSeconds < cacheDuration)
    {
      onComplete?.Invoke(cachedWeather);
      return;
    }

    StartCoroutine(GetWeatherCoroutine(city, onComplete));
  }

  private IEnumerator GetWeatherCoroutine(string city, Action<WeatherData> onComplete)
  {
    string url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days={forecastDays}&aqi=no&alerts=no";
    using UnityWebRequest request = UnityWebRequest.Get(url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      string json = request.downloadHandler.text;
      WeatherData weatherData = JsonUtility.FromJson<WeatherData>(json);

      cachedWeather = weatherData;
      lastFetchTime = DateTime.Now;

      SaveWeatherCache(json);

      onComplete?.Invoke(weatherData);
    }
    else
    {
      Debug.LogError($"Weather API Error: {request.error}");
      onComplete?.Invoke(cachedWeather);
    }
  }

  public event Action<WeatherData> OnForecastUpdated;

  public void FetchForecast(string location, int days = 3, Action<WeatherData> onComplete = null)
  {
    StartCoroutine(GetForecastCoroutine(location, days, onComplete));
  }

  private IEnumerator GetForecastCoroutine(string location, int days, Action<WeatherData> onComplete)
  {
    string url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={location}&days={days}&aqi=no&alerts=no";

    using UnityWebRequest request = UnityWebRequest.Get(url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      string json = request.downloadHandler.text;
      WeatherData forecastResponse = JsonUtility.FromJson<WeatherData>(json);

      hourlyWeatherSlider.SetWeatherJsonData(json);
      Debug.Log("WeatherAPI Response:\n" + json);
      OnForecastUpdated?.Invoke(forecastResponse);
    }
    else
    {
      Debug.LogError($"Forecast API Error: {request.error}\nResponse Code: {request.responseCode}");
      onComplete?.Invoke(null);
    }
  }

}
