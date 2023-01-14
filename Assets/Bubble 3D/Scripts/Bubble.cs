using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private Material fillMaterial;
    [SerializeField]
    private Animator bubbleAnimator;
    [SerializeField]
    private GameObject[] bubbleIcons;
    [SerializeField]
    private SuppliesVisualisationInfo suppliesVisualisationInfo;

    private readonly int shaderProperyID_step = Shader.PropertyToID("_Step");
    private readonly int animatorStateID_appearance = Animator.StringToHash("Appearance");
    private readonly int animatorStateID_disappearance = Animator.StringToHash("Disappearance");

    private const float fillingSpeed = 1.5f;

    private Image[] bubbleImages;

    public float FillStep => fillMaterial.GetFloat(shaderProperyID_step);

    private void Awake()
    {
        Init();
        Reset();
    }

    private void Init()
    {
        bubbleImages = new Image[bubbleIcons.Length];

        for (int i = 0; i < bubbleIcons.Length; i++)
            bubbleImages[i] = bubbleIcons[i].GetComponent<Image>();
    }
    
    private void Reset()
    {
        fillMaterial.SetFloat(shaderProperyID_step, 0);

        for (int i = 0; i < bubbleIcons.Length; i++)
            bubbleIcons[i].SetActive(false);
    }

    public void Show(SupplieType[] supplieTypes)
    {
        Reset();

        for (int i = 0; i < supplieTypes.Length; i++)
        {
            bubbleImages[i].material = suppliesVisualisationInfo.GetMaterial(supplieTypes[i]);
            bubbleIcons[i].SetActive(true);
        }

        bubbleAnimator.Play(animatorStateID_appearance);
    }

    public void Fill(float fillAmount)
    {
        float currentFillAmount = fillMaterial.GetFloat(shaderProperyID_step);
        if (fillAmount <= 0 || currentFillAmount == 1) return;

        float targetFillAmount = Mathf.Clamp01(currentFillAmount + fillAmount);
        StartCoroutine(FillAnimation(currentFillAmount, targetFillAmount));
    }

    private IEnumerator FillAnimation(float currentFillAmount, float targetFillAmount)
    {
        float step = 0;
        float animSpeed = fillingSpeed / Mathf.Abs(currentFillAmount - targetFillAmount);

        while (step < 1)
        {
            step += animSpeed * Time.deltaTime;
            float fillValue = Mathf.Lerp(currentFillAmount, targetFillAmount, step);
            fillMaterial.SetFloat(shaderProperyID_step, fillValue);

            yield return null;
        }

        if (targetFillAmount >= 1)
            bubbleAnimator.Play(animatorStateID_disappearance);
    }
}