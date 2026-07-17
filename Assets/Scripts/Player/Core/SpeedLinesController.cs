using System;
using UnityEngine;

public class SpeedLinesController : MonoBehaviour
{
    [SerializeField] private Material speedLinesMaterial;
    [SerializeField] private float fadeSpeed;
    
    private static readonly int INTENSITY_ID = Shader.PropertyToID("_Intensity");
    private float targetIntensity;
    private float currentIntensity;

    private void Awake()
    {
        SetIntensityImmediate(0f);
    }

    private void Update()
    {
        currentIntensity = Mathf.MoveTowards(
            currentIntensity,
            targetIntensity,
            fadeSpeed * Time.deltaTime
        );

        speedLinesMaterial.SetFloat(INTENSITY_ID, currentIntensity);
    }

    public void SetIntensityImmediate(float intensity)
    {
        currentIntensity = Mathf.Clamp01(intensity);
        targetIntensity = currentIntensity;
        
        speedLinesMaterial.SetFloat(INTENSITY_ID, currentIntensity);
    }

    public void SetIntensity(float intensity)
    {
        targetIntensity = Mathf.Clamp01(intensity);
    }

    private void OnDisable()
    {
        speedLinesMaterial.SetFloat(INTENSITY_ID, 0f);
    }
}