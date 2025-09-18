using UnityEngine;

public class WeatherUIButtonHandler : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private LocationServiceManager locationProvider;

    public void ShowHourly()
    {
        string location = locationProvider.GetWeatherApiLocationString();
        if (location != "Unknown")
        {
            weatherService.FetchForecast(location, 1);
        }
        else
        {
            Debug.LogWarning("Location is not yet ready. Cannot fetch hourly forecast.");
        }
    }

    public void ShowThreeDay()
    {
        string location = locationProvider.GetWeatherApiLocationString();
        if (location != "Unknown")
        {
            weatherService.FetchForecast(location, 3);
        }
        else
        {
            Debug.LogWarning("Location is not yet ready. Cannot fetch 3-day forecast.");
        }
    }
}