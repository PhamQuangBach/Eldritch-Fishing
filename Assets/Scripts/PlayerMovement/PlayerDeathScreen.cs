using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


public class PlayerDeathScreen : MonoBehaviour
{
    private const string deathMonster = "Flesh is torn from your bones, as your soul is ripped from your body.\nYour mind wanders to your family, your friends.\nThey will not remember you.";
    private const string deathMoon = "The blood moon has risen. You look to the lake, it beckons you.\nThe coldness sends shivers up your spine.\nThe dark glacial waters are comforting.\nYou close your eyes as you feel HIM caress you.";

    [SerializeField]
    private TextMeshProUGUI flavourText;

    private float timeToRestart = 1f;

    private bool canRestart = false;

    private void Update()
    {
        RestartGame();
    }

    public void StartGameOver()
    {
        switch (GameManager.instance.GetGameState())
        {
            case GameState.Death:
                flavourText.SetText(deathMonster);
                break;

            case GameState.TimeOut:
                flavourText.SetText(deathMoon);
                break;
        }

        

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