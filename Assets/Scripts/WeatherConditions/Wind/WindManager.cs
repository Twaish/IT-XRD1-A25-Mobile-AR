using UnityEngine;

[RequireComponent(typeof(WindEffectHandler))]
public class WindManager : MonoBehaviour
{
  [SerializeField] private WeatherService weatherService;
  private WindEffectHandler windEffectHandler;

  private void Awake()
  {
    windEffectHandler = GetComponent<WindEffectHandler>();
    if (windEffectHandler == null)
    {
      Debug.LogError("WindManager: WindEffectHandler not found");
      enabled = false;
      return;
    }

    if (weatherService != null)
    {
      weatherService.OnWeatherUpdated += OnWeatherUpdated;
    }
    else
    {
      Debug.LogError("WindManager: WeatherService instance not found");
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

  private void OnWeatherUpdated(WeatherData weather)
  {
    if (weather == null) return;

    float windSpeed = weather.current.wind_kph;
    float windDegree = weather.current.wind_degree;

    if (windSpeed > 0.8f)
    {
      windEffectHandler.SetActive(true);
      windEffectHandler.SetWind(windSpeed, windDegree);
      Debug.Log($"WindManager: Wind active at {windSpeed} kph, {windDegree}°");
    }
    else
    {
      windEffectHandler.SetActive(false);
      Debug.Log("WindManager: Calm conditions");
    }
  }

}
