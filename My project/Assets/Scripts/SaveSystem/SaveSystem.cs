using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private float resetTimer = 0f;
    private float resetDelay = 120f;
    public int dayCount = 1;
    public event Action<string> OnDayEnd;
    private bool isPaused = false;

    private void Update()
    {
        EndUIScreen();
    }
    private void Awake()

    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Product Manager Duplicate");
            Destroy(gameObject);
        }
    }

    public void EndUIScreen()
    {
        resetTimer += Time.deltaTime;
        if (resetTimer >= resetDelay)
        {
            OnDayEnd.Invoke("Gambit");
            TogglePause();
            MouseLock(true);
            resetTimer = 0f;
        }
    }

    public void NextDay()
    {
        dayCount++;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        MouseLock(false);
        TogglePause();
        CashierManager.Instance.TotalProfit = 0f;
        CashierManager.Instance.TotalNPCProcessed = 0;
        CashierManager.Instance.TotalItemsScanned = 0;
    }
    
    public int DayCount{get{return dayCount;}}
    
    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void MouseLock(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
    
}