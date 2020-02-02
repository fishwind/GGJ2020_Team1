using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Entity, IBreakable, IRepairable
{
    [Header("Item Variables")]
    public float repairTime;
    private Coroutine repairCoroutine;

    #region Init / Destroy
    private void Awake()
    {
        GlobalEvents.OnPlayerDestroyedAll += AttemptBreak;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnPlayerDestroyedAll -= AttemptBreak;
    }
    #endregion

    #region Private Methods
    void Break()
    {
        currItemState = ItemStates.Broken;
        currItemActionState = ItemActionState.RepairHit;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
        BreakFeedback();
    }

    void Repair()
    {
        currItemState = ItemStates.Fixed;
        currItemActionState = ItemActionState.None;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
    }

    #endregion

    #region IRepairable
    // Used By Player to start Repairing Item
    public void StartRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine == null && currItemState == ItemStates.Broken)
            repairCoroutine = StartCoroutine(RepairingingCoroutine());
    }

    // Used By Player to stop Repairing Item in the middle of repairing
    public void StopRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
    }

    // Broken >> Cleared
    public void CompleteRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }

        if (currItemState == ItemStates.Broken)
        {
            Repair();
        }
    }
 
    public float GetRepairTime() {
        return repairTime;
    }

    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        CompleteRepairing();
    }
    #endregion

    #region IBreakable
    public void AttemptBreak(int itemTier)
    {
        // TODO: Check if Hero Lvl Strng Enuff to break
        if (!CANIBREAK(itemTier)) return;

        // Only Break if Already Fixed
        if (currItemState == ItemStates.Fixed)
        {
            Break();
        }
        else
        {
            // TODO: Feedback to player/system that pot not fixed
        }
    }
    #endregion
}
