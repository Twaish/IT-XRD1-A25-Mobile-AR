using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
#if UNITY_AR_FOUNDATION
using UnityEngine.XR.ARFoundation;
#endif

public class CompassManager : MonoBehaviour
{
    public static CompassManager Instance { get; private set; }

    [Header("UI Settings")]
    [SerializeField] private Image compassUI;
    [SerializeField] private TextMeshProUGUI compassText;

    [Header("AR Foundation (optional)")]
#if UNITY_AR_FOUNDATION
    [SerializeField] private ARSessionOrigin arSessionOrigin;
    [SerializeField] private Camera arCamera;
#endif

    private bool compassReady = false;
    private bool fallbackARorGyro = false;

    private float currentHeading = 0f;
    private float baselineYaw = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartCompassService());
    }

    private IEnumerator StartCompassService()
    {
        Debug.Log("Starting Compass Service...");

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("Location not enabled. Compass may not work.");
        }
        else
        {
            Input.location.Start();
        }

        Input.compass.enabled = true;

        float timeout = 10f; 
        while (Input.compass.timestamp == 0f && timeout > 0f)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (Input.compass.enabled && Input.compass.timestamp > 0f)
        {
            compassReady = true;
            Debug.Log($"Compass enabled successfully. TrueHeading: {Input.compass.trueHeading}");
        }
        else
        {
            fallbackARorGyro = true;
            Debug.LogWarning("Compass not providing valid data. Falling back to gyro/AR.");

#if UNITY_AR_FOUNDATION
            if (arCamera != null)
            {
                baselineYaw = arCamera.transform.eulerAngles.y;
                Debug.Log($"Baseline yaw from AR camera: {baselineYaw}");
            }
            else
#endif
            {
                if (SystemInfo.supportsGyroscope)
                {
                    Quaternion deviceRotation = Input.gyro.attitude;
                    deviceRotation = new Quaternion(deviceRotation.x, deviceRotation.y, -deviceRotation.z, -deviceRotation.w);
                    deviceRotation = Quaternion.Euler(90, 0, 0) * deviceRotation;
                    baselineYaw = deviceRotation.eulerAngles.y;
                    Debug.Log($"Baseline yaw from gyro: {baselineYaw}");
                }
                else
                {
                    Debug.LogWarning("No valid fallback available. Heading will remain 0.");
                }
            }
        }
    }

    private void Update()
    {
        if (compassReady)
        {
            currentHeading = Input.compass.trueHeading;
            if (float.IsNaN(currentHeading))
                currentHeading = 0f;
        }
        else if (fallbackARorGyro)
        {
#if UNITY_AR_FOUNDATION
            if (arCamera != null)
            {
                currentHeading = arCamera.transform.eulerAngles.y - baselineYaw;
                if (currentHeading < 0f) currentHeading += 360f;
            }
            else
#endif
            {
                currentHeading = GetGyroHeading();
            }
        }
        else
        {
            currentHeading = 0f;
        }

        if (compassUI != null)
            compassUI.rectTransform.localRotation = Quaternion.Lerp(compassUI.rectTransform.localRotation,
                                                                  Quaternion.Euler(0, 0, -currentHeading),
                                                                  Time.deltaTime * 5f);

        if (compassText != null)
            compassText.text = $"{currentHeading:0}°";
    }

    private float GetGyroHeading()
    {
        if (!SystemInfo.supportsGyroscope) return 0f;

        Quaternion deviceRotation = Input.gyro.attitude;
        deviceRotation = new Quaternion(deviceRotation.x, deviceRotation.y, -deviceRotation.z, -deviceRotation.w);
        deviceRotation = Quaternion.Euler(90, 0, 0) * deviceRotation;

        float yaw = deviceRotation.eulerAngles.y;
        float heading = yaw - baselineYaw;
        if (heading < 0f) heading += 360f;

        return heading;
    }

    public bool IsCompassReady() => compassReady || fallbackARorGyro;
    public float GetHeading() => currentHeading;
    public Quaternion GetHeadingRotation() => Quaternion.Euler(0, currentHeading, 0);
}
