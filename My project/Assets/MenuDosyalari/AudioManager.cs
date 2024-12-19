using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource bgMusicSource;
    public AudioSource[] sfxSource;

    private float bgMusicVolume = 1.0f;
    private float sfxVolume = 1.0f;
    private float bgMusicTime = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += onSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadVolumeSettings();
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignAudioSources();

        if (bgMusicSource != null)
        {
            bgMusicSource.time = bgMusicTime;
            if (!bgMusicSource.isPlaying)
            {
                bgMusicSource.Play();
            }
        }
        ApplyVolumeSettings();
    }

    private void AssignAudioSources() //sesleri ve efektleri bulur
    {
        bgMusicSource = GameObject.Find("bg")?.GetComponent<AudioSource>();

        if (bgMusicSource != null)
        {
            bgMusicSource.loop = true;
        }

        // Güncellenmiþ yöntem: FindObjectsByType kullanýlýyor
        AudioSource[] allAudioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        sfxSource = System.Array.FindAll(allAudioSources, source => source != bgMusicSource);
    }

    private void LoadVolumeSettings()
    {
        bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void ApplyVolumeSettings()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.volume = bgMusicVolume;
        }

        foreach (var sfx in sfxSource)
        {
            if (sfx != null)
            {
                sfx.volume = sfxVolume;
            }
        }
    }

    public void SetBGMusicVolume(float volume)
    {
        bgMusicVolume = volume;
        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        ApplyVolumeSettings();
    }

    private void Update()
    {
        if (bgMusicSource != null && bgMusicSource.isPlaying)
        {
            bgMusicTime = bgMusicSource.time;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }




    //public static AudioManager instance;

    //public AudioSource bgMusicSource;
    //public AudioSource[] sfxSource;
    //public AudioSource denemeSesi; // denemeSesi isimli AudioSource

    //public Slider bgMusicSlider;  // BG müziði sesini ayarlayan slider
    //public Slider sfxSlider;      // SFX sesini ayarlayan slider

    //private float bgMusicVolume = 1.0f;
    //private float sfxVolume = 1.0f;
    //private float bgMusicTime = 0f;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);

    //        SceneManager.sceneLoaded += onSceneLoaded;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    LoadVolumeSettings();
    //    AssignSliders(); // Slider deðerlerini baþta atýyoruz
    //}

    //private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    AssignAudioSources();

    //    if (bgMusicSource != null)
    //    {
    //        bgMusicSource.time = bgMusicTime;
    //        if (!bgMusicSource.isPlaying)
    //        {
    //            bgMusicSource.Play();
    //        }
    //    }
    //    ApplyVolumeSettings();
    //}

    //private void AssignAudioSources()
    //{
    //    bgMusicSource = GameObject.Find("bg")?.GetComponent<AudioSource>();
    //    denemeSesi = GameObject.Find("denemeSesi")?.GetComponent<AudioSource>(); // denemeSesi kaynaðýný atýyoruz.

    //    if (bgMusicSource != null)
    //    {
    //        bgMusicSource.loop = true;
    //    }

    //    // Yeni API kullanýmý ile FindObjectsOfType yerine FindObjectsByType kullanýldý.
    //    AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None); // Sýralama olmadan buluyoruz.

    //    // bgMusicSource ve denemeSesi dýþýndaki tüm ses kaynaklarýný filtreliyoruz.
    //    sfxSource = System.Array.FindAll(allAudioSources, source => source != bgMusicSource && source != denemeSesi);
    //}

    //private void LoadVolumeSettings()
    //{
    //    bgMusicVolume = PlayerPrefs.GetFloat("BGMusicVolume", 1f);
    //    sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    //}

    //private void ApplyVolumeSettings()
    //{
    //    if (bgMusicSource != null)
    //    {
    //        bgMusicSource.volume = bgMusicVolume;
    //    }

    //    foreach (var sfx in sfxSource)
    //    {
    //        if (sfx != null)
    //        {
    //            sfx.volume = sfxVolume;
    //        }
    //    }
    //}

    //// Sliders atandý ve baþlatýldýðýnda çaðýrýlýyor
    //private void AssignSliders()
    //{
    //    if (bgMusicSlider != null)
    //    {
    //        bgMusicSlider.value = bgMusicVolume;
    //        bgMusicSlider.onValueChanged.AddListener(SetBGMusicVolume);
    //    }

    //    if (sfxSlider != null)
    //    {
    //        sfxSlider.value = sfxVolume;
    //        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    //    }
    //}

    //public void SetBGMusicVolume(float volume)
    //{
    //    bgMusicVolume = volume;
    //    PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
    //    ApplyVolumeSettings();
    //}

    //public void SetSFXVolume(float volume)
    //{
    //    sfxVolume = volume;
    //    PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    //    ApplyVolumeSettings();
    //}

    //private void Update()
    //{
    //    if (bgMusicSource != null && bgMusicSource.isPlaying)
    //    {
    //        bgMusicTime = bgMusicSource.time;
    //    }
    //}

    //private void OnApplicationQuit()
    //{
    //    PlayerPrefs.SetFloat("BGMusicVolume", bgMusicVolume);
    //    PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    //    PlayerPrefs.Save();
    //}


}
