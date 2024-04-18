using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffElectro : MonoBehaviour
{
    public ElectricityController electroCon;
    // Start is called before the first frame update
    void Start()
    {
        ElectricityController electroCon = GetComponent<ElectricityController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            electroCon.electricityOn = false;
        }
    }
}
