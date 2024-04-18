using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WaterCountZone : MonoBehaviour
{
    public bool inZone = false;
    // Start is called before the first frame update
    void Start()
    {
        inZone = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }
}
