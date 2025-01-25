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

    [SerializeField]
    private Canvas prologueCanvas;

    private void Start()
    {
        StartCoroutine(PrologueStart());
    }

    private IEnumerator PrologueStart()
    {
        yield return new WaitForSeconds(timeToReadPrologue);
        
        playerMovement.ScreenFade.FadeOut();

        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);
    }
}