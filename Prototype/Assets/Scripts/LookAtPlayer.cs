using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Collider triggerCollider;

    void Update()
    {
        if (IsPlayerInsideTrigger())
        {
            if (playerTransform != null)
            {
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = rotationToPlayer;
            }
        }
		else
        {
		    transform.rotation = Quaternion.Euler(154f, -6f, 8f);
		}
    }
    
    bool IsPlayerInsideTrigger()
    {
        if (triggerCollider != null)
        {
            return triggerCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds);
        }

        return false;
    }
}
