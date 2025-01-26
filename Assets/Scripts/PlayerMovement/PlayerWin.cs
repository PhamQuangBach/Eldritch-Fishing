using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerWin : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private GameObject winScreen;
    
    [SerializeField]
    private RectTransform credits;


    private GameManager gameManager;

    private bool creditsStarted = true;

    private void Start()
    {
        gameManager = GameManager.instance;

        StartCoroutine(WinTimer());
    }

    private void Update()
    {
        if (!creditsStarted)
            return;

        Vector2 offset = credits.offsetMax;
        offset.y += Time.deltaTime * 100;
        credits.offsetMax = offset;
    }

    public void Win()
    {
        StartCoroutine(WinTimer());
    }

    private IEnumerator WinTimer()
    {
        yield return new WaitForSeconds(15f);

        Destroy(winScreen);
        creditsStarted = true;

        yield return new WaitForSeconds(100f);

        Application.Quit();
    }
}