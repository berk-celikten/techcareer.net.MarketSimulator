using UnityEngine;
using System.Collections;

public class NPCAtShelfState : INPCState
{
    public void Enter(NPC npc)
    {
        Transform shelf = npc.MoveToShelfWithProducts();
        if (shelf != null)
        {
            Transform stopPosition = shelf.Find("NPCStopPosition");
            if (stopPosition != null)
            {
                shelf = stopPosition;
            }
            npc.agent.SetDestination(shelf.position);
            Debug.Log("NPC: Entering Shelf State");
        }
        else
        {
            Debug.Log("NPC: I see no shelf, so I will die...");
            npc.DestroyMe();
        }
    }

    public void Update(NPC npc)
    {
        if (npc.agent.remainingDistance <= npc.agent.stoppingDistance)
        {
            Debug.Log("NPC: Reached Shelf");
            int itemsPicked = npc.PickItemFromShelf();
            if (itemsPicked > 0)
            {
                npc.ChangeState(new NPCMoveToCashierState());
            }
            else
            {
                npc.GetAgent().SetDestination(npc.leavePoint.position);
                npc.StartCoroutine(ReachedLeavePoint(npc));
            }

            // if (npc.AreThereProductsOnShelves())
            // {
            //     npc.PickItemFromShelf();
            //     npc.ChangeState(new NPCMoveToCashierState());
            // }
            // else
            // {
            //     Debug.Log("No products left, waiting...");
            // }
        }
    }

    public void Exit(NPC npc)
    {
        Debug.Log("NPC: Exiting Shelf State");
    }

    private IEnumerator ReachedLeavePoint(NPC npc)
    {
        return npc.WaitForLeave();
    }
}