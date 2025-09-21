using UnityEngine;
using static UnityEngine.ParticleSystem;

public class RainEffectHandler : MonoBehaviour
{
  [SerializeField] private ParticleSystem rainParticleSystem;

  [Header("Modifiers")]
  [Tooltip("Rain rate modifier to scale emission rate")]
  [SerializeField] private float rateModifier = 10f;
  [Tooltip("Wind effect modifier to scale wind influence on rain")]
  [SerializeField] private float windModifier = 1f;

  // private EmissionModule emissionModule;
  // private VelocityOverLifetimeModule velocityModule;

  private float lastPrecipMm;
  private float lastWindKph;
  private float lastWindDegree;

  private void Awake()
  {
    if (rainParticleSystem == null)
    {
      Debug.LogError("RainEffectHandler: RainParticleSystem not found");
      enabled = false;
      return;
    }

    // emissionModule = rainParticleSystem.emission;
    // velocityModule = rainParticleSystem.velocityOverLifetime;
  }

  public void SetRainRate(float precipMm)
  {
    lastPrecipMm = precipMm;
    EmissionModule emissionModule = rainParticleSystem.emission;
    float clampedPrecip = Mathf.Clamp(precipMm, 0f, 20f);

    float rateOverTime = Mathf.Lerp(0f, 200f, clampedPrecip / 20f);
    emissionModule.rateOverTime = rateOverTime * rateModifier;
  }

  public void SetRainVelocity(float windKph, float windDegree)
  {
    lastWindKph = windKph;
    lastWindDegree = windDegree;
    float windMs = windKph / 3.6f; // km/h to m/s

    float angleRad = (windDegree + 180f) * Mathf.Deg2Rad;

    float x = Mathf.Sin(angleRad) * windMs * windModifier;
    float z = Mathf.Cos(angleRad) * windMs * windModifier;

    VelocityOverLifetimeModule velocityModule = rainParticleSystem.velocityOverLifetime;
    velocityModule.enabled = true;
    velocityModule.x = new MinMaxCurve(x);
    velocityModule.z = new MinMaxCurve(z);
  }

  private void OnValidate()
  {
    SetRainRate(lastPrecipMm);
    SetRainVelocity(lastWindKph, lastWindDegree);
  }
  
  public void SetActive(bool active)
  {
      if (rainParticleSystem == null) return;

      if (active && !rainParticleSystem.isPlaying)
      {
          rainParticleSystem.Play();
      }
      else if (!active && rainParticleSystem.isPlaying)
      {
          rainParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
      }
  }

}
