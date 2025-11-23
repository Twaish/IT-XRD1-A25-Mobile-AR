using UnityEngine;

public class EffectsTester : MonoBehaviour
{
    [Header("Effect Handlers")]
    [SerializeField] private LeavesEffectHandler leavesEffectHandler;
    [SerializeField] private FogEffectHandler fogEffectHandler;
    [SerializeField] private RainEffectHandler rainEffectHandler;
    [SerializeField] private WindEffectHandler windEffectHandler;

    [Header("Weather Conditions")]
    [SerializeField] private float windSpeed = 60f;
    [SerializeField] private float windDegree = 67f;
    [SerializeField] private float visibility = 0.7f;
    [SerializeField] private float precip = 30f;

    private void Start()
    {
        leavesEffectHandler.SetActive(true);
        rainEffectHandler.SetActive(true);
        windEffectHandler.SetActive(true);
        UpdateAll();
    }

    private void UpdateAll()
    {
        UpdateLeaves();
        UpdateFog();
        UpdateRain();
        UpdateWind();
    }

    private void OnValidate()
    {
        UpdateAll();
    }

    private void UpdateLeaves()
    {
        leavesEffectHandler.SetLeavesRate(windSpeed);
        leavesEffectHandler.SetLeavesVelocity(windSpeed, windDegree);
    }

    private void UpdateFog()
    {
        fogEffectHandler.UpdateFogByVisibility(visibility);
    }

    private void UpdateRain()
    {
        rainEffectHandler.SetRainRate(precip);
        rainEffectHandler.SetRainVelocity(windSpeed, windDegree);
    }

    private void UpdateWind()
    {
        windEffectHandler.SetWind(windSpeed, windDegree);
    }
}
