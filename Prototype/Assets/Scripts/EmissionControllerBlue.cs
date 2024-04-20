using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControllerBlue : MonoBehaviour
{
    public Material material;
    public Material orbMaterial;
    public GameObject[] orbGlowing;
    public Color emissionColorOn; 
    public Color emissionColorOff; 
    public Color emissionColorOnOrb; 
    public Color emissionColorOffOrb; 

    private bool canGrab;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            SetEmission(canGrab);
        }
        
        if (orbMaterial != null && renderer != null)
        {
            orbMaterial.EnableKeyword("_EMISSION");
            SetEmission(canGrab);
        }

        if (orbGlowing != null && renderer != null)
        {
            foreach (GameObject orbObject in orbGlowing)
            {
                orbObject.SetActive(canGrab);
            }
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
        
        if (orbMaterial != null)
        {
            Color targetColor = isEnabled ? emissionColorOnOrb : emissionColorOffOrb;
            material.SetColor("_EmissionColor", targetColor);
        }
        
        if (orbGlowing != null)
        {
            foreach (GameObject orbObject in orbGlowing)
            {
                orbObject.SetActive(canGrab);
            }
        }
    }
}
