using System.Collections;
using UnityEngine;

public class FixBox : MonoBehaviour
{
    public bool isBoxOnPlatform;
    public PlatformMoveController platController;
    public GameObject block;

    private void Start()
    {
        PlatformMoveController platController = GetComponent<PlatformMoveController>();
        block.SetActive(false);
    }

    private void Update()
    {
        if (platController != null && (platController.isMoving || platController.isMovingBack))
        {
            block.SetActive(isBoxOnPlatform);
        }
        else
        {
            block.SetActive(false);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            isBoxOnPlatform = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            isBoxOnPlatform = false;
        }
    }
}