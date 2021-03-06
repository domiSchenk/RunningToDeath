using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter()
    {
        Debug.Log("Player died");
        //get player by tag
        GameObject player = GameObject.FindGameObjectWithTag("PlayerController");
        Destroy(player);
        LevelManager.instance.Respawn();
    }

}
