﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Entity
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

    #region Private Methods
    void Break()
    {
        currItemState = ItemStates.Broken;
        currItemActionState = ItemActionState.Repair;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
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
            Repair();
        }
    }

    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        CompleteRepairing();
    }
    #endregion

    #region IBreakable
    public override void AttemptBreak(int itemTier)
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
}
