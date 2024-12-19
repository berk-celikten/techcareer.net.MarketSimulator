using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections;
using TMPro.Examples;

public class NPC : MonoBehaviour
{
    public ShelfWrapper[] shelves;
    public ShelfWrapper selectedShelf;
    public Transform cashierPosition;
    public Transform holdPosition;
    public Transform leavePoint;

    public NavMeshAgent agent;
    public INPCState currentState;
    public Animator animator;

    public bool isAtShelf = false;
    public bool isAtCashier = false;
    private GameObject pickedItem;
    public List<ProductReference> products;
    private CashierManager cashierManager;

    public void Awake()
    {
        selectedShelf = null;
        agent = GetComponent<NavMeshAgent>();
        cashierManager = CashierManager.Instance;
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        ChangeState(new NPCAtShelfState());
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update(this);
        }
        if (!agent.isStopped)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void ChangeState(INPCState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter(this);
        }

        if (newState is NPCMoveToCashierState)
        {
            if (cashierManager.IsProcessingCustomer() || !cashierManager.CustomerMovingToCashier(this))
            {
                ChangeState(new NPCWaitingForCashierState());
            }
        }
    }

    public Transform MoveToShelfWithProducts()
    {
        List<ShelfWrapper> shelvesWithProducts = new List<ShelfWrapper>();
        foreach (ShelfWrapper shelf in shelves)
        {
            if (shelf.HasAnyItems())
            {
                shelvesWithProducts.Add(shelf);
            }
        }

        if (shelvesWithProducts.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, shelvesWithProducts.Count);
        selectedShelf = shelvesWithProducts[randomIndex];

        return selectedShelf.transform;
    }

    public IEnumerator WaitForLeave()
    {
        while (Vector3.Distance(transform.position, leavePoint.position) > agent.stoppingDistance)
        {
            yield return null;
        }
        DestroyMe();
    }

    public int PickItemFromShelf()
    {
        List<ProductReference> takenItems = selectedShelf.NPCTakeRandomItems();

        //Debug.Log($"NPC {gameObject.name} şu ürünleri aldı:");
        foreach (ProductReference item in takenItems)
        {
            products.Add(item);
        }
        return takenItems.Count;
    }


    private bool PickSpecificProductFromShelf(Shelf shelf, ProductSO productSO)
    {
        foreach (Transform itemPosition in shelf.itemPositions)
        {
            if (shelf.IsPositionOccupied(itemPosition))
            {
                if (itemPosition.childCount > 0)
                {
                    Transform product = itemPosition.GetChild(0);
                    ProductReference productReference = product.GetComponent<ProductReference>();

                    if (productReference != null && productReference.product == productSO)
                    {
                        Debug.Log($"[NPC] Picked item: {product.name} from shelf: {shelf.name} at position: {itemPosition.name}");

                        Destroy(product.gameObject);

                        shelf.FreePosition(itemPosition);

                        return true;
                    }
                }
            }
        }

        Debug.LogWarning($"[NPC] Couldn't pick item: {productSO.productName} from shelf: {shelf.name}. Product not found.");
        return false;
    }

    public void DestroyMe()
    {
        Debug.Log("Destekteyim");
        Destroy(gameObject);
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}