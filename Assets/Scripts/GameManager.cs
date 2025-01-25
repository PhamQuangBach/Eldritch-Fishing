using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Game,
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

    private GameState gameState = GameState.Game;

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
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
    }

    public void OnDeath()
    {
        gameState = GameState.Death;
    }

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}