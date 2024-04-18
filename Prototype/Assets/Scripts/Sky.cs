using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;

    public enum BillboardType
    {
        LookAtCamera,
        CameraForward
    };

    void LateUpdate()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
