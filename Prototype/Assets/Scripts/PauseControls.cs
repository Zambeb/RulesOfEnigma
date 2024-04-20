using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControls : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject controlsMenu;

    public HeroCharacterController hero;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }
    }

    public void Keyboard()
    {
        hero.controlMouse = false;
    }

    public void Mouse()
    {
        hero.controlMouse = true;
    }
}
