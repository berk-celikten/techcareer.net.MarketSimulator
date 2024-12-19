using UnityEngine;

public class EscTusu : MonoBehaviour
{
    // Canvas bile�eni referans�
    [SerializeField] private Canvas canvas;

    // Ba�lang��ta Canvas'�n g�r�n�rl���n� ayarlamak i�in
    private void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas referans� atanmad�!");
            return;
        }

        // Canvas ba�lang��ta devre d���ysa gizli yap
        canvas.enabled = false;
    }

    private void Update()
    {
        // ESC tu�una bas�ld���nda Canvas g�r�n�rl���n� de�i�tir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas != null)
            {
                canvas.enabled = !canvas.enabled;
            }
        }
    }
}
