using UnityEngine;
using TMPro;

public class FocusZone : MonoBehaviour
{
    [Header("Focus Settings")]
    public float requiredTimeInZone = 5f;
    public string taskName = "Water Plant";
    public TaskManager taskManager;

    [Header("Visuals")]
    public GameObject zoneCircle;
    public TextMeshProUGUI countdownText;

    private float _timer = 0f;
    private bool _playerInZone = false;
    private bool _taskCompleted = false;

    void Start()
    {
        if (zoneCircle != null) zoneCircle.SetActive(false);
        if (countdownText != null) countdownText.text = $"Time Remaining: {requiredTimeInZone:F1}";
    }

    void Update()
    {
        if (_playerInZone && !_taskCompleted)
        {
            _timer += Time.deltaTime;
            float timeRemaining = Mathf.Max(0, requiredTimeInZone - _timer);

            if (countdownText != null)
                countdownText.text = $"Time Remaining: {timeRemaining:F1}";

            if (_timer >= requiredTimeInZone)
                TaskCompleted();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Focus Zone");
            _playerInZone = true;
            _timer = 0f;
            if (zoneCircle != null) zoneCircle.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Exited Focus Zone");
            _playerInZone = false;
            _timer = 0f;
            if (zoneCircle != null) zoneCircle.SetActive(false);
        }
    }

    void TaskCompleted()
    {
        _taskCompleted = true;
        if (zoneCircle != null) zoneCircle.SetActive(false);
        taskManager.LogTaskCompletion(taskName);
    }
}