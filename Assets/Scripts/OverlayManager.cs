using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    [SerializeField] private Image overlayImage;
    [SerializeField] private Image overlayImageLeft;
    [SerializeField] private Image overlayImageRight;
    [SerializeField] private IntVar level;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    [SerializeField] TMPro.TextMeshProUGUI splashUI;
    [SerializeField] private string[] splashText;

    public static OverlayManager instance { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewLevel()
    {
        overlayImage.enabled = true;
        Color color = Color.HSVToRGB(1f / 40 * ((level.value - 1) % 40), 1f, 1f);
        color.a = .5f;
        overlayImage.color = color;
        animator.Play("OverlayOpen");
        overlayImageLeft.enabled = false;
        overlayImageRight.enabled = false;
        animatorLeft.Play("Reset");
        animatorRight.Play("Reset");
        Invoke(nameof(CloseOverlay), 1.5f);
        splashUI.text = splashText[level.value - 1];
    }

    public void CloseOverlay()
    {
        overlayImage.enabled = false;
        animator.Play("Reset");
        Color color = Color.HSVToRGB(1f / 40 * ((level.value - 1) % 40), 1f, 1f);
        color.a = .5f;
        overlayImageRight.enabled = true;
        overlayImageLeft.enabled = true;
        overlayImageLeft.color = color;
        overlayImageRight.color = color;
        animatorLeft.Play("OverlayClose");
        animatorRight.Play("OverlayClose");
    }
}
