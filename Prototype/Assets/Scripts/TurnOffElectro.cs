using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffElectro : MonoBehaviour
{
    public ElectricityController electroCon;
    
    void Start()
    {
        ElectricityController electroCon = GetComponent<ElectricityController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            electroCon.electricityOn = false;
        }
    }
}
