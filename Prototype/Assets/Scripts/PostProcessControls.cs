using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessControls : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;

    [Header("Post Processing Effects")] private Vignette _vignette;
    [Header("Post Processing Effects")] private Bloom _bloom;
    
    public float minVinIntensity = 0.2f;  
    public float maxVinIntensity = 0.7f;  
    //public float changeSpeed = 0.5f;  
    private float currentIntensity; 
    
    public float minBloomIntensity = 0.2f; 
    public float maxBloomIntensity = 0.7f; 
    //public float changeSpeed = 0.5f;  
    private float currentBloomIntensity;

    void Start()
    {
        postProcessingVolume.profile.TryGet(out _vignette);
        currentIntensity = _vignette.intensity.value;
        postProcessingVolume.profile.TryGet(out _bloom);
        currentBloomIntensity = _bloom.intensity.value;
    }
    
    void Update()
    {
        float targetIntensity = Random.Range(minVinIntensity, maxVinIntensity);
        
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, targetIntensity, Time.deltaTime);
        currentIntensity = _vignette.intensity.value;
        
        float targetBloomIntensity = Random.Range(minBloomIntensity, maxBloomIntensity);
        
        _bloom.intensity.value = targetBloomIntensity;
        currentBloomIntensity = _bloom.intensity.value;
    }
}
