using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Respawn();
    }

    // Update is called once per frame
    public void Respawn()
    {
        Debug.Log("Respawn");
        var player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cinemachineVirtualCamera.Follow = player.transform;
        player.SetActive(true);
    }
}
