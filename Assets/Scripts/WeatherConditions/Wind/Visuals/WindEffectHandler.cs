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

    Vector3 windVec = WindUtils.CalculateWindVector(windKph, windDegree, windModifier);

    ForceOverLifetimeModule forceModule = windParticleSystem.forceOverLifetime;
    forceModule.enabled = true;
    forceModule.x = new MinMaxCurve(windVec.x);
    forceModule.z = new MinMaxCurve(windVec.z);
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
