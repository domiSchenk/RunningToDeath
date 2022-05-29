using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArchievementManager : MonoBehaviour
{
    public static ArchievementManager instance;
    [SerializeField] private CanvasGroup panelArchivement;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;

    private float showTime;
    private bool showPanel = false;

    public enum Archievements
    {
        DefaultGoal = 1,
        SoulGoal = 2,
        HiddenGoal = 3,
        HarakiriGoal = 4,
        All = 5

    }
    public Archievement DefaultGoal { get; set; } = new Archievement(Archievements.DefaultGoal, "Standard Goal", "You have reached the standard goal");
    public Archievement SoulGoal { get; set; } = new Archievement(Archievements.SoulGoal, "Soul Goal", "You have reached the soul goal");
    public Archievement HiddenGoal { get; set; } = new Archievement(Archievements.HiddenGoal, "Hidden Goal", "You have reached the hidden goal");
    public Archievement HarakiriGoal { get; set; } = new Archievement(Archievements.HarakiriGoal, "Harakiri Goal", "You have reached the harakiri goal");
    public Archievement AllGoal { get; set; } = new Archievement(Archievements.All, "All Goals", "Congratulations, You have reached all goals!");

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (showTime > 0)
        {
            showTime -= (Time.deltaTime * 2);
            if (showTime <= 0)
            {
                showPanel = false;
            }
        }

        if (showPanel)
        {
            ShowPanelArchievment();
        }
        else
        {
            HidePanelAchievement();
        }

    }

    public void ShowArchivement(Archievements archivements)
    {
        var archivement = GetArchivement(archivements);
        if (archivement != null && !archivement.IsCompleted)
        {
            ShowAndCompleteArchivement(archivement);
            CheckIfAllDone();
        }
    }

    public void CheckIfAllDone()
    {
        if (DefaultGoal.IsCompleted && SoulGoal.IsCompleted && HiddenGoal.IsCompleted && HarakiriGoal.IsCompleted)
        {
            LevelManager.instance.GameDone();
        }
    }

    public void Reset()
    {
        DefaultGoal.Reset();
        SoulGoal.Reset();
        HiddenGoal.Reset();
        HarakiriGoal.Reset();
        AllGoal.Reset();
    }

    private Archievement GetArchivement(Archievements archivements)
    {
        switch (archivements)
        {
            case Archievements.DefaultGoal:
                return DefaultGoal;
            case Archievements.SoulGoal:
                return SoulGoal;
            case Archievements.HiddenGoal:
                return HiddenGoal;
            case Archievements.HarakiriGoal:
                return HarakiriGoal;
            case Archievements.All:
                return AllGoal;
            default:
                return null;
        }
    }

    private void ShowAndCompleteArchivement(Archievement archivement)
    {
        if (archivement != null && !archivement.IsCompleted)
        {
            showTime = 6f;
            showPanel = true;
            archivement.Complete();
            title.text = archivement.Title;
            description.text = archivement.Description;
        }
    }

    private void HidePanelAchievement()
    {
        if (panelArchivement.alpha > 0)
        {
            panelArchivement.alpha -= Time.deltaTime * 2;
        }
    }
    private void ShowPanelArchievment()
    {
        if (panelArchivement.alpha < 1)
        {
            panelArchivement.alpha += Time.deltaTime * 2;
        }
    }
}



public class Archievement
{

    public ArchievementManager.Archievements ArchivementId { get; set; }
    public bool IsCompleted { get; private set; }
    public string Title { get; set; }
    public string Description { get; set; }


    public Archievement(ArchievementManager.Archievements archivement, string title, string description)
    {
        this.ArchivementId = archivement;
        this.Title = title;
        this.Description = description;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Reset()
    {
        IsCompleted = false;
    }
}

