using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Entity, IBreakable
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