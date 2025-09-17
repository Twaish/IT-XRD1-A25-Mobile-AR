using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class WeatherResponse
{
  public Location location;
  public Current current;

  [Serializable]
  public class Location
  {
    public string name;
    public string region;
    public string country;
    public float lat;
    public float lon;
    public string tz_id;
    public int localtime_epoch;
    public string localtime;
  }

  [Serializable]
  public class Current
  {
    public float temp_c;
    public float temp_f;
    public int is_day;
    public int last_updated_epoch;
    public string last_updated;

    public Condition condition;

    [Serializable]
    public class Condition
    {
      public string text;
      public string icon;
      public int code;
    }
    public float wind_mph;
    public float wind_kph;
    public int wind_degree;
    public string wind_dir;
    public int pressure_mb;
    public float pressure_in;
    public float precip_mm;
    public float precip_in;
    public int humidity;
    public int cloud;
    public float feelslike_c;
    public float feelslike_f;
    public float windchill_c;
    public float windchill_f;
    public float heatindex_c;
    public float heatindex_f;
    public float dewpoint_c;
    public float dewpoint_f;
    public float vis_km;
    public float vis_miles;
    public float uv;
    public float gust_mph;
    public float gust_kph;
  }
}

[Serializable]
public class ApiConfig
{
  public string WeatherApiKey;
  public string Location;
}

[Serializable]
public class ForecastResponse
{
    public Location location;
    public Current current;
    public Forecast forecast;

    [Serializable]
    public class Location
    {
        public string name;
        public string region;
        public string country;
        public float lat;
        public float lon;
        public string tz_id;
        public int localtime_epoch;
        public string localtime;
    }

    [Serializable]
    public class Current
    {
        public float temp_c;
        public float temp_f;
        public int is_day;
        public int last_updated_epoch;
        public string last_updated;
        public Condition condition;
        public float wind_mph;
        public float wind_kph;
        public int wind_degree;
        public string wind_dir;
        public int pressure_mb;
        public float pressure_in;
        public float precip_mm;
        public float precip_in;
        public int humidity;
        public int cloud;
        public float feelslike_c;
        public float feelslike_f;
        public float windchill_c;
        public float windchill_f;
        public float heatindex_c;
        public float heatindex_f;
        public float dewpoint_c;
        public float dewpoint_f;
        public float vis_km;
        public float vis_miles;
        public float uv;
        public float gust_mph;
        public float gust_kph;
    }

    [Serializable]
    public class Forecast
    {
        public ForecastDay[] forecastday;
    }

    [Serializable]
    public class ForecastDay
    {
        public string date;
        public Day day;
        public Hour[] hour;
    }

    [Serializable]
    public class Day
    {
        public float maxtemp_c;
        public float mintemp_c;
        public float avgtemp_c;
        public float maxwind_kph;
        public float totalprecip_mm;
        public int daily_chance_of_rain;
        public Condition condition;
    }

    [Serializable]
    public class Hour
    {
        public string time;
        public float temp_c;
        public int humidity;
        public float wind_kph;
        public Condition condition;
    }

    [Serializable]
    public class Condition
    {
        public string text;
        public string icon;
        public int code;
    }
}

public class WeatherApiConfigLoader
{
  public static ApiConfig LoadWeatherApiConfig()
  {
    string path = Path.Combine(Application.streamingAssetsPath, "Config/weather_api.json");

    if (!File.Exists(path))
    {
      Debug.LogError("API key file not found!");
      return null;
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

  [Header("Debug Overrides")]
  [SerializeField] private bool enableDebugMode = false;
  [SerializeField] private float debugPrecipitation;
  [SerializeField] private float debugWindKph;
  [SerializeField] private int debugWindDegree;

  private WeatherResponse cachedWeather;
  private DateTime lastFetchTime;

  private string CacheFilePath => Path.Combine(Application.persistentDataPath, cacheFileName);

  public event Action<WeatherResponse> OnWeatherUpdated;

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
        cachedWeather = JsonUtility.FromJson<WeatherResponse>(json);
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
  public void FetchWeather(string city, Action<WeatherResponse> onComplete)
  {
    if (cachedWeather != null && (DateTime.Now - lastFetchTime).TotalSeconds < cacheDuration)
    {
      onComplete?.Invoke(cachedWeather);
      return;
    }

    StartCoroutine(GetWeatherCoroutine(city, onComplete));
  }

  private IEnumerator GetWeatherCoroutine(string city, Action<WeatherResponse> onComplete)
  {
    string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}";
    using UnityWebRequest request = UnityWebRequest.Get(url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
      string json = request.downloadHandler.text;
      WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(json);

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

  public event Action<ForecastResponse> OnForecastUpdated;

  public void FetchForecast(string city, int days = 3, Action<ForecastResponse> onComplete = null)
  {
      StartCoroutine(GetForecastCoroutine(city, days, onComplete));
  }

  private IEnumerator GetForecastCoroutine(string city, int days, Action<ForecastResponse> onComplete)
  {
      string url = $"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days={days}&aqi=no&alerts=no";

      using UnityWebRequest request = UnityWebRequest.Get(url);
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success)
      {
          string json = request.downloadHandler.text;

          Debug.Log("WeatherAPI Response:\n" + json);

          ForecastResponse forecastData = JsonUtility.FromJson<ForecastResponse>(json);
          OnForecastUpdated?.Invoke(forecastData);
          onComplete?.Invoke(forecastData);
      }
      else
      {
          Debug.LogError($"Forecast API Error: {request.error}\nResponse Code: {request.responseCode}");
          onComplete?.Invoke(null);
      }
  }

}
