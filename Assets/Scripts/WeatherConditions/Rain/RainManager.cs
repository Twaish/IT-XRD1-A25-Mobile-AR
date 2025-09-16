using UnityEngine;

[RequireComponent(typeof(RainEffectHandler))]
public class RainManager : MonoBehaviour
{
  [SerializeField] private WeatherService weatherService;
  private RainEffectHandler rainEffectHandler;

  private void Awake()
  {
    rainEffectHandler = GetComponent<RainEffectHandler>();
    if (rainEffectHandler == null)
    {
      Debug.LogError("RainManager: RainEffectHandler not found");
      enabled = false;
      return;
    }

    if (weatherService != null)
    {
      weatherService.OnWeatherUpdated += OnWeatherUpdated;
    }
    else
    {
      Debug.LogError("RainManager: WeatherService instance not found");
      enabled = false;
    }
  }

  private void OnDestroy()
  {
    if (weatherService != null)
    {
      weatherService.OnWeatherUpdated -= OnWeatherUpdated;
    }
  }

  private void OnWeatherUpdated(WeatherResponse weather)
  {
    if (weather == null) return;

    float precip = weather.current.precip_mm;
    float windSpeed = weather.current.wind_kph;
    float windDegree = weather.current.wind_degree;

    rainEffectHandler.SetRainRate(precip);
    rainEffectHandler.SetRainVelocity(windSpeed, windDegree);
    Debug.Log($"RainManager: Updated rain with precip {precip} mm, wind {windSpeed} kph at {windDegree}°");
  }
  
}
