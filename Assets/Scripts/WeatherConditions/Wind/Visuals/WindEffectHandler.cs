using UnityEngine;

public class WindEffectHandler : MonoBehaviour
{
  [SerializeField] private ParticleSystem windParticleSystem;

  [Header("Modifiers")]
  [Tooltip("Wind strength scaling factor for streaks")]
  [SerializeField] private float windModifier = 1f;

  private float lastWindKph;
  private float lastWindDegree;
  private Vector3 currentWindVec;
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

    if (CompassManager.Instance != null)
    {
      float deviceHeading = CompassManager.Instance.GetHeading();
      Quaternion rotation = Quaternion.Euler(0, -deviceHeading, 0);
      windVec = rotation * windVec;
    }

    var forceModule = windParticleSystem.forceOverLifetime;
    forceModule.enabled = true;
    forceModule.x = new ParticleSystem.MinMaxCurve(windVec.x);
    forceModule.z = new ParticleSystem.MinMaxCurve(windVec.z);

    windParticleSystem.transform.rotation = Quaternion.LookRotation(currentWindVec.normalized);
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
