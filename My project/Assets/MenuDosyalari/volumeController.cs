using UnityEngine;
using UnityEngine.UI;

public class volumeController : MonoBehaviour
{
    public AudioSource audioSource; // Ses kaynaðý
    public Slider volumeSlider; // Ses kontrolü için slider

    void Awake()
    {
        // Bu objeyi sahne deðiþiminde yok olmamasý için 'DontDestroyOnLoad' metodunu çaðýrýyoruz.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Önceki ses seviyesini yükle veya varsayýlan olarak 0.5 ayarla
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.5f);
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Slider deðeri deðiþtiðinde ses seviyesini güncelle
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Ses seviyesini slider'ýn deðeri ile ayarlayan ve kaydeden fonksiyon
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("volume", volume); // Yeni ses seviyesini kaydet
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Oyun kapatýldýðýnda ayarlarý kaydet
    }
}
