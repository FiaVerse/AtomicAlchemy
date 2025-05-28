using UnityEngine;
using System;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
  private Dictionary<string, TaskData> taskDictionary = new();

    public void LogTaskCompletion(string taskName)
    {
        if (!taskDictionary.ContainsKey(taskName))
        {
            TaskData newTask = LoadTask(taskName) ?? new TaskData { taskName = taskName };
            taskDictionary[taskName] = newTask;
        }

        TaskData task = taskDictionary[taskName];
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (task.completionDates.Contains(today))
        {
            Debug.LogWarning($"{taskName} already completed today.");
            return;
        }

        task.completionDates.Add(today);
        Debug.Log($"✅ {taskName} completed on {today}");

        if (!task.rewarded && IsThreeDayStreak(task.completionDates))
        {
            task.rewarded = true;
            Debug.Log($"🎉 {taskName} — 3-day streak reward unlocked!");
            // Reward logic can be triggered externally if needed
        }

        SaveTask(task);
    }

    private bool IsThreeDayStreak(List<string> dates)
    {
        dates.Sort();
        int streak = 1;

        for (int i = dates.Count - 2; i >= 0; i--)
        {
            DateTime prev = DateTime.Parse(dates[i]);
            DateTime next = DateTime.Parse(dates[i + 1]);
            if ((next - prev).TotalDays == 1)
                streak++;
            else
                streak = 1;

            if (streak >= 3)
                return true;
        }

        return false;
    }

    private void SaveTask(TaskData task)
    {
        string json = JsonUtility.ToJson(task);
        PlayerPrefs.SetString("TaskData_" + task.taskName, json);
        PlayerPrefs.Save();
    }

    private TaskData LoadTask(string taskName)
    {
        string key = "TaskData_" + taskName;
        if (PlayerPrefs.HasKey(key))
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<TaskData>(json);
        }
        return null;
    }
} 

