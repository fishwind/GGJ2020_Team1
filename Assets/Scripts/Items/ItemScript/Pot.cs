using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Entity, IFireable, IBreakable, IRepairable
{
    [Header("Item Variables")]
    public float repairTime;
    private Coroutine repairCoroutine;

    #region Init / Destroy
    private void Awake()
    {
        GlobalEvents.OnPlayerStartDestroyAll += AttemptBreak;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnPlayerStartDestroyAll -= AttemptBreak;
    }
    #endregion

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

    #region Private Methods
    void Break()
    {
        currItemState = ItemStates.Broken;
        currItemActionState = ItemActionState.RepairSweep;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
    }

    void Repair()
    {
        currItemState = ItemStates.Cleared;
        currItemActionState = ItemActionState.None;

        UpdateItemMesh();

        // TODO: Destroy the Object, Clean up, Animations, Play Sounds
        Destroy(gameObject);
    }

    void Fired()
    {
        currItemState = ItemStates.Fixed;
        currItemActionState = ItemActionState.Pickup;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
    }

    #endregion

    #region IBreakable
    public void AttemptBreak(int itemTier)
    {
        // TODO: Check if Hero Lvl Strng Enuff to break

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


    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        CompleteRepairing();
    }
    #endregion

    #region IFireable
    public void CompleteFiring()
    {
        if (currItemState == ItemStates.Unfired)
        {
            Fired();
        }
    }

    public bool CheckIfFireable()
    {
        return (currItemState == ItemStates.Unfired);
    }
    #endregion
}
