using System.Collections;
using UnityEngine;
using Environment;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Setup")]
    [SerializeField]
    private DropZone dropZone;
    [SerializeField]
    private TextMeshProUGUI deliveredRobotsText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Settings")]
    [SerializeField]
    private float timeLimit = 120f;
    [SerializeField]
    private int deliveredRobotsTarget = 6;

    public float TimeLimit { set { timeLimit = value; } }
    public int DeliveredRobotsTarget { set { deliveredRobotsTarget = value; } }

    private int deliveredRobotsCount = 0;

    private Coroutine timer;
    private bool isUpdatingDeliveredRobotsCount;

    private void Awake() => Init();

    private void Start() => timer = StartCoroutine(Timer());

    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
            dropZone.OnRobotDelivered += UpdateDeliveredRobotsCount;
            isUpdatingDeliveredRobotsCount = true;
        }
        else
            Destroy(gameObject);
    }

    private void UpdateDeliveredRobotsCount()
    {
        deliveredRobotsCount++;

        //deliveredRobotsText.text = $"{deliveredRobotsCount}/{deliveredRobotsTarget}";
        Debug.Log($"{deliveredRobotsCount}/{deliveredRobotsTarget}");

        if (deliveredRobotsCount == deliveredRobotsTarget)
            DisplayWinScreen();
    }

    private void DisplayWinScreen()
    {
        StopTimer();

        throw new System.NotImplementedException();
    }

    private void DisplayLoseScreen()
    {
        StopDeliveredRobotsCount();

        throw new System.NotImplementedException();
    }

    private IEnumerator Timer()
    {
        yield return null;

        int secondsElapsed = 0;

        while (secondsElapsed <= timeLimit)
        {
            int minutes = secondsElapsed / 60;
            int seconds = secondsElapsed % 60;
            //timerText.text = $"{minutes:00}:{seconds:00}";
            Debug.Log($"{minutes:00}:{seconds:00}");

            yield return new WaitForSeconds(1f);

            secondsElapsed++;
        }

        DisplayLoseScreen();
    }

    private void StopTimer()
    {
        if (timer == null) return;

        StopCoroutine(timer);
        timer = null;
    }

    private void StopDeliveredRobotsCount()
    {
        if (!isUpdatingDeliveredRobotsCount) return;

        dropZone.OnRobotDelivered -= UpdateDeliveredRobotsCount;
        isUpdatingDeliveredRobotsCount = false;
    }
}