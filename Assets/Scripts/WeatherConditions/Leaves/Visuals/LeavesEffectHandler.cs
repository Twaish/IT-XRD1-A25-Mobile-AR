using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LeavesEffectHandler : MonoBehaviour
{
  [SerializeField] private ParticleSystem leavesParticleSystem;

  [Header("Modifiers")]
  [Tooltip("Leaves rate modifier to scale emission rate")]
  [SerializeField] private float rateModifier = 1f;
  [Tooltip("Wind effect modifier to scale wind influence on leaves")]
  [SerializeField] private float windModifier = 1f;

  private float lastWindKph;
  private float lastWindDegree;

  private void Awake()
  {
    if (leavesParticleSystem == null)
    {
      Debug.LogError("LeavesEffectHandler: LeavesParticleSystem not found");
      enabled = false;
      return;
    }
  }

  public void SetLeavesRate(float windKph)
  {
    EmissionModule emissionModule = leavesParticleSystem.emission;
    float clampedWindKph = Mathf.Clamp(windKph, 0f, 20f);

    float rateOverTime = Mathf.Lerp(0f, 20f, clampedWindKph / 20f);
    emissionModule.rateOverTime = rateOverTime * rateModifier;
  }

  public void SetLeavesVelocity(float windKph, float windDegree)
  {
    lastWindKph = windKph;
    lastWindDegree = windDegree;

    Vector3 windVec = WindUtils.CalculateWindVector(windKph, windDegree, windModifier);

    VelocityOverLifetimeModule velocityModule = leavesParticleSystem.velocityOverLifetime;
    velocityModule.enabled = true;
    velocityModule.x = new MinMaxCurve(windVec.x);
    velocityModule.z = new MinMaxCurve(windVec.z);
  }

  private void OnValidate()
  {
    SetLeavesRate(lastWindKph);
    SetLeavesVelocity(lastWindKph, lastWindDegree);
  }
  
  public void SetActive(bool active)
  {
    if (leavesParticleSystem == null) return;

    if (active && !leavesParticleSystem.isPlaying) {
      leavesParticleSystem.Play();
    } else if (!active && leavesParticleSystem.isPlaying) {
      leavesParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
  }

}
