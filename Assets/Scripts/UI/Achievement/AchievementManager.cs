using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    public static List<Achievement> achievements;

    [SerializeField] private int integer;
    [SerializeField] private float floating_point;

    [SerializeField] private Canvas achievementCanvas;
    [SerializeField] private TextMeshProUGUI achievementMessage;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private AudioClip achievementSound;
    [SerializeField] private List<Sprite> trophyImages;
    [SerializeField] private Image trophyImage;
    private AudioSource audioSource;
    private int unlockedAchievementsCount = 0;
    private int spiderWebsDestroyed = 0;
    // private int enemiesKilled = 0;

    public bool AchievementUnlocked(string achievementName)
    {
        bool result = false;

        if (achievements == null)
            return false;

        Achievement[] achievementsArray = achievements.ToArray();
        Achievement a = Array.Find(achievementsArray, ach => achievementName == ach.title);

        if (a == null)
            return false;

        result = a.achieved;

        return result;
    }

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        if (achievements == null)
        {
            achievements = new List<Achievement>();
            achievements.Add(new Achievement("Step By Step", "Set your integer to 110 or above.", (object o) => integer >= 100, this));
            achievements.Add(new Achievement("Not So Precise", "Set your floating point to 230.5 or above.", (object o) => floating_point >= 230.5, this));
            achievements.Add(new Achievement("Clean Master", "Destroy 10 spider webs.", (object o) => spiderWebsDestroyed >= 10, this));
            // achievements.Add(new Achievement("Enemy Slayer", "Kill 10 enemies.", (object o) => enemiesKilled >= 10, this));
        }
        else
        {
            foreach (var achievement in achievements)
            {
                achievement.Reset();
            }
        }
    }

    private void Update()
    {
        CheckAchievementCompletion();
    }

    private void CheckAchievementCompletion()
    {
        if (achievements == null)
            return;

        foreach (var achievement in achievements)
        {
            achievement.UpdateCompletion();
        }
    }

    public void ShowAchievement(string title)
    {
        achievementMessage.text = title;
        parentObject.SetActive(true); // Activate the parent object
        PlayAchievementSound(); // Play the achievement sound
        UpdateTrophyImage(); // Update the trophy image
        StartCoroutine(HideAchievementCanvas());
    }

    private void PlayAchievementSound()
    {
        if (achievementSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(achievementSound);
        }
    }

    private void UpdateTrophyImage()
    {
        unlockedAchievementsCount++;
        if (unlockedAchievementsCount <= trophyImages.Count)
        {
            trophyImage.sprite = trophyImages[Mathf.Clamp(unlockedAchievementsCount - 1, 0, trophyImages.Count - 1)];
        }
    }

    private IEnumerator HideAchievementCanvas()
    {
        yield return new WaitForSeconds(5);
        if (parentObject != null)
        {
            parentObject.SetActive(false); // Deactivate the parent object after the animation
        }
    }

    public void IncrementSpiderWebsDestroyed()
    {
        spiderWebsDestroyed++;
        CheckSpiderWebAchievement();
    }

    private void CheckSpiderWebAchievement()
    {
        Achievement cleanMasterAchievement = achievements.Find(a => a.title == "Clean Master");
        if (cleanMasterAchievement != null && !cleanMasterAchievement.achieved)
        {
            cleanMasterAchievement.UpdateCompletion();
        }
    }
    // public void IncrementEnemiesKilled()
    // {
    //     enemiesKilled++;
    //     CheckEnemyKillAchievement();
    // }

    // private void CheckEnemyKillAchievement()
    // {
    //     Achievement enemySlayerAchievement = achievements.Find(a => a.title == "Enemy Slayer");
    //     if (enemySlayerAchievement != null && !enemySlayerAchievement.achieved)
    //     {
    //         enemySlayerAchievement.UpdateCompletion();
    //     }
    // }
}

public class Achievement
{
    private AchievementManager achievementManager;

    public Achievement(string title, string description, Predicate<object> requirement, AchievementManager manager)
    {
        this.title = title;
        this.description = description;
        this.requirement = requirement;
        this.achievementManager = manager;
    }

    public string title;
    public string description;
    public Predicate<object> requirement;

    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
            return;

        if (RequirementsMet())
        {
            achieved = true;
            if (achievementManager != null)
            {
                achievementManager.ShowAchievement(title); // Show the achievement canvas
            }
        }
    }

    public bool RequirementsMet()
    {
        return requirement.Invoke(null);
    }

    public void Reset()
    {
        achieved = false;
    }
}