using UnityEngine;

public class NPCWaitingForCashierState : INPCState
{
    private float checkInterval = 1f;
    private float timer = 0f;

    public void Enter(NPC npc)
    {
        Debug.Log("NPC: Waiting for cashier");
        npc.agent.isStopped = true;
    }

    public void Update(NPC npc)
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            if (!CashierManager.Instance.IsProcessingCustomer() &&
                CashierManager.Instance.CustomerMovingToCashier(npc))
            {
                npc.ChangeState(new NPCMoveToCashierState());
            }
        }
    }

    public void Exit(NPC npc)
    {
        Debug.Log("NPC: Exiting C");
        npc.agent.isStopped = false;
    }
}