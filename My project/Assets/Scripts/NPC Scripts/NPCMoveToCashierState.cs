using UnityEngine;

public class NPCMoveToCashierState : INPCState
{
    public void Enter(NPC npc)
    {
        MoveToCashier(npc);
        Debug.Log("NPC: Moving to Cashier");
    }

    public void Update(NPC npc)
    {
        if (CashierManager.Instance.IsProcessingCustomer() || !CashierManager.Instance.CustomerMovingToCashier(npc))
        {
            npc.ChangeState(new NPCWaitingForCashierState());
            return;
        }

        if (npc.agent.remainingDistance <= npc.agent.stoppingDistance)
        {
            Debug.Log("NPC: Reached Cashier");
            npc.isAtCashier = true;
            npc.ChangeState(new NPCCashierState());
        }
    }

    public void Exit(NPC npc)
    {
        Debug.Log("NPC: Exiting MoveToCashier Steate");
    }

    public void MoveToCashier(NPC npc)
    {
        if (npc.cashierPosition != null)
        {
            npc.agent.SetDestination(npc.cashierPosition.position);
        }
    }
}

