
using System;
using System.Collections.Generic;
using UnityEngine;


public class TaskData : MonoBehaviour
{
    public string taskName;
    public List<string> completionDates = new List<string>(); // Format: yyyy-MM-dd
    public bool rewarded = false;
}