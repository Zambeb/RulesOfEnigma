using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip whoosh;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
