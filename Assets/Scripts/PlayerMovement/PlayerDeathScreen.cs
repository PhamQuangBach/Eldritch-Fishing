using System.Collections;
using UnityEngine;


public class PlayerDeathScreen : MonoBehaviour
{
    [SerializeField]
    private float timeToRestart = 1f;

    private bool canRestart = false;

    private void Update()
    {
        RestartGame();
    }

    public void StartGameOver()
    {
        StartCoroutine(WaitUntilRestart());
    }

    private IEnumerator WaitUntilRestart()
    {
        yield return new WaitForSeconds(timeToRestart);

        canRestart = true;
    }

    private void RestartGame()
    {
        if (!canRestart)
            return;

        Debug.Log("Can Restart");

        if (Input.GetKeyDown(KeyCode.Space))
        {


            GameManager manager = GameManager.instance;

            manager.RestartGame();
        }
    }
}