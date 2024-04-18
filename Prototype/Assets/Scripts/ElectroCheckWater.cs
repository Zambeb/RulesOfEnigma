using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCheckWater : MonoBehaviour
{
    public WaterCounter waterCount;
    
    // Start is called before the first frame update
    void Start()
    {
        WaterCounter waterCount = GetComponent<WaterCounter>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterChecker"))
        {
            waterCount.waterCount += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterChecker"))
        {
            waterCount.waterCount -= 1;
        }
    }
}
