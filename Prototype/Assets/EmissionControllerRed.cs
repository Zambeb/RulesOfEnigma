using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControllerRed : MonoBehaviour
{
    public Material material;
    public Material orbMaterial;// Ссылка на материал с параметром Emission
    public GameObject[] orbGlowing;
    public Color emissionColorOn; // Цвет для включенного Emission
    public Color emissionColorOff; // Цвет для выключенного Emission
    public Color emissionColorOnOrb; // Цвет для включенного Emission
    public Color emissionColorOffOrb; // Цвет для выключенного Emission

    private bool canFire;

    void Start()
    {
        // Получаем компонент Renderer объекта
        Renderer renderer = GetComponent<Renderer>();

        // Убеждаемся, что у объекта есть материал и компонент Renderer
        if (material != null && renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            // Изначально устанавливаем цвет Emission в соответствии с canJump
            SetEmission(canFire);
        }
        
        if (orbMaterial != null && renderer != null)
        {
            orbMaterial.EnableKeyword("_EMISSION");
            // Изначально устанавливаем цвет Emission в соответствии с canJump
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
        // Проверяем значение canJump из контроллера персонажа
        canFire = FindObjectOfType<HeroCharacterController>().canFire;

        // Устанавливаем цвет Emission в соответствии с canJump
        SetEmission(canFire);
    }

    void SetEmission(bool isEnabled)
    {
        if (material != null)
        {
            // Устанавливаем цвет Emission в зависимости от состояния canJump
            Color targetColor = isEnabled ? emissionColorOn : emissionColorOff;
            material.SetColor("_EmissionColor", targetColor);

            // Если вы используете стандартный материал, может понадобиться включить Emission вручную
            //material.EnableKeyword("_EMISSION");
        }
        
        if (orbMaterial != null)
        {
            // Устанавливаем цвет Emission в зависимости от состояния canJump
            Color targetColor = isEnabled ? emissionColorOnOrb : emissionColorOffOrb;
            material.SetColor("_EmissionColor", targetColor);

            // Если вы используете стандартный материал, может понадобиться включить Emission вручную
            //material.EnableKeyword("_EMISSION");
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
