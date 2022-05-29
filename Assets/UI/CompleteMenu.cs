using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        ArchievementManager.instance.Reset();
        LevelManager.instance.deathCount = 0;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
