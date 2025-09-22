using UnityEngine;
using UnityEngine.UI;

public class WeatherUIManager : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private LocationServiceManager locationProvider;
    [SerializeField] private WeatherSlider weatherSlider;

    public Text forecastText;

    private ForecastResponse cachedForecast;

    private void Start()
    {
        weatherService.OnForecastUpdated += OnForecastReceived;

        string location = locationProvider.GetWeatherApiLocationString();
        if (location == "Unknown")
        {
            Debug.LogWarning("Location not ready. Using default: Horsens");
            location = "Horsens";
        }

        weatherService.FetchForecast(location, 3);
    }

    private void OnForecastReceived(ForecastResponse forecast)
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
