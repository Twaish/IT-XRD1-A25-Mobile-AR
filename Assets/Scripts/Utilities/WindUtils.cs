using UnityEngine;

public static class WindUtils
{
  // Translate wind speed and direction to force vector
  public static Vector3 CalculateWindVector(float windKph, float windDegree, float modifier = 1f)
  {
    float windMs = windKph / 3.6f; // km/h to m/s
    float angleRad = (windDegree + 180f) * Mathf.Deg2Rad;

    float x = Mathf.Sin(angleRad) * windMs * modifier;
    float z = Mathf.Cos(angleRad) * windMs * modifier;

    return new Vector3(x, 0, z);
  }  
}
