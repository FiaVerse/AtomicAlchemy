using UnityEngine;
using System;

public class TaskProgress : MonoBehaviour
{
    [Header("Reward")]
    public GameObject rewardPrefab;      // what spawns
    public Transform rewardSpawnPoint;   // where

    const string LastDateKey  = "Water_LastDate";
    const string StreakKey    = "Water_Streak";

    DateTime lastDate;
    int streak;

    void Awake()
    {
        // Load persisted data (defaults if none)
        lastDate = PlayerPrefs.HasKey(LastDateKey) ?
            DateTime.Parse(PlayerPrefs.GetString(LastDateKey)) :
            DateTime.MinValue;

        streak   = PlayerPrefs.GetInt(StreakKey, 0);
    }

    // Hook this to your “I drank!” button
    public void OnDrinkLogged()
    {
        DateTime today = DateTime.Today;

        // Already logged today? bail.
        if (lastDate == today)
        {
            Debug.LogWarning("Water already logged for today.");
            return;
        }

        // Increment or reset streak
        streak = (lastDate == today.AddDays(-1)) ? streak + 1 : 1;

        // Persist progress
        lastDate = today;
        PlayerPrefs.SetString(LastDateKey, today.ToString("yyyy-MM-dd"));
        PlayerPrefs.SetInt(StreakKey, streak);
        PlayerPrefs.Save();

        Debug.Log($"Streak is now {streak} day(s).");

        // Check for reward
        if (streak >= 1) // days of streak is 1 now
        {
            SpawnReward();
            streak = 0;                             // reset for next round
            PlayerPrefs.SetInt(StreakKey, streak);
            PlayerPrefs.Save();
        }
    }

    void SpawnReward()
    {
       // Instantiate(rewardPrefab,
          //  rewardSpawnPoint ? rewardSpawnPoint.position : Vector3.zero,
          //  Quaternion.identity);

        Debug.Log("Reward spawned — nice hydration streak!");
    }
}