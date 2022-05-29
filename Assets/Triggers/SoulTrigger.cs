using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulTrigger : MonoBehaviour
{
    void OnTriggerEnter()
    {
        Debug.Log("Soul died");
        //get player by tag
        GameObject player = GameObject.FindGameObjectWithTag("PlayerController");
        Destroy(player);
        LevelManager.instance.RespawnSoul();
    }

}
