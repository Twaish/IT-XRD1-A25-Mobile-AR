using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class WeatherSlider : MonoBehaviour
{
    public Slider weatherSlider;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI temperatureText;
    public Button toggleButton;
    public TextMeshProUGUI toggleButtonText;

    private ForecastResponse weatherData;

    private bool showingDaily = true; // Start in daily mode
    private int selectedDayIndex = 0; // Which day’s hours to show in hourly mode

    private void Awake()
    {
        if (weatherSlider != null)
        {
            weatherSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("Weather Slider is not assigned in the Inspector!");
        }

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleMode);
        }
        else
        {
            Debug.LogError("Toggle Button is not assigned in the Inspector!");
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
                SetupDailyMode();
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

    private void ToggleMode()
    {
        showingDaily = !showingDaily;

        if (showingDaily)
        {
            SetupDailyMode();
        }
        else
        {
            SetupHourlyMode(selectedDayIndex);
        }
    }

    private void SetupDailyMode()
    {
        weatherSlider.minValue = 0;
        weatherSlider.maxValue = weatherData.forecast.forecastday.Length - 1;
        weatherSlider.wholeNumbers = true;
        weatherSlider.value = selectedDayIndex; // keep track of last selected day

        toggleButtonText.text = "Switch to Hourly";
        UpdateWeatherDisplay((int)weatherSlider.value);
    }

    private void SetupHourlyMode(int dayIndex)
    {
        if (dayIndex < 0 || dayIndex >= weatherData.forecast.forecastday.Length)
        {
            Debug.LogError("Day index out of range.");
            return;
        }

        weatherSlider.minValue = 0;
        weatherSlider.maxValue = weatherData.forecast.forecastday[dayIndex].hour.Length - 1;
        weatherSlider.wholeNumbers = true;
        weatherSlider.value = 0; // start at first hour

        toggleButtonText.text = "Switch to Daily";
        UpdateWeatherDisplay((int)weatherSlider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateWeatherDisplay((int)value);
    }

    private void UpdateWeatherDisplay(int index)
    {
        if (weatherData == null || weatherData.forecast.forecastday.Length == 0)
        {
            Debug.LogError("Weather data is not available.");
            return;
        }

        if (showingDaily)
        {
            // Show day info
            selectedDayIndex = index; // save which day user selected
            var dayData = weatherData.forecast.forecastday[index];

            headerText.text = "Date: " + dayData.date;
            temperatureText.text = "Avg Temp: " + dayData.day.avgtemp_c + "°C";
        }
        else
        {
            // Show hourly info for selected day
            var dayData = weatherData.forecast.forecastday[selectedDayIndex];
            if (index < 0 || index >= dayData.hour.Length)
            {
                Debug.LogError("Hour index out of range.");
                return;
            }

            var hourlyData = dayData.hour[index];
            headerText.text = "Time: " + hourlyData.time;
            temperatureText.text = "Temp: " + hourlyData.temp_c + "°C";
        }
    }
}
