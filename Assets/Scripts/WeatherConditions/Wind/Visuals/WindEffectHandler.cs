using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindEffectHandler : MonoBehaviour
{
  [SerializeField] private ParticleSystem windParticleSystem;

  [Header("Modifiers")]
  [Tooltip("Wind strength scaling factor for streaks")]
  [SerializeField] private float windModifier = 1f;

  private float lastWindKph;
  private float lastWindDegree;

  private void Awake()
  {
    if (windParticleSystem == null)
    {
      Debug.LogError("WindEffectHandler: WindParticleSystem not found");
      enabled = false;
      return;
    }
  }

  public void SetWind(float windKph, float windDegree)
  {
    lastWindKph = windKph;
    lastWindDegree = windDegree;

    float windMs = windKph / 3.6f; // km/h to m/s

    float angleRad = (windDegree + 180f) * Mathf.Deg2Rad;

    float x = Mathf.Sin(angleRad) * windMs * windModifier;
    float z = Mathf.Cos(angleRad) * windMs * windModifier;

    ForceOverLifetimeModule forceModule = windParticleSystem.forceOverLifetime;
    forceModule.enabled = true;
    forceModule.x = new MinMaxCurve(x);
    forceModule.z = new MinMaxCurve(z);
  }

  private void OnValidate()
  {
    if (windParticleSystem == null) return;
    SetWind(lastWindKph, lastWindDegree);
  }
  
  public void SetActive(bool active)
  {
    if (windParticleSystem == null) return;

    if (active && !windParticleSystem.isPlaying)
    {
      windParticleSystem.Play();
    }
    else if (!active && windParticleSystem.isPlaying)
    {
      windParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
  }

}
