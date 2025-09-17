using UnityEngine;
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

    private void DisplayForecast(ForecastResponse forecast)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var hour in forecast.forecast.forecastday[0].hour)
        {
            GameObject item = Instantiate(forecastItemPrefab, contentParent);
            var texts = item.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = hour.time;
            texts[1].text = $"{hour.temp_c}°C";
            texts[2].text = hour.condition.text;
        }
    }
}
