using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherUIManager : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject forecastItemPrefab;

    private void Start()
    {
        weatherService.OnForecastUpdated += DisplayForecast;
        weatherService.FetchForecast("London", 3);
    }

    public Text forecastText;

    public void DisplayForecast(ForecastResponse forecast)
    {
        if (forecast?.forecast?.forecastday != null && forecast.forecast.forecastday.Length > 0)
        {
            ForecastResponse.ForecastDay today = forecast.forecast.forecastday[0];

            string forecastInfo = $"Forecast for {today.date}:\n" +
                                  $"Max Temp: {today.day.maxtemp_c}°C\n" +
                                  $"Min Temp: {today.day.mintemp_c}°C\n" +
                                  $"Condition: {today.day.condition.text}\n" +
                                  $"Rain Chance: {today.day.daily_chance_of_rain}%";
            
            forecastText.text = forecastInfo;
            Debug.Log("Forecast data successfully displayed.");
        }
        else
        {
            // Handle the case where no forecast data is available
            forecastText.text = "No forecast data available.";
            Debug.LogError("WeatherUIManager: No forecast data received or forecast array is empty.");
        }
    }
}
