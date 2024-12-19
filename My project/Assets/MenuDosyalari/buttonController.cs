using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonController : MonoBehaviour
{
    public GameObject[] settingsDisable; // Devre dýþý býrakýlacak GameObject'leri tutacak dizi
    public GameObject[] settingsEnable;  // Aktif hale getirilecek GameObject'leri tutacak dizi


    public GameObject[] backDisable; // Devre dýþý býrakýlacak GameObject'leri tutacak dizi
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
        EditorApplication.isPlaying = false; // Editör'de oyunu durdurur
#else
            Application.Quit(); // Build alýndýðýnda oyunu kapatýr
#endif

        Debug.Log("Oyun kapandý."); // Çýkýþ iþlemi geri bildirim olarakd gösterilir
    }

    public void DisableGameObjectsSettings()
    {
        // Tüm GameObject'leri devre dýþý býrak
        foreach (GameObject obj in settingsDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // GameObject'i devre dýþý býrak
            }
        }
    }

    // Butona basýldýðýnda çaðrýlacak fonksiyon
    public void EnableGameObjectsSettings()
    {
        // Tüm GameObject'leri aktif yap
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
        // Tüm GameObject'leri devre dýþý býrak
        foreach (GameObject obj in backDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // GameObject'i devre dýþý býrak
            }
        }
    }

    public void EnableGameObjectsBack()
    {
        // Tüm GameObject'leri aktif yap
        foreach (GameObject obj in backEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);  // GameObject'i aktif yap
            }
        }
    }

}
