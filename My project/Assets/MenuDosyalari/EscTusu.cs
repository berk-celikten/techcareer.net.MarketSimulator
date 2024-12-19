using UnityEngine;

public class EscTusu : MonoBehaviour
{
    // Canvas bileþeni referansý
    [SerializeField] private Canvas canvas;

    // Baþlangýçta Canvas'ýn görünürlüðünü ayarlamak için
    private void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas referansý atanmadý!");
            return;
        }

        // Canvas baþlangýçta devre dýþýysa gizli yap
        canvas.enabled = false;
    }

    private void Update()
    {
        // ESC tuþuna basýldýðýnda Canvas görünürlüðünü deðiþtir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas != null)
            {
                canvas.enabled = !canvas.enabled;
            }
        }
    }
}
