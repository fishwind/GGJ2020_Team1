using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public Transform camTransform;

    public float shakeDuration = 0f;

    public float shakeAmount = 0.5f;
    private float currShakeAmount;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (CameraShake.Instance != null)
        {
            Destroy(this);
        }
        Instance = this;

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * currShakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
            currShakeAmount = shakeAmount;
        }
    }

    public void Shake(float duration)
    {
        shakeDuration = duration;
    }

    public void Shake(float duration, float amount)
    {
        shakeDuration = duration;
        currShakeAmount = amount;
    }

}