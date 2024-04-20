using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject controlsMenu;
    public HeroCharacterController heroController;
    public Button controlButton;
    public TextMeshProUGUI buttonText;
    public AudioSource audioSource;
    public AudioClip whoosh;
    
    void Start()
    {
        controlsMenu.SetActive(false);

    }

    public void SwitchControls ()
    {

    }

    public void StartGame()
    {
        if (heroController != null)
        {
            heroController.ToggleMainMenu(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Whoosh()
    {
        audioSource.PlayOneShot(whoosh);
    }
}
