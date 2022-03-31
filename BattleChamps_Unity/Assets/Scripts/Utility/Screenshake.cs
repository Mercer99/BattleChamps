using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Screenshake : MonoBehaviour
{
    private float shakeTimeRemaining, shakeIntensity, fadeTime;
    public float shakeMovementLimits = 0.2f;
    public float shakeDuration = 0.1f;
    public float shakeIntensityMultiplier = 1f;

    public Vignette pp_Vignette;

    private void Start()
    {
        Volume volume = GetComponent<Volume>();
        if (volume.profile.TryGet<Vignette>(out pp_Vignette))
        {
            pp_Vignette.intensity.value = 0f;
        }

        isShaking = false;
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-shakeMovementLimits, shakeMovementLimits) * shakeIntensity;
            float yAmount = Random.Range(-shakeMovementLimits, shakeMovementLimits) * shakeIntensity;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakeIntensity = Mathf.MoveTowards(shakeIntensity, 0f, fadeTime * Time.deltaTime);
        }
    }

    public void StartShake(float intensityMultiplyer)
    {
        shakeTimeRemaining = shakeDuration;
        shakeIntensity = shakeIntensityMultiplier + intensityMultiplyer;

        fadeTime = shakeIntensityMultiplier / shakeDuration;

        ActivateVignette(shakeDuration);
    }

    public void ActivateVignette(float duration)
    {
        StartCoroutine(FadeVignette(duration));
        if (!isShaking)
        { StartCoroutine(FadeVignette(duration)); }
    }

    private bool isShaking = false;

    private IEnumerator FadeVignette(float duration)
    {
        isShaking = true;

        DOTween.Sequence()
            .Append(DOTween.To(() => pp_Vignette.intensity.value, x => pp_Vignette.intensity.value = x, 0.25f, duration/2))
            //.AppendInterval(duration)
            .Append(DOTween.To(() => pp_Vignette.intensity.value, x => pp_Vignette.intensity.value = x, 0, duration/2))
            .OnComplete(() =>
            {
                isShaking = false;
            });

        yield return null;
    }
}
