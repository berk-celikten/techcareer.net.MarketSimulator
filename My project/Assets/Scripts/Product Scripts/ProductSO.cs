using UnityEngine;

[CreateAssetMenu(fileName = "New Product", menuName = "Supermarket/Product")]
public class ProductSO : ScriptableObject
{
    public string productName;
    public int price;
    public GameObject prefab;
    public GameObject boxPrefab;
    public Sprite icon;
    [TextArea(3, 10)]
    public string description;
    public string licenseKeyword = "";
    public int maxCountOnShelf;
}