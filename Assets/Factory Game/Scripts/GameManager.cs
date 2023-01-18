using System.Collections;
using UnityEngine;
using Environment;
using TMPro;
using ModularRobot;
using BaseUnit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global dependencies")]
    [SerializeField]
    private GameplaySettingsData gameplaySettingsData;
    [Header("Local dependencies")]
    [SerializeField]
    private Player player;
    [SerializeField]
    private RobotSpawner robotSpawner;
    [SerializeField]
    private DropZone dropZone;
    [Header("Local UI dependencies")]
    [SerializeField]
    private UIHandlerMainScene UIhandler;
    [SerializeField]
    private TextMeshProUGUI deliveredRobotsText;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private float timeLimit = 120f;
    private int secondsElapsed = 0;
    private int deliveredRobotsTarget = 6;
    private int deliveredRobotsCount = 0;

    private Coroutine timer;
    private bool isUpdatingDeliveredRobotsCount;

    private void Awake() => Init();

    private void Start()
    {
        LoadGameplaySettings();
        timer = StartCoroutine(Timer());
    }

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

    private void LoadGameplaySettings()
    {
        if (CachedData.Instance == null) return;

        GameDifficulty difficulty = CachedData.Instance.PreferredDifficulty;
        GameplaySettings gameplaySettings = gameplaySettingsData.GetGameplaySettings(difficulty);

        timeLimit = gameplaySettings.timeLimit;
        deliveredRobotsTarget = gameplaySettings.requiredRepairs;

        robotSpawner.Configure(gameplaySettings.spawnDelayRange, gameplaySettings.spawnLimit);
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
        UIhandler.ShowWinPanel();

        player.IsReceivingCommands = false;

        PlayerResult playerResult = new(deliveredRobotsCount, secondsElapsed);
        CachedData.Instance.SavePlayerResult(playerResult);
    }

    private void DisplayLoseScreen()
    {
        StopDeliveredRobotsCount();
        UIhandler.ShowLosePanel();

        player.IsReceivingCommands = false;
    }

    private IEnumerator Timer()
    {
        yield return null;

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