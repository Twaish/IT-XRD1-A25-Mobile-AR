using UnityEngine;
using System.Collections;
using TMPro;

public class WeatherUIManager : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private LocationServiceManager locationProvider;
    [SerializeField] private WeatherSlider weatherSlider;

    public TextMeshProUGUI forecastText;

    private WeatherData cachedForecast;

    private void Start()
    {
        weatherService.OnForecastUpdated += OnForecastReceived;

        string defaultLocation = "Buenaventura";
        weatherService.FetchForecast(defaultLocation, 3);
        Debug.Log($"Using default location: {defaultLocation}");

        StartCoroutine(WaitForAndFetchRealLocation());
    }

    private IEnumerator WaitForAndFetchRealLocation()
    {
        string realLocation = locationProvider.GetWeatherApiLocationString();

        while (realLocation == "Unknown")
        {
            yield return new WaitForSeconds(1f);
            realLocation = locationProvider.GetWeatherApiLocationString();
        }

        Debug.Log($"Real location found: {realLocation}. Updating weather data.");
        weatherService.FetchForecast(realLocation, 3);
    }

    private void OnForecastReceived(WeatherData forecast)
    {
        if (forecast == null || forecast.forecast == null || forecast.forecast.forecastday.Length == 0)
        {
            forecastText.text = "No forecast data available.";
            Debug.LogError("No forecast received.");
            return;
        }

        cachedForecast = forecast;

        var today = forecast.forecast.forecastday[0];
        forecastText.text = $"Forecast for {today.date}:\n" +
                            $"Max Temp: {today.day.maxtemp_c}°C\n" +
                            $"Min Temp: {today.day.mintemp_c}°C\n" +
                            $"Condition: {today.day.condition.text}\n" +
                            $"Rain Chance: {today.day.daily_chance_of_rain}%";

        weatherSlider.SetForecast(cachedForecast);
    }
}
