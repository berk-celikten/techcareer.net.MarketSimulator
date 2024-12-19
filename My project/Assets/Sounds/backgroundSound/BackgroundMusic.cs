using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{       //Sahne de�i�irken arka plan m�zi�inin kald��� yerden devam etmesi
    private static BackgroundMusic instance;

    void Awake()
    {
        // E�er instance yoksa, bu nesneyi instance olarak belirle ve yok olmamas�n� sa�la
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Zaten bir instance varsa, bu yeni olan� yok et
            Destroy(gameObject);
        }
    }
}
