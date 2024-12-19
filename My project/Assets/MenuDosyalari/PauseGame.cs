using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    private void Update()
    {
        // ESC tuþuna basýldýðýnda oyunu durdur veya baþlat
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        // Oyunu durdur
        Time.timeScale = 0f; // Oyun fiziði ve zamanlamayý durdurur
        isPaused = true;

        // Ýsteðe baðlý: Cursor'u görünür yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        // Oyunu yeniden baþlat
        Time.timeScale = 1f; // Oyun fiziði ve zamanlamayý devam ettirir
        isPaused = false;

        // Ýsteðe baðlý: Cursor'u tekrar gizle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
