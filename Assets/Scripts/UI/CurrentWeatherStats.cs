using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CurrentWeatherUI : MonoBehaviour
{
    [Header("References")]
    public WeatherService weatherService;

    [Header("UI Elements")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI temperatureText;
    public Image weatherIcon;

    [Header("Weather Icons")]
    public Sprite sunnyIcon;
    public Sprite cloudyIcon;
    public Sprite rainyIcon;
    public Sprite snowyIcon;
    public Sprite windyIcon;

    [Tooltip("Wind speed (km/h) Threshold for windy icon")]
    [Range(0f, 100f)]
    public float windyThreshold = 25f;

    private void OnEnable()
    {
        if (weatherService != null)
            weatherService.OnWeatherUpdated += UpdateUI;

        UpdateDayOfWeek();
    }

    private void OnDisable()
    {
        if (weatherService != null)
            weatherService.OnWeatherUpdated -= UpdateUI;
    }

    private void UpdateUI(WeatherData data)
    {
        if (data == null || data.current == null)
        {
            Debug.LogError("CurrentWeatherUI: Weather data is null!");
            return;
        }

        // --- Temperature ---
        temperatureText.text = $"{data.current.temp_c:F1}°C";

        // --- Pick the right image ---
        string condition = data.current.condition.text.ToLower();
        float windKph = data.current.wind_kph;

        Sprite selectedIcon = GetWeatherIcon(condition, windKph);
        if (weatherIcon != null && selectedIcon != null)
            weatherIcon.sprite = selectedIcon;

        UpdateDayOfWeek();
    }

     private void UpdateDayOfWeek()
    {
        if (dayText != null)
        {
            string dayName = DateTime.Now.ToString("dddd"); // e.g. "Monday"
            dayText.text = dayName;
        }
    }

    private Sprite GetWeatherIcon(string condition, float windKph)
    {
        if (windKph >= windyThreshold)
            return windyIcon;
        
        if (condition.Contains("sun") || condition.Contains("clear"))
            return sunnyIcon;

        if (condition.Contains("cloud"))
            return cloudyIcon;

        if (condition.Contains("rain") || condition.Contains("drizzle") || condition.Contains("fog") || condition.Contains("mist") || condition.Contains("haze"))
            return rainyIcon;

        if (condition.Contains("snow"))
            return snowyIcon;

        // Default fallback
        return cloudyIcon;
    }
}

