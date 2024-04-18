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
    
    public float minVinIntensity = 0.2f;  // Минимальная интенсивность виньетки
    public float maxVinIntensity = 0.7f;  // Максимальная интенсивность виньетки
    //public float changeSpeed = 0.5f;  // Скорость изменения интенсивности
    private float currentIntensity;  // Текущая интенсивность
    
    public float minBloomIntensity = 0.2f;  // Минимальная интенсивность виньетки
    public float maxBloomIntensity = 0.7f;  // Максимальная интенсивность виньетки
    //public float changeSpeed = 0.5f;  // Скорость изменения интенсивности
    private float currentBloomIntensity;  // Текущая интенсивность
    // Start is called before the first frame update
    void Start()
    {
        postProcessingVolume.profile.TryGet(out _vignette);
        currentIntensity = _vignette.intensity.value;
        postProcessingVolume.profile.TryGet(out _bloom);
        currentBloomIntensity = _bloom.intensity.value;
    }

    // Update is called once per frame
    void Update()
    {
        // Генерация случайного значения интенсивности в заданном диапазоне
        float targetIntensity = Random.Range(minVinIntensity, maxVinIntensity);
        
        // Изменение интенсивности виньетки
        _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, targetIntensity, Time.deltaTime);
        currentIntensity = _vignette.intensity.value;
        
        float targetBloomIntensity = Random.Range(minBloomIntensity, maxBloomIntensity);
        
        // Изменение интенсивности виньетки
        _bloom.intensity.value = targetBloomIntensity;
        currentBloomIntensity = _bloom.intensity.value;
    }
}
