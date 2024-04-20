using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    public GameObject textKeyboard;
    public GameObject textMouse;

    public HeroCharacterController characterController;
    
    public float pulseHeight = 0.01f;
    public float pulseSpeed = 2f;

    void Start()
    {
        characterController.GetComponent<HeroCharacterController>();
        textKeyboard.SetActive(false);
        textMouse.SetActive(false);
    }
    
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * pulseSpeed) * pulseHeight;
        
        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (characterController.controlMouse)
            {
                textMouse.SetActive(true);
            }
            else
            {
                textKeyboard.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMouse.SetActive(false);
            textKeyboard.SetActive(false);
        }
    }
}