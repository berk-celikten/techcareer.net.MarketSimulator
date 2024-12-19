using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Transform[] itemPositions;
    private bool[] occupiedPositions;
    private ProductReference[] placedProducts;

    private void Start()
    {
        occupiedPositions = new bool[itemPositions.Length];
        placedProducts = new ProductReference[itemPositions.Length];
        UpdateShelf();
    }

    public void UpdateShelf()
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            Transform itemPosition = itemPositions[i];

            if (itemPosition.childCount > 0)
            {
                Transform productTransform = itemPosition.GetChild(0);
                ProductReference productRef = productTransform.GetComponent<ProductReference>();

                if (productRef != null)
                {
                    occupiedPositions[i] = true;
                    placedProducts[i] = productRef;
                    Debug.Log("Detected product: " + productRef.product.productName + " in position " + i);
                }
            }
            else
            {
                occupiedPositions[i] = false;
                placedProducts[i] = null;
            }
        }
    }

    public List<ProductSO> GetRandomProductsOnShelf()
    {
        List<ProductSO> availableProducts = new List<ProductSO>();

        for (int i = 0; i < placedProducts.Length; i++)
        {
            if (occupiedPositions[i] && placedProducts[i] != null)
            {
                availableProducts.Add(placedProducts[i].product);
            }
        }

        if (availableProducts.Count == 0)
        {
            return new List<ProductSO>();
        }

        int totalProducts = Mathf.Min(4, availableProducts.Count);
        totalProducts = Random.Range(1, totalProducts + 1);

        List<ProductSO> selectedProducts = new List<ProductSO>();

        for (int i = 0; i < totalProducts; i++)
        {
            int randomIndex = Random.Range(0, availableProducts.Count);
            ProductSO selectedProduct = availableProducts[randomIndex];

            selectedProducts.Add(selectedProduct);
            availableProducts.RemoveAt(randomIndex);
        }

        return selectedProducts;
    }

    public bool IsPositionOccupied(Transform position)
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (itemPositions[i] == position)
            {
                return occupiedPositions[i];
            }
        }
        return false;
    }

    public ProductReference GetProductAtPosition(Transform position)
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (itemPositions[i] == position)
            {
                return placedProducts[i];
            }
        }
        return null;
    }

    public void ForceUpdateShelf()
    {
        UpdateShelf();
    }

    public ProductReference[] GetAllProductsOnShelf()
    {
        return placedProducts;
    }

    public Transform GetAvailablePosition()
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (!occupiedPositions[i])
            {
                occupiedPositions[i] = true;
                return itemPositions[i];
            }
        }
        return null;
    }

    public void FreePosition(Transform position)
    {
        for (int i = 0; i < itemPositions.Length; i++)
        {
            if (itemPositions[i] == position)
            {
                occupiedPositions[i] = false;
                placedProducts[i] = null;
                Debug.Log("Freed position " + i + " on shelf.");
                break;
            }
        }
    }
}
