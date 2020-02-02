﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Entity, IBreakable
{
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

    void Break()
    {
        currItemState = ItemStates.Broken;
        currItemActionState = ItemActionState.RepairSweep;

        UpdateItemMesh();
        // TODO: Animations, Play Sounds
        StartVisualFeedback();
    }

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
