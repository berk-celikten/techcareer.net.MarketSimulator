using UnityEngine;

[CreateAssetMenu(fileName = "New License", menuName = "Supermarket/License")]
public class LicenseSO : ScriptableObject
{
    public string licenseName;
    public int price;
    public Sprite icon;
    public string description;
    public string licenseKeyword;
    public bool isPurchased;
    public bool isActive;

}
