using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArchievementManager;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private Archievements archivement;

    void OnTriggerEnter()
    {
        ArchievementManager.instance.ShowArchivement(archivement);
        Destroy(gameObject);
    }

}
