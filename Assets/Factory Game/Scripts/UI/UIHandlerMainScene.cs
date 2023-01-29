using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandlerMainScene : MonoBehaviour
{
    [SerializeField]
    private WinPanel winPanel;
    [SerializeField]
    private LosePanel losePanel;
    [SerializeField]
    private CommandsTray commandsTray;
    [SerializeField]
    private Image timeBarBackground;
    [SerializeField]
    private Image progressBarBackground;

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

    public void FillTimeBar(float fillAmount) => timeBarBackground.fillAmount = Mathf.Clamp01(fillAmount);

    public void FillProgressBar(float fillAmount) => progressBarBackground.fillAmount = Mathf.Clamp01(fillAmount);

    private void LoadMainMenu() => SceneManager.LoadScene(mainMenuIndex);

    private void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}