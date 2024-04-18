using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCounter : MonoBehaviour
{
    public int waterCount;
    public WaterCountZone waterZone;
    public ElectricityController electro;
    
    // Start is called before the first frame update
    void Start()
    {
        WaterCountZone waterZone = GetComponent<WaterCountZone>();
        electro = GetComponent<ElectricityController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waterZone.inZone)
        {
            if (waterCount >=3)
            {
                electro.electricityOn = true;
            }

            else
            {
                electro.electricityOn = false;
            }
        }
    }
}
