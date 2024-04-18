using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControls : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject controlsMenu;

    public HeroCharacterController hero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
