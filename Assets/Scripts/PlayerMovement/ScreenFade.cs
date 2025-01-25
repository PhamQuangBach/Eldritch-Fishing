using UnityEngine;
using UnityEngine.UI;


public class ScreenFade : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 2f;

    private CanvasGroup fadeCanvas;

    private float alphaTo = 0f;

    
    private void Start()
    {
        fadeCanvas = GetComponent<CanvasGroup>();

        FadeOut();
    }

    private void Update()
    {
        float alpha = fadeCanvas.alpha;

        fadeCanvas.alpha = Mathf.Lerp(alpha, alphaTo, fadeSpeed * Time.deltaTime);

        if (alpha < 0.01f)
        {
            alpha = 0f;
        }
        else if (alpha > 0.95f)
        {
            alpha = 1f;
        }
    }

    public void FadeIn()
    {
        alphaTo = 1f;
    }

    public void FadeOut()
    {
        alphaTo = 0f;
    }
}