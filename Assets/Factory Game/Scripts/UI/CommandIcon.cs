using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CommandIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image actionImage;
    [SerializeField]
    private Image cancelImage;

    private Image backgroundImage;

    public event Action OnClick;

    private void Awake() => Init();

    public void OnPointerEnter(PointerEventData eventData) => EnableCancelSubicon();

    public void OnPointerExit(PointerEventData eventData) => DisableCancelSubicon();

    public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke();

    private void Init() => backgroundImage = GetComponent<Image>();

    public void SetActionIcon(Sprite sprite) => actionImage.sprite = sprite;

    public void SetDefaultVisual()
    {
        actionImage.sprite = null;
        DisableCancelSubicon();
    }

    public void AllowInteraction() => backgroundImage.raycastTarget = true;

    public void ProhibitInteraction() => backgroundImage.raycastTarget = false;

    private void EnableCancelSubicon() => cancelImage.gameObject.SetActive(true);

    private void DisableCancelSubicon() => cancelImage.gameObject.SetActive(false);
}