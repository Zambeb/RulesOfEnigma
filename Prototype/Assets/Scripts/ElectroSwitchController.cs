using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroSwitchController : MonoBehaviour
{

    public ElectricityController electroController;
    
    public GameObject lever;
    private Quaternion initialLeverRotation;
    private bool leverRotated;

    public AudioSource audioSource;
    public AudioClip[] switchSounds;
    public AudioClip lampSoundOn;
    public AudioClip lampSoundOff;
    
    void Start()
    {
        ElectricityController electroController = GetComponent<ElectricityController>();
        initialLeverRotation = lever.transform.rotation;
    }

    public void SwitchElectro()
    {
        audioSource.PlayOneShot(switchSounds[Random.Range(0, switchSounds.Length)]);
        electroController.electricityOn = !electroController.electricityOn;
        if (electroController.electricityOn)
        {
            audioSource.PlayOneShot(lampSoundOn);
        }
        else
        {
            audioSource.PlayOneShot(lampSoundOff);
        }
        if (!leverRotated)
        {
            Quaternion targetRotation = initialLeverRotation * Quaternion.Euler(80f, 0f, 0f);
            lever.transform.rotation = targetRotation;
        }
        else
        {
            lever.transform.rotation = initialLeverRotation;
        }

        leverRotated = !leverRotated;
    }
}
