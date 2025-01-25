using System.Collections;
using UnityEngine;


public class PlayerStartingPrologue : MonoBehaviour
{   
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private PrologueCollection prologueCollection;

    [SerializeField]
    private float timeToReadPrologue = 15f;

    private void Start()
    {
        StartCoroutine(PrologueStart());
    }

    private IEnumerator PrologueStart()
    {
        yield return new WaitForSecondsRealtime(timeToReadPrologue);
        playerMovement.ScreenFade.FadeOut();

        yield return new WaitForSeconds(5f);

        GameManager manager = GameManager.instance;
        if (manager != null)
        {
            manager.StartGame();

            gameObject.SetActive(false);
        }
    }
}