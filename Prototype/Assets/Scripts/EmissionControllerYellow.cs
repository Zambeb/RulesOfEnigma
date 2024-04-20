using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControllerYellow : MonoBehaviour
{
    public Material material;
    public Material orbMaterial;
    public GameObject[] orbGlowing;
    public Color emissionColorOn;
    public Color emissionColorOff;
    public Color emissionColorOnOrb;
    public Color emissionColorOffOrb;

    private bool canJump;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            SetEmission(canJump);
        }
        
        if (orbMaterial != null && renderer != null)
        {
            orbMaterial.EnableKeyword("_EMISSION");
            SetEmission(canJump);
        }

        if (orbGlowing != null && renderer != null)
        {
            foreach (GameObject orbObject in orbGlowing)
            {
                orbObject.SetActive(canJump);
            }
        }
    }

    void Update()
    {
        canJump = FindObjectOfType<HeroCharacterController>().canJump;
        SetEmission(canJump);
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
                orbObject.SetActive(canJump);
            }
        }
    }
}
