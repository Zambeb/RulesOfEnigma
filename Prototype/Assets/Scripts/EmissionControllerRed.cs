using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControllerRed : MonoBehaviour
{
    public Material material;
    public Material orbMaterial;
    public GameObject[] orbGlowing;
    public Color emissionColorOn; 
    public Color emissionColorOff; 
    public Color emissionColorOnOrb; 
    public Color emissionColorOffOrb; 

    private bool canFire;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            SetEmission(canFire);
        }
        
        if (orbMaterial != null && renderer != null)
        {
            orbMaterial.EnableKeyword("_EMISSION");
            SetEmission(canFire);
        }

        if (orbGlowing != null && renderer != null)
        {
            foreach (GameObject orbObject in orbGlowing)
            {
                orbObject.SetActive(canFire);
            }
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
        
        if (orbMaterial != null)
        {
            Color targetColor = isEnabled ? emissionColorOnOrb : emissionColorOffOrb;
            material.SetColor("_EmissionColor", targetColor);
        }
        
        if (orbGlowing != null)
        {
            foreach (GameObject orbObject in orbGlowing)
            {
                orbObject.SetActive(canFire);
            }
        }
    }
}
