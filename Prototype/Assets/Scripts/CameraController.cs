using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] player;
    public Transform triggerCenter; 
    [SerializeField] private GameObject[] cameraFocus;

    public float maxDistance = 10f;
    private float multiplierAtMaxDistance = 1f; 
    private float multiplierAtCenter = 2f; 

    void Update()
    {
        float distanceToCenter = Vector3.Distance(player[0].transform.position, triggerCenter.position);
        float multiplier = Mathf.Lerp(multiplierAtMaxDistance, multiplierAtCenter, distanceToCenter / maxDistance);
        float smoothness = 5f; 
        float targetZ = -multiplier * maxDistance; 
        Vector3 targetPosition = new Vector3(cameraFocus[0].transform.position.x, cameraFocus[0].transform.position.y, targetZ);
        cameraFocus[0].transform.position = Vector3.Lerp(cameraFocus[0].transform.position, targetPosition, Time.deltaTime * smoothness);
    }
}


