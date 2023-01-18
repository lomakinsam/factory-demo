using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerMainScene : MonoBehaviour
{
    [SerializeField]
    private WinPanel winPanel;
    [SerializeField]
    private LosePanel losePanel;
    [SerializeField]
    private CommandsTray commandsTray;

    private const int mainMenuIndex = 0;

    private void Awake() => Init();

    private void Init()
    {
        winPanel.OnContinueButtonClick += LoadMainMenu;
        losePanel.OnContinueButtonClick += LoadMainMenu;
        losePanel.OnRetryButtonClick += ReloadCurrentScene;
    }

    public void ShowWinPanel()
    {
        commandsTray.Interactable = false;
        winPanel.gameObject.SetActive(true);
    }

    public void ShowLosePanel()
    {
        commandsTray.Interactable = false;
        losePanel.gameObject.SetActive(true);
    }

    private void LoadMainMenu() => SceneManager.LoadScene(mainMenuIndex);

    private void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}