using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour, IRepairable, IBreakable, IPlaceable
{
    // Variables to tweak
    public float itemPlaceHeight;
    public float repairTime;
    public ItemStates currItemState = ItemStates.Fixed;
    public ItemActionState currItemActionState = ItemActionState.None;

    // Define Enums
    public Animator animator;

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

    #region Private Methods
    // Broken >> Cleared
    void RepairPot()
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
    #endregion

    #region IBreakable
    // Used by the "Hero"
    public void AttemptBreak()
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
    public void StartRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine == null)
            repairCoroutine = StartCoroutine(RepairingingCoroutine());
    }

    // Used By Player to stop Repairing Item in the middle of repairing
    public void StopRepairing()
    {
        // Stop Coroutine if currently in Progress
        if (repairCoroutine != null)
            StopCoroutine(repairCoroutine);
    }

    IEnumerator RepairingingCoroutine()
    {
        yield return new WaitForSeconds(repairTime);
        RepairPot();
    }
    #endregion

    #region IPlaceable
    public float getPlaceHeight()
    {
        return itemPlaceHeight;
    }
    #endregion

    #region Old Unused Code
    /*
    void PlayerInteract()
    {
        switch (currItemState) {
            case PotStates.Unfired:
                break;
            case PotStates.Fixed:
                break;
            case PotStates.Broken:
                RepairPot();
                break;
            case PotStates.Cleared:
                break;
            default:
                Debug.LogError("Pot either has no valid States / missing Behavior");
                break;
        }
    }*/
    #endregion
}
