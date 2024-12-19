using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;

    private void Update()
    {
        // ESC tu�una bas�ld���nda oyunu durdur veya ba�lat
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
        Time.timeScale = 0f; // Oyun fizi�i ve zamanlamay� durdurur
        isPaused = true;

        // �ste�e ba�l�: Cursor'u g�r�n�r yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        // Oyunu yeniden ba�lat
        Time.timeScale = 1f; // Oyun fizi�i ve zamanlamay� devam ettirir
        isPaused = false;

        // �ste�e ba�l�: Cursor'u tekrar gizle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
