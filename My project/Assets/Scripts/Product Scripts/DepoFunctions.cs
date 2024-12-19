using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DepoFunctions : MonoBehaviour
{
    public GameObject depoCanvasPrefab;
    private GameObject depoCanvasInstance;
    public GameObject itemSpawnPoint;
    public GameObject gameUIPrefab;
    private GameObject gameUIInstance;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!ProductManager.Instance.priceTagPrefab.activeSelf)
            {
                bool depoAktif = !depoCanvasInstance.activeSelf;

                if (depoAktif)
                {
                    UIFunctions uiFunctions = depoCanvasInstance.GetComponent<UIFunctions>();
                    uiFunctions.returnDesktop();
                }

                depoCanvasInstance.SetActive(depoAktif);

                gameUIInstance.SetActive(!depoAktif);


                if (depoAktif)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitiliazeUI();
    }

    private void InitiliazeUI()
    {
        depoCanvasInstance = Instantiate(depoCanvasPrefab);
        UIFunctions uiFunctions = depoCanvasInstance.GetComponent<UIFunctions>();

        depoCanvasInstance.SetActive(false);

        gameUIInstance = Instantiate(gameUIPrefab);
        gameUIInstance.SetActive(true);
        ProductManager.Instance.priceTagPrefab = gameUIInstance.transform.Find("PriceTagUI").gameObject;
        itemSpawnPoint = GameObject.Find("ItemSpawnPoint");
        ProductManager.Instance.spawnArea = itemSpawnPoint.transform;
        CashierManager.Instance.checkoutArea = GameObject.Find("CustomerItemPlace").transform;
    }
}
