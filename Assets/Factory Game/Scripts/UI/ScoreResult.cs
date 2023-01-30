using UnityEngine;
using TMPro;

public class ScoreResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreOutputField;
    [SerializeField]
    private TextMeshProUGUI timeOutputField;

    public void SetResult(int score, int timeInSeconds)
    {
        scoreOutputField.text = $"Repairs - {score}";
        timeOutputField.text = $"Time - {timeInSeconds / 60:00}:{timeInSeconds % 60:00}";
    }
}