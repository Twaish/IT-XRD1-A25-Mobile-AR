using UnityEngine;

[RequireComponent(typeof(LeavesEffectHandler))]
public class LeavesManager : MonoBehaviour
{
  [SerializeField] private WeatherService weatherService;
  private LeavesEffectHandler leavesEffectHandler;

  private void Awake()
  {
    leavesEffectHandler = GetComponent<LeavesEffectHandler>();
    if (leavesEffectHandler == null)
    {
      Debug.LogError("LeavesManager: LeavesEffectHandler not found");
      enabled = false;
      return;
    }

    if (weatherService != null)
    {
      weatherService.OnWeatherUpdated += OnWeatherUpdated;
    }
    else
    {
      Debug.LogError("LeavesManager: WeatherService instance not found");
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

      if (windSpeed > 0.01f)
      {
          leavesEffectHandler.SetActive(true);
          leavesEffectHandler.SetLeavesRate(windSpeed);
          leavesEffectHandler.SetLeavesVelocity(windSpeed, windDegree);
      }
      else
      {
          leavesEffectHandler.SetActive(false);
      }
  }
  
}
