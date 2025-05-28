using UnityEngine;
using UnityEngine.UI;

public class TaskUi : MonoBehaviour
{
    public TaskManager taskManager;
    public InputField taskInput;

    public void OnAddTaskClicked()
    {
        string taskName = taskInput.text.Trim();
        if (!string.IsNullOrEmpty(taskName))
        {
            taskManager.LogTaskCompletion(taskName);
            taskInput.text = "";
        }
    }

    public void OnCompleteTaskClicked(string taskName)
    {
        taskManager.LogTaskCompletion(taskName);
    }
}

