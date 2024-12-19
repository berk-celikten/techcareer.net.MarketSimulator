using UnityEngine;
using System.Collections.Generic;

public class ShelfWrapper : MonoBehaviour
{
    public List<ShelfScript> itemZones = new List<ShelfScript>();
    private const int MAX_ITEMS_FOR_NPC = 5;

    void Start()
    {
        // Tüm child raflarý bul ve listeye ekle
        for (int i = 1; i <= 6; i++)
        {
            ShelfScript shelf = transform.Find($"ItemZone{i}")?.GetComponent<ShelfScript>();
            if (shelf != null)
            {
                itemZones.Add(shelf);
            }
        }
    }

    public List<ProductReference> NPCTakeRandomItems()
    {
        List<ProductReference> takenItems = new List<ProductReference>();
        int itemsToTake = Random.Range(1, MAX_ITEMS_FOR_NPC + 1);

        for (int i = 0; i < itemsToTake; i++)
        {
            ProductReference item = TakeRandomItemFromRandomShelf();
            if (item != null)
            {
                takenItems.Add(item);
            }
            else
            {
                break;
            }
        }
        return takenItems;
    }

    private ProductReference TakeRandomItemFromRandomShelf()
    {
        List<ShelfScript> availableShelves = itemZones.FindAll(shelf => shelf.GetItemCount() > 0);

        if (availableShelves.Count == 0)
        {
            return null;
        }

        ShelfScript randomShelf = availableShelves[Random.Range(0, availableShelves.Count)];

        ProductReference takenItem = randomShelf.TakeLastItemForNPC();

        if (takenItem != null)
        {
            Debug.Log("Alýnan ürün: " + takenItem.product.productName + ", Kalan ürün sayýsý: " + randomShelf.GetItemCount());
        }
        else
        {
            //Debug.Log("Ürün alýnamadý.");
        }

        return takenItem;
    }

    public bool HasAnyItems()
    {
        foreach (ShelfScript shelf in itemZones)
        {
            if (shelf.GetItemCount() > 0)
            {
                return true;
            }
        }
        return false;
    }
}
