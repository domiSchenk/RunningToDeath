using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private CanvasGroup panelComplete;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private GameObject skyBoxPrefab;
    [SerializeField] private GameObject taintedEnv;
    [SerializeField] private Material skyBoxNormal;
    [SerializeField] private Material skyBoxSoul;

    [SerializeField] private Light sun;

    [SerializeField] private Transform respawnPoint;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private TextMeshProUGUI text;

    public int deathCount = 0;
    private bool showPanelComplete = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        StartGame();
    }

    private void Update()
    {
        if (showPanelComplete)
        {
            ShowPanelComplete();
        }
        else
        {
            HidePanelComplete();
        }

    }

    public void StartGame()
    {
        deathCount = 0;
        spawn(playerPrefab, skyBoxNormal, new Color(1, 0.957f, 0.839f, 1));
    }

    // Update is called once per frame
    public void Respawn()
    {
        Debug.Log("Respawn");
        taintedEnv.SetActive(false);
        spawn(playerPrefab, skyBoxNormal, new Color(1, 0.957f, 0.839f, 1));
        AddDeathCount();
    }

    public void RespawnSoul()
    {
        Debug.Log("Respawn Soul");
        taintedEnv.SetActive(true);
        spawn(soulPrefab, skyBoxSoul, new Color(1, 0.345f, 0.337f, 1));
        AddDeathCount();
    }

    private void spawn(GameObject prefab, Material sky, Color sunColor)
    {
        var player = Instantiate(prefab, respawnPoint.position, Quaternion.identity);
        sun.color = sunColor;
        skyBoxPrefab.GetComponent<Renderer>().material = sky;
        player.tag = "PlayerController";
        cinemachineVirtualCamera.Follow = player.transform;
        player.SetActive(true);
    }

    public void AddDeathCount()
    {
        deathCount++;
        text.text = deathCount.ToString();
    }

    public void GameDone()
    {
        showPanelComplete = true;
        panelComplete.blocksRaycasts = true;
        panelComplete.interactable = true;
    }

    private void HidePanelComplete()
    {
        if (panelComplete.alpha > 0)
        {
            panelComplete.alpha -= Time.deltaTime * 2;
        }
    }
    private void ShowPanelComplete()
    {
        if (panelComplete.alpha < 1)
        {
            panelComplete.alpha += Time.deltaTime * 2;
        }
    }
}
