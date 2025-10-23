using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForecastWeatherStats : MonoBehaviour
{
    [Header("References")]
    public WeatherService weatherService;

    [Header("UI Elements")]
    public TextMeshProUGUI day1Text;
    public TextMeshProUGUI temperatureMin1Text;
    public TextMeshProUGUI temperatureMax1Text;
    public Image weather1Icon;

    public TextMeshProUGUI day2Text;
    public TextMeshProUGUI temperatureMin2Text;
    public TextMeshProUGUI temperatureMax2Text;
    public Image weather2Icon;

    public TextMeshProUGUI day3Text;
    public TextMeshProUGUI temperatureMin3Text;
    public TextMeshProUGUI temperatureMax3Text;
    public Image weather3Icon;

    [Header("Weather Icons")]
    public Sprite sunnyIcon;
    public Sprite cloudyIcon;
    public Sprite rainyIcon;
    public Sprite snowyIcon;
    public Sprite windyIcon;

    [Header("Settings")]
    [Tooltip("Wind speed (km/h) Threshold for windy icon")]
    [Range(0f, 100f)]
    public float windyThreshold = 25f;

    private void OnEnable()
    {
        if (weatherService != null)
            weatherService.OnForecastUpdated += UpdateForecastUI;

        // Request forecast when enabled
        weatherService.FetchForecast("auto:ip", 3);
    }

    private void OnDisable()
    {
        if (weatherService != null)
            weatherService.OnForecastUpdated -= UpdateForecastUI;
    }

    private void UpdateForecastUI(WeatherData data)
    {
        if (data == null || data.forecast == null || data.forecast.forecastday == null)
        {
            Debug.LogError("ForecastWeatherStats: Forecast data is null or incomplete!");
            return;
        }

        var days = data.forecast.forecastday;

        // Handle each day safely
        if (days.Length > 0)
            SetDayUI(days[0], day1Text, temperatureMin1Text, temperatureMax1Text, weather1Icon, 0);
        if (days.Length > 1)
            SetDayUI(days[1], day2Text, temperatureMin2Text, temperatureMax2Text, weather2Icon, 1);
        if (days.Length > 2)
            SetDayUI(days[2], day3Text, temperatureMin3Text, temperatureMax3Text, weather3Icon, 2);
    }

    private void SetDayUI(
        ForecastDay dayData,
        TextMeshProUGUI dayText,
        TextMeshProUGUI minTempText,
        TextMeshProUGUI maxTempText,
        Image iconImage,
        int dayOffset
    )
    {
        if (dayData == null || dayData.day == null)
            return;

        // 🗓️ Display day name
        if (dayText != null)
        {
            string dayName = DateTime.Now.AddDays(dayOffset).ToString("dddd");
            dayText.text = dayName;
        }

        // 🌡️ Temperatures
        if (minTempText != null)
            minTempText.text = $"{dayData.day.mintemp_c:F0}°C";
        if (maxTempText != null)
            maxTempText.text = $"{dayData.day.maxtemp_c:F0}°C";

        // 🌦️ Icon
        string condition = dayData.day.condition.text.ToLower();
        float windKph = dayData.day.maxwind_kph;

        Sprite icon = GetWeatherIcon(condition, windKph);
        if (iconImage != null && icon != null)
            iconImage.sprite = icon;
    }

    private Sprite GetWeatherIcon(string condition, float windKph)
    {
        // 🌬️ Windy override
        if (windKph >= windyThreshold)
            return windyIcon;

        // ☀️ Basic condition matching
        if (condition.Contains("sun") || condition.Contains("clear"))
            return sunnyIcon;

        if (condition.Contains("cloud"))
            return cloudyIcon;

        if (
            condition.Contains("rain")
            || condition.Contains("drizzle")
            || condition.Contains("fog")
            || condition.Contains("mist")
            || condition.Contains("haze")
        )
            return rainyIcon;

        if (condition.Contains("snow"))
            return snowyIcon;

        // Default fallback
        return cloudyIcon;
    }
}
