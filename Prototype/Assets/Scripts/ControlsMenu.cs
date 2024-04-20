using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject controlsMenu;
    
    private GameMaster gm;

    public MainMainMenu mainScript;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainScript != null)
            {
                mainScript.Whoosh();
            }

            mainMenu.SetActive(true);
            controlsMenu.SetActive(false);
        }
    }

    public void KeyboardControls()
    {
        gm.controls = false;
    }
    
    public void MouseControls()
    {
        gm.controls = true;
    }
}
