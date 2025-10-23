using UnityEngine;

[RequireComponent(typeof(FogEffectHandler))]
public class FogManager : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    private FogEffectHandler fogEffectHandler;

    private void Awake()
    {
        fogEffectHandler = GetComponent<FogEffectHandler>();

        if (fogEffectHandler == null)
        {
            Debug.LogError("FogManager: FogEffectHandler not found.");
            enabled = false;
            return;
        }

        if (weatherService != null)
        {
            weatherService.OnWeatherUpdated += OnWeatherUpdated;
        }
        else
        {
            Debug.LogError("FogManager: WeatherService not assigned.");
            enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (weatherService != null)
            weatherService.OnWeatherUpdated -= OnWeatherUpdated;
    }

    private void OnWeatherUpdated(WeatherResponse weather)
    {
        if (weather == null) return;

        float visibility = weather.current.vis_km;
        fogEffectHandler.UpdateFogByVisibility(visibility);
        Debug.Log($"FogManager: Updated fog with visibility = {visibility} km");
    }
}
