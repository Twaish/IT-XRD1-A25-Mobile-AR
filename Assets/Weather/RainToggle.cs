using UnityEngine;

public class RainToggle : MonoBehaviour
{
    public ParticleSystem rainEffect;
    private bool isRaining = true;

    public void ToggleRain()
    {
        if (isRaining)
        {
            rainEffect.Stop();
            isRaining = false;
        }
        else
        {
            rainEffect.Play();
            isRaining = true;
        }
    }
}