using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwitch : MonoBehaviour
{
    public int cableCount;
    
    public GameObject lever;
    private Quaternion initialLeverRotation;
    private bool leverRotated;
    public WaterBossAnimator waterBossAnimator;
    public AudioSource cryingSound;

    public GameObject light;
    public GameObject sparks;

    void Start()
    {
        WaterBossAnimator waterBossAnimator = GetComponent<WaterBossAnimator>();
        waterBossAnimator.Idle();
        cableCount = 0;
        initialLeverRotation = lever.transform.rotation;
        light.SetActive(false);
        sparks.SetActive(false);
    }

    void Update()
    {
        if (cableCount >= 2)
        {
            light.SetActive(true);
        }
    }

    public void Activate()
    {
        RotateLever();
        if (cableCount >= 2)
        {
            Win();
        }
    }

    void Win()
    {
        waterBossAnimator.Death();
        sparks.SetActive(true);
        //deathSource.PlayOneShot(deathSound);
        cryingSound.enabled = false;
        Debug.Log("You won");
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
