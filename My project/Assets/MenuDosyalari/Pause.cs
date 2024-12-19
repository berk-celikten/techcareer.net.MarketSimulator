using System.Xml.Linq;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static bool gameIsPaused;

    public GameObject Canvas;
    public GameObject bgMusicSlider;
    public GameObject sfxSlider;
    public GameObject SfxText;
    public GameObject BackGroundText;
   // public GameObject MainMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                TogglePause();
            }
        }

    }

    public void Resume()
    {
        Canvas.SetActive(false);
        sfxSlider.SetActive(false);
        SfxText.SetActive(false);
        BackGroundText.SetActive(false);
      //  MainMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;

    }

    public void TogglePause()  // 'Pause' yerine 'TogglePause' kullanýldý
    {
        Canvas.SetActive(true);
        sfxSlider.SetActive(true);
        SfxText.SetActive(true);
        BackGroundText.SetActive(true);
      //  MainMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

}
