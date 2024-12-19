using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{       //Sahne deðiþirken arka plan müziðinin kaldýðý yerden devam etmesi
    private static BackgroundMusic instance;

    void Awake()
    {
        // Eðer instance yoksa, bu nesneyi instance olarak belirle ve yok olmamasýný saðla
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Zaten bir instance varsa, bu yeni olaný yok et
            Destroy(gameObject);
        }
    }
}
