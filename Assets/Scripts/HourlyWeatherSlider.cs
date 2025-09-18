using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class HourlyWeatherSlider : MonoBehaviour
{
    public Slider hourlySlider;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI temperatureText;

    private ForecastResponse weatherData;

    private const int HoursPerDay = 24;

    private void Awake()
    {
        if (hourlySlider != null)
        {
            hourlySlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("Hourly Slider is not assigned in the Inspector!");
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
                hourlySlider.minValue = 0;
                hourlySlider.maxValue = weatherData.forecast.forecastday[0].hour.Length - 1;
                hourlySlider.wholeNumbers = true;

                hourlySlider.value = 0;
                UpdateWeatherDisplay((int)hourlySlider.value);

                Debug.Log("Weather data successfully loaded and slider configured.");
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

    private void UpdateWeatherDisplay(int hourIndex)
    {
        if (weatherData == null || weatherData.forecast.forecastday.Length == 0 || hourIndex < 0 || hourIndex >= weatherData.forecast.forecastday[0].hour.Length)
        {
            Debug.LogError("Weather data is not available or the hour index is out of range.");
            return;
        }

        ForecastResponse.Hour hourlyData = weatherData.forecast.forecastday[0].hour[hourIndex];

        timeText.text = "Time: " + hourlyData.time;
        temperatureText.text = "Temp: " + hourlyData.temp_c + "°C";
    }
}