using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class NPCCashierState : INPCState
{
    private NPC currentNpc;
    private Transform playerTransform;
    private Coroutine lookAtPlayerCoroutine;

    public void Enter(NPC npc)
    {
        currentNpc = npc;

        CashierManager.Instance.CustomerArrived(npc.products);
        npc.agent.isStopped = true;

        CashierManager.Instance.OnTimeToPay += HandleTimeToPay;
        CashierManager.Instance.OnTransactionCompleted += HandleTransactionCompleted;

        lookAtPlayerCoroutine = npc.StartCoroutine(TrackAndLookAtPlayer());
    }

    public void Update(NPC npc)
    {
        //
    }

    public void Exit(NPC npc)
    {
        CashierManager.Instance.OnTimeToPay -= HandleTimeToPay;
        CashierManager.Instance.OnTransactionCompleted -= HandleTransactionCompleted;
    }

    private IEnumerator TrackAndLookAtPlayer()
    {
        while (true)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;

                Vector3 directionToPlayer = playerTransform.position - currentNpc.transform.position;
                directionToPlayer.y = 0;

                if (directionToPlayer != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

                    currentNpc.transform.rotation = Quaternion.Slerp(currentNpc.transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void HandleTimeToPay()
    {
        decimal total = CashierManager.Instance.GetCurrentTotal();
        decimal payment;

        decimal[] commonCurrencies = { 0.01m, 0.05m, 0.10m, 0.25m, 0.50m, 1m, 5m, 10m, 20m, 50m, 100m, 200m };

        if (UnityEngine.Random.value < 0.01f) // %0.1 ihtimalle tam tutar ödeme
        {
            Debug.Log("NPC: Tam ödeme");
            payment = total;
        }
        else // %70 ihtimalle fazla ödeme
        {
            Debug.Log("NPC: Fazla ödeme");
            decimal roundedTotal = Math.Ceiling(total);

            payment = commonCurrencies.FirstOrDefault(c => c >= roundedTotal);

            if (payment == 0)
            {
                payment = commonCurrencies.Last();
            }
        }

        payment = Math.Max(total, payment);

        CashierManager.Instance.SetReceivedMoney(payment);
    }

    public void HandleTransactionCompleted()
    {
        if (lookAtPlayerCoroutine != null)
        {
            currentNpc.StopCoroutine(lookAtPlayerCoroutine);
        }

        currentNpc.agent.isStopped = false;
        currentNpc.agent.updateRotation = true;
        currentNpc.agent.stoppingDistance = 2f;
        currentNpc.GetAgent().SetDestination(currentNpc.leavePoint.position);
        currentNpc.StartCoroutine(ReachedLeavePoint());
    }

    private IEnumerator ReachedLeavePoint()
    {
        CashierManager.Instance.OnTimeToPay -= HandleTimeToPay;
        CashierManager.Instance.OnTransactionCompleted -= HandleTransactionCompleted;
        return currentNpc.WaitForLeave();
    }
}