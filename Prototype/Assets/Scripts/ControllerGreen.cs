using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGreen : MonoBehaviour
{
    public Material material; 
    public Color emissionColorOn; 
    public Color emissionColorOff; 

    private bool canGrab;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            SetEmission(canGrab);
        }
    }

    void Update()
    {
        canGrab = FindObjectOfType<HeroCharacterController>().canGrab;
        
        SetEmission(canGrab);
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