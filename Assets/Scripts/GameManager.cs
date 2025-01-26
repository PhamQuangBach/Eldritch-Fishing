using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Prepare,
    Game,
    Paused,
    Death,
    TimeOut,
    Win
}


public class GameManager : MonoBehaviour
{
    public static float CurTime { get => instance.curTime; }
    public static bool IsPaused { get => instance.isPaused; }
    public static GameState GameState { get => instance.gameState; }

    public static GameManager instance;

    private GameState gameState = GameState.Prepare;

    private bool isPaused = false;
    private float curTime = 0;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateCurrentTime();
    }

    private void UpdateCurrentTime()
    {
        curTime += Time.deltaTime;
    }

    public void PauseGame()
    {
        if (gameState != GameState.Game)
            return;

        gameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        if (gameState != GameState.Paused)
            return;

        gameState = GameState.Game;
    }

    public void StartGame()
    {
        if (gameState != GameState.Prepare)
            return;

        gameState = GameState.Game;
    }

    //This never seems to be called!
    /*public void OnDeath()
    {
        gameState = GameState.Death;
    }*/

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    public GameState GetGameState()
    { 
        return gameState;
    }

    public void SetGameState(GameState newState)
    {
        gameState = newState;
    }
}