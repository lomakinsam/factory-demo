using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class UIHandlerMenu : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button switchDifficultyButton;
    [SerializeField]
    private TextMeshProUGUI switchDifficultyButtonText;
    [SerializeField]
    private Button exitButton;
    [Space]
    [SerializeField] 
    private Transform playerResultsContainer;
    [SerializeField]
    private ScoreResult playerResultPrefab;

    private const int mainSceneIndex = 1;

    private void Awake() => Init();

    private void Start()
    {
        DrawLeaderboard();
        LoadPreferredDiffiulty();
    }

    private void Init()
    {
        startButton.onClick.AddListener(LoadMainScene);
        switchDifficultyButton.onClick.AddListener(SwitchDifficlty);
        exitButton.onClick.AddListener(QuitGame);
    }

    private void LoadMainScene() => SceneManager.LoadScene(mainSceneIndex);

    private void LoadPreferredDiffiulty()
    {
        GameDifficulty preferredDifficulty = CachedData.Instance.PreferredDifficulty;
        switchDifficultyButtonText.text = preferredDifficulty.ToString();
    }

    private void SwitchDifficlty()
    {
        GameDifficulty preferredDifficulty = CachedData.Instance.PreferredDifficulty;

        int maxDifficulty = System.Enum.GetValues(typeof(GameDifficulty)).Length - 1;

        if ((int)preferredDifficulty == maxDifficulty)
            preferredDifficulty = 0;
        else
            preferredDifficulty++;

        CachedData.Instance.SavePreferredDifficulty(preferredDifficulty);

        switchDifficultyButtonText.text = preferredDifficulty.ToString();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void DrawLeaderboard()
    {
        PlayerResult[] playerResults = CachedData.Instance.Leaderboard.PlayerResults;

        for (int i = 0; i < playerResults.Length; i++)
        {
            if (playerResults[i].score == 0 || playerResults[i].timeInSeconds == 0) continue;

            ScoreResult playerResult = Instantiate(playerResultPrefab, playerResultsContainer);
            playerResult.SetResult(playerResults[i].score, playerResults[i].timeInSeconds);
        }
    }
}