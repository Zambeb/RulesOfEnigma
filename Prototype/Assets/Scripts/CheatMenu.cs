using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenu : MonoBehaviour
{
    public HeroCharacterController heroController;
    public GameMaster gm;
    public GameObject barn;
    public GameObject hero;
    private Vector3 barnPosition;

    void Start()
    {
        if (barn != null)
        {
            barnPosition = barn.transform.position;
        }
        else
        {
            Debug.LogError("Barn location is not assigned in CheatMenu.");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        // Ваш код обновления, если необходимо
    }

    public void AllAbilities()
    {
        if (heroController != null)
        {
            heroController.canJump = true;
            heroController.canGrab = true;
            heroController.canFire = true;
        }
        else
        {
            Debug.LogError("HeroController is not assigned in CheatMenu.");
        }
    }

    public void BarnRespawn()
    {
        gm.lastCheckPointPos = barnPosition;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}