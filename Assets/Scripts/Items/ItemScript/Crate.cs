﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Entity, IBreakable, IRepairable
{
    [Header("Item Variables")]
    public float repairTime;
    private Coroutine repairCoroutine;
    public GameObject m_Coins;

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
        currItemActionState = ItemActionState.RepairSweep;

        UpdateItemMesh();

        // TODO: Animations, Play Sounds
        Instantiate(m_Coins, transform.position, Quaternion.identity);
    }

    void Repair()
    {
        currItemState = ItemStates.Cleared;
        currItemActionState = ItemActionState.None;

        UpdateItemMesh();

        // TODO: Destroy the Object, Clean up, Animations, Play Sounds
        Destroy(gameObject);
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

    #region IRepairable
    // Used By Player to start Repairing Item
    public void StartRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine == null && currItemState == ItemStates.Broken)
        {
            repairCoroutine = StartCoroutine(RepairingingCoroutine());
            repairCoroutine = null;
            StartVisualFeedback(repairTime);
        }
    }

    // Used By Player to stop Repairing Item in the middle of repairing
    public void StopRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            StopVisualFeedback();
        }
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

}