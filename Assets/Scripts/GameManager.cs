using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static float CurTime { get => instance.curTime; }
    public static bool IsPaused { get => instance.isPaused; }
    
    public static GameManager instance;

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
}