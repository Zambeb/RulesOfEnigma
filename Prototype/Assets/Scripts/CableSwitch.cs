using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSwitch : MonoBehaviour
{
    public GameObject connectPoint;

    public BossSwitch bossSwitch;

    private bool isActive;
    
    public GameObject lever;
    private Quaternion initialLeverRotation;
    private bool leverRotated;
    public AudioSource source;
    public AudioClip splashSound;
    
    void Start()
    {
        BossSwitch bossSwitch = GetComponent<BossSwitch>();
        isActive = false;
        initialLeverRotation = lever.transform.rotation;
    }

    public void ActivateSwitch()
    {
        if (!isActive)
        {
            RotateLever();
            connectPoint.SetActive(false);
            isActive = true;
            bossSwitch.cableCount++;
            source.PlayOneShot(splashSound);
        }

    }
    
    private void RotateLever()
    {
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
