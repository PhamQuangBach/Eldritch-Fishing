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

    [SerializeField]
    private float maxTime = 600;

    public static GameManager instance;

    //Means if this is first attempt, brain too tired, used to skip intro window, in case player died and restarted
    public static bool FirstPlay = true;

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
        if (gameState == GameState.Win)
            return;
        
        curTime += Time.deltaTime;
        if (curTime > maxTime)
        {
            SetGameState(GameState.TimeOut);
            PlayerMovement.instance.KillTimeOut();
        }
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

    public void Win()
    {
        gameState = GameState.Win;

        PlayerMovement.instance.Win();
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