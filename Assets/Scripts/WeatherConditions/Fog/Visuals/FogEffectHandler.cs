using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FogEffectHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem fogParticleSystem;

    private ShapeModule shapeModule;

    private void Awake()
    {
        if (fogParticleSystem == null)
        {
            Debug.LogError("FogEffectHandler: No ParticleSystem assigned.");
            enabled = false;
            return;
        }

        shapeModule = fogParticleSystem.shape;
    }

    public void UpdateFogByVisibility(float visibilityKm)
    {
        float clampedVis = Mathf.Clamp(visibilityKm, 0.1f, 10f);
        float x = Mathf.Lerp(5f, 50f, clampedVis / 10f);
        shapeModule.scale = new Vector3(x, x, x);
    }
}
