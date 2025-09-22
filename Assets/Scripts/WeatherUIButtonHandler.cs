using UnityEngine;

public class WeatherUIButtonHandler : MonoBehaviour
{
    [SerializeField] private WeatherSlider weatherSlider;

    public void ShowHourly()
    {
        weatherSlider.SwitchToHourly();
    }

    public void ShowThreeDay()
    {
        weatherSlider.SwitchToDaily();
    }
}
