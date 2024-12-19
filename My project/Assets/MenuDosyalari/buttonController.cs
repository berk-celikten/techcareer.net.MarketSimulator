using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonController : MonoBehaviour
{
    public GameObject[] settingsDisable; // Devre d��� b�rak�lacak GameObject'leri tutacak dizi
    public GameObject[] settingsEnable;  // Aktif hale getirilecek GameObject'leri tutacak dizi


    public GameObject[] backDisable; // Devre d��� b�rak�lacak GameObject'leri tutacak dizi
    public GameObject[] backEnable;  // Aktif hale getirilecek GameObject'leri tutacak dizi

    public void LoadScene(string sceneName)
    {
        float bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();


        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Edit�r'de oyunu durdurur
#else
            Application.Quit(); // Build al�nd���nda oyunu kapat�r
#endif

        Debug.Log("Oyun kapand�."); // ��k�� i�lemi geri bildirim olarakd g�sterilir
    }

    public void DisableGameObjectsSettings()
    {
        // T�m GameObject'leri devre d��� b�rak
        foreach (GameObject obj in settingsDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // GameObject'i devre d��� b�rak
            }
        }
    }

    // Butona bas�ld���nda �a�r�lacak fonksiyon
    public void EnableGameObjectsSettings()
    {
        // T�m GameObject'leri aktif yap
        foreach (GameObject obj in settingsEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);  // GameObject'i aktif yap
            }
        }
    }

    public void DisableGameObjectsBack()
    {
        // T�m GameObject'leri devre d��� b�rak
        foreach (GameObject obj in backDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // GameObject'i devre d��� b�rak
            }
        }
    }

    public void EnableGameObjectsBack()
    {
        // T�m GameObject'leri aktif yap
        foreach (GameObject obj in backEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);  // GameObject'i aktif yap
            }
        }
    }

}
