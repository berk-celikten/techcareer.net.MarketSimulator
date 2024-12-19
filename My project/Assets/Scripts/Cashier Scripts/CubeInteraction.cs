using UnityEngine;
using TMPro;

public class CubeInteraction : MonoBehaviour
{
    public Color hoverColor = Color.red;
    public float canvasOffset = 0.6f;
    public float canvasScale = 0.01f;

    [Header("Text Settings")]
    public string defaultText = "50 Kuruþ";
    public double price = 0.50;
    public Color textColor = Color.white;
    public int fontSize = 24;
    public bool isBold = true;

    private Renderer cubeRenderer;
    private Color originalColor;
    private Canvas worldSpaceCanvas;
    private TextMeshProUGUI tmpText;
    private bool isHovering = false;
    private bool isPaying = false;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            originalColor = cubeRenderer.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on this GameObject.");
        }

        CreateWorldSpaceCanvas();

        CashierManager.Instance.OnReceivedMoneyUpdated += HandleReceivedMoneyUpdated;
        CashierManager.Instance.OnTransactionCompleted += HandleTransactionCompleted;
    }

    private void OnDestroy()
    {
        CashierManager.Instance.OnReceivedMoneyUpdated -= HandleReceivedMoneyUpdated;
        CashierManager.Instance.OnTransactionCompleted -= HandleTransactionCompleted;
    }

    private void HandleReceivedMoneyUpdated(decimal receivedMoney)
    {
        isPaying = true;
    }

    private void HandleTransactionCompleted()
    {
        isPaying = false;
    }

    void Update()
    {
        if (worldSpaceCanvas != null)
        {
            worldSpaceCanvas.transform.LookAt(worldSpaceCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            if (cubeRenderer != null) cubeRenderer.material.color = hoverColor;
            isHovering = true;
        }
        else
        {
            if (cubeRenderer != null) cubeRenderer.material.color = originalColor;
            if (tmpText != null) tmpText.text = defaultText;
            isHovering = false;
        }

        if (isHovering && isPaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                decimal value = (decimal)price;
                CashierManager.Instance.GiveChange(value);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                decimal value = (decimal)(-price);
                CashierManager.Instance.GiveChange(value);
            }
        }
    }

    void CreateWorldSpaceCanvas()
    {
        GameObject canvasObject = new GameObject("WorldSpaceCanvas");
        worldSpaceCanvas = canvasObject.AddComponent<Canvas>();
        worldSpaceCanvas.renderMode = RenderMode.WorldSpace;

        canvasObject.transform.SetParent(transform);
        canvasObject.transform.localPosition = Vector3.up * (canvasOffset + 0.2f); // Y ekseninde 0.2 birim daha yükseðe taþýdýk
        canvasObject.transform.localScale = Vector3.one * canvasScale;

        GameObject textObject = new GameObject("TMPText");
        tmpText = textObject.AddComponent<TextMeshProUGUI>();
        tmpText.text = defaultText;
        tmpText.color = textColor;
        tmpText.fontSize = fontSize;
        tmpText.fontStyle = isBold ? FontStyles.Bold : FontStyles.Normal;
        tmpText.alignment = TextAlignmentOptions.Center;

        textObject.transform.SetParent(canvasObject.transform, false);
        RectTransform rectTransform = tmpText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200, 50);

        // Ensure TextMeshPro asset is loaded
        if (tmpText.font == null)
        {
            tmpText.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            if (tmpText.font == null)
            {
                Debug.LogError("Failed to load TextMeshPro font asset. Please ensure TextMeshPro Essential Resources are imported.");
            }
        }
    }
}