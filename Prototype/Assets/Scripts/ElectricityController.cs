using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityController : MonoBehaviour
{

    public bool electricityOn = true;
    public GameObject[] electricityObjects;
    public WaterCountZone waterZone;

    // Start is called before the first frame update
    void Start()
    {
        WaterCountZone waterZone = GetComponent<WaterCountZone>();
    }

    // Update is called once per frame
    void Update()
    {

        
            if (electricityOn)
            {
                foreach (GameObject electroObj in electricityObjects)
                {
                    electroObj.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject electroObj in electricityObjects)
                {
                    electroObj.SetActive(false);
                }
            }
        
    }
    
}
