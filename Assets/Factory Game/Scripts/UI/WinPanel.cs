using System;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public event Action OnContinueButtonClick;

    [SerializeField]
    private Button continueButton;

    private void Awake() => Init();

    private void Init() => continueButton.onClick.AddListener(() => OnContinueButtonClick?.Invoke());
}