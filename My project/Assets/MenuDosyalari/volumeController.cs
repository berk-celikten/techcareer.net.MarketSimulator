using UnityEngine;
using UnityEngine.UI;

public class volumeController : MonoBehaviour
{
    public AudioSource audioSource; // Ses kayna��
    public Slider volumeSlider; // Ses kontrol� i�in slider

    void Awake()
    {
        // Bu objeyi sahne de�i�iminde yok olmamas� i�in 'DontDestroyOnLoad' metodunu �a��r�yoruz.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // �nceki ses seviyesini y�kle veya varsay�lan olarak 0.5 ayarla
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Slider de�eri de�i�ti�inde ses seviyesini g�ncelle
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Ses seviyesini slider'�n de�eri ile ayarlayan ve kaydeden fonksiyon
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("volume", volume); // Yeni ses seviyesini kaydet
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Oyun kapat�ld���nda ayarlar� kaydet
    }
}
