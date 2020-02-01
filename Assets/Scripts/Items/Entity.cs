using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IPlaceable, IBreakable, IRepairable
{

    [Header("Debug Stuff")]
    public float itemPlaceHeight;
    public ItemStates currItemState = ItemStates.Fixed;
    public ItemActionState currItemActionState = ItemActionState.None;

    // References to the Meshes
    [SerializeField] private GameObject meshUnfired;
    [SerializeField] private GameObject meshFixed;
    [SerializeField] private GameObject meshBroken;

    #region IPlaceable
    public virtual float GetPlaceHeight()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IBreakable
    public virtual void AttemptBreak()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IRepairable
    public virtual void StartRepairing()
    {
        throw new System.NotImplementedException();
    }

    public virtual void StopRepairing()
    {
        throw new System.NotImplementedException();
    }

    public virtual void CompleteRepairing()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    protected void UpdateItemMesh()
    {
        meshUnfired.SetActive(currItemState == ItemStates.Unfired);
        meshFixed.SetActive(currItemState == ItemStates.Fixed);
        meshBroken.SetActive(currItemState == ItemStates.Broken);
    }

    // public abstract void AbstractMethod();
}
