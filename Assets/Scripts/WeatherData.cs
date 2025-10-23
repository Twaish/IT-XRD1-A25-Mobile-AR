using System;
using System.Collections.Generic;

[Serializable]
public class WeatherData
{
    public Location location;
    public Current current;
    public Forecast forecast;
}

[Serializable]
public class Location
{
    public string name;
    public string region;
    public string country;
    public float lat;
    public float lon;
    public string tz_id;
    public long localtime_epoch;
    public string localtime;
}

[Serializable]
public class BaseWeatherMetrics
{
    public float temp_c;
    public float temp_f;
    public int is_day;
    public Condition condition;
    public float wind_mph;
    public float wind_kph;
    public int wind_degree;
    public string wind_dir;
    public float pressure_mb;
    public float pressure_in;
    public float precip_mm;
    public float precip_in;
    public int humidity;
    public int cloud;
    public float feelslike_c;
    public float feelslike_f;
    public float windchill_c;
    public float windchill_f;
    public float heatindex_c;
    public float heatindex_f;
    public float dewpoint_c;
    public float dewpoint_f;
    public float vis_km;
    public float vis_miles;
    public float uv;
    public float gust_mph;
    public float gust_kph;
    public float? short_rad;
    public float? diff_rad;
    public float? dni;
    public float? gti;
}

[Serializable]
public class Current : BaseWeatherMetrics
{
    public long last_updated_epoch;
    public string last_updated;
}

[Serializable]
public class Condition
{
    public string text;
    public string icon;
    public int code;
}

[Serializable]
public class Forecast
{
    public ForecastDay[] forecastday;
}

[Serializable]
public class ForecastDay
{
    public string date;
    public long date_epoch;
    public Day day;
    public Astro astro;
    public Hour[] hour;
}

[Serializable]
public class Day
{
    public float maxtemp_c;
    public float maxtemp_f;
    public float mintemp_c;
    public float mintemp_f;
    public float avgtemp_c;
    public float avgtemp_f;
    public float maxwind_mph;
    public float maxwind_kph;
    public float totalprecip_mm;
    public float totalprecip_in;
    public float totalsnow_cm;
    public float avgvis_km;
    public float avgvis_miles;
    public int avghumidity;
    public int daily_will_it_rain;
    public int daily_chance_of_rain;
    public int daily_will_it_snow;
    public int daily_chance_of_snow;
    public Condition condition;
    public float uv;
}

[Serializable]
public class Astro
{
    public string sunrise;
    public string sunset;
    public string moonrise;
    public string moonset;
    public string moon_phase;
    public string moon_illumination;
    public int is_moon_up;
    public int is_sun_up;
}

[Serializable]
public class Hour : BaseWeatherMetrics
{
    public long time_epoch;
    public string time;
    public float snow_cm;
    public int will_it_rain;
    public int chance_of_rain;
    public int will_it_snow;
    public int chance_of_snow;
}