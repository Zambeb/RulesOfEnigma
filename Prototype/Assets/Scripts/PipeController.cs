using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    public Material material; 
    public Color emissionColorOn; 
    public Color emissionColorOff; 

	[SerializeField] private int switchID;
	[SerializeField] private GameObject[] platformsToActivate;

	public GameObject lever;
	public bool anyMoving = false;
	
	public ElectricityController electroController;

	private Quaternion initialLeverRotation;
	private bool leverRotated;
	
    void Start()
    {
	    ElectricityController electroController = GetComponent<ElectricityController>();
	    initialLeverRotation = lever.transform.rotation;
    }


    public void ActivateSwitch()
    {
	    
	    if (IsAnyPlatformMoving())
	    {
		    return;
	    }
	    
	    ApplySwitchLogic();
	    RotateLever();
    }

    private bool IsAnyPlatformMoving()
    {
	    foreach (GameObject platform in platformsToActivate)
	    {
		    Pipe platformController = platform.GetComponent<Pipe>();
		    
		    if (platformController != null && (platformController.isMoving))
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
		    
		    Pipe platformController = platform.GetComponent<Pipe>();

		    if (platformController != null && !platformController.isMoving)
		    { 
			    platformController.ActivatePlatform();
		    }
	    }
    }
    
    private void RotateLever()
    {
	    if (!leverRotated)
	    {
		    Quaternion targetRotation = initialLeverRotation * Quaternion.Euler(80f, 0f, 0f);
		    lever.transform.rotation = targetRotation;
	    }
	    else
	    {
		    lever.transform.rotation = initialLeverRotation;
	    }

	    leverRotated = !leverRotated;
    }
}
