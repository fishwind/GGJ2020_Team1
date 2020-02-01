using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Entity, IFireable
{
    public float repairTime;
    private Coroutine repairCoroutine;

    #region Public Methods
    // Used by the Lvl/Map Manager
    public void CreateFixedPot()
    {
        currItemState = ItemStates.Fixed;
        currItemActionState = ItemActionState.Pickup;
        // TODO: Animations, Play Sounds

    }

    // Used by the Pottery Wheel
    public void CreateUnfiredPot()
    {
        currItemState = ItemStates.Unfired;
        currItemActionState = ItemActionState.Pickup;
        // TODO: Animations, Play Sounds

    }
    #endregion

    #region IBreakable
    // Used by the "Hero"
    public override void AttemptBreak()
    {
        // Only Break if Already Fixed
        if (currItemState == ItemStates.Fixed)
        {
            currItemState = ItemStates.Broken;
            currItemActionState = ItemActionState.Repair;
            // TODO: Animations, Play Sounds

        }
        else
        {
            // TODO: Check if Hero Lvl Strng Enuff to break

            // TODO: Feedback to player/system that pot not fixed
        }
    }
    #endregion

    #region IRepairable
    // Used By Player to start Repairing Item
    public override void StartRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine == null && currItemState == ItemStates.Broken)
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
            currItemState = ItemStates.Cleared;
            currItemActionState = ItemActionState.None;

            // TODO: Destroy the Object, Clean up, Animations, Play Sounds
            Destroy(gameObject);
        }
    }


    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        CompleteRepairing();
    }
    #endregion

    #region IPlaceable
    public override float GetPlaceHeight()
    {
        return itemPlaceHeight;
    }
    #endregion

    #region IFireable
    public void CompleteFiring()
    {
        if (currItemState == ItemStates.Unfired)
        {
            currItemState = ItemStates.Fixed;
            currItemActionState = ItemActionState.Pickup;
            // TODO: Animations, Play Sounds
        }
    }

    public bool CheckIfFireable()
    {
        return (currItemState == ItemStates.Unfired);
    }
    #endregion
}
