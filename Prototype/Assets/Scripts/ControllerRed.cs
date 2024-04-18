using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRed : MonoBehaviour
{
    public Material material; 
    public Color emissionColorOn; 
    public Color emissionColorOff; 

    private bool canFire;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            SetEmission(canFire);
        }
    }

    void Update()
    {
        canFire = FindObjectOfType<HeroCharacterController>().canFire;
        
        SetEmission(canFire);
    }

    void SetEmission(bool isEnabled)
    {
        if (material != null)
        {
            Color targetColor = isEnabled ? emissionColorOn : emissionColorOff;
            material.SetColor("_EmissionColor", targetColor);
            
        }
    }
}