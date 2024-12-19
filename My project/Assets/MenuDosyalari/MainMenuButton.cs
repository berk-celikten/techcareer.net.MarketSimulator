using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        float bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();

        Time.timeScale = 1.0f;

        SceneManager.LoadScene(sceneName);
    }
}
