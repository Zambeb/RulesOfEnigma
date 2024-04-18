using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpont : MonoBehaviour
{
    private GameMaster gm;
    public HeroCharacterController player;
    
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.lastCheckPointPos = transform.position;
            gm.canJump = player.canJump;
            gm.canGrab = player.canGrab;
            gm.canFire = player.canFire;
            gm.controls = player.controlMouse;
        }
    }
}
