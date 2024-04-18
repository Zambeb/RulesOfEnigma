using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Material material; 
    public Color emissionColorOn; 
    public Color emissionColorOff; 

	[SerializeField] private int switchID;
	[SerializeField] private GameObject[] platformsToActivate;
	public Vector2 movementAmount;

	public GameObject lever;
	public bool anyMoving = false;

	public bool isActive = false;
	public ElectricityController electroController;
	public FirstBossController bossController;

	private Quaternion initialLeverRotation;
	private bool leverRotated;
	
    void Start()
    {
	    ElectricityController electroController = GetComponent<ElectricityController>();
	    initialLeverRotation = lever.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateSwitch()
    {
	    
	    if (IsAnyPlatformMoving() || !electroController.electricityOn)
	    {
		    return;
	    }

	    isActive = true;
	    ApplySwitchLogic();
	    RotateLever();
    }

    public void DeactivateSwitch()
    {
	    if (IsAnyPlatformMoving() || !electroController.electricityOn)
	    {
		    return;
	    }

	    isActive = false;
	    ApplySwitchLogic();
	    RotateLever();
    }

    private bool IsAnyPlatformMoving()
    {
	    foreach (GameObject platform in platformsToActivate)
	    {
		    PlatformMoveController platformController = platform.GetComponent<PlatformMoveController>();
		    
		    if (platformController != null && (platformController.isMoving || platformController.isMovingBack))
		    {
			    return true;
		    }
	    }
	    return false;
    }

    private void ApplySwitchLogic()
    {
	    foreach (GameObject platform in platformsToActivate)
	    {
		    
		    PlatformMoveController platformController = platform.GetComponent<PlatformMoveController>();
		    platformController.movementAmount = movementAmount;

		    if (platformController != null && !platformController.isMoving && !platformController.isMovingBack && electroController.electricityOn)
		    {
			    if (isActive)
			    {
				    platformController.ActivatePlatform();
			    }
			    else
			    {
				    platformController.DeactivatePlatform();
			    }
		    }
	    }
    }
    
    private void RotateLever()
    {
	    if (!leverRotated)
	    {
		    Quaternion targetRotation = initialLeverRotation * Quaternion.Euler(0f, 0f, 80f);
		    lever.transform.rotation = targetRotation;
	    }
	    else
	    {
		    lever.transform.rotation = initialLeverRotation;
	    }

	    leverRotated = !leverRotated;
    }
}
