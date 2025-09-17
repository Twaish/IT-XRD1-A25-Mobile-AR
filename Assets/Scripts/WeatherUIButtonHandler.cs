using UnityEngine;

public class WeatherUIButtonHandler : MonoBehaviour
{
    [SerializeField] private WeatherService weatherService;
    [SerializeField] private string location = "London";

    public void ShowHourly()
    {
        weatherService.FetchForecast(location, 1);
    }

    public void ShowThreeDay()
    {
        weatherService.FetchForecast(location, 3);
    }
}
