using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Entity
{
    public float repairTime;
    private Coroutine repairCoroutine;

    #region IRepairable
    // Used By Player to start Repairing Item
    public override void StartRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine == null)
            repairCoroutine = StartCoroutine(RepairingingCoroutine());
    }

    // Used By Player to stop Repairing Item in the middle of repairing
    public override void StopRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
    }

    // Broken >> Cleared
    public override void CompleteRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }

        if (currItemState == ItemStates.Broken)
        {
            currItemState = ItemStates.Fixed;
            currItemActionState = ItemActionState.Pickup;

            // TODO: Destroy the Object, Clean up, Animations, Play Sounds
        }
    }

    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        CompleteRepairing();
    }
    #endregion
}
