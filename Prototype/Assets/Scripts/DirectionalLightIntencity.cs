using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightIntensity : MonoBehaviour
{
    public Light directionalLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float changeSpeed = 1.0f; 

    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
            if (directionalLight == null)
            {
                Debug.LogError("Directional Light component not found.");
            }
        }
    }

    void Update()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        
        float newIntensity = Mathf.Lerp(directionalLight.intensity, targetIntensity, changeSpeed * Time.deltaTime);
        
        directionalLight.intensity = newIntensity;
    }
}