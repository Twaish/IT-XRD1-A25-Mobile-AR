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
    public ParticleSystem rainParticleSystem;
    public ParticleSystem windParticleSystem;
    private ForecastResponse weatherData;
    public SunLocationManager sunLocationManager;
    public Transform Wind;
    private bool showingDaily = true;
    private int selectedDayIndex = 0;
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
        weatherSlider.value = selectedDayIndex;

        toggleButtonText.text = "Daily";
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
        weatherSlider.value = 0;

        toggleButtonText.text = "Hourly";
        UpdateWeatherDisplay((int)weatherSlider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateWeatherDisplay((int)value);
    }

    private void UpdateRainEffect(float precipitationMm)
    {
        if (rainParticleSystem == null) return;

        var emission = rainParticleSystem.emission;

        if (precipitationMm <= 0f)
        {
            emission.rateOverTime = 0f;
            if (rainParticleSystem.isPlaying) rainParticleSystem.Stop();
        }
        else
        {
            float intensity = Mathf.Clamp(precipitationMm * 50f, 10f, 500f);
            emission.rateOverTime = intensity;

            if (!rainParticleSystem.isPlaying) rainParticleSystem.Play();
        }
    }
    private void UpdateWindEffect(float windKph)
    {
        if (windParticleSystem == null) return;

        var emission = windParticleSystem.emission;
        var main = windParticleSystem.main;

        if (windKph <= 0f)
        {
            emission.rateOverTime = 0f;
            if (windParticleSystem.isPlaying) windParticleSystem.Stop();
        }
        else
        {
            float intensity = Mathf.Clamp(windKph * 10f, 20f, 200f);
            emission.rateOverTime = intensity;
            main.startSpeed = windKph / 5f;

            if (!windParticleSystem.isPlaying) windParticleSystem.Play();
        }
    }

    private void UpdateWeatherDisplay(int index)
    {
        if (weatherData == null || weatherData.forecast.forecastday.Length == 0)
        {
            Debug.LogError("Weather data is not available.");
            return;
        }

        float windDirectionDegrees;

        if (showingDaily)
        {
            selectedDayIndex = index;
            var dayData = weatherData.forecast.forecastday[index];

            headerText.text = "Date: " + dayData.date;
            temperatureText.text = dayData.day.avgtemp_c + "°C";
            UpdateRainEffect(dayData.day.totalprecip_mm);
            UpdateWindEffect(dayData.day.maxwind_kph); 
            windDirectionDegrees = dayData.day.maxwind_kph;
        }
        else
        {
            var dayData = weatherData.forecast.forecastday[selectedDayIndex];
            if (index < 0 || index >= dayData.hour.Length)
            {
                Debug.LogError("Hour index out of range.");
                return;
            }

            var hourlyData = dayData.hour[index];
            headerText.text = "Time: " + hourlyData.time;
            temperatureText.text = hourlyData.temp_c + "°C";
            UpdateRainEffect(hourlyData.precip_mm);
            UpdateWindEffect(hourlyData.wind_kph); 
            windDirectionDegrees = hourlyData.wind_degree;
        }

        if (Wind != null)
        {
            Wind.rotation = Quaternion.Euler(0, windDirectionDegrees, 0);
        }
        
        if (sunLocationManager != null)
        {
            DateTime targetTime;
            var condition = "";

            if (showingDaily)
            {
                var dayData = weatherData.forecast.forecastday[index];
                targetTime = DateTime.Parse(dayData.date);
                condition = dayData.day.condition.text;
            }
            else
            {
                var hourData = weatherData.forecast.forecastday[selectedDayIndex].hour[index];
                targetTime = DateTime.Parse(hourData.time);
                condition = hourData.condition.text;
            }

            sunLocationManager.UpdateSunForTime(targetTime, condition, weatherData.location, weatherData.current.cloud);
        }
    }
    public void SwitchToDaily()
    {
        showingDaily = true;
        SetupDailyMode();
    }

    public void SwitchToHourly()
    {
        showingDaily = false;
        SetupHourlyMode(selectedDayIndex);
    }
    
    public void SetForecast(ForecastResponse forecast)
    {
    if (forecast == null || forecast.forecast == null || forecast.forecast.forecastday.Length == 0)
    {
        Debug.LogError("WeatherSlider: No forecast data provided.");
        return;
    }

    weatherData = forecast; 
    SetupDailyMode();       
    }
}
