using System;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    public event Action OnRetryButtonClick;
    public event Action OnContinueButtonClick;

    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button continueButton;
    
    private void Awake() => Init();

    private void Init()
    {
        retryButton.onClick.AddListener(() => OnRetryButtonClick?.Invoke());
        continueButton.onClick.AddListener(() => OnContinueButtonClick?.Invoke());
    }
}