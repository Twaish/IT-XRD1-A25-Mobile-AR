using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DailyWeatherSlider : MonoBehaviour
{
    public Slider dailySlider;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI temperatureText;

    private ForecastResponse weatherData;

    private void Awake()
    {
        if (dailySlider != null)
        {
            dailySlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("Daily Slider is not assigned in the Inspector!");
        }
    }

    public void SetWeatherJsonData(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogError("Received an empty or null JSON string.");
            return;
        }

        try
        {
            weatherData = JsonUtility.FromJson<ForecastResponse>(jsonString);

            if (weatherData != null && weatherData.forecast != null && weatherData.forecast.forecastday.Length > 0)
            {
                dailySlider.minValue = 0;
                dailySlider.maxValue = weatherData.forecast.forecastday.Length - 1;
                dailySlider.wholeNumbers = true;

                dailySlider.value = 0;
                UpdateWeatherDisplay((int)dailySlider.value);

                Debug.Log("Daily weather data successfully loaded and slider configured.");
            }
            else
            {
                Debug.LogError("Failed to deserialize weather data. The JSON format might be incorrect.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON deserialization failed: {e.Message}");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateWeatherDisplay((int)value);
    }

    private void UpdateWeatherDisplay(int dayIndex)
    {
        if (weatherData == null || weatherData.forecast.forecastday.Length == 0 || dayIndex < 0 || dayIndex >= weatherData.forecast.forecastday.Length)
        {
            Debug.LogError("Weather data is not available or the day index is out of range.");
            return;
        }

        ForecastResponse.ForecastDay dayData = weatherData.forecast.forecastday[dayIndex];

        dateText.text = "Date: " + dayData.date;
        temperatureText.text = "Avg Temp: " + dayData.day.avgtemp_c + "°C";
    }
}
